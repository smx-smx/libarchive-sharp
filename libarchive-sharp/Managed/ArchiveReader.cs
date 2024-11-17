#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using Smx.SharpIO.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveOperationFailedException : Exception
    {
        public ArchiveOperationFailedException(string func, ArchiveError err)
            : base($"{func} failed: {Enum.GetName(err)}")
        { }

        public ArchiveOperationFailedException(string func, string err)
            : base($"{func} failed: {err}")
        { }

        public ArchiveOperationFailedException(TypedPointer<archive> handle, string func)
            : base($"{func} failed: {archive_errno(handle):X}: {archive_error_string(handle)}")
        { }

        public ArchiveOperationFailedException(TypedPointer<archive> handle, string func, string err)
            : base($"{func} failed: {err}. ({archive_errno(handle):X}: {archive_error_string(handle)})")
        { }
    }

    public class ArchiveReaderOptions
    {
        public ICollection<ArchiveFormat> EnableFormats { get; set; } = Array.Empty<ArchiveFormat>();
        public ICollection<ArchiveFilter> EnableFilters { get; set; } = Array.Empty<ArchiveFilter>();
    }

    public class ArchiveReader : Archive, IDisposable
    {
        private readonly TypedPointer<archive> _handle;
        private bool _disposed = false;
        private readonly bool _owned;
        private readonly ArchiveInputStream _inputStream;

        private ArchiveDataStream _stream;
        private readonly Delegates.archive_read_callback _read_callback;
        private readonly Delegates.archive_close_callback _close_callback;
        private readonly Delegates.archive_seek_callback _seek_callback;
        private readonly Delegates.archive_skip_callback _skip_callback;

        public ArchiveInputStream InputStream => _inputStream;

        public ArchiveReader(Stream stream, ArchiveReaderOptions? opts = null)
            : this(NewHandle(opts), new ArchiveDataStream(stream), true)
        {
        }

        public ArchiveReader(TypedPointer<archive> handle, ArchiveDataStream stream, bool owned)
            : base(handle)
        {
            _handle = handle;
            _stream = stream;
            _owned = owned;
            _inputStream = new ArchiveInputStream(handle);

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

        private static TypedPointer<archive> NewHandle(ArchiveReaderOptions? opts = null)
        {
            var handle = archive_read_new();
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_new), "out of memory");
            }
            if (opts == null)
            {
                archive_read_support_format_all(handle);
                archive_read_support_filter_all(handle);
            } else
            {
                foreach (var format in opts.EnableFormats)
                {
                    archive_read_support_format_by_code(handle, format);
                }
                foreach (var filter in opts.EnableFilters)
                {
                    archive_read_support_filter_by_code(handle, filter);
                }
            }
            return handle;
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

        public void SetFormatOption(string module, string option, string value)
        {
            var err = archive_read_set_format_option(_handle, module, option, value);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_set_format_option), err);
            }
        }

        public void SetFilterOption(string module, string option, string value)
        {
            var err = archive_read_set_filter_option(_handle, module, option, value);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_set_filter_option), err);
            }
        }

        public void SetOption(string module, string option, string value)
        {
            var err = archive_read_set_option(_handle, module, option, value);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_set_option), err);
            }
        }

        public void SetOptions(string options)
        {
            var err = archive_read_set_options(_handle, options);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(nameof(archive_read_set_options), err);
            }
        }
    }
}
