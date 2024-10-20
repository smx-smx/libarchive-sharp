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
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveOutputStream : Stream
    {
        private readonly TypedPointer<archive> _handle;

        public ArchiveOutputStream(TypedPointer<archive> handle)
        {
            _handle = handle;
        }

        public override bool CanRead => false;

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => true;

        public override long Length => throw new NotImplementedException();

        public override long Position
        {
            get => archive_seek_data(_handle, 0, SeekOrigin.Current);
            set => Seek(value, SeekOrigin.Begin);
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return archive_seek_data(_handle, offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            unsafe
            {
                fixed (byte* dptr = buffer)
                {
                    archive_write_data(_handle, new nint(dptr), (nuint)count);
                }
            }
        }
    }
}
