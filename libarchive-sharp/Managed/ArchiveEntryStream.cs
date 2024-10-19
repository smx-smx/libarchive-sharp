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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveEntryStream : Stream
    {
        private readonly ArchiveEntry _entry;
        private long _position;

        public ArchiveEntryStream(ArchiveEntry entry)
        {
            _entry = entry;
            _position = 0;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => _entry.Size;

        public override long Position { get => _position; set => _position = value; }

        public override void Flush()
        { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            unsafe
            {
                fixed (byte* dptr = buffer)
                {
                    return (int)archive_read_data(_entry.Archive, new nint(dptr), (nuint)count);
                }
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return archive_seek_data(_entry.Archive, offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
