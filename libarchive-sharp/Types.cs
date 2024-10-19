#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
﻿using System;
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
}
