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
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

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

            var msLazy = new Lazy<Stream>(() => ms);

            var reader = new ArchiveReader(new List<Lazy<Stream>> {
                msLazy
            }, new ArchiveReaderOptions
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
