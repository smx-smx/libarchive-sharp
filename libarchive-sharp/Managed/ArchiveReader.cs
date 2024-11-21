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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace libarchive.Managed
{
    public class ArchiveOperationFailedException : Exception
    {
        public ArchiveOperationFailedException(TypedPointer<archive> handle, string func, ArchiveError err)
            : base($"{func} failed: {Enum.GetName(err)} ({archive_error_string(handle)})")
        { }
        public ArchiveOperationFailedException(string func, ArchiveError err)
            : base($"{func} failed: {Enum.GetName(err)}")
        { }

        public ArchiveOperationFailedException(string func, string err)
            : base($"{func} failed: {err}")
        { }

        public ArchiveOperationFailedException(TypedPointer<archive> handle, string func)
            : base($"{func} failed: {archive_errno(handle):X}: {archive_error_string(handle)}")
        { }

        public ArchiveOperationFailedException(TypedPointer<archive> handle, string func, string err)
            : base($"{func} failed: {err}. ({archive_errno(handle):X}: {archive_error_string(handle)})")
        { }
    }

    public class ArchiveReaderOptions
    {
        public ICollection<ArchiveFormat> EnableFormats { get; set; } = Array.Empty<ArchiveFormat>();
        public ICollection<ArchiveFilter> EnableFilters { get; set; } = Array.Empty<ArchiveFilter>();
    }

    public class ArchiveReader : Archive, IDisposable
    {
        private readonly TypedPointer<archive> _handle;
        private readonly ArchiveInputStream _inputStream;

        private IDictionary<int, IArchiveStream> _streams;
        private readonly Delegates.archive_read_callback _read_callback;
        private readonly Delegates.archive_close_callback _close_callback;
        private readonly Delegates.archive_seek_callback _seek_callback;
        private readonly Delegates.archive_skip_callback _skip_callback;
        private readonly Delegates.archive_switch_callback _switch_callback;

        public delegate void SwitchRequestedDelegate();
        public event SwitchRequestedDelegate? OnSwitchRequested;

        public ArchiveInputStream InputStream => _inputStream;

        public ArchiveReader(Stream stream, ArchiveReaderOptions? opts = null)
            : this(NewHandle(opts), [new ArchiveDataStream(stream)], true)
        {
        }

        public ArchiveReader(ICollection<Stream> streams, ArchiveReaderOptions? opts = null)
            : this(NewHandle(opts), streams.Select(s => new ArchiveDataStream(s) as IArchiveStream).ToList(), true)
        {
        }

        public ArchiveReader(ICollection<Lazy<Stream>> streams, ArchiveReaderOptions? opts = null)
            : this(NewHandle(opts), streams.Select(s => new ArchiveDataStreamLazy(s) as IArchiveStream).ToList(), true)
        { }

        public ArchiveReader(ICollection<ArchiveDataStream> streams, ArchiveReaderOptions? opts = null)
            : this(NewHandle(opts), streams.Cast<IArchiveStream>().ToList(), true)
        { }

        public void ReadToFd(int fd_dest)
        {
            var err = archive_read_data_into_fd(_handle, fd_dest);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_data_into_fd), err);
            }
        }

        public ArchiveReader(TypedPointer<archive> handle, IList<IArchiveStream> streams, bool owned)
            : base(handle, owned)
        {
            _handle = handle;
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

            //archive_read_set_open_callback(handle, (arch, data) => ArchiveError.OK);
            archive_read_set_read_callback(handle, _read_callback);
            archive_read_set_seek_callback(handle, _seek_callback);
            archive_read_set_skip_callback(handle, _skip_callback);
            archive_read_set_close_callback(handle, _close_callback);
            archive_read_set_switch_callback(handle, _switch_callback);
            archive_read_open1(handle);
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

        public IEnumerable<ArchiveEntry> Entries
        {
            get
            {
                while (archive_read_next_header(_handle, out var entry) == ArchiveError.OK)
                {
                    yield return new ArchiveEntry(_handle, entry, false);
                }
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
                archive_read_support_format_all(handle);
                archive_read_support_filter_all(handle);
            } else
            {
                foreach (var format in opts.EnableFormats)
                {
                    archive_read_support_format_by_code(handle, format);
                }
                foreach (var filter in opts.EnableFilters)
                {
                    archive_read_support_filter_by_code(handle, filter);
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
