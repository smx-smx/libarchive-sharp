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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;
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

        public static implicit operator TypedPointer<archive_entry>(ArchiveEntry entry) => entry.Handle;

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

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            { }

            if (_owned && _handle.Address != 0)
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

        public string SourcePath
        {
            get => archive_entry_sourcepath_w(_handle);
            set => archive_entry_copy_sourcepath_w(_handle, value);
        }

        public string UserName
        {
            get => archive_entry_uname_w(_handle);
            set => archive_entry_copy_uname_w(_handle, value);
        }

        public string GroupName
        {
            get => archive_entry_gname_w(_handle);
            set => archive_entry_copy_gname_w(_handle, value);
        }

        public string PathName
        {
            get => archive_entry_pathname_w(_handle);
            set => archive_entry_copy_pathname_w(_handle, value);
        }

        public void SetLink(string target)
        {
            archive_entry_copy_link_w(Handle, target);
        }

        public string SymLinkTarget
        {
            get => archive_entry_symlink_w(_handle);
            set => archive_entry_copy_symlink_w(_handle, value);
        }

        public uint HardLinkCount
        {
            get => archive_entry_nlink(_handle);
            set => archive_entry_set_nlink(_handle, value);
        }

        public string HardLinkTarget
        {
            get => archive_entry_hardlink_w(_handle);
            set => archive_entry_copy_hardlink_w(_handle, value);
        }
        public ushort? Permissions
        {
            get => archive_entry_perm_is_set(_handle) ? archive_entry_perm(_handle) : null;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                archive_entry_set_perm(_handle, value.Value);
            }
        }

        public int AccessTimeNanoseconds
        {
            get => archive_entry_atime_nsec(_handle);
        }

        public DateTime? AccessTime
        {
            get => archive_entry_atime_is_set(_handle) ? DateTime.UnixEpoch.AddSeconds(archive_entry_atime(_handle)).ToLocalTime() : null;
            set
            {
                if (value == null)
                {
                    archive_entry_unset_atime(_handle);
                    return;
                }
                var v = value.Value;
                archive_entry_set_atime(
                    _handle,
                    (long)v.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                    v.Nanosecond
                );
            }
        }

        public int BirthTimeNanoseconds
        {
            get => archive_entry_birthtime_nsec(_handle);
        }

        public DateTime? BirthTime
        {
            get => archive_entry_birthtime_is_set(_handle) ? DateTime.UnixEpoch.AddSeconds(archive_entry_birthtime(_handle)).ToLocalTime() : null;
            set
            {
                if (value == null)
                {
                    archive_entry_unset_birthtime(_handle);
                    return;
                }
                var v = value.Value;
                archive_entry_set_birthtime(
                    _handle,
                    (long)v.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                    v.Nanosecond
                );
            }
        }

        public int CreationTimeNanoseconds
        {
            get => archive_entry_ctime_nsec(_handle);
        }

        public DateTime? CreationTime
        {
            get => archive_entry_ctime_is_set(_handle) ? DateTime.UnixEpoch.AddSeconds(archive_entry_ctime(_handle)).ToLocalTime() : null;
            set
            {
                if (value == null)
                {
                    archive_entry_unset_ctime(_handle);
                    return;
                }
                var v = value.Value;
                archive_entry_set_ctime(
                    _handle,
                    (long)v.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                    v.Nanosecond
                );
            }
        }

        public int LastWriteTimeNanoseconds
        {
            get => archive_entry_mtime_nsec(_handle);
        }

        public DateTime? LastWriteTime
        {
            get => archive_entry_mtime_is_set(_handle) ? DateTime.UnixEpoch.AddSeconds(archive_entry_mtime(_handle)).ToLocalTime() : null;
            set
            {
                if (value == null)
                {
                    archive_entry_unset_mtime(_handle);
                    return;
                }
                var v = value.Value;
                archive_entry_set_mtime(
                    _handle,
                    (long)v.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                    v.Nanosecond
                );
            }
        }

        public ArchiveEntryType? FileType
        {
            get => archive_entry_filetype_is_set(_handle) ? archive_entry_filetype(_handle) : null;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                archive_entry_set_filetype(_handle, value.Value);
            }
        }

        public long Inode64
        {
            get => archive_entry_ino64(_handle);
            set => archive_entry_set_ino64(_handle, value);
        }

        public long? Inode
        {
            get => archive_entry_ino_is_set(_handle) ? archive_entry_ino(_handle) : null;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                archive_entry_set_ino(_handle, value.Value);
            }
        }
        public uint? Device
        {
            get => archive_entry_dev_is_set(_handle) ? archive_entry_dev(_handle) : null;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                archive_entry_set_dev(_handle, value.Value);
            }
        }
        public uint? RootDevice
        {
            get => archive_entry_rdev_is_set(_handle) ? archive_entry_rdev(_handle) : null;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                archive_entry_set_rdev(_handle, value.Value);
            }
        }
        public uint RootDeviceMajor
        {
            get => archive_entry_rdevmajor(_handle);
            set => archive_entry_set_rdevmajor(_handle, value);
        }
        public uint RootDeviceMinor
        {
            get => archive_entry_rdevminor(_handle);
            set => archive_entry_set_rdevminor(_handle, value);
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
        public long? Uid
        {
            get => archive_entry_uid_is_set(_handle) ? archive_entry_uid(_handle) : null;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                archive_entry_set_uid(_handle, value.Value);
            }
        }
        public long? Gid
        {
            get => archive_entry_gid_is_set(_handle) ? archive_entry_gid(_handle) : null;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                archive_entry_set_gid(_handle, value.Value);
            }
        }
        public bool IsEncrypted
        {
            get => archive_entry_is_encrypted(_handle);
        }
        public bool IsEncryptedData
        {
            get => archive_entry_is_data_encrypted(_handle);
            set => archive_entry_set_is_data_encrypted(_handle, (sbyte)(value ? 1 : 0));
        }

        public bool IsEncryptedMetaData
        {
            get => archive_entry_is_metadata_encrypted(_handle);
            set => archive_entry_set_is_metadata_encrypted(_handle, (sbyte)(value ? 1 : 0));
        }

        public long? Size
        {
            get => archive_entry_size_is_set(_handle) ? archive_entry_size(_handle) : null;
            set
            {
                if (value == null)
                {
                    archive_entry_unset_size(_handle);
                    return;
                }
                archive_entry_set_size(_handle, value.Value);
            }
        }
        public ushort Mode
        {
            get => archive_entry_mode(_handle);
            set => archive_entry_set_mode(_handle, value);
        }

        public string ModeString
        {
            get => archive_entry_strmode(_handle);
        }

        public uint FileFlagsSet
        {
            get
            {
                archive_entry_fflags(_handle, out var set, out var _);
                return set;
            }
            set => archive_entry_set_fflags(_handle, FileFlagsClear, value);
        }
        public uint FileFlagsClear
        {
            get
            {
                archive_entry_fflags(_handle, out var _, out var clear);
                return clear;
            }
            set => archive_entry_set_fflags(_handle, FileFlagsSet, value);
        }

        public string FileFlagsText
        {
            get => archive_entry_fflags_text(_handle);
            set => archive_entry_copy_fflags_text_w(_handle, value);
        }

        public ArchiveEntrySymlinkType SymlinkType
        {
            get => archive_entry_symlink_type(_handle);
            set => archive_entry_set_symlink_type(_handle, value);
        }

        public Memory<byte> MacMetadata
        {
            get
            {
                var dptr = archive_entry_mac_metadata(_handle, out nuint size);
                if (dptr == 0)
                {
                    throw new ArchiveOperationFailedException(nameof(archive_entry_mac_metadata), "corrupted memory");
                }
                var mem = new byte[size];
                Marshal.Copy(dptr, mem, 0, mem.Length);
                return mem;
            }
        }

        public override string ToString()
        {
            return PathName;
        }

        public void SetAcl(string text, ArchiveEntryAclType type)
        {
            var err = archive_entry_acl_from_text_w(_handle, text, type);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_archive, nameof(archive_entry_acl_from_text_w), err);
            }
        }

        public ArchiveEntryAcl Acl => new ArchiveEntryAcl(_handle);
        public ArchiveEntryXattrList Xattrs => new ArchiveEntryXattrList(_handle);
        public ArchiveEntrySparseList Sparse => new ArchiveEntrySparseList(_handle);
        public ArchiveEntryAclList GetAcls(ArchiveEntryAclType wantType) => new ArchiveEntryAclList(_handle, wantType);

        public void Clear()
        {
            unsafe
            {
                archive_entry_clear(_handle);
            }
        }
    }
}
