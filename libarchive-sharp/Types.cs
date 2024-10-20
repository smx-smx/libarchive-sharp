#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libarchive
{
    public class Constants
    {
        public const ushort AE_IFMT = 0xF000;
        public const ushort AE_IFREG = 0x8000;
        public const ushort AE_IFLNK = 0xA000;
        public const ushort AE_IFSOCK = 0xC000;
        public const ushort AE_IFCHR = 0x2000;
        public const ushort AE_IFBLK = 0x6000;
        public const ushort AE_IFDIR = 0x4000;
        public const ushort AE_IFIFO = 0x1000;
    }

    public enum ArchiveEntryType : ushort
    {
        Undefined = 0,
        File = Constants.AE_IFREG,
        Directory = Constants.AE_IFDIR,
        Symlink = Constants.AE_IFLNK,
        Socket = Constants.AE_IFSOCK,
        CharDevice = Constants.AE_IFCHR,
        BlockDevice = Constants.AE_IFBLK,
        NamedPipe = Constants.AE_IFIFO,
    }

    [Flags]
    public enum ArchiveExtractFlags
    {
        /* Default: Do not try to set owner/group. */
        OWNER = (0x0001),
        /* Default: Do obey umask, do not restore SUID/SGID/SVTX bits. */
        PERM = (0x0002),
        /* Default: Do not restore mtime/atime. */
        TIME = (0x0004),
        /* Default: Replace existing files. */
        NO_OVERWRITE = (0x0008),
        /* Default: Try create first, unlink only if create fails with EEXIST. */
        UNLINK = (0x0010),
        /* Default: Do not restore ACLs. */
        ACL = (0x0020),
        /* Default: Do not restore fflags. */
        FFLAGS = (0x0040),
        /* Default: Do not restore xattrs. */
        XATTR = (0x0080),
        /* Default: Do not try to guard against extracts redirected by symlinks. */
        /* Note: With UNLINK, will remove any intermediate symlink. */
        SECURE_SYMLINKS = (0x0100),
        /* Default: Do not reject entries with '..' as path elements. */
        SECURE_NODOTDOT = (0x0200),
        /* Default: Create parent directories as needed. */
        NO_AUTODIR = (0x0400),
        /* Default: Overwrite files, even if one on disk is newer. */
        NO_OVERWRITE_NEWER = (0x0800),
        /* Detect blocks of 0 and write holes instead. */
        SPARSE = (0x1000),
        /* Default: Do not restore Mac extended metadata. */
        /* This has no effect except on Mac OS. */
        MAC_METADATA = (0x2000),
        /* Default: Use HFS+ compression if it was compressed. */
        /* This has no effect except on Mac OS v10.6 or later. */
        NO_HFS_COMPRESSION = (0x4000),
        /* Default: Do not use HFS+ compression if it was not compressed. */
        /* This has no effect except on Mac OS v10.6 or later. */
        HFS_COMPRESSION_FORCED = (0x8000),
        /* Default: Do not reject entries with absolute paths */
        SECURE_NOABSOLUTEPATHS = (0x10000),
        /* Default: Do not clear no-change flags when unlinking object */
        CLEAR_NOCHANGE_FFLAGS = (0x20000),
        /* Default: Do not extract atomically (using rename) */
        SAFE_WRITES = (0x40000)
    }
}
