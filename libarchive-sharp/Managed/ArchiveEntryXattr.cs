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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static libarchive.Methods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace libarchive.Managed
{
    public record ArchiveEntryXattr(string Name, Memory<byte> Value);

    public class ArchiveEntryXattrList : ICollection<ArchiveEntryXattr>
    {
        private readonly TypedPointer<archive_entry> _handle;

        public ArchiveEntryXattrList(TypedPointer<archive_entry> handle)
        {
            _handle = handle;
        }

        public int Count => archive_entry_xattr_count(_handle);

        public bool IsReadOnly => false;

        public void Add(ArchiveEntryXattr item)
        {
            using var handle = item.Value.Pin();
            unsafe
            {
                archive_entry_xattr_add_entry(_handle, item.Name, handle.Pointer, (nuint)item.Value.Length);
            }
        }

        private IEnumerable<ArchiveEntryXattr> ItemsEnumerable
        {
            get
            {
                for (; ; )
                {
                    if (archive_entry_xattr_next(_handle, out var name, out var value_ptr, out var value_size) != ArchiveError.OK)
                    {
                        yield break;
                    }
                    var data = new byte[value_size];
                    Marshal.Copy(value_ptr, data, 0, (int)value_size);
                    yield return new ArchiveEntryXattr(name, data);
                }
            }
        }

        public void Clear()
        {
            archive_entry_xattr_clear(_handle);
        }

        public bool Contains(ArchiveEntryXattr item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(ArchiveEntryXattr[] array, int arrayIndex)
        {
            foreach (var x in ItemsEnumerable)
            {
                array[arrayIndex++] = x;
            }
        }

        public IEnumerator<ArchiveEntryXattr> GetEnumerator()
        {
            return ItemsEnumerable.GetEnumerator();
        }

        private int IndexOf(ArchiveEntryXattr item)
        {
            var i = 0;
            foreach (var x in ItemsEnumerable)
            {
                if (x.Name.Equals(item.Name)) return i;
                ++i;
            }
            return -1;
        }

        public bool Remove(ArchiveEntryXattr item)
        {
            throw new NotSupportedException();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Reset()
        {
            return archive_entry_xattr_reset(_handle);
        }
    }
}
