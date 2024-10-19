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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveEntry : IDisposable
    {
        private readonly TypedPointer<archive> _archive;
        private readonly TypedPointer<archive_entry> _handle;
        private readonly bool _owned;
        private bool _disposed;
        private bool _sizeProvided;

        public bool HasSize => _sizeProvided;

        public TypedPointer<archive> Archive => _archive;
        public TypedPointer<archive_entry> Handle => _handle;

        public ArchiveEntry() : this(
            new TypedPointer<archive>(0),
            ArchiveEntry.NewHandle(new TypedPointer<archive>(0)),
            true
        )
        { }

        public ArchiveEntry(TypedPointer<archive> archive) : this(
            archive,
            ArchiveEntry.NewHandle(archive),
            true
        )
        { }

        public ArchiveEntry(TypedPointer<archive> archive, TypedPointer<archive_entry> handle, bool owned)
        {
            _archive = archive;
            _handle = handle;
            _owned = owned;
            _sizeProvided = false;
        }

        private static TypedPointer<archive_entry> NewHandle(TypedPointer<archive> archive)
        {
            // NOTE: archive pointer can be NULL, it's used for charset info
            var handle = archive_entry_new2(archive);
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_entry_new), "out of memory");
            }
            return handle;
        }

        public ArchiveEntryStream OpenStream()
        {
            return new ArchiveEntryStream(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            { }

            if (_owned)
            {
                archive_entry_free(_handle);
            }
            _disposed = true;
        }

        ~ArchiveEntry()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public string PathName
        {
            get => archive_entry_pathname_w(_handle);
            set => archive_entry_copy_pathname_w(_handle, value);
        }
        public string SymLinkTarget
        {
            get => archive_entry_symlink_w(_handle);
            set => archive_entry_copy_symlink_w(_handle, value);
        }
        public string HardLinkTarget
        {
            get => archive_entry_hardlink_w(_handle);
            set => archive_entry_copy_hardlink_w(_handle, value);
        }
        public ushort Permissions
        {
            get => archive_entry_perm(_handle);
            set => archive_entry_set_perm(_handle, value);
        }
        public DateTime AccessTime
        {
            get => DateTime.UnixEpoch.AddSeconds(archive_entry_atime(_handle)).ToLocalTime();
            set => archive_entry_set_atime(
                _handle,
                (long)value.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                value.Nanosecond
            );
        }
        public DateTime BirthTime
        {
            get => DateTime.UnixEpoch.AddSeconds(archive_entry_birthtime(_handle)).ToLocalTime();
            set => archive_entry_set_birthtime(
                _handle,
                (long)value.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                value.Nanosecond
            );
        }
        public DateTime CreationTime
        {
            get => DateTime.UnixEpoch.AddSeconds(archive_entry_ctime(_handle)).ToLocalTime();
            set => archive_entry_set_ctime(
                _handle,
                (long)value.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                value.Nanosecond
            );
        }
        public DateTime LastWriteTime
        {
            get => DateTime.UnixEpoch.AddSeconds(archive_entry_mtime(_handle)).ToLocalTime();
            set => archive_entry_set_mtime(
                _handle,
                (long)value.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                value.Nanosecond);
        }
        public ArchiveEntryType FileType
        {
            get => archive_entry_filetype(_handle);
            set => archive_entry_set_filetype(_handle, value);
        }

        public long Inode
        {
            get => archive_entry_ino(_handle);
            set => archive_entry_set_ino(_handle, value);
        }
        public uint Device
        {
            get => archive_entry_dev(_handle);
            set => archive_entry_set_dev(_handle, value);
        }
        public uint DeviceMajor
        {
            get => archive_entry_devmajor(_handle);
            set => archive_entry_set_devmajor(_handle, value);
        }
        public uint DeviceMinor
        {
            get => archive_entry_devminor(_handle);
            set => archive_entry_set_devminor(_handle, value);
        }
        public long Uid
        {
            get => archive_entry_uid(_handle);
            set => archive_entry_set_uid(_handle, value);
        }
        public long Gid
        {
            get => archive_entry_gid(_handle);
            set => archive_entry_set_gid(_handle, value);
        }
        public bool IsEncrypted
        {
            get => archive_entry_is_encrypted(_handle) == 1;
        }
        public bool IsEncryptedData
        {
            get => archive_entry_is_data_encrypted(_handle) == 1;
            set => archive_entry_set_is_data_encrypted(_handle, (sbyte)(value ? 1 : 0));
        }
        public long Size
        {
            get => archive_entry_size(_handle);
            set
            {
                _sizeProvided = true;
                archive_entry_set_size(_handle, value);
            }
        }
        public ushort Mode
        {
            get => archive_entry_mode(_handle);
            set => archive_entry_set_mode(_handle, value);
        }

        public override string ToString()
        {
            return PathName;
        }
    }
}
