#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
﻿using Smx.SharpIO.Memory;
using System.Collections;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public record ArchiveEntrySparse(long Offset, long Length);

    public class ArchiveEntrySparseList : ICollection<ArchiveEntrySparse>
    {
        private readonly TypedPointer<archive_entry> _handle;

        public ArchiveEntrySparseList(TypedPointer<archive_entry> handle)
        {
            _handle = handle;
        }

        public int Count => archive_entry_sparse_count(_handle);

        public bool IsReadOnly => false;

        public void Add(ArchiveEntrySparse item)
        {
            archive_entry_sparse_add_entry(_handle, item.Offset, item.Length);
        }

        public void Clear()
        {
            archive_entry_sparse_clear(_handle);
        }

        public bool Contains(ArchiveEntrySparse item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(ArchiveEntrySparse[] array, int arrayIndex)
        {
            foreach (var s in ItemsEnumerable)
            {
                array[arrayIndex++] = s;
            }
        }

        private int IndexOf(ArchiveEntrySparse item)
        {
            var i = 0;
            foreach (var s in ItemsEnumerable)
            {
                if (s == item) return i;
                ++i;
            }
            return -1;
        }

        private IEnumerable<ArchiveEntrySparse> ItemsEnumerable
        {
            get
            {
                for (; ; )
                {
                    if (archive_entry_sparse_next(_handle, out var offset, out var length) != ArchiveError.OK)
                    {
                        yield break;
                    }

                    yield return new ArchiveEntrySparse(offset, length);
                }
            }
        }

        public IEnumerator<ArchiveEntrySparse> GetEnumerator()
        {
            return ItemsEnumerable.GetEnumerator();
        }

        public bool Remove(ArchiveEntrySparse item)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Reset()
        {
            return archive_entry_sparse_reset(_handle);
        }
    }
}
