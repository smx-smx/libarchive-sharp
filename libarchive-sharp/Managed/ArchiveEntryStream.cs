#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using System.Collections;
using System.Runtime.InteropServices;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveEntryStream : IEnumerable<ArchiveEntryItem>, IEnumerator<ArchiveEntryItem>
    {
        private readonly ArchiveReader _reader;
        private ArchiveEntryItem? _item;
        private int _index;
        private bool _greedy;
        private bool _skipUnread;

        public ArchiveEntryStream(ArchiveReader reader, bool greedy = false, bool skipUnread = true)
        {
            _reader = reader;
            _index = 0;

            _greedy = greedy;
            _skipUnread = skipUnread;
        }

        public ArchiveEntryItem Current
        {
            get
            {
                if (_item == null) throw new InvalidOperationException(nameof(_item));
                return _item;
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        private IEnumerable<ArchiveDataBlock> GetBlocks()
        {
            for (; ; )
            {
                var err = archive_read_data_block(_reader,
                        out var buff,
                        out var size, out var offset);
                if (err != ArchiveError.OK)
                {
                    yield break;
                }

                var data = new byte[size];
                Marshal.Copy(buff.Address, data, 0, data.Length);
                var block = new ArchiveDataBlock(data, offset);
                yield return block;
            }
        }

        private void ConsumeDataBlocks(bool skipping)
        {
            if (skipping || _item == null)
            {
                var err = archive_read_data_skip(_reader);
                if (err != ArchiveError.OK)
                {
                    throw new ArchiveOperationFailedException(_reader, nameof(archive_read_data_skip), err);
                }
                return;
            }

            for (; ; )
            {
                var err = archive_read_data_block(_reader,
                    out var buff,
                    out var size, out var offset);
                if (err != ArchiveError.OK)
                {
                    if (err != ArchiveError.EOF)
                    {
                        throw new ArchiveOperationFailedException(_reader, nameof(archive_read_data_block), err);
                    }
                    break;
                }

                var block = new byte[size];
                Marshal.Copy(buff.Address, block, 0, block.Length);
                var item = new ArchiveDataBlock(block, offset);
            }
        }

        public bool MoveNext()
        {
            if (_item != null)
            {
                // skip unread data
                ConsumeDataBlocks(skipping: _skipUnread);
            }

            var entry = new ArchiveEntry();
            if (archive_read_next_header2(_reader, entry) != ArchiveError.OK)
            {
                entry.Dispose();
                return false;
            }
            _item = new ArchiveEntryItem(_index++, entry, GetBlocks());
            if (_greedy)
            {
                ConsumeDataBlocks(skipping: false);
            }
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
            //_index = 0;
        }

        public IEnumerator<ArchiveEntryItem> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
