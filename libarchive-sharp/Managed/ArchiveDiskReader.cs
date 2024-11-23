#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
﻿using Smx.SharpIO.Memory;
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

        public ArchiveDiskReader(TypedPointer<archive> handle, bool owned)
            : base(handle, owned)
        { }

        public ArchiveDiskReader() : this(NewHandle(), true)
        { }

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
