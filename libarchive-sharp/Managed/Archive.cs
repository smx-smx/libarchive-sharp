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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class Archive : IDisposable
    {
        public static int VersionNumber => archive_version_number();
        public static string VersionString => archive_version_string();
        public static string VersionDetails => archive_version_details();
        public static string VersionZlib => archive_zlib_version();
        public static string VersionLzma => archive_liblzma_version();
        public static string VersionBzlib => archive_bzlib_version();
        public static string VersionLz4 => archive_liblz4_version();
        public static string VersionZstd => archive_libzstd_version();

        private readonly TypedPointer<archive> _handle;
        protected bool _disposed;
        protected readonly bool _owned;

        public TypedPointer<archive> ArchiveHandle => _handle;

        public static implicit operator TypedPointer<archive>(Archive self) => self.ArchiveHandle;

        public long FilterCount => archive_filter_count(_handle);
        public string GetFilterName(int idx)
        {
            return archive_filter_name(_handle, idx);
        }
        public long GetFilterBytes(int idx)
        {
            return archive_filter_bytes(_handle, idx);
        }
        public int GetFilterCode(int idx)
        {
            return archive_filter_code(_handle, idx);
        }

        protected Archive(TypedPointer<archive> handle, bool owned)
        {
            _handle = handle;
            _owned = owned;
        }

        public void CopyError(TypedPointer<archive> from)
        {
            archive_copy_error(_handle, from);
        }

        public int LastError => archive_errno(_handle);
        public string LastErrorString => archive_error_string(_handle);

        public ArchiveFormat Format => archive_format(_handle);
        public string FormatName => archive_format_name(_handle);
        public ArchiveCompression Compression => archive_compression(_handle);
        public string CompressionName => archive_compression_name(_handle);

        public long PositionCompressed => archive_position_compressed(_handle);
        public long PositionUncompressed => archive_position_uncompressed(_handle);

        public void SetError(int errorNumber, string errorMessage)
        {
            // escape sprintf string
            errorMessage = errorMessage.Replace("%", "%%");
            archive_set_error(_handle, errorNumber, errorMessage, __arglist());
        }

        public void ClearError()
        {
            archive_clear_error(_handle);
        }

        public int FileCount => archive_file_count(_handle);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }

                if (_owned)
                {
                    archive_free(_handle);
                }

                _disposed = true;
            }
        }

        ~Archive()
        {
            Dispose(disposing: false);
        }

        public virtual void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
