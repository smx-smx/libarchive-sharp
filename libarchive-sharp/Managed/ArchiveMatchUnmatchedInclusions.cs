#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using Smx.SharpIO.Memory;
using System.Collections;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveMatchUnmatchedInclusions : IEnumerable<string>
    {
        private TypedPointer<archive> _handle;

        public ArchiveMatchUnmatchedInclusions(TypedPointer<archive> handle)
        {
            _handle = handle;
        }

        public int Count
        {
            get
            {
                var res = archive_match_path_unmatched_inclusions(_handle);
                if (res < 0)
                {
                    throw new ArchiveOperationFailedException(_handle, nameof(archive_match_path_unmatched_inclusions), (ArchiveError)res);
                }
                return res;
            }
        }

        private IEnumerable<string> ItemsEnumerable
        {
            get
            {
                for (; ; )
                {
                    var res = archive_match_path_unmatched_inclusions_next_w(_handle, out var str);
                    if (res == ArchiveError.EOF) yield break;
                    yield return str;
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ItemsEnumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
