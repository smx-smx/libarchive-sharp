#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;

namespace tests
{
    public class BindingsCoverage
    {
        [Explicit("manual trigger, on-demand")]
        [Test]
        public void DebugWriteRemainingBindings()
        {
            var all = typeof(libarchive.Methods)
                .GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Select(m => m.Name)
                .ToHashSet();

            var deprecated = typeof(libarchive.Methods)
                .GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() != null)
                .Select(m => m.Name)
                .ToHashSet();

            var excluded = new HashSet<string>()
            {
                /// covered by <see cref="archive_write_set_format"/>
                nameof(archive_write_set_format_7zip),
                nameof(archive_write_set_format_cpio),
                nameof(archive_write_set_format_cpio_bin),
                nameof(archive_write_set_format_cpio_pwb),
                nameof(archive_write_set_format_cpio_odc),
                nameof(archive_write_set_format_cpio_newc),
                nameof(archive_write_set_format_iso9660),
                nameof(archive_write_set_format_mtree),
                nameof(archive_write_set_format_raw),
                nameof(archive_write_set_format_shar),
                nameof(archive_write_set_format_shar),
                nameof(archive_write_set_format_shar_dump),
                nameof(archive_write_set_format_pax_restricted),
                nameof(archive_write_set_format_gnutar),
                nameof(archive_write_set_format_pax),
                nameof(archive_write_set_format_pax_restricted),
                nameof(archive_write_set_format_ustar),
                nameof(archive_write_set_format_warc),
                nameof(archive_write_set_format_xar),
                nameof(archive_write_set_format_zip),
                /// covered by <see cref="archive_write_add_filter" />
                nameof(archive_write_add_filter_none),
                nameof(archive_write_add_filter_gzip),
                nameof(archive_write_add_filter_bzip2),
                nameof(archive_write_add_filter_compress),
                nameof(archive_write_add_filter_grzip),
                nameof(archive_write_add_filter_lrzip),
                nameof(archive_write_add_filter_lz4),
                nameof(archive_write_add_filter_lzip),
                nameof(archive_write_add_filter_lzma),
                nameof(archive_write_add_filter_lzip),
                nameof(archive_write_add_filter_uuencode),
                nameof(archive_write_add_filter_xz),
                nameof(archive_write_add_filter_zstd),
                /// covered by <see cref="archive_entry_hardlink_w" />
                nameof(archive_entry_hardlink),
                nameof(archive_entry_hardlink_utf8),
                nameof(archive_entry_set_hardlink),
                nameof(archive_entry_set_hardlink_utf8),
                nameof(archive_entry_copy_hardlink),
                /// covered by <see cref="archive_entry_symlink_w" />
                nameof(archive_entry_symlink),
                nameof(archive_entry_symlink_utf8),
                nameof(archive_entry_set_symlink),
                nameof(archive_entry_set_symlink_utf8),
                nameof(archive_entry_copy_symlink),
                /// covered by <see cref="archive_entry_pathname_w"/>
                nameof(archive_entry_pathname),
                nameof(archive_entry_pathname_utf8),
                nameof(archive_entry_set_pathname),
                nameof(archive_entry_set_pathname_utf8),
                /// covered by <see cref="archive_entry_uname_w" />
                nameof(archive_entry_uname),
                nameof(archive_entry_uname_utf8),
                nameof(archive_entry_set_uname),
                nameof(archive_entry_set_uname_utf8),
                nameof(archive_entry_copy_uname_w),
                /// covered by <see cref="archive_entry_gname_w" />
                nameof(archive_entry_gname),
                nameof(archive_entry_gname_utf8),
                nameof(archive_entry_set_gname),
                nameof(archive_entry_copy_gname),
                nameof(archive_entry_set_gname_utf8),
                /// covered by <see cref="archive_entry_copy_link_w" />
                nameof(archive_entry_set_link),
                nameof(archive_entry_set_link_utf8),
                nameof(archive_entry_copy_link),
                /// covered by <see cref="archive_entry_sourcepath_w"/>
                nameof(archive_entry_sourcepath),
                nameof(archive_entry_copy_sourcepath),
                /// covered by <see cref="archive_entry_new2" />
                nameof(archive_entry_new),
                /// covered by <see cref="archive_read_open1" /> and <see cref="ArchiveReader" />
                nameof(archive_read_open),
                nameof(archive_read_open2),
                nameof(archive_read_open_filename),
                nameof(archive_read_open_filenames),
                nameof(archive_read_open_filename_w),
                nameof(archive_read_open_filenames_w),
                nameof(archive_read_open_file),
                nameof(archive_read_open_memory),
                nameof(archive_read_open_memory2),
                nameof(archive_read_open_fd),
                nameof(archive_read_open_FILE),
                /// covered by <see cref="archive_write_open2" /> and <see cref="ArchiveWriter"/>
                nameof(archive_write_open),
                nameof(archive_write_open_fd),
                nameof(archive_write_open_filename),
                nameof(archive_write_open_filename_w),
                nameof(archive_write_open_file),
                nameof(archive_write_open_FILE),
                nameof(archive_write_open_memory),
                /// covered by <see cref="archive_entry_acl_from_text_w"/>
                nameof(archive_entry_acl_from_text),
                /// covered by <see cref="archive_entry_acl_add_entry_w"/>
                nameof(archive_entry_acl_add_entry),
                /// covered by <see cref="archive_entry_acl_to_text_w"/>
                nameof(archive_entry_acl_to_text),
                /// covered by <see cref="archive_read_support_format_by_code" />
                nameof(archive_read_support_format_7zip),
                nameof(archive_read_support_format_ar),
                nameof(archive_read_support_format_cab),
                nameof(archive_read_support_format_cpio),
                nameof(archive_read_support_format_empty),
                nameof(archive_read_support_format_iso9660),
                nameof(archive_read_support_format_lha),
                nameof(archive_read_support_format_mtree),
                nameof(archive_read_support_format_rar),
                nameof(archive_read_support_format_rar5),
                nameof(archive_read_support_format_raw),
                nameof(archive_read_support_format_tar),
                nameof(archive_read_support_format_warc),
                nameof(archive_read_support_format_xar),
                nameof(archive_read_support_format_zip),
                /// covered by <see cref="read_support_filter_by_code" />
                nameof(archive_read_support_filter_none),
                nameof(archive_read_support_filter_gzip),
                nameof(archive_read_support_filter_bzip2),
                nameof(archive_read_support_filter_compress),
                nameof(archive_read_support_filter_lzma),
                nameof(archive_read_support_filter_xz),
                nameof(archive_read_support_filter_uu),
                nameof(archive_read_support_filter_rpm),
                nameof(archive_read_support_filter_lzip),
                nameof(archive_read_support_filter_lrzip),
                nameof(archive_read_support_filter_lzop),
                nameof(archive_read_support_filter_grzip),
                nameof(archive_read_support_filter_lz4),
                nameof(archive_read_support_filter_zstd),
                /// covered by <see cref="archive_entry_copy_pathname_w" />
                nameof(archive_entry_copy_pathname),
                /// covered by <see cref="archive_match_include_gname_w"/>
                nameof(archive_match_include_gname),
                /// covered by <see cref="archive_match_include_uname_w" />
                nameof(archive_match_include_uname),
                /// covered by <see cref="archive_match_exclude_pattern_w" />
                nameof(archive_match_exclude_pattern),
                /// covered by <see cref="archive_match_include_pattern_w" />
                nameof(archive_match_include_pattern),
                /// covered by <see cref="archive_match_exclude_pattern_from_file_w" />
                nameof(archive_match_exclude_pattern_from_file),
                /// covered by <see cref="archive_match_include_pattern_from_file_w" />
                nameof(archive_match_include_pattern_from_file),
                /// covered by <see cref="archive_match_include_date_w" />
                nameof(archive_match_include_date),
                /// covered by <see cref="archive_entry_copy_fflags_text_w" />
                nameof(archive_entry_copy_fflags_text),
                // exclude multi-encoding string updates (we always use Unicode)
                nameof(archive_entry_update_gname_utf8),
                nameof(archive_entry_update_hardlink_utf8),
                nameof(archive_entry_update_link_utf8),
                nameof(archive_entry_update_pathname_utf8),
                nameof(archive_entry_update_symlink_utf8),
                nameof(archive_entry_update_uname_utf8),
                /// covered by <see cref="archive_match_include_file_time_w" />
                nameof(archive_match_include_file_time),
                /// covered by <see cref="archive_match_path_unmatched_inclusions_next_w" />
                nameof(archive_match_path_unmatched_inclusions_next)
            }.Concat(deprecated);

