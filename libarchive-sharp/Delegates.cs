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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libarchive
{
    public static class Delegates
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ArchiveError archive_open_callback(TypedPointer<archive> archive, nint client_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long archive_read_callback(TypedPointer<archive> archive, nint client_data, TypedPointer<nint> buffer);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long archive_seek_callback(TypedPointer<archive> archive, nint client_data, long offset, SeekOrigin whence);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long archive_skip_callback(TypedPointer<archive> archive, nint client_data, long request);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ArchiveError archive_close_callback(TypedPointer<archive> archive, nint client_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ArchiveError archive_switch_callback(TypedPointer<archive> archive, nint client_data1, nint client_data2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public delegate string archive_passphrase_callback(TypedPointer<archive> archive, nint client_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void progress_func(nint user_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long archive_write_callback(TypedPointer<archive> archive, nint client_data, nint buffer, nuint length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ArchiveError archive_free_callback(TypedPointer<archive> archive, nint client_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long archive_group_lookup_callback(
            nint private_data,
            [MarshalAs(UnmanagedType.LPStr)] string gname,
            long gid);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void archive_lookup_cleanup_callback(nint private_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public delegate string? archive_user_name_lookup_callback(nint private_data, long uid);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public delegate string? archive_group_name_lookup_callback(nint private_data, long gid);
    }
}
