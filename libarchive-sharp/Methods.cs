#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using libarchive.Managed;
using Smx.SharpIO.Memory;
using System.Runtime.InteropServices;
using static libarchive.Delegates;

namespace libarchive;

public enum ArchiveError
{
    EOF = 1,
    OK = 0,
    RETRY = -10,
    WARN = -20,
    FAILED = -25,
    FATAL = -30,
    ARCHIVE_READ_FORMAT_ENCRYPTION_DONT_KNOW = -1,
    ARCHIVE_READ_FORMAT_ENCRYPTION_UNSUPPORTED = -2
}

public static unsafe partial class Methods
{
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_version_number();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_version_string();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_version_details();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_zlib_version();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_liblzma_version();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_bzlib_version();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_liblz4_version();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_libzstd_version();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive *")]
    public static extern TypedPointer<archive> archive_read_new();

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_all(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_bzip2(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_compress(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_gzip(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_lzip(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_lzma(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_none(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_program(TypedPointer<archive> archive, [MarshalAs(UnmanagedType.LPStr)] string command);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_program_signature(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string cmd,
        [NativeTypeName("const void *")] nint signature,
        [NativeTypeName("size_t")] nuint signature_length);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_rpm(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_uu(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_compression_xz(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_all(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_by_code(
        TypedPointer<archive> archive,
        ArchiveFilter filter_code);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_bzip2(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_compress(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_gzip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_grzip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_lrzip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_lz4(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_lzip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_lzma(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_lzop(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_none(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_program(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string command);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_program_signature(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string cmd,
        [NativeTypeName("const void *")] nint signature,
        [NativeTypeName("size_t")] nuint signature_length);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_rpm(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_uu(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_xz(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_filter_zstd(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_7zip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_all(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_ar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_by_code(
        TypedPointer<archive> archive,
        ArchiveFormat format_code);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_cab(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_cpio(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_empty(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_gnutar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_iso9660(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_lha(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_mtree(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_rar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_rar5(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_raw(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_tar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_warc(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_xar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_zip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_zip_streamable(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_support_format_zip_seekable(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_format(TypedPointer<archive> archive, ArchiveFormat code);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_append_filter(TypedPointer<archive> archive, ArchiveFilter code);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_append_filter_program(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string cmd);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_append_filter_program_signature(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string cmd,
        [NativeTypeName("const void *")] nint signature,
        [NativeTypeName("size_t")] nuint signature_len);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_open_callback(TypedPointer<archive> archive, archive_open_callback? client_opener);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_read_callback(TypedPointer<archive> archive, archive_read_callback? client_reader);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_seek_callback(TypedPointer<archive> archive, archive_seek_callback? client_seeker);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_skip_callback(TypedPointer<archive> archive, archive_skip_callback? client_skipper);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_close_callback(TypedPointer<archive> archive, archive_close_callback? client_closer);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_switch_callback(TypedPointer<archive> archive, archive_switch_callback? client_switcher);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_callback_data(TypedPointer<archive> archive, nint client_data);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_callback_data2(TypedPointer<archive> archive, nint client_data, uint iindex);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_add_callback_data(TypedPointer<archive> archive, nint client_data, uint iindex);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_append_callback_data(TypedPointer<archive> archive, nint client_data);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_prepend_callback_data(TypedPointer<archive> archive, nint client_data);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open1(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open(
        TypedPointer<archive> archive,
        nint client_data,
        archive_open_callback? client_opener,
        archive_read_callback? client_reader,
        archive_close_callback? client_closer);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open2(
        TypedPointer<archive> archive,
        nint client_data,
        archive_open_callback? client_opener,
        archive_read_callback? client_reader,
        archive_skip_callback? client_skipper,
        archive_close_callback? client_closer);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open_filename(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string filename,
        [NativeTypeName("size_t")] nuint block_size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open_filenames(
        TypedPointer<archive> archive,
        TypedPointer<string_array_ansi> filenames,
        [NativeTypeName("size_t")] nuint block_size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open_filename_w(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPWStr)] string filename,
        [NativeTypeName("size_t")] nuint block_size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open_filenames_w(
        TypedPointer<archive> archive,
        TypedPointer<string_array_unicode> filenames,
        [NativeTypeName("size_t")] nuint block_size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open_file(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string filename,
        [NativeTypeName("size_t")] nuint block_size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open_memory(
        TypedPointer<archive> archive,
        [NativeTypeName("const void *")] nint buff,
        [NativeTypeName("size_t")] nuint size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open_memory2(
        TypedPointer<archive> archive,
        [NativeTypeName("const void *")] nint buff,
        [NativeTypeName("size_t")] nuint size,
        [NativeTypeName("size_t")] nuint read_size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open_fd(
        TypedPointer<archive> archive,
        int fd,
        [NativeTypeName("size_t")] nuint block_size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_open_FILE(
        TypedPointer<archive> archive,
        [NativeTypeName("FILE *")] nint fileHandle);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_next_header(
        TypedPointer<archive> archive,
        [NativeTypeName("struct archive_entry **")] out TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_next_header2(
        TypedPointer<archive> archive,
        TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_read_header_position(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_has_encrypted_entries(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_format_capabilities(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_ssize_t")]
    public static extern long archive_read_data(
        TypedPointer<archive> archive,
        nint buffer,
        [NativeTypeName("size_t")] nuint size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_seek_data(
        TypedPointer<archive> archive,
        [NativeTypeName("la_int64_t")] long offset,
        SeekOrigin whence);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_data_block(
        TypedPointer<archive> archive,
        out TypedPointer<byte> buff,
        out nuint size,
        out long offset);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_data_skip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_data_into_fd(TypedPointer<archive> archive, int fd);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_format_option(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string module,
        [MarshalAs(UnmanagedType.LPStr)] string option,
        [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_filter_option(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string module,
        [MarshalAs(UnmanagedType.LPStr)] string option,
        [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_option(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string module,
        [MarshalAs(UnmanagedType.LPStr)] string option,
        [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_set_options(TypedPointer<archive> archive, [MarshalAs(UnmanagedType.LPStr)] string opts);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_read_add_passphrase(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string passphrase);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_set_passphrase_callback(
        TypedPointer<archive> archive,
        nint client_data,
        archive_passphrase_callback archive_passphrase_callback);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_extract(TypedPointer<archive> archive, TypedPointer<archive_entry> entry, int flags);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_extract2(TypedPointer<archive> archive, TypedPointer<archive_entry> entry, TypedPointer<archive> param2);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_read_extract_set_progress_callback(
        TypedPointer<archive> archive, progress_func progress_func, nint user_data);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_read_extract_set_skip_file(TypedPointer<archive> archive, [NativeTypeName("la_int64_t")] long param1, [NativeTypeName("la_int64_t")] long param2);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_close(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_free(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_finish(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive *")]
    public static extern TypedPointer<archive> archive_write_new();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_bytes_per_block(TypedPointer<archive> archive, int bytes_per_block);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_write_get_bytes_per_block(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_bytes_in_last_block(TypedPointer<archive> archive, int bytes_in_last_block);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_write_get_bytes_in_last_block(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_write_set_skip_file(TypedPointer<archive> archive, [NativeTypeName("la_int64_t")] long param1, [NativeTypeName("la_int64_t")] long param2);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_compression_bzip2(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_compression_compress(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_compression_gzip(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_compression_lzip(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_compression_lzma(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_compression_none(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_compression_program(TypedPointer<archive> archive, [MarshalAs(UnmanagedType.LPStr)] string cmd);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_compression_xz(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter(TypedPointer<archive> archive, ArchiveFilter filter_code);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_by_name(TypedPointer<archive> archive, [MarshalAs(UnmanagedType.LPStr)] string name);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_b64encode(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_bzip2(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_compress(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_grzip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_gzip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_lrzip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_lz4(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_lzip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_lzma(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_lzop(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_none(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_program(TypedPointer<archive> archive, [MarshalAs(UnmanagedType.LPStr)] string cmd);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_uuencode(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_xz(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_add_filter_zstd(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format(TypedPointer<archive> archive, ArchiveFormat format_code);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_by_name(TypedPointer<archive> archive, [MarshalAs(UnmanagedType.LPStr)] string name);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_7zip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_ar_bsd(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_ar_svr4(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_cpio(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_cpio_bin(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_cpio_newc(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_cpio_odc(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_cpio_pwb(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_gnutar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_iso9660(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_mtree(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_mtree_classic(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_pax(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_pax_restricted(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_raw(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_shar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_shar_dump(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_ustar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_v7tar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_warc(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_xar(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_zip(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_filter_by_ext(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string filename);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_filter_by_ext_def(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string filename,
        [MarshalAs(UnmanagedType.LPStr)] string def_ext);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_zip_set_compression_deflate(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_zip_set_compression_store(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_open(
        TypedPointer<archive> archive,
        nint client_data,
        archive_open_callback archive_open_callback,
        archive_write_callback archive_write_callback,
        archive_close_callback archive_close_callback);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_open2(
        TypedPointer<archive> archive,
        nint client_data,
        archive_open_callback archive_open_callback,
        archive_write_callback archive_write_callback,
        archive_close_callback archive_close_callback,
        archive_free_callback archive_free_callback);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_open_fd(TypedPointer<archive> archive, int fd);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_open_filename(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string file);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_open_filename_w(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPWStr)] string file);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_open_file(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string file);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_open_FILE(
        TypedPointer<archive> archive,
        [NativeTypeName("FILE *")] nint fileHandle);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_open_memory(
        TypedPointer<archive> archive,
        void* buffer,
        [NativeTypeName("size_t")] nuint buffSize,
        [NativeTypeName("size_t *")] nuint* used);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_header(TypedPointer<archive> archive, TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_ssize_t")]
    public static extern long archive_write_data(
        TypedPointer<archive> archive,
        [NativeTypeName("const void *")] nint buff,
        [NativeTypeName("size_t")] nuint s);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_ssize_t")]
    public static extern long archive_write_data_block(TypedPointer<archive> archive, [NativeTypeName("const void *")] void* param1, [NativeTypeName("size_t")] nuint param2, [NativeTypeName("la_int64_t")] long param3);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_finish_entry(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_close(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_fail(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_free(TypedPointer<archive> archive);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_write_finish(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_format_option(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string module,
        [MarshalAs(UnmanagedType.LPStr)] string option,
        [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_filter_option(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string module,
        [MarshalAs(UnmanagedType.LPStr)] string option,
        [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_option(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string module,
        [MarshalAs(UnmanagedType.LPStr)] string option,
        [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_set_options(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string opts);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_write_set_passphrase(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string passphrase);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_write_set_passphrase_callback(
        TypedPointer<archive> archive, nint client_data,
        archive_passphrase_callback archive_passphrase_callback);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive *")]
    public static extern TypedPointer<archive> archive_write_disk_new();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_disk_set_skip_file(
        TypedPointer<archive> archive,
        [NativeTypeName("la_int64_t")] long device,
        [NativeTypeName("la_int64_t")] long inode);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_disk_set_options(TypedPointer<archive> archive, ArchiveExtractFlags flags);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_disk_set_standard_lookup(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_write_disk_set_group_lookup(
        TypedPointer<archive> archive,
        void* private_data,
        archive_group_lookup_callback lookup_gid,
        archive_lookup_cleanup_callback cleanup_gid);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_write_disk_set_user_lookup(
        TypedPointer<archive> archive,
        void* param1,
        [NativeTypeName("la_int64_t (*)(void *, const char *, la_int64_t)")] delegate* unmanaged[Cdecl]<void*, sbyte*, long, long> param2,
        archive_lookup_cleanup_callback param3);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_write_disk_gid(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string name,
        [NativeTypeName("la_int64_t")] long id);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_write_disk_uid(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string param1,
        [NativeTypeName("la_int64_t")] long param2);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive *")]
    public static extern TypedPointer<archive> archive_read_disk_new();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_symlink_logical(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_symlink_physical(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_symlink_hybrid(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_entry_from_file(TypedPointer<archive> archive, TypedPointer<archive_entry> entry, int param2, [NativeTypeName("const struct stat *")] void* param3);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_read_disk_gname(TypedPointer<archive> archive, [NativeTypeName("la_int64_t")] long param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_read_disk_uname(TypedPointer<archive> archive, [NativeTypeName("la_int64_t")] long param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_standard_lookup(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_gname_lookup(
        TypedPointer<archive> archive,
        nint private_data,
        [NativeTypeName("const char *(*)(void *, la_int64_t)")] delegate* unmanaged[Cdecl]<void*, long, sbyte*> param2,
        archive_lookup_cleanup_callback param3);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_uname_lookup(
        TypedPointer<archive> archive,
        nint private_data,
        [NativeTypeName("const char *(*)(void *, la_int64_t)")] delegate* unmanaged[Cdecl]<void*, long, sbyte*> param2,
        archive_lookup_cleanup_callback param3);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_open(TypedPointer<archive> archive, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_open_w(TypedPointer<archive> archive, [MarshalAs(UnmanagedType.LPWStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_descend(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_can_descend(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_current_filesystem(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_current_filesystem_is_synthetic(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_current_filesystem_is_remote(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_atime_restored(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_behavior(TypedPointer<archive> archive, int flags);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_matching(
        TypedPointer<archive> archive,
        TypedPointer<archive> matching,
        [NativeTypeName("void (*)(struct archive *, void *, struct archive_entry *)")] delegate* unmanaged[Cdecl]<TypedPointer<archive>, void*, archive_entry*, void> excluded_func,
        nint client_data);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_read_disk_set_metadata_filter_callback(
        TypedPointer<archive> archive,
        [NativeTypeName("int (*)(struct archive *, void *, struct archive_entry *)")] delegate* unmanaged[Cdecl]<TypedPointer<archive>, void*, archive_entry*, int> metadata_filter_func,
        nint client_data);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_free(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_filter_count(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_filter_bytes(TypedPointer<archive> archive, int n);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_filter_code(TypedPointer<archive> archive, int n);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_filter_name(TypedPointer<archive> archive, int n);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_position_compressed(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_position_uncompressed(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_compression_name(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveCompression archive_compression(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_errno(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_error_string(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_format_name(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveFormat archive_format(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_clear_error(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_set_error(
        TypedPointer<archive> archive,
        int err,
        [MarshalAs(UnmanagedType.LPStr)] string fmt,
        __arglist);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_copy_error(TypedPointer<archive> dest, TypedPointer<archive> src);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_file_count(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive *")]
    public static extern TypedPointer<archive> archive_match_new();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_free(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_excluded(TypedPointer<archive> archive, TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_path_excluded(TypedPointer<archive> archive, TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_set_inclusion_recursion(TypedPointer<archive> archive, int enabled);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_exclude_pattern(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string pattern);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_exclude_pattern_w(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPWStr)] string pattern);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_exclude_pattern_from_file(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string pathname,
        int nullSeparator);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_exclude_pattern_from_file_w(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPWStr)] string pathname,
        int nullSeparator);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_include_pattern(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string pattern);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_include_pattern_w(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPWStr)] string pattern);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_include_pattern_from_file(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string pathname,
        int nullSeparator);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_include_pattern_from_file_w(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPWStr)] string pathname,
        int nullSeparator);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_path_unmatched_inclusions(TypedPointer<archive> archive);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_path_unmatched_inclusions_next(
        TypedPointer<archive> archive,
        TypedPointer<string_array_ansi> param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_path_unmatched_inclusions_next_w(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPWStr)]
        out string p);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_time_excluded(
        TypedPointer<archive> archive,
        TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_include_time(
        TypedPointer<archive> archive,
        ArchiveMatchFlags flag,
        [NativeTypeName("time_t")] long sec,
        [NativeTypeName("long")] int nsec);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_include_date(
        TypedPointer<archive> archive,
        ArchiveMatchFlags flag,
        [MarshalAs(UnmanagedType.LPStr)] string datestr);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_include_date_w(
        TypedPointer<archive> archive,
        ArchiveMatchFlags flag,
        [MarshalAs(UnmanagedType.LPWStr)] string datestr);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_include_file_time(
        TypedPointer<archive> archive,
        ArchiveMatchFlags flag,
        [MarshalAs(UnmanagedType.LPStr)] string pathname);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_include_file_time_w(
        TypedPointer<archive> archive,
        ArchiveMatchFlags flag,
        [MarshalAs(UnmanagedType.LPWStr)] string pathname);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_exclude_entry(
        TypedPointer<archive> archive,
        ArchiveMatchFlags flag,
        TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_owner_excluded(
        TypedPointer<archive> archive,
        TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_include_uid(
        TypedPointer<archive> archive,
        [NativeTypeName("la_int64_t")] long uid);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_include_gid(
        TypedPointer<archive> archive,
        [NativeTypeName("la_int64_t")] long gid);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_include_uname(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string uname);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_include_uname_w(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPWStr)] string uname);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_match_include_gname(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPStr)] string gname);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_match_include_gname_w(
        TypedPointer<archive> archive,
        [MarshalAs(UnmanagedType.LPWStr)] string gname);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_utility_string_sort(TypedPointer<string_array_ansi> strings);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive_entry *")]
    public static extern archive_entry* archive_entry_clear(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive_entry *")]
    public static extern archive_entry* archive_entry_clone(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_free(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive_entry *")]
    public static extern TypedPointer<archive_entry> archive_entry_new();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive_entry *")]
    public static extern TypedPointer<archive_entry> archive_entry_new2([NativeTypeName("struct archive *")] TypedPointer<archive> a);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("time_t")]
    public static extern long archive_entry_atime(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("long")]
    public static extern int archive_entry_atime_nsec(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_atime_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("time_t")]
    public static extern long archive_entry_birthtime(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("long")]
    public static extern int archive_entry_birthtime_nsec(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_birthtime_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("time_t")]
    public static extern long archive_entry_ctime(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("long")]
    public static extern int archive_entry_ctime_nsec(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_ctime_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("dev_t")]
    public static extern uint archive_entry_dev(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_dev_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("dev_t")]
    public static extern uint archive_entry_devmajor(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("dev_t")]
    public static extern uint archive_entry_devminor(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("unsigned short")]
    public static extern ArchiveEntryType archive_entry_filetype(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_filetype_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_fflags(
        TypedPointer<archive_entry> entry,
        out uint set,
        out uint clear);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_fflags_text(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_entry_gid(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_gid_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_gname(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPUTF8Str)]
    public static extern string archive_entry_gname_utf8(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPWStr)]
    public static extern string archive_entry_gname_w(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_hardlink(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPUTF8Str)]
    public static extern string archive_entry_hardlink_utf8(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPWStr)]
    public static extern string archive_entry_hardlink_w(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_entry_ino(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_entry_ino64(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_ino_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("unsigned short")]
    public static extern ushort archive_entry_mode(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("time_t")]
    public static extern long archive_entry_mtime(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("long")]
    public static extern int archive_entry_mtime_nsec(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_mtime_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("unsigned int")]
    public static extern uint archive_entry_nlink(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_pathname(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPUTF8Str)]
    public static extern string archive_entry_pathname_utf8(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPWStr)]
    public static extern string archive_entry_pathname_w(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("unsigned short")]
    public static extern ushort archive_entry_perm(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_perm_is_set(TypedPointer<archive_entry> entry);
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_rdev_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("dev_t")]
    public static extern uint archive_entry_rdev(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("dev_t")]
    public static extern uint archive_entry_rdevmajor(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("dev_t")]
    public static extern uint archive_entry_rdevminor(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_sourcepath(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPWStr)]
    public static extern string archive_entry_sourcepath_w(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_entry_size(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_size_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_strmode(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_symlink(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPUTF8Str)]
    public static extern string archive_entry_symlink_utf8(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveEntrySymlinkType archive_entry_symlink_type(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPWStr)]
    public static extern string archive_entry_symlink_w(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("la_int64_t")]
    public static extern long archive_entry_uid(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_uid_is_set(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_uname(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPUTF8Str)]
    public static extern string archive_entry_uname_utf8(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPWStr)]
    public static extern string archive_entry_uname_w(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_is_data_encrypted(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_is_metadata_encrypted(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool archive_entry_is_encrypted(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_atime(TypedPointer<archive_entry> entry, [NativeTypeName("time_t")] long param1, [NativeTypeName("long")] int param2);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_unset_atime(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_bhfi(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("BY_HANDLE_FILE_INFORMATION *")] Windows.Win32.Storage.FileSystem.BY_HANDLE_FILE_INFORMATION* param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_birthtime(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("time_t")] long t,
        [NativeTypeName("long")] int ns);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_unset_birthtime(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_ctime(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("time_t")] long t,
        [NativeTypeName("long")] int ns);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_unset_ctime(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_dev(TypedPointer<archive_entry> entry, [NativeTypeName("dev_t")] uint param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_devmajor(TypedPointer<archive_entry> entry, [NativeTypeName("dev_t")] uint param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_devminor(TypedPointer<archive_entry> entry, [NativeTypeName("dev_t")] uint param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_filetype(TypedPointer<archive_entry> entry, [NativeTypeName("unsigned int")] ArchiveEntryType type);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_fflags(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("unsigned long")] uint set,
        [NativeTypeName("unsigned long")] uint clear);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_copy_fflags_text(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPWStr)]
    public static extern string archive_entry_copy_fflags_text_w(
        TypedPointer<archive_entry> entry,
        [MarshalAs(UnmanagedType.LPWStr)] string flags);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_gid(TypedPointer<archive_entry> entry, [NativeTypeName("la_int64_t")] long param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_gname(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_gname_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_gname(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_gname_w(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPWStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_update_gname_utf8(
        TypedPointer<archive_entry> entry,
        [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_hardlink(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_hardlink_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_hardlink(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_hardlink_w(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPWStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_update_hardlink_utf8(
        TypedPointer<archive_entry> entry,
        [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_ino(TypedPointer<archive_entry> entry, [NativeTypeName("la_int64_t")] long param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_ino64(TypedPointer<archive_entry> entry, [NativeTypeName("la_int64_t")] long param1);

    /// <summary>
    /// Set symlink if symlink is already set, else set hardlink.
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="target"></param>
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_link(
        TypedPointer<archive_entry> entry,
        [MarshalAs(UnmanagedType.LPStr)] string target);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_link_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_link(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_link_w(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPWStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_update_link_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_mode(TypedPointer<archive_entry> entry, [NativeTypeName("unsigned short")] ushort param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_mtime(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("time_t")] long t,
        [NativeTypeName("long")] int ns);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_unset_mtime(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_nlink(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("unsigned int")] uint nlink);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_pathname(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_pathname_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_pathname(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_pathname_w(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPWStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_update_pathname_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_perm(TypedPointer<archive_entry> entry, [NativeTypeName("unsigned short")] ushort param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_rdev(TypedPointer<archive_entry> entry, [NativeTypeName("dev_t")] uint param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_rdevmajor(TypedPointer<archive_entry> entry, [NativeTypeName("dev_t")] uint param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_rdevminor(TypedPointer<archive_entry> entry, [NativeTypeName("dev_t")] uint param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_size(TypedPointer<archive_entry> entry, [NativeTypeName("la_int64_t")] long param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_unset_size(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_sourcepath(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_sourcepath_w(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPWStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_symlink(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_symlink_type(TypedPointer<archive_entry> entry, ArchiveEntrySymlinkType type);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_symlink_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_symlink(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_symlink_w(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPWStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_update_symlink_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_uid(TypedPointer<archive_entry> entry, [NativeTypeName("la_int64_t")] long param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_uname(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_uname_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_uname(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_uname_w(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPWStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_update_uname_utf8(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_is_data_encrypted(TypedPointer<archive_entry> entry, [NativeTypeName("char")] sbyte is_encrypted);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_set_is_metadata_encrypted(TypedPointer<archive_entry> entry, [NativeTypeName("char")] sbyte is_encrypted);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("const struct stat *")]
    public static extern void* archive_entry_stat(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_stat(TypedPointer<archive_entry> entry, [NativeTypeName("const struct stat *")] void* param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("const void *")]
    public static extern nint archive_entry_mac_metadata(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("size_t *")] out nuint s);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_copy_mac_metadata(TypedPointer<archive_entry> entry, [NativeTypeName("const void *")] void* param1, [NativeTypeName("size_t")] nuint param2);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("const unsigned char *")]
    public static extern byte* archive_entry_digest(TypedPointer<archive_entry> entry, int param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_acl_clear(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_acl_add_entry(TypedPointer<archive_entry> entry, int param1, int param2, int param3, int param4, [MarshalAs(UnmanagedType.LPStr)] string param5);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_entry_acl_add_entry_w(
        TypedPointer<archive_entry> entry,
        int type, int permset, int tag, int qual,
        [MarshalAs(UnmanagedType.LPWStr)] string name);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_acl_reset(
        TypedPointer<archive_entry> entry,
        ArchiveEntryAclType want_type);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_entry_acl_next(
        TypedPointer<archive_entry> entry,
        ArchiveEntryAclType want_type,
        out int type,
        out int permset,
        out int tag, out int id,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
        out string name);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.LPWStr)]
    public static extern string archive_entry_acl_to_text_w(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("la_ssize_t *")] out long text_len,
        ArchiveEntryAclType flags);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.LPStr)]
    public static extern string archive_entry_acl_to_text(TypedPointer<archive_entry> entry, [NativeTypeName("la_ssize_t *")] long* param1, int param2);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_entry_acl_from_text_w(
        TypedPointer<archive_entry> entry,
        [MarshalAs(UnmanagedType.LPWStr)] string text,
        ArchiveEntryAclType type);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_acl_from_text(TypedPointer<archive_entry> entry, [MarshalAs(UnmanagedType.LPStr)] string param1, int param2);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.LPWStr)]
    public static extern string archive_entry_acl_text_w(TypedPointer<archive_entry> entry, int param1);

    [Obsolete]
    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
    public static extern string archive_entry_acl_text(TypedPointer<archive_entry> entry, int param1);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveEntryAclType archive_entry_acl_types(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_acl_count(
        TypedPointer<archive_entry> entry,
        ArchiveEntryAclType want_type);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    [return: NativeTypeName("struct archive_acl *")]
    public static extern TypedPointer<archive_acl> archive_entry_acl(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_xattr_clear(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_xattr_add_entry(
        TypedPointer<archive_entry> entry,
        [MarshalAs(UnmanagedType.LPStr)] string name,
        [NativeTypeName("const void *")] void* value,
        [NativeTypeName("size_t")] nuint size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_xattr_count(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_xattr_reset(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_entry_xattr_next(
        TypedPointer<archive_entry> entry,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UnownedStr), MarshalCookie = UnownedStr.LPStr)]
        out string name,
        [NativeTypeName("const void **")] out nint value,
        [NativeTypeName("size_t *")] out nuint size);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_sparse_clear(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_sparse_add_entry(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("la_int64_t")] long offset,
        [NativeTypeName("la_int64_t")] long length);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_sparse_count(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int archive_entry_sparse_reset(TypedPointer<archive_entry> entry);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ArchiveError archive_entry_sparse_next(
        TypedPointer<archive_entry> entry,
        [NativeTypeName("la_int64_t *")] out long offset,
        [NativeTypeName("la_int64_t *")] out long length);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern TypedPointer<archive_entry_linkresolver> archive_entry_linkresolver_new();

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_linkresolver_set_strategy(TypedPointer<archive_entry_linkresolver> res, ArchiveFormat fmt);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_linkresolver_free(TypedPointer<archive_entry_linkresolver> param0);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void archive_entry_linkify(
        TypedPointer<archive_entry_linkresolver> res,
        [NativeTypeName("struct archive_entry **")] ref TypedPointer<archive_entry> e,
        [NativeTypeName("struct archive_entry **")] out TypedPointer<archive_entry> f);

    [DllImport("archive", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern TypedPointer<archive_entry> archive_entry_partial_links(TypedPointer<archive_entry_linkresolver> res, [NativeTypeName("unsigned int *")] uint* links);
}
