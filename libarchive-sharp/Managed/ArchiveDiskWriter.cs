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
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.Storage.FileSystem;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveDiskWriter : ArchiveWriter
    {
        private readonly ArchiveOutputStream _outputStream;

        public ArchiveOutputStream OutputStream => _outputStream;

        public ArchiveDiskWriter() : this(NewHandle(), 0, true)
        { }

        private Delegates.archive_group_lookup_callback? _group_lookup;
        private Delegates.archive_user_lookup_callback? _user_lookup;

        public Delegates.archive_user_lookup_callback? UserLookup
        {
            get => _user_lookup;
            set
            {
                _user_lookup = value;
                var err = archive_write_disk_set_user_lookup(_handle, 0, _user_lookup, null);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_write_disk_set_user_lookup), err);
                }
            }
        }

        public Delegates.archive_group_lookup_callback? GroupLookup
        {
            get => _group_lookup;
            set
            {
                _group_lookup = value;
                var err = archive_write_disk_set_group_lookup(_handle, 0, _group_lookup, null);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_write_disk_set_group_lookup), err);
                }
            }
        }

        public ArchiveDiskWriter(
            TypedPointer<archive> handle,
            ArchiveExtractFlags flags,
            bool owned
        ) : base(handle, owned)
        {
            _outputStream = new ArchiveOutputStream(_handle);

            var err = archive_write_disk_set_options(handle, flags);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(handle, nameof(archive_write_disk_set_options), err);
            }
            archive_write_disk_set_standard_lookup(handle);
        }

        public void SetSkipFile(long device, long inode)
        {
            var err = archive_write_disk_set_skip_file(_handle, device, inode);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_disk_set_skip_file), err);
            }
        }

        public long GetRealGid(string name, long id)
        {
            var res = archive_write_disk_gid(_handle, name, id);
            if (res < 0)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_disk_gid), (ArchiveError)res);
            }
            return res;
        }

        public long GetRealUid(string name, long id)
        {
            var res = archive_write_disk_uid(_handle, name, id);
            if (res < 0)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_write_disk_uid), (ArchiveError)res);
            }
            return res;
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
