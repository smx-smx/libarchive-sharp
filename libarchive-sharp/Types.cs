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

    public enum ArchiveEntrySymlinkType : int
    {
        Undefined = 0,
        File = 1,
        Directory = 2
    }

    [Flags]
    public enum ArchiveEntryAclType : int
    {
        Access = 0x00000100,
        Default = 0x00000200,
        Allow = 0x00000400,
        Deny = 0x00000800,
        Audit = 0x00001000,
        Alarm = 0x00002000,
        Posix1E = Access | Default,
        Nfs4 = Allow | Deny | Audit | Alarm,
        All = -1
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

    public class ArchiveFormatNames
    {
        public const string SevenZip = "7zip";
        public const string Ar = "ar";
        public const string ArBsd = "arbsd";
        public const string ArGnu = "argnu";
        public const string ArSvr4 = "arsvr4";
        public const string Bin = "bin";
        public const string BsdTar = "bsdtar";
        public const string Cd9660 = "cd9660";
        public const string Cpio = "cpio";
        public const string GnuTar = "gnutar";
        public const string Iso = "iso";
        public const string Iso9660 = "iso9660";
        public const string Mtree = "mtree";
        public const string MtreeClassic = "mtree-classic";
        public const string NewC = "newc";
        public const string Odc = "odc";
        public const string OldTar = "oldtar";
        public const string Pax = "pax";
        public const string Paxr = "paxr";
        public const string Posix = "posix";
        public const string Pwb = "pwb";
        public const string Raw = "raw";
        public const string Rpax = "rpax";
        public const string Shar = "shar";
        public const string SharDump = "shardump";
        public const string UsTar = "ustar";
        public const string V7Tar = "v7tar";
        public const string V7 = "v7";
        public const string Warc = "warc";
        public const string Xar = "xar";
        public const string Zip = "zip";
    }

    public enum ArchiveFormat
    {
        BASE_MASK = 0xff0000,
        CPIO = 0x10000,
        CPIO_POSIX = (CPIO | 1),
        CPIO_BIN_LE = (CPIO | 2),
        CPIO_BIN_BE = (CPIO | 3),
        CPIO_SVR4_NOCRC = (CPIO | 4),
        CPIO_SVR4_CRC = (CPIO | 5),
        CPIO_AFIO_LARGE = (CPIO | 6),
        CPIO_PWB = (CPIO | 7),
        SHAR = 0x20000,
        SHAR_BASE = (SHAR | 1),
        SHAR_DUMP = (SHAR | 2),
        TAR = 0x30000,
        TAR_USTAR = (TAR | 1),
        TAR_PAX_INTERCHANGE = (TAR | 2),
        TAR_PAX_RESTRICTED = (TAR | 3),
        TAR_GNUTAR = (TAR | 4),
        ISO9660 = 0x40000,
        ISO9660_ROCKRIDGE = (ISO9660 | 1),
        ZIP = 0x50000,
        EMPTY = 0x60000,
        AR = 0x70000,
        AR_GNU = (AR | 1),
        AR_BSD = (AR | 2),
        MTREE = 0x80000,
        RAW = 0x90000,
        XAR = 0xA0000,
        LHA = 0xB0000,
        CAB = 0xC0000,
        RAR = 0xD0000,
        SEVENZIP = 0xE0000,
        WARC = 0xF0000,
        RAR_V5 = 0x100000,
    }

    public enum ArchiveFilter
    {
        NONE = 0,
        GZIP = 1,
        BZIP2 = 2,
        COMPRESS = 3,
        PROGRAM = 4,
        LZMA = 5,
        XZ = 6,
        UU = 7,
        RPM = 8,
        LZIP = 9,
        LRZIP = 10,
        LZOP = 11,
        GRZIP = 12,
        LZ4 = 13,
        ZSTD = 14
    }

    public enum ArchiveMatchTimeFlag
    {
        MTIME = 0x0100,
        CTIME = 0x0200,
    }

    public enum ArchiveMatchComparisonFlag
    {
        NEWER = 0x0001,
        OLDER = 0x0002,
        EQUAL = 0x0010
    }

    [Flags]
    public enum ArchiveMatchFlags
    {
        MTIME = 0x0100,
        CTIME = 0x0200,
        NEWER = 0x0001,
        OLDER = 0x0002,
        EQUAL = 0x0010
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
