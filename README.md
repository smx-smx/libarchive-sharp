# libarchive-sharp
.NET bindings for [libarchive](https://libarchive.org/)

Initial bindings created with [ClangSharp](https://github.com/dotnet/ClangSharp) and then refactored.

Example usage (taken from unit tests)

```csharp
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
Console.WriteLine(entry.PathName);
Assert.That(entry.PathName == "test.txt");
Assert.That(entry.FileType == ArchiveEntryType.File);
Assert.That(entry.Permissions == Convert.ToUInt16("644", 8));
Assert.That(entry.Size > 0);
```