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
using System;
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

        private MemoryStream GivenTestArchive()
        {
            var ms = new MemoryStream();
            using (var writer = new ArchiveWriter(
                ms,
                ArchiveFormat.TAR_USTAR,
                leaveOpen: true
            ))
            {
                writer.AddEntry(new ArchiveEntry
                {
                    PathName = "test1.txt",
                    FileType = ArchiveEntryType.File,
                    Permissions = Convert.ToUInt16("644", 8)
                }, new MemoryStream(Encoding.ASCII.GetBytes("Hello world")));

                writer.AddEntry(new ArchiveEntry
                {
                    PathName = "test2.txt",
                    FileType = ArchiveEntryType.File,
                    Permissions = Convert.ToUInt16("644", 8)
                }, new MemoryStream(Encoding.ASCII.GetBytes("Hello again")));
            }
            ms.Position = 0;
            File.WriteAllBytes("arch.tar.bz2", ms.ToArray());
            return ms;
        }

        [Test]
        public void TestSkip()
        {
            using var ms = GivenTestArchive();
            var reader = new ArchiveReader(ms, new ArchiveReaderOptions
            {
                EnableFormats = [ArchiveFormat.TAR_USTAR],
                EnableFilters = [ArchiveFilter.BZIP2]
            });

            var entry = reader.Entries.Skip(1).First();
            Assert.That(entry.Header.PathName, Is.EqualTo("test2.txt"));
            Assert.That(entry.Header.FileType, Is.EqualTo(ArchiveEntryType.File));
            Assert.That(entry.Header.Permissions, Is.EqualTo(Convert.ToUInt16("644", 8)));
            Assert.That(entry.Header.Size, Is.GreaterThan(0));
            Assert.That(Encoding.ASCII.GetString(entry.Bytes.ToArray()), Is.EqualTo("Hello again"));
        }

        [Test]
        public void TestStream()
        {
            using var ms = GivenTestArchive();
            var reader = new ArchiveReader(ms, new ArchiveReaderOptions
            {
                EnableFormats = [ArchiveFormat.TAR_USTAR],
                EnableFilters = [ArchiveFilter.BZIP2]
            });

            foreach (var entry in reader.Entries)
            {
                switch (entry.Index)
                {
                    case 0:
                        Assert.That(entry.Header.PathName, Is.EqualTo("test1.txt"));
                        Assert.That(entry.Header.FileType, Is.EqualTo(ArchiveEntryType.File));
                        Assert.That(entry.Header.Permissions, Is.EqualTo(Convert.ToUInt16("644", 8)));
                        Assert.That(entry.Header.Size, Is.GreaterThan(0));
                        Assert.That(Encoding.ASCII.GetString(entry.Bytes.ToArray()), Is.EqualTo("Hello world"));
                        break;
                    case 1:
                        Assert.That(entry.Header.PathName, Is.EqualTo("test2.txt"));
                        Assert.That(entry.Header.FileType, Is.EqualTo(ArchiveEntryType.File));
                        Assert.That(entry.Header.Permissions, Is.EqualTo(Convert.ToUInt16("644", 8)));
                        Assert.That(entry.Header.Size, Is.GreaterThan(0));
                        Assert.That(Encoding.ASCII.GetString(entry.Bytes.ToArray()), Is.EqualTo("Hello again"));
                        break;
                }
            }
        }

        [Test]
        public void TestExtract()
        {
            using var ms = GivenTestArchive();

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
            Assert.That(entry.Header.PathName, Is.EqualTo("test1.txt"));
            Assert.That(entry.Header.FileType, Is.EqualTo(ArchiveEntryType.File));
            Assert.That(entry.Header.Permissions, Is.EqualTo(Convert.ToUInt16("644", 8)));
            Assert.That(entry.Header.Size, Is.GreaterThan(0));

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
