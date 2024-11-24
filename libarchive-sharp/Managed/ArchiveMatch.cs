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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveMatch : Archive, IDisposable
    {
        private readonly TypedPointer<archive> _handle;

        public ArchiveMatch(TypedPointer<archive> handle, bool owned) : base(handle, owned)
        {
            _handle = handle;
        }

        public ArchiveMatch()
            : this(NewHandle(), true)
        {
        }

        private Delegates.archive_match_excluded_callback? _match_excluded_callback;

        public void SetMatchingCallback(TypedPointer<archive> archive, Delegates.archive_match_excluded_callback? callback)
        {
            _match_excluded_callback = callback;
            var err = archive_read_disk_set_matching(_handle, archive, _match_excluded_callback, 0);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_disk_set_matching), err);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            { }

            archive_match_free(_handle);
            _disposed = true;
        }

        private static TypedPointer<archive> NewHandle()
        {
            var handle = archive_match_new();
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_match_new), "out of memory");
            }
            return handle;
        }

        public void SetInclusionRecursion(bool enable)
        {
            var err = archive_match_set_inclusion_recursion(_handle, enable ? 1 : 0);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_set_inclusion_recursion), err);
            }
        }

        private ArchiveMatch IncludeTime(DateTime time, ArchiveMatchFlags flags)
        {
            var elapsed = time.Subtract(DateTime.UnixEpoch);
            var err = archive_match_include_time(_handle, flags, (long)elapsed.TotalSeconds, elapsed.Nanoseconds);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_time), err);
            }
            return this;
        }

        private ArchiveMatch IncludeFileTime(string pathname, ArchiveMatchFlags flags)
        {
            var err = archive_match_include_file_time_w(_handle, flags, pathname);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_file_time_w), err);
            }
            return this;
        }

        public ArchiveMatch IncludeCreationTime(DateTime time, ArchiveMatchComparisonFlag mode)
        {
            return IncludeTime(time, (ArchiveMatchFlags)ArchiveMatchTimeFlag.CTIME | (ArchiveMatchFlags)mode);
        }
        public ArchiveMatch IncludeLastWriteTime(DateTime time, ArchiveMatchComparisonFlag mode)
        {
            return IncludeTime(time, (ArchiveMatchFlags)ArchiveMatchTimeFlag.MTIME | (ArchiveMatchFlags)mode);
        }

        public ArchiveMatch IncludeCreationFileTime(string pathname, ArchiveMatchComparisonFlag mode)
        {
            return IncludeFileTime(pathname, (ArchiveMatchFlags)ArchiveMatchTimeFlag.CTIME | (ArchiveMatchFlags)mode);
        }
        public ArchiveMatch IncludeLastWriteFileTime(string pathname, ArchiveMatchComparisonFlag mode)
        {
            return IncludeFileTime(pathname, (ArchiveMatchFlags)ArchiveMatchTimeFlag.MTIME | (ArchiveMatchFlags)mode);
        }

        public ArchiveMatch IncludeData(DateTime time, ArchiveMatchFlags flags = 0)
        {
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(time);
            var sDateTime = time.ToString("ddd MMM dd HH:mm:ss 'UTC' yyyy");
            var err = archive_match_include_date_w(_handle, flags, sDateTime);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_date_w), err);
            }
            return this;
        }

        public bool IsExcluded(TypedPointer<archive_entry> entry)
        {
            var res = archive_match_excluded(_handle, entry);
            if (res < 0)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_excluded), (ArchiveError)res);
            }
            return res == 0 ? false : true;
        }

        public bool IsOwnerExcluded(TypedPointer<archive_entry> entry)
        {
            var res = archive_match_owner_excluded(_handle, entry);
            if (res < 0)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_owner_excluded), (ArchiveError)res);
            }
            return res == 0 ? false : true;
        }

        public bool IsTimeExcluded(TypedPointer<archive_entry> entry)
        {
            var res = archive_match_time_excluded(_handle, entry);
            if (res < 0)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_time_excluded), (ArchiveError)res);
            }
            return res == 0 ? false : true;
        }

        public ArchiveMatch ExcludeEntry(TypedPointer<archive_entry> entry, ArchiveMatchFlags flags = 0)
        {
            var err = archive_match_exclude_entry(_handle, flags, entry);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_exclude_entry), err);
            }
            return this;
        }

        public ArchiveMatch IncludePathPatternsFromFile(string pathname, char separator)
        {
            var err = archive_match_include_pattern_from_file_w(_handle, pathname, separator);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_pattern_from_file_w), err);
            }
            return this;
        }

        public ArchiveMatch ExcludePathPatternsFromFile(string pathname, char separator)
        {
            var err = archive_match_exclude_pattern_from_file_w(_handle, pathname, separator);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_exclude_pattern_from_file_w), err);
            }
            return this;
        }

        public ArchiveMatch IncludePathPattern(string pattern)
        {
            var err = archive_match_include_pattern_w(_handle, pattern);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_pattern_w), err);
            }
            return this;
        }

        public ArchiveMatch ExcludePathPattern(string pattern)
        {
            var err = archive_match_exclude_pattern_w(_handle, pattern);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_exclude_pattern_w), err);
            }
            return this;
        }

        public ArchiveMatch IncludeUid(long uid)
        {
            var err = archive_match_include_uid(_handle, uid);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_uid), err);
            }
            return this;
        }

        public ArchiveMatch IncludeGid(long gid)
        {
            var err = archive_match_include_gid(_handle, gid);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_gid), err);
            }
            return this;

        }

        public ArchiveMatch IncludeGroupName(string gname)
        {
            var err = archive_match_include_gname_w(_handle, gname);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_gname_w), err);
            }
            return this;
        }

        public ArchiveMatch IncludeUserName(string uname)
        {
            var err = archive_match_include_uname_w(_handle, uname);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_uname_w), err);
            }
            return this;
        }

        public bool IsPathExcluded(TypedPointer<archive_entry> entry)
        {
            var err = archive_match_path_excluded(_handle, entry);
            if (err < 0)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_path_excluded), (ArchiveError)err);
            }
            return err == 0 ? false : true;
        }

        ~ArchiveMatch()
        {
            Dispose(false);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ArchiveMatchUnmatchedInclusions UnmatchedInclusions => new ArchiveMatchUnmatchedInclusions(_handle);
    }
}
