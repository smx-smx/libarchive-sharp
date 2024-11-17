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
    public class ArchiveEntryAcl
    {
        private readonly TypedPointer<archive_entry> _entry;
        private readonly TypedPointer<archive_acl> _handle;

        public ArchiveEntryAcl(TypedPointer<archive_entry> entry, TypedPointer<archive_acl> handle)
        {
            _entry = entry;
            _handle = handle;
        }

        public ArchiveEntryAcl(TypedPointer<archive_entry> entry)
            : this(entry, NewHandle(entry))
        { }

        private static TypedPointer<archive_acl> NewHandle(TypedPointer<archive_entry> entry)
        {
            var handle = archive_entry_acl(entry);
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_entry_acl), "corrupted memory");
            }
            return handle;
        }

        public ArchiveEntryAclType Types => archive_entry_acl_types(_entry);

        public string ToString(ArchiveEntryAclType flags)
        {
            return archive_entry_acl_to_text_w(_entry, out var textLength, flags);
        }

        public override string ToString()
        {
            return archive_entry_acl_to_text_w(_entry, out var textLength, ArchiveEntryAclType.All);
        }
    }
}
