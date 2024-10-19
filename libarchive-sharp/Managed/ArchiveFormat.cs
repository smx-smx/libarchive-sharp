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

namespace libarchive.Managed
{
    public enum ArchiveCompression
    {
        bzip2,
        compress,
        gzip,
        lzip,
        lzma,
        none,
        program,
        xz
    }

    public enum ArchiveFilter
    {
        b64encode,
        bzip2,
        compress,
        grzip,
        gzip,
        lrzip,
        lz4,
        lzip,
        lzma,
        lzop,
        none,
        program,
        uuencode,
        xz,
        zstd
    }

    public enum ArchiveFormat
    {
        sevenzip,
        ar_bsd, ar_svr4,
        cpio, cpio_bin, cpio_newc, cpio_odc, cpio_pwb,
        gnutar,
        iso9660,
        mtree, mtree_classic,
        pax, pax_restricted,
        raw,
        shar, shar_dump,
        ustar,
        v7tar,
        warc,
        xar,
        zip,
    }
}
