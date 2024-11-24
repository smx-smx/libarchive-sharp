#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
﻿using System.Runtime.CompilerServices;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveEntryBuilder : IDisposable
    {
        private readonly ArchiveWriter _writer;
        private bool _closed;

        private void AddEntryHeader(SharedPtr<ArchiveEntry> entryRef, Stream? stream = null)
        {
            var entry = entryRef.AddRef();
            try
            {
                if (stream != null && entry.Size == null)
                {
                    entry.Size = stream.Length;
                }

                var err = archive_write_header(_writer, entry);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(_writer, nameof(archive_write_header), err);
                }
            } finally
            {
                entryRef.Release();
            }
        }


        public ArchiveEntryBuilder(ArchiveWriter archiveWriter, SharedPtr<ArchiveEntry> entryRef)
        {
            _writer = archiveWriter;
            AddEntryHeader(entryRef);
            _closed = false;
        }

        public long AddData(Stream stream)
        {
            long totalWritten = 0;
            long totalRead = 0;

            var buf = new Memory<byte>(new byte[_writer.BufferSize]);
            using var bufRef = buf.Pin();
            nint bufferHandle;
            unsafe
            {
                bufferHandle = new nint(bufRef.Pointer);
            }

            int nRead;
            do
            {
                nRead = stream.Read(buf.Span);
                if (nRead < 1) break;

                totalRead += nRead;

                var nWritten = archive_write_data(_writer, bufferHandle, (nuint)nRead);
                if (nWritten < 0)
                {
                    throw new ArchiveOperationFailedException(_writer, nameof(archive_write_data), (ArchiveError)nWritten);
                }

                totalWritten += nWritten;
            } while (nRead > 0);

            return totalWritten;
        }

        public long AddData(Stream stream, long amount, long archiveOffset = -1)
        {
            var bufferSize = Math.Max(Math.Min(amount, _writer.BufferSize), 1);

            var bufBytes = new byte[bufferSize];
            var buf = new Memory<byte>(bufBytes);
            using var bufRef = buf.Pin();

            nint bufferHandle;
            unsafe
            {
                bufferHandle = new nint(bufRef.Pointer);
            }

            var isSparse = archiveOffset > -1;

            var remaining = amount;
            long totalWritten = 0;

            while (remaining > 0)
            {
                // do a shorter read for the trailing read
                var toRead = (int)((remaining < bufferSize)
                    ? remaining
                    : bufferSize);

                var nRead = stream.Read(bufBytes, 0, toRead);
                remaining -= nRead;


                var nWritten = (isSparse)
                    ? archive_write_data_block(_writer, bufferHandle, (nuint)nRead, archiveOffset)
                    : archive_write_data(_writer, bufferHandle, (nuint)nRead);
                if (nWritten < 0)
                {
                    throw new ArchiveOperationFailedException(_writer, nameof(archive_write_data), (ArchiveError)nWritten);
                }

                totalWritten += nWritten;
                if (isSparse)
                {
                    archiveOffset += nWritten;
                }
            }

            return totalWritten;
        }

        public long AddData(ArchiveDataBlock dataBlock)
        {
            using var mem = dataBlock.Data.Pin();
            long res;
            unsafe
            {
                res = archive_write_data_block(
                    _writer,
                    new nint(mem.Pointer),
                    (nuint)dataBlock.Data.Length,
                    dataBlock.Offset);
            }
            if (res < 0)
            {
                throw new ArchiveOperationFailedException(_writer, nameof(archive_write_data_block), (ArchiveError)res);
            }
            return res;
        }

        public void Close()
        {
            if (_closed) return;
            _closed = true;
            var err = archive_write_finish_entry(_writer);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_writer, nameof(archive_write_finish_entry), err);
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
