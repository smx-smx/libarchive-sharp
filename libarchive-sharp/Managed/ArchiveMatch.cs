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

        private ArchiveMatch WithTime(DateTime time, ArchiveMatchFlags flags)
        {
            var elapsed = time.Subtract(DateTime.UnixEpoch);
            var err = archive_match_include_time(_handle, flags, (long)elapsed.TotalSeconds, elapsed.Nanoseconds);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_time), err);
            }
            return this;
        }

        private ArchiveMatch WithFileTime(string pathname, ArchiveMatchFlags flags)
        {
            var err = archive_match_include_file_time_w(_handle, flags, pathname);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_file_time_w), err);
            }
            return this;
        }

        public ArchiveMatch WithCreationTime(DateTime time, ArchiveMatchComparisonFlag mode)
        {
            return WithTime(time, (ArchiveMatchFlags)ArchiveMatchTimeFlag.CTIME | (ArchiveMatchFlags)mode);
        }
        public ArchiveMatch WithLastWriteTime(DateTime time, ArchiveMatchComparisonFlag mode)
        {
            return WithTime(time, (ArchiveMatchFlags)ArchiveMatchTimeFlag.MTIME | (ArchiveMatchFlags)mode);
        }

        public ArchiveMatch WithCreationFileTime(string pathname, ArchiveMatchComparisonFlag mode)
        {
            return WithFileTime(pathname, (ArchiveMatchFlags)ArchiveMatchTimeFlag.CTIME | (ArchiveMatchFlags)mode);
        }
        public ArchiveMatch WithLastWriteFileTime(string pathname, ArchiveMatchComparisonFlag mode)
        {
            return WithFileTime(pathname, (ArchiveMatchFlags)ArchiveMatchTimeFlag.MTIME | (ArchiveMatchFlags)mode);
        }

        public ArchiveMatch WithDate(DateTime time, ArchiveMatchFlags flags = 0)
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

        public bool IsTimeExcluded(TypedPointer<archive_entry> entry)
        {
            var err = archive_match_time_excluded(_handle, entry);
            if (err < 0)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_time_excluded), (ArchiveError)err);
            }
            return err == 0 ? false : true;
        }

        public ArchiveMatch WithoutEntry(TypedPointer<archive_entry> entry, ArchiveMatchFlags flags = 0)
        {
            var err = archive_match_exclude_entry(_handle, flags, entry);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_exclude_entry), err);
            }
            return this;
        }

        public ArchiveMatch WithFilePattern(string pathname, char separator)
        {
            var err = archive_match_include_pattern_from_file_w(_handle, pathname, separator);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_pattern_from_file_w), err);
            }
            return this;
        }

        public ArchiveMatch WithoutFilePattern(string pathname, char separator)
        {
            var err = archive_match_exclude_pattern_from_file_w(_handle, pathname, separator);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_exclude_pattern_from_file_w), err);
            }
            return this;
        }

        public ArchiveMatch WithPattern(string pattern)
        {
            var err = archive_match_include_pattern_w(_handle, pattern);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_pattern_w), err);
            }
            return this;
        }

        public ArchiveMatch WithoutPattern(string pattern)
        {
            var err = archive_match_exclude_pattern_w(_handle, pattern);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_exclude_pattern_w), err);
            }
            return this;
        }

        public ArchiveMatch WithUid(long uid)
        {
            var err = archive_match_include_uid(_handle, uid);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_uid), err);
            }
            return this;
        }

        public ArchiveMatch WithGid(long gid)
        {
            var err = archive_match_include_gid(_handle, gid);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_gid), err);
            }
            return this;

        }

        public ArchiveMatch WithGroupName(string gname)
        {
            var err = archive_match_include_gname_w(_handle, gname);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_match_include_gname_w), err);
            }
            return this;
        }

        public ArchiveMatch WithUserName(string uname)
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
