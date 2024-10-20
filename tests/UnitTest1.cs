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
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            LibArchive.Initialize(@"P:\projects\libarchive-sharp\libarchive\bin\archive");
        }

        [Test]
        public void Test1()
        {
            using var ms = new MemoryStream();
            using (var writer = new ArchiveWriter(
                ms,
                ArchiveFormat.sevenzip,
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
            var reader = new ArchiveReader(ms);

            var entry = reader.Entries.First();
            Assert.That(entry.PathName == "test.txt");
            Assert.That(entry.FileType == ArchiveEntryType.File);
            Assert.That(entry.Permissions == Convert.ToUInt16("644", 8));
            Assert.That(entry.Size > 0);

            // extract file
            using var dwr = new ArchiveDiskWriter();
            dwr.AddEntry(entry, reader.InputStream);

            Assert.That(File.Exists("test.txt"));
            Assert.That(File.ReadAllText("test.txt") == "Hello world");
        }
    }
}
