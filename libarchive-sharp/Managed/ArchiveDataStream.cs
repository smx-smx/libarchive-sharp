#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using Smx.SharpIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libarchive.Managed
{
    public class ArchiveDataStreamLazy : IArchiveStream, IDisposable
    {
        private bool _disposed;
        private readonly Lazy<ArchiveDataStream> _factory;
        private ArchiveDataStream _instance => _factory.Value;

        public ArchiveDataStreamLazy(Lazy<Stream> factory, int bufferSize = -1)
        {
            _factory = new Lazy<ArchiveDataStream>(() => new ArchiveDataStream(factory.Value, bufferSize: bufferSize));
        }

        public long Read(out nint pData)
        {
            return _instance.Read(out pData);
        }

        public long Seek(long offset, SeekOrigin whence)
        {
            return _instance.Seek(offset, whence);
        }

        public long Skip(long request)
        {
            return _instance.Skip(request);
        }

        public void Dispose()
        {
            if (_factory.IsValueCreated)
            {
                _instance.Dispose();
            }
        }

        public ArchiveError Close()
        {
            if (_factory.IsValueCreated)
            {
                return _instance.Close();
            }
            return ArchiveError.OK;
        }
    }

    public class ArchiveDataStream : IArchiveStream, IDisposable
    {
        private readonly Stream _stream;
        private readonly bool _leaveOpen;
        private bool _disposed;
        private Memory<byte> _buffer;
        private nint _bufferHandle;

        public ArchiveDataStream(Stream baseStream, bool leaveOpen = false, int bufferSize = -1)
        {
            _stream = baseStream;
            _leaveOpen = leaveOpen;
            _disposed = false;

            if (bufferSize <= 0) bufferSize = ArchiveConstants.BUFFER_SIZE;
            _buffer = new byte[bufferSize];

            unsafe
            {
                _bufferHandle = new nint(Unsafe.AsPointer(ref _buffer.Span.GetPinnableReference()));
            }
        }

        public ArchiveError Close()
        {
            Dispose();
            return ArchiveError.OK;
        }

        public void Dispose()
        {
            if (!_disposed && !_leaveOpen)
            {
                _stream.Close();
                _stream.Dispose();
                _disposed = true;
            }
        }

        public long Read(out nint pData)
        {
            var nRead = _stream.Read(_buffer.Span);
            pData = _bufferHandle;
            return nRead;
        }

        public long Write(ReadOnlySpan<byte> data)
        {
            var pre = _stream.Position;
            _stream.Write(data);
            var post = _stream.Position;
            return post - pre;
        }

        public long Seek(long offset, SeekOrigin whence)
        {
            return _stream.Seek(offset, whence);
        }

        public long Skip(long request)
        {
            /**
             * [snip from libarchive]
             * The client
		     * skipper is allowed to skip by less than requested
		     * if it needs to maintain block alignment.  The
		     * seeker is not allowed to play such games, so using
		     * the seeker here may be a performance loss compared
		     * to just reading and discarding.  That's why we
		     * only do this for skips of over 64k.
		     **/
            var remaining = _stream.Length - _stream.Position;
            var bytesToSkip = Math.Min(request, remaining);
            var offsetOld = Seek(0, SeekOrigin.Current);
            var offsetNew = Seek(bytesToSkip, SeekOrigin.Current);
            return offsetNew - offsetOld;
        }
    }
}
