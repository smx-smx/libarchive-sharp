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
    public enum ArchiveMode
    {
        Read,
        Write
    }

    public class ArchiveOperationFailedException : Exception
    {
        public ArchiveOperationFailedException(string func, ArchiveError err)
            : base($"{func} failed: {Enum.GetName(err)}")
        { }

        public ArchiveOperationFailedException(string func, string err)
            : base($"{func} failed: {err}")
        { }
    }

    public class ArchiveReader : IDisposable
    {
        private readonly TypedPointer<archive> _handle;
        private bool _disposed = false;
        private bool _owned;

        public int LastError => archive_errno(_handle);
        public string LastErrorString => archive_error_string(_handle);

        private ArchiveStream _stream;
        private readonly Delegates.archive_read_callback _read_callback;
        private readonly Delegates.archive_close_callback _close_callback;
        private readonly Delegates.archive_seek_callback _seek_callback;
        private readonly Delegates.archive_skip_callback _skip_callback;

        public ArchiveReader(Stream stream)
            : this(NewHandle(), new ArchiveStream(stream), true)
        { }

        public ArchiveReader(TypedPointer<archive> handle, ArchiveStream stream, bool owned)
        {
            _handle = handle;
            _stream = stream;
            _owned = owned;

            // required to allocated client datasets, or we will crash
            archive_read_set_callback_data(handle, 0);

            _read_callback = (arch, data, outData) => stream.Read(out outData.Value);
            _seek_callback = (arch, data, offset, whence) => stream.Seek(offset, whence);
            _skip_callback = (arch, data, request) => stream.Skip(request);
            _close_callback = (arch, data) => stream.Close();

            //archive_read_set_open_callback(handle, (arch, data) => ArchiveError.OK);
            archive_read_set_read_callback(handle, _read_callback);
            archive_read_set_seek_callback(handle, _seek_callback);
            archive_read_set_skip_callback(handle, _skip_callback);
            archive_read_set_close_callback(handle, _close_callback);
            archive_read_open1(handle);
        }

        public IEnumerable<ArchiveEntry> Entries
        {
            get
            {
                while (archive_read_next_header(_handle, out var entry) == ArchiveError.OK)
                {
                    yield return new ArchiveEntry(_handle, entry, false);
                }
            }
        }

        private static TypedPointer<archive> NewHandle()
        {
            var arch = archive_read_new();
            if (arch.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_new), "out of memory");
            }
            archive_read_support_format_all(arch);
            archive_read_support_filter_all(arch);
            archive_read_support_compression_all(arch);
            return arch;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            { }

            archive_read_close(_handle);
            if (_owned)
            {
                archive_read_free(_handle);
            }
            _disposed = true;
        }

        ~ArchiveReader()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
