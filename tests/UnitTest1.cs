#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using libarchive;
using libarchive.Managed;
using Microsoft.VisualStudio.CodeCoverage;
using Mono.Cecil;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using static libarchive.Methods;

namespace tests
{
    public class Tests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            if (OperatingSystem.IsWindows())
            {
                // $FIXME
                LibArchive.Initialize(@"P:\projects\libarchive-sharp\libarchive\bin\archive");
            } else
            {
                LibArchive.Initialize(@"/usr/lib/x86_64-linux-gnu/libarchive.so");
            }
        }

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
                nameof(archive_write_set_format_ar_bsd),
                nameof(archive_write_set_format_ar_svr4),
                nameof(archive_write_set_format_cpio),
                nameof(archive_write_set_format_cpio_bin),
                nameof(archive_write_set_format_cpio_newc),
                nameof(archive_write_set_format_cpio_odc),
                nameof(archive_write_set_format_cpio_pwb),
                nameof(archive_write_set_format_gnutar),
                nameof(archive_write_set_format_iso9660),
                nameof(archive_write_set_format_mtree),
                nameof(archive_write_set_format_mtree_classic),
                nameof(archive_write_set_format_pax),
                nameof(archive_write_set_format_pax_restricted),
                nameof(archive_write_set_format_raw),
                nameof(archive_write_set_format_shar),
                nameof(archive_write_set_format_shar_dump),
                nameof(archive_write_set_format_ustar),
                nameof(archive_write_set_format_v7tar),
                nameof(archive_write_set_format_warc),
                nameof(archive_write_set_format_xar),
                nameof(archive_write_set_format_zip),
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

        [Test]
        public void Test1()
        {
            using var ms = new MemoryStream();
            using (var writer = new ArchiveWriter(
                ms,
                ArchiveFormat.TAR_USTAR,
                leaveOpen: true
            ))
            {
                writer.AddEntry(new ArchiveEntry
                {
                    PathName = "test.txt",
                    FileType = ArchiveEntryType.File,
                    Permissions = Convert.ToUInt16("644", 8)
                }, new MemoryStream(Encoding.ASCII.GetBytes("Hello world")));
            }

            ms.Position = 0;

            File.WriteAllBytes("arch.tar.bz2", ms.ToArray());

            var reader = new ArchiveReader(ms, new ArchiveReaderOptions
            {
                EnableFormats = [ArchiveFormat.TAR_USTAR],
                EnableFilters = [ArchiveFilter.BZIP2]
            });

            File.Delete("file.txt");

            var entry = reader.Entries.First();
            Assert.That(entry.PathName, Is.EqualTo("test.txt"));
            Assert.That(entry.FileType, Is.EqualTo(ArchiveEntryType.File));
            Assert.That(entry.Permissions, Is.EqualTo(Convert.ToUInt16("644", 8)));
            Assert.That(entry.Size, Is.GreaterThan(0));

            // extract file
            using var dwr = new ArchiveDiskWriter();
            dwr.AddEntry(entry, reader.InputStream);

            Assert.That(File.Exists("test.txt"));
            Assert.That(File.ReadAllText("test.txt"), Is.EqualTo("Hello world"));

            reader.SetError(1234, "testing %s");
            Assert.That(reader.LastError, Is.EqualTo(1234));
            Assert.That(reader.LastErrorString, Is.EqualTo("testing %s"));
        }
    }
}
