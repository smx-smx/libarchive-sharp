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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public record ArchiveEntryAcl(int Type, int Permset, int Tag, int Qual, string Name);

    public class ArchiveEntryAclList : ICollection<ArchiveEntryAcl>
    {
        private const ArchiveEntryAclType ACL_ALL = (ArchiveEntryAclType)(-1);

        private readonly TypedPointer<archive_entry> _handle;

        public ArchiveEntryAclType AclTypeFlags { get; private set; }

        public ArchiveEntryAclList(TypedPointer<archive_entry> handle, ArchiveEntryAclType want_type)
        {
            _handle = handle;
            AclTypeFlags = want_type;
        }

        public int GetCount(ArchiveEntryAclType type) => archive_entry_acl_count(_handle, type);

        public bool IsReadOnly => throw new NotImplementedException();

        public int Count => GetCount(AclTypeFlags);

        public void Add(ArchiveEntryAcl item)
        {
            var err = archive_entry_acl_add_entry_w(_handle, item.Type, item.Permset, item.Tag, item.Qual, item.Name);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(archive_entry_acl_add_entry_w), err);
            }
        }

        public void Clear()
        {
            archive_entry_acl_clear(_handle);
        }

        private IEnumerable<ArchiveEntryAcl> ItemsEnumerable
        {
            get
            {
                for (; ; )
                {
                    if (archive_entry_acl_next(_handle, AclTypeFlags,
                        out var type, out var permset, out var tag,
                        out var id, out var name
                    ) != ArchiveError.OK) yield break;
                    yield return new ArchiveEntryAcl(type, permset, tag, id, name);
                }
            }
        }

        private int IndexOf(ArchiveEntryAcl item)
        {
            var i = 0;
            foreach (var x in ItemsEnumerable)
            {
                if (x.Qual == item.Qual && x.Name.Equals(item.Name)) return i;
                ++i;
            }
            return -1;
        }

        public bool Contains(ArchiveEntryAcl item)
        {
            return IndexOf(item) >= -1;
        }

        public void CopyTo(ArchiveEntryAcl[] array, int arrayIndex)
        {
            foreach (var a in ItemsEnumerable)
            {
                array[arrayIndex++] = a;
            }
        }

        public IEnumerator<ArchiveEntryAcl> GetEnumerator()
        {
            return ItemsEnumerable.GetEnumerator();
        }

        public bool Remove(ArchiveEntryAcl item)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Reset()
        {
            archive_entry_acl_reset(_handle, AclTypeFlags);
        }
    }
}
