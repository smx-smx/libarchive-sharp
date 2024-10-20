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
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveWriterOptions
    {
        public bool LeaveOpen { get; set; } = false;
        public required ArchiveFormat Format { get; set; }
        public ArchiveFilter? Filter { get; set; } = null;
    }

    public class ArchiveWriter : IDisposable
    {
        private readonly TypedPointer<archive> _handle;
        private readonly bool _owned;
        private bool _disposed;

        private ArchiveDataStream? _stream;
        private readonly Delegates.archive_open_callback? _open_callback;
        private readonly Delegates.archive_write_callback? _write_callback;
        private readonly Delegates.archive_close_callback? _close_callback;
        private readonly Delegates.archive_free_callback? _free_callback;

        public ArchiveWriter(
            Stream stream,
            ArchiveFormat format,
            ArchiveCompression? compression = null,
            ArchiveFilter? filter = null,
            bool leaveOpen = false
        )
            : this(
                  handle: NewHandle(),
                  stream: new ArchiveDataStream(stream, leaveOpen),
                  owned: true,
                  format: format,
                  compression: compression,
                  filter: filter
            )
        { }

        public ArchiveWriter(
            TypedPointer<archive> handle,
            bool owned
        )
        {
            _handle = handle;
            _owned = owned;
        }

        public ArchiveWriter(
            TypedPointer<archive> handle,
            ArchiveDataStream stream,
            bool owned,
            ArchiveFormat format,
            ArchiveCompression? compression = null,
            ArchiveFilter? filter = null)
        {
            _handle = handle;
            _owned = owned;
            _disposed = false;
            _stream = stream;

            if (GetArchiveFormatSetter(format)(handle) != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(GetArchiveCompressionSetter), "failed to set archive format");
            }
            if (filter != null)
            {
                if (GetArchiveFilterSetter(filter.Value)(handle) != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(nameof(GetArchiveFilterSetter), "failed to set archive filter");
                }
            }

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
                handle, 0,
                archive_open_callback: _open_callback,
                archive_write_callback: _write_callback,
                archive_close_callback: _close_callback,
                archive_free_callback: _free_callback
            );
        }

        public void AddEntry(SharedPtr<ArchiveEntry> entryRef, Stream? stream = null)
        {
            var entry = entryRef.AddRef();

            if (stream != null && !entry.HasSize)
            {
                entry.Size = stream.Length;
            }

            if (archive_write_header(_handle, entry.Handle) != ArchiveError.OK)
            {
                throw new InvalidOperationException("failed to write archive entry");
            }
            entryRef.Release();

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
                    throw new ArchiveOperationFailedException(nameof(archive_write_data), "failed to write data");
                }
            } while (nRead > 0);

            if (archive_write_finish_entry(_handle) != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(archive_write_finish_entry), "failed to close entry");
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

        private static Func<TypedPointer<archive>, ArchiveError> GetArchiveCompressionSetter(ArchiveCompression compression)
        {
            return compression switch
            {
                ArchiveCompression.none => archive_write_set_compression_none,
                ArchiveCompression.compress => archive_write_set_compression_compress,
                ArchiveCompression.xz => archive_write_set_compression_xz,
                ArchiveCompression.lzma => archive_write_set_compression_lzma,
                // $FIXME
                //ArchiveCompression.program => archive_write_set_compression_program,
                ArchiveCompression.lzip => archive_write_set_compression_lzip,
                ArchiveCompression.bzip2 => archive_write_set_compression_bzip2,
                _ => throw new NotSupportedException(Enum.GetName(compression))
            };
        }

        private static Func<TypedPointer<archive>, ArchiveError> GetArchiveFilterSetter(ArchiveFilter filter)
        {
            return filter switch
            {
                ArchiveFilter.b64encode => archive_write_add_filter_b64encode,
                ArchiveFilter.bzip2 => archive_write_add_filter_bzip2,
                ArchiveFilter.compress => archive_write_add_filter_compress,
                ArchiveFilter.gzip => archive_write_add_filter_gzip,
                ArchiveFilter.lrzip => archive_write_add_filter_lrzip,
                ArchiveFilter.lz4 => archive_write_add_filter_lz4,
                ArchiveFilter.lzip => archive_write_add_filter_lzip,
                ArchiveFilter.lzma => archive_write_add_filter_lzma,
                ArchiveFilter.lzop => archive_write_add_filter_lzop,
                ArchiveFilter.uuencode => archive_write_add_filter_uuencode,
                ArchiveFilter.xz => archive_write_add_filter_xz,
                ArchiveFilter.zstd => archive_write_add_filter_zstd,
                _ => throw new NotSupportedException(Enum.GetName(filter))
            };
        }

        private static Func<TypedPointer<archive>, ArchiveError> GetArchiveFormatSetter(ArchiveFormat format)
        {
            return format switch
            {
                ArchiveFormat.sevenzip => archive_write_set_format_7zip,
                ArchiveFormat.ar_bsd => archive_write_set_format_ar_bsd,
                ArchiveFormat.ar_svr4 => archive_write_set_format_ar_svr4,
                ArchiveFormat.cpio => archive_write_set_format_cpio,
                ArchiveFormat.cpio_bin => archive_write_set_format_cpio_bin,
                ArchiveFormat.cpio_newc => archive_write_set_format_cpio_newc,
                ArchiveFormat.cpio_odc => archive_write_set_format_cpio_odc,
                ArchiveFormat.cpio_pwb => archive_write_set_format_cpio_pwb,
                ArchiveFormat.gnutar => archive_write_set_format_gnutar,
                ArchiveFormat.iso9660 => archive_write_set_format_iso9660,
                ArchiveFormat.mtree => archive_write_set_format_mtree,
                ArchiveFormat.mtree_classic => archive_write_set_format_mtree_classic,
                ArchiveFormat.pax => archive_write_set_format_pax,
                ArchiveFormat.pax_restricted => archive_write_set_format_pax_restricted,
                ArchiveFormat.raw => archive_write_set_format_raw,
                ArchiveFormat.shar => archive_write_set_format_shar,
                ArchiveFormat.shar_dump => archive_write_set_format_shar_dump,
                ArchiveFormat.ustar => archive_write_set_format_ustar,
                ArchiveFormat.v7tar => archive_write_set_format_v7tar,
                ArchiveFormat.warc => archive_write_set_format_warc,
                ArchiveFormat.xar => archive_write_set_format_xar,
                ArchiveFormat.zip => archive_write_set_format_zip,
                _ => throw new NotSupportedException(Enum.GetName(format))
            };
        }

        protected virtual void Dispose(bool disposing)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
