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
    public class ArchiveEntryLinkResolver : IDisposable
    {
        private readonly TypedPointer<archive_entry_linkresolver> _handle;
        private readonly bool _owned;
        private bool _disposed;

        public ArchiveEntryLinkResolver(ArchiveFormat format) : this(
            handle: NewHandle(),
            owned: true,
            format: format
        )
        { }

        public ArchiveEntryLinkResolver(
            TypedPointer<archive_entry_linkresolver> handle,
            bool owned,
            ArchiveFormat format)
        {
            _handle = handle;
            _owned = owned;
            _disposed = false;
            archive_entry_linkresolver_set_strategy(handle, format);
        }

        public (ArchiveEntry? entry1, ArchiveEntry? entry2) Linkify(ArchiveEntry entry)
        {
            var arch = entry.Archive;
            var ptr1 = entry.Handle;
            archive_entry_linkify(_handle, ref ptr1, out var ptr2);

            var entry1 = ptr1.Address == 0 ? null : new ArchiveEntry(arch, ptr1, true);
            var entry2 = ptr2.Address == 0 ? null : new ArchiveEntry(arch, ptr2, true);
            return (entry1, entry2);
        }

        private static TypedPointer<archive_entry_linkresolver> NewHandle()
        {
            var handle = archive_entry_linkresolver_new();
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_entry_linkresolver_new), "out of memory");
            }
            return handle;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            { }

            if (_owned)
            {
                archive_entry_linkresolver_free(_handle);
            }

            _disposed = true;
        }

        ~ArchiveEntryLinkResolver()
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
