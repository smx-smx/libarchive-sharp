#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using Smx.SharpIO.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveWriterOptions
    {
        public bool LeaveOpen { get; set; } = false;
        public required ArchiveFormat Format { get; set; }
        public ArchiveFilter? Filter { get; set; } = null;
    }

    public class ArchiveWriter : Archive, IDisposable
    {
        private Delegates.archive_open_callback? _open_callback;
        private Delegates.archive_write_callback? _write_callback;
        private Delegates.archive_close_callback? _close_callback;
        private Delegates.archive_free_callback? _free_callback;

        public int BytesPerBlock
        {
            get => archive_write_get_bytes_per_block(_handle);
            set
            {
                var err = archive_write_set_bytes_per_block(_handle, value);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_write_set_bytes_per_block), err);
                }
            }
        }

        public int BytesInLastBlock
        {
            get => archive_write_get_bytes_in_last_block(_handle);
            set
            {
                var err = archive_write_set_bytes_in_last_block(_handle, value);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_write_set_bytes_in_last_block), err);
                }
            }
        }

        public ArchiveWriter(TypedPointer<archive> handle, bool owned)
            : base(handle, owned)
        {
        }

        public ArchiveWriter(
            Stream stream,
            ArchiveFormat format,
            ArchiveCompression? compression = null,
            ICollection<ArchiveFilter>? filters = null,
            bool leaveOpen = false
        )
            : this(
                  handle: NewHandle(),
                  stream: new ArchiveDataStream(stream, leaveOpen),
                  owned: true,
                  format: format,
                  filters: filters,
                  compression: compression
            )
        { }

        private Func<TypedPointer<archive>, ArchiveError>? GetFilterSetter(string filterName)
        {
            switch (filterName)
            {
                case ArchiveFilterNames.base64:
                    return archive_write_add_filter_b64encode;
                case ArchiveFilterNames.bzip2:
                    return archive_write_add_filter_bzip2;
                case ArchiveFilterNames.compress:
                    return archive_write_add_filter_compress;
                case ArchiveFilterNames.grzip:
                    return archive_write_add_filter_grzip;
                case ArchiveFilterNames.gzip:
                    return archive_write_add_filter_gzip;
                case ArchiveFilterNames.lrzip:
                    return archive_write_add_filter_lrzip;
                case ArchiveFilterNames.lz4:
                    return archive_write_add_filter_lz4;
                case ArchiveFilterNames.lzip:
                    return archive_write_add_filter_lzip;
                case ArchiveFilterNames.lzma:
                    return archive_write_add_filter_lzma;
                case ArchiveFilterNames.lzop:
                    return archive_write_add_filter_lzop;
                case ArchiveFilterNames.uuencode:
                    return archive_write_add_filter_uuencode;
                case ArchiveFilterNames.xz:
                    return archive_write_add_filter_xz;
                case ArchiveFilterNames.zstd:
                    return archive_write_add_filter_zstd;
                default:
                    return null;
            }
        }

        private Func<TypedPointer<archive>, ArchiveError>? GetFormatSetter(string formatName)
        {
            switch (formatName)
            {
                case ArchiveFormatNames.ArBsd:
                    return archive_write_set_format_ar_bsd;
                case ArchiveFormatNames.ArGnu:
                case ArchiveFormatNames.ArSvr4:
                    return archive_write_set_format_ar_svr4;
                case ArchiveFormatNames.Bin:
                    return archive_write_set_format_cpio_bin;
                case ArchiveFormatNames.BsdTar:
                case ArchiveFormatNames.Rpax:
                case ArchiveFormatNames.Paxr:
                    return archive_write_set_format_pax_restricted;
                case ArchiveFormatNames.Cd9660:
                case ArchiveFormatNames.Iso:
                case ArchiveFormatNames.Iso9660:
                    return archive_write_set_format_iso9660;
                case ArchiveFormatNames.Cpio:
                    return archive_write_set_format_cpio;
                case ArchiveFormatNames.GnuTar:
                    return archive_write_set_format_gnutar;
                case ArchiveFormatNames.Mtree:
                    return archive_write_set_format_mtree;
                case ArchiveFormatNames.MtreeClassic:
                    return archive_write_set_format_mtree_classic;
                case ArchiveFormatNames.NewC:
                    return archive_write_set_format_cpio_newc;
                case ArchiveFormatNames.Odc:
                    return archive_write_set_format_cpio_odc;
                case ArchiveFormatNames.OldTar:
                case ArchiveFormatNames.V7Tar:
                case ArchiveFormatNames.V7:
                    return archive_write_set_format_v7tar;
                case ArchiveFormatNames.Pax:
                case ArchiveFormatNames.Posix:
                    return archive_write_set_format_pax;
                case ArchiveFormatNames.Pwb:
                    return archive_write_set_format_cpio_pwb;
                case ArchiveFormatNames.Raw:
                    return archive_write_set_format_raw;
                case ArchiveFormatNames.SevenZip:
                    return archive_write_set_format_7zip;
                case ArchiveFormatNames.Shar:
                    return archive_write_set_format_shar;
                case ArchiveFormatNames.SharDump:
                    return archive_write_set_format_shar_dump;
                case ArchiveFormatNames.UsTar:
                    return archive_write_set_format_ustar;
                case ArchiveFormatNames.Warc:
                    return archive_write_set_format_warc;
                case ArchiveFormatNames.Xar:
                    return archive_write_set_format_xar;
                case ArchiveFormatNames.Zip:
                    return archive_write_set_format_zip;
                default:
                    return null;
            }
        }

        private void SetFormat(string format)
        {
            var setter = GetFormatSetter(format);

            var res = (setter == null)
                    ? archive_write_set_format_by_name(_handle, format)
                    : setter(_handle);

            if (res != ArchiveError.OK)
            {
                var methodName = (setter == null)
                    ? nameof(archive_write_set_format_by_name)
                    : setter.Method.Name;
                throw new ArchiveOperationFailedException(_handle, methodName, "failed to set archive format");
            }
        }

        private void SetFormat(ArchiveFormat format)
        {
            if (archive_write_set_format(_handle, format) != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_set_format), "failed to set archive format");
            }
        }

        private void AddFilter(string filter)
        {
            var setter = GetFilterSetter(filter);

            var res = (setter == null)
                ? archive_write_add_filter_by_name(_handle, filter)
                : setter(_handle);

            if (res != ArchiveError.OK)
            {
                var metodName = (setter == null)
                    ? nameof(archive_write_add_filter_by_name)
                    : setter.Method.Name;
                throw new ArchiveOperationFailedException(_handle, metodName, "failed to add archive filter");
            }
        }

        private void AddFilter(ArchiveFilter filter)
        {
            if (archive_write_add_filter(_handle, filter) != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_add_filter), "failed to add archive filter");
            }
        }

        private void Setup(ArchiveDataStream stream)
        {
            _open_callback = (arch, data) => ArchiveError.OK;
            _write_callback = (arch, data, buff, len) =>
            {
                ReadOnlySpan<byte> bufSpan;
                unsafe
                {
                    bufSpan = new ReadOnlySpan<byte>(buff.ToPointer(), (int)len);
                }
                return stream.Write(bufSpan);
            };
            _close_callback = (arch, data) =>
            {
                stream.Dispose();
                return ArchiveError.OK;
            };
            _free_callback = (arch, data) => ArchiveError.OK;

            archive_write_open2(
                _handle, 0,
                archive_open_callback: _open_callback,
                archive_write_callback: _write_callback,
                archive_close_callback: _close_callback,
                archive_free_callback: _free_callback
            );
        }

        private ArchiveWriter(
            TypedPointer<archive> handle,
            ArchiveDataStream stream,
            bool owned
        ) : base(handle, owned)
        {
            _disposed = false;
        }

        public void AddFilterProgram(string cmd)
        {
            var res = archive_write_add_filter_program(_handle, cmd);
            if (res != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_add_filter_program), res);
            }
        }

        public ArchiveWriter(
            TypedPointer<archive> handle,
            ArchiveDataStream stream,
            bool owned,
            string format,
            ICollection<string>? filters = null,
            ArchiveCompression? compression = null
        ) : this(handle, stream, owned)
        {
            archive_write_set_format_by_name(_handle, format);
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    AddFilter(filter);
                }
            }
            Setup(stream);
        }

        public ArchiveWriter(
            TypedPointer<archive> handle,
            ArchiveDataStream stream,
            bool owned,
            ArchiveFormat format,
            ICollection<ArchiveFilter>? filters = null,
            ArchiveCompression? compression = null
        ) : this(handle, stream, owned)
        {
            var res = archive_write_set_format(handle, format);
            if (res != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(handle, nameof(archive_write_set_format), res);
            }
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    AddFilter(filter);
                }
            }
            Setup(stream);
        }

        public void AddEntry(ArchiveEntryItem entryRef, Stream? stream = null)
        {
            AddEntry(entryRef.Header, stream);
        }

        public void AddEntry(SharedPtr<ArchiveEntry> entryRef, Stream? stream = null)
        {
            var entry = entryRef.AddRef();
            try
            {
                if (stream != null && entry.Size == null)
                {
                    entry.Size = stream.Length;
                }

                if (archive_write_header(_handle, entry) != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_write_header), "failed to write archive entry");
                }
            } finally
            {
                entryRef.Release();
            }

            if (stream == null)
            {
                return;
            }

            var buf = new Memory<byte>(new byte[ArchiveConstants.BUFFER_SIZE]);
            nint bufferHandle;
            unsafe
            {
                bufferHandle = new nint(Unsafe.AsPointer(ref buf.Span.GetPinnableReference()));
            }

            int nRead;
            do
            {
                nRead = stream.Read(buf.Span);
                if (nRead < 1) break;
                var nWritten = archive_write_data(_handle, bufferHandle, (nuint)nRead);
                if (nWritten < 0)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_write_data), "failed to write data");
                }
            } while (nRead > 0);

            if (archive_write_finish_entry(_handle) != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_finish_entry), "failed to close entry");
            }
        }

        public void AddEntry(ArchiveEntry entry, Stream? stream = null)
        {
            AddEntry(new SharedPtr<ArchiveEntry>(entry), stream);
        }

        private static TypedPointer<archive> NewHandle()
        {
            var handle = archive_write_new();
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_write_new), "out of memory");
            }
            return handle;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            { }



            archive_write_close(_handle);
            if (_owned)
            {
                archive_write_free(_handle);
            }

            _disposed = true;
        }

        ~ArchiveWriter()
        {
            Dispose(false);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetFailure()
        {
            archive_write_fail(_handle);
        }

        public void SetFormatOption(string module, string option, string value)
        {
            var err = archive_write_set_format_option(_handle, module, option, value);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_set_format_option), err);
            }
        }

        public void SetFilterOption(string module, string option, string value)
        {
            var err = archive_write_set_filter_option(_handle, module, option, value);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_set_filter_option), err);
            }
        }

        public void SetOption(string module, string option, string value)
        {
            var err = archive_write_set_option(_handle, module, option, value);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_set_option), err);
            }
        }

        public void SetOptions(string options)
        {
            var err = archive_write_set_options(_handle, options);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_set_options), err);
            }
        }
    }
}