            var asm = AssemblyDefinition.ReadAssembly("libarchive-sharp.dll");

            var called = asm.Modules
                .Where(mod => mod.HasTypes)
                // types + nested types (IEnumerable with yield)
                .SelectMany(mod => mod.Types.SelectMany(t => new TypeDefinition[] { t }.Concat(t.NestedTypes)))
                .Where(type => type.HasMethods)
                .SelectMany(type => type.Methods)
                .Where(meth => meth != null && meth.HasBody)
                .SelectMany(meth => meth.Body.Instructions)
                .Select(insn => insn.Operand as MethodReference)
                .Where(opnd => opnd != null && opnd.DeclaringType.FullName == "libarchive.Methods")
                .Select(opnd => opnd!.Name)
                .ToHashSet();


            var pop = all.Except(excluded);
            var notCalled = string.Join(Environment.NewLine, pop.Except(called));
            File.WriteAllText("todo.txt", notCalled);

            var deprecatedCalls = called.Intersect(deprecated);
            if (deprecatedCalls.Any())
            {
                foreach (var call in deprecatedCalls)
                {
                    Console.Error.WriteLine($"[WARN] deprecated function used: {call}");
                }
            }

            var popCount = pop.Count();
            var coverage = (((double)called.Count / popCount) * 100).ToString("F2");
            Console.WriteLine($"Coverage: {called.Count}/{popCount} ({coverage}%)");
        }
    }
}
