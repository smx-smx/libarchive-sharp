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
using Windows.Win32.Storage.FileSystem;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveDiskWriter : ArchiveWriter
    {
        private readonly TypedPointer<archive> _handle;
        private readonly bool _owned;
        private bool _disposed;
        private readonly ArchiveOutputStream _outputStream;

        public ArchiveOutputStream OutputStream => _outputStream;

        public ArchiveDiskWriter() : this(NewHandle(), 0, true)
        { }

        public ArchiveDiskWriter(
            TypedPointer<archive> handle,
            ArchiveExtractFlags flags,
            bool owned
        ) : base(handle, owned)
        {
            _handle = handle;
            _owned = owned;
            _disposed = false;
            _outputStream = new ArchiveOutputStream(_handle);

            if (archive_write_disk_set_options(handle, flags) != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(handle, nameof(archive_write_disk_set_options), "failed to set write disk flags");
            }
            archive_write_disk_set_standard_lookup(handle);
        }

        private static TypedPointer<archive> NewHandle()
        {
            var handle = archive_write_disk_new();
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_write_disk_new), "out of memory");
            }
            return handle;
        }
    }
}
