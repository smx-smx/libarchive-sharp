#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using Smx.SharpIO;
using Smx.SharpIO.Memory;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static libarchive.Methods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace libarchive.Managed
{
    public record ArchiveDataBlock(Memory<byte> Data, long Offset);

    public class ArchiveEntryItem
    {
        private readonly int _index;
        private readonly ArchiveEntry _entry;
        public ArchiveEntry Header => _entry;
        public int Index => _index;

        private bool _consumed;
        private MemoryStream _data;

        private readonly IEnumerable<ArchiveDataBlock> _producer;
        private readonly ArchiveDataStreamer _streamer;

        public IEnumerable<byte> Bytes
        {
            get
            {
                if (!_consumed)
                {
                    foreach (var b in _streamer.GetBytes())
                    {
                        _data.WriteByte(b);
                        yield return b;
                    }
                    _consumed = true;
                } else
                {
                    if (!_data.TryGetBuffer(out var buf))
                    {
                        throw new InvalidOperationException();
                    }
                    foreach (var b in buf)
                    {
                        yield return b;
                    }
                }
            }
        }

        public ArchiveEntryItem(
            int index,
            ArchiveEntry entry,
            IEnumerable<ArchiveDataBlock> producer)
        {
            _index = index;
            _entry = entry;
            _consumed = false;
            _producer = producer;
            _streamer = new ArchiveDataStreamer(_producer);
            _data = new MemoryStream();
        }
    }

    public class ArchiveReader : Archive, IDisposable
    {
        private readonly ArchiveInputStream _inputStream;

        private IDictionary<int, IArchiveStream> _streams;
        private readonly Delegates.archive_read_callback _read_callback;
        private readonly Delegates.archive_close_callback _close_callback;
        private readonly Delegates.archive_seek_callback _seek_callback;
        private readonly Delegates.archive_skip_callback _skip_callback;
        private readonly Delegates.archive_switch_callback _switch_callback;
        private Delegates.archive_passphrase_callback? _passphrase_callback;
        private Delegates.progress_callback? _progress_callback;

        public delegate void SwitchRequestedDelegate();
        public delegate void ProgressDelegate();

        public event SwitchRequestedDelegate? OnSwitchRequested;
        public event ProgressDelegate? OnProgress;

        public ArchiveCallbackData CallbackData { get; private set; }
        public IEnumerable<ArchiveEntryItem> Entries { get; private set; }

        public ArchiveInputStream InputStream => _inputStream;

        private ArchiveReaderOptions? _opts;

        public int BufferSize => _opts?.BufferSize ?? ArchiveConstants.BUFFER_SIZE;

        public ArchiveReader(Stream stream, ArchiveReaderOptions? opts = null)
            : this(NewHandle(opts), [new ArchiveDataStream(stream)], true, opts)
        {
        }

        private static IList<IArchiveStream> CreateArchiveStreams(ICollection<Stream> streams, ArchiveReaderOptions? opts = null)
        {
            var bufferSize = opts?.BufferSize ?? ArchiveConstants.BUFFER_SIZE;
            return streams.Select(s =>
            {
                var ds = new ArchiveDataStream(s, bufferSize: bufferSize);
                return ds as IArchiveStream;
            }).ToList();
        }

        private static IList<IArchiveStream> CreateArchiveStreams(ICollection<Lazy<Stream>> streams, ArchiveReaderOptions? opts = null)
        {
            var bufferSize = opts?.BufferSize ?? ArchiveConstants.BUFFER_SIZE;
            return streams.Select(s =>
            {
                var ds = new ArchiveDataStreamLazy(s, bufferSize: bufferSize);
                return ds as IArchiveStream;
            }).ToList();
        }

        public ArchiveReader(ICollection<Stream> streams, ArchiveReaderOptions? opts = null)
            : this(NewHandle(opts), CreateArchiveStreams(streams, opts), true, opts)
        {
        }

        public ArchiveReader(ICollection<Lazy<Stream>> streams, ArchiveReaderOptions? opts = null)
            : this(NewHandle(opts), CreateArchiveStreams(streams, opts), true, opts)
        { }

        public ArchiveReader(ICollection<ArchiveDataStream> streams, ArchiveReaderOptions? opts = null)
            : this(NewHandle(opts), streams.Cast<IArchiveStream>().ToList(), true, opts)
        { }

        public Delegates.archive_passphrase_callback? PassphraseCallback
        {
            get => _passphrase_callback;
            set
            {
                _passphrase_callback = value;
                var err = archive_read_set_passphrase_callback(_handle, 0, _passphrase_callback);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_read_set_passphrase_callback), err);
                }
            }
        }

        private Delegates.progress_callback? ProgressCallback
        {
            get => _progress_callback;
            set
            {
                _progress_callback = value;
                archive_read_extract_set_progress_callback(_handle, _progress_callback, 0);
            }
        }

        public void ReadToFd(int fd_dest)
        {
            var err = archive_read_data_into_fd(_handle, fd_dest);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_data_into_fd), err);
            }
        }

        public ArchiveReader(
            TypedPointer<archive> handle,
            IList<IArchiveStream> streams,
            bool owned,
            ArchiveReaderOptions? opts
        )
            : base(handle, owned)
        {
            _opts = opts;
            _streams = streams.Select((s, i) => (i + 1, s))
                .ToDictionary(
                    // 1-based index identity
                    itm => itm.Item1,
                    itm => itm.s
                );

            _inputStream = new ArchiveInputStream(handle);

            // register stream identifiers
            foreach (var key in _streams.Keys)
            {
                archive_read_append_callback_data(handle, key);
            }

            _read_callback = (arch, data, outData) =>
            {
                if (data == 0) throw new ArgumentNullException(nameof(data));
                var stream = _streams[(int)data];
                return stream.Read(out outData.Value);
            };
            _seek_callback = (arch, data, offset, whence) =>
            {
                if (data == 0) throw new ArgumentNullException(nameof(data));
                var stream = _streams[(int)data];
                return stream.Seek(offset, whence);
            };
            _skip_callback = (arch, data, request) =>
            {
                if (data == 0) throw new ArgumentNullException(nameof(data));
                var stream = _streams[(int)data];
                return stream.Skip(request);
            };
            _close_callback = (arch, data) =>
            {
                if (data == 0) throw new ArgumentNullException(nameof(data));
                var stream = _streams[(int)data];
                return stream.Close();
            };
            _switch_callback = (arch, data1, data2) =>
            {
                if (data1 != 0)
                {
                    var stream1 = _streams[(int)data1];
                    stream1.Dispose();
                    _streams.Remove((int)data1);
                }
                if (data2 != 0)
                {
                    var stream2 = _streams[(int)data2];
                }
                return ArchiveError.OK;
            };

            archive_read_set_open_callback(handle, (arch, data) => ArchiveError.OK);
            archive_read_set_read_callback(handle, _read_callback);
            archive_read_set_seek_callback(handle, _seek_callback);
            archive_read_set_skip_callback(handle, _skip_callback);
            archive_read_set_close_callback(handle, _close_callback);
            archive_read_set_switch_callback(handle, _switch_callback);
            archive_read_open1(handle);

            CallbackData = new ArchiveCallbackData(_handle);
            Entries = new ArchiveEntryStream(this);
            ProgressCallback = (_) =>
            {
                OnProgress?.Invoke();
            };
        }

        public long HeaderPosition => archive_read_header_position(_handle);
        public int FormatCapabilities => archive_read_format_capabilities(_handle);

        public bool HasEncryptedEntries
        {
            get
            {
                var res = archive_read_has_encrypted_entries(_handle);
                if ((int)res < 0)
                {
                    throw new ArchiveOperationFailedException(nameof(archive_read_has_encrypted_entries), Enum.GetName(res) ?? "unknown failure");
                }
                return (int)res > 0 ? true : false;
            }
        }

        public void SetFormat(ArchiveFormat format)
        {
            var err = archive_read_set_format(_handle, format);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_set_format), err);
            }
        }

        public void AddPassphrase(string passphrase)
        {
            var err = archive_read_add_passphrase(_handle, passphrase);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_add_passphrase), err);
            }
        }

        public void AppendFilter(ArchiveFilter filter)
        {
            var err = archive_read_append_filter(_handle, filter);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_append_filter), err);
            }
        }

        public void AppendFilterProgram(string cmd)
        {
            var err = archive_read_append_filter_program(_handle, cmd);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_append_filter_program), err);
            }
        }

        public void AppendFilterProgramBySignature(string cmd, Memory<byte> signature)
        {
            using var bufRef = signature.Pin();
            nint dptr;
            unsafe
            {
                dptr = new nint(bufRef.Pointer);
            }
            var err = archive_read_append_filter_program_signature(_handle, cmd, dptr, (nuint)signature.Length);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_append_filter_program_signature), err);
            }
        }

        public void Extract(TypedPointer<archive_entry> entry, ArchiveExtractFlags flags)
        {
            var err = archive_read_extract(_handle, entry, flags);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_extract), err);
            }
        }

        public void Extract(TypedPointer<archive_entry> entry, ArchiveDiskWriter writer)
        {
            var err = archive_read_extract2(_handle, entry, writer);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_extract2), err);
            }
        }

        public void RequestFormatByProgram(string program)
        {
            var err = archive_read_support_filter_program(_handle, program);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_support_filter_program), err);
            }
        }

        private static Func<TypedPointer<archive>, ArchiveError>? GetRequestFormatFunc(ArchiveFormat format)
        {
            switch ((ArchiveFormat)((uint)format & 0xff0000))
            {
                case ArchiveFormat.AR:
                    return archive_read_support_format_ar;
                case ArchiveFormat.SEVENZIP:
                    return archive_read_support_format_7zip;
                case ArchiveFormat.CAB:
                    return archive_read_support_format_cab;
                case ArchiveFormat.CPIO:
                    return archive_read_support_format_cpio;
                case ArchiveFormat.EMPTY:
                    return archive_read_support_format_empty;
                case ArchiveFormat.ISO9660:
                    return archive_read_support_format_iso9660;
                case ArchiveFormat.LHA:
                    return archive_read_support_format_lha;
                case ArchiveFormat.MTREE:
                    return archive_read_support_format_mtree;
                case ArchiveFormat.RAR:
                    return archive_read_support_format_rar;
                case ArchiveFormat.RAR_V5:
                    return archive_read_support_format_rar5;
                case ArchiveFormat.RAW:
                    return archive_read_support_format_raw;
                case ArchiveFormat.TAR:
                    return archive_read_support_format_tar;
                case ArchiveFormat.WARC:
                    return archive_read_support_format_warc;
                case ArchiveFormat.XAR:
                    return archive_read_support_format_xar;
                case ArchiveFormat.ZIP:
                    return archive_read_support_format_zip;
                default:
                    return null;
            }
        }

        private static Func<TypedPointer<archive>, ArchiveError>? GetFilterSetter(ArchiveFilter filter)
        {

            switch (filter)
            {
                case ArchiveFilter.NONE:
                    return archive_read_support_filter_none;
                case ArchiveFilter.GZIP:
                    return archive_read_support_filter_gzip;
                case ArchiveFilter.BZIP2:
                    return archive_read_support_filter_bzip2;
                case ArchiveFilter.COMPRESS:
                    return archive_read_support_filter_compress;
                case ArchiveFilter.LZMA:
                    return archive_read_support_filter_lzma;
                case ArchiveFilter.XZ:
                    return archive_read_support_filter_xz;
                case ArchiveFilter.UU:
                    return archive_read_support_filter_uu;
                case ArchiveFilter.RPM:
                    return archive_read_support_filter_rpm;
                case ArchiveFilter.LZIP:
                    return archive_read_support_filter_lzip;
                case ArchiveFilter.LRZIP:
                    return archive_read_support_filter_lrzip;
                case ArchiveFilter.LZOP:
                    return archive_read_support_filter_lzop;
                case ArchiveFilter.GRZIP:
                    return archive_read_support_filter_grzip;
                case ArchiveFilter.LZ4:
                    return archive_read_support_filter_lz4;
                case ArchiveFilter.ZSTD:
                    return archive_read_support_filter_zstd;
                default:
                    return null;
            }
        }

        private static void RequestFilter(TypedPointer<archive> handle, ArchiveFilter filter)
        {
            var func = GetFilterSetter(filter);
            var err = (func == null)
                ? archive_read_support_filter_by_code(handle, filter)
                : func(handle);

            if (err != ArchiveError.OK)
            {
                var methodName = (func == null)
                    ? nameof(archive_read_support_filter_by_code)
                    : func.Method.Name;
                throw new ArchiveOperationFailedException(handle, methodName, err);
            }
        }

        private static void RequestFormat(TypedPointer<archive> handle, ArchiveFormat format)
        {
            var func = GetRequestFormatFunc(format);
            var err = (func == null)
                ? archive_read_support_format_by_code(handle, format)
                : func(handle);

            if (err != ArchiveError.OK)
            {
                var methodName = (func == null)
                    ? nameof(archive_read_support_format_by_code)
                    : func.Method.Name;
                throw new ArchiveOperationFailedException(handle, methodName, err);
            }
        }

        public void RequestFilter(ArchiveFilter filter)
        {
            RequestFilter(filter);
        }

        public void RequestFormat(ArchiveFormat format)
        {
            RequestFormat(_handle, format);
        }

        public void RequestFormatGnuTar()
        {
            var err = archive_read_support_format_gnutar(_handle);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_support_format_gnutar), err);
            }
        }

        public void RequestFormatZipStreamable()
        {
            var err = archive_read_support_format_zip_streamable(_handle);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_support_format_zip_streamable), err);
            }
        }

        public void RequestFormatZipSeekable()
        {
            var err = archive_read_support_format_zip_seekable(_handle);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_support_format_zip_seekable), err);
            }
        }

        public void RequestFormatByProgramSignature(string program, Memory<byte> signature)
        {
            using var bufRef = signature.Pin();
            nint dptr;
            unsafe
            {
                dptr = new nint(bufRef.Pointer);
            }
            var err = archive_read_support_filter_program_signature(_handle, program, dptr, (nuint)signature.Length);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_support_filter_program_signature), err);
            }
        }

        private static TypedPointer<archive> NewHandle(ArchiveReaderOptions? opts = null)
        {
            var handle = archive_read_new();
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_new), "out of memory");
            }
            if (opts == null)
            {
                var err = archive_read_support_format_all(handle);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(handle, nameof(archive_read_support_format_all), err);
                }
                err = archive_read_support_filter_all(handle);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(handle, nameof(archive_read_support_filter_all), err);
                }
            } else
            {
                foreach (var format in opts.EnableFormats)
                {
                    RequestFormat(handle, format);
                }
                foreach (var filter in opts.EnableFilters)
                {
                    RequestFilter(handle, filter);
                }
            }
            return handle;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            { }

            archive_read_close(_handle);
            if (_owned)
            {
                archive_read_free(_handle);
            }
            _disposed = true;
        }

        ~ArchiveReader()
        {
            Dispose(false);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetFormatOption(string module, string option, string value)
        {
            var err = archive_read_set_format_option(_handle, module, option, value);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_set_format_option), err);
            }
        }

        public void SetFilterOption(string module, string option, string value)
        {
            var err = archive_read_set_filter_option(_handle, module, option, value);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_set_filter_option), err);
            }
        }

        public void SetOption(string module, string option, string value)
        {
            var err = archive_read_set_option(_handle, module, option, value);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_set_option), err);
            }
        }

        public void SetOptions(string options)
        {
            var err = archive_read_set_options(_handle, options);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_set_options), err);
            }
        }
    }
}
