#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
namespace libarchive.Managed
{
    public class ArchiveReaderOptions
    {
        public int BufferSize { get; set; } = ArchiveConstants.BUFFER_SIZE;
        public ICollection<ArchiveFormat> EnableFormats { get; set; } = Array.Empty<ArchiveFormat>();
        public ICollection<ArchiveFilter> EnableFilters { get; set; } = Array.Empty<ArchiveFilter>();
    }
}
