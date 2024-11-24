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
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveDiskReader : Archive
    {
        private static readonly Delegates.archive_lookup_cleanup_callback _dummy_cleanup = (data) => { };
        private Delegates.archive_user_name_lookup_callback? _uname_lookup;
        private Delegates.archive_group_name_lookup_callback? _gname_lookup;

        private bool RestoreAccessTime
        {
            set
            {
                if (!value)
                {
                    throw new ArgumentException("use SetBehavior to clear this flag");
                } else
                {
                    var err = archive_read_disk_set_atime_restored(_handle);
                    if (err != ArchiveError.OK)
                    {
                        throw new ArchiveOperationFailedException(_handle, nameof(archive_read_disk_set_atime_restored), err);
                    }
                }
            }
        }

        public void SetBehavior(ArchiveReadDiskFlags flags)
        {
            var err = archive_read_disk_set_behavior(_handle, flags);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_disk_set_behavior), err);
            }
        }

        public bool CanDescend
        {
            get
            {
                var res = archive_read_disk_can_descend(_handle);
                if (res < 0)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_read_disk_can_descend), (ArchiveError)res);
                }
                return res == 1;
            }
        }

        public int CurrentFilesystem
        {
            get => archive_read_disk_current_filesystem(_handle);
        }

        public bool CurrentFilesystemIsRemote
        {
            get
            {
                var res = archive_read_disk_current_filesystem_is_remote(_handle);
                if (res < 0)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_read_disk_current_filesystem_is_remote), (ArchiveError)res);
                }
                return res == 1;
            }
        }

        public bool CurrentFilesystemIsSynthetic
        {
            get
            {
                var res = archive_read_disk_current_filesystem_is_synthetic(_handle);
                if (res < 0)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_read_disk_current_filesystem_is_synthetic), (ArchiveError)res);
                }
                return res == 1;
            }
        }

        public ArchiveSymlinkMode SymlinkMode
        {
            set
            {
                Func<TypedPointer<archive>, ArchiveError> func = value switch
                {
                    ArchiveSymlinkMode.Logical => archive_read_disk_set_symlink_logical,
                    ArchiveSymlinkMode.Physical => archive_read_disk_set_symlink_physical,
                    ArchiveSymlinkMode.Hybrid => archive_read_disk_set_symlink_hybrid,
                    _ => throw new ArgumentException($"Invalid Symlink Mode {Enum.GetName(value)}")
                };
                var err = func(_handle);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(_handle, func.Method.Name, err);
                }
            }
        }

        public ArchiveDiskReader(TypedPointer<archive> handle, bool owned)
            : base(handle, owned)
        { }

        public ArchiveDiskReader() : this(NewHandle(), true)
        { }

        public ArchiveDiskReader(string pathName) : this(NewHandle(), true)
        {
            var err = archive_read_disk_open_w(_handle, pathName);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_disk_open_w), err);
            }
        }

        public Delegates.archive_user_name_lookup_callback UserNameLookup
        {
            set
            {
                _uname_lookup = value;
                archive_read_disk_set_uname_lookup(
                    _handle, 0,
                    _uname_lookup,
                    _dummy_cleanup);
            }
        }

        public Delegates.archive_group_name_lookup_callback GroupNameLookup
        {
            set
            {
                _gname_lookup = value;
                archive_read_disk_set_gname_lookup(
                    _handle, 0,
                    _gname_lookup,
                    _dummy_cleanup);
            }
        }

        public void SetStandardLookup()
        {
            var err = archive_read_disk_set_standard_lookup(_handle);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_disk_set_standard_lookup), err);
            }
        }

        public string? GetUserName(long uid)
        {
            return archive_read_disk_uname(_handle, uid);
        }

        public string? GetGroupName(long gid)
        {
            return archive_read_disk_gname(_handle, gid);
        }

        private static TypedPointer<archive> NewHandle()
        {
            var handle = archive_read_disk_new();
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_disk_new), "out of memory");
            }
            return handle;
        }
    }
}
