#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
namespace libarchive.Managed
{
    public class ArchiveDataStreamer
    {
        private readonly IEnumerable<ArchiveDataBlock> _producer;

        public ArchiveDataStreamer(IEnumerable<ArchiveDataBlock> producer)
        {
            _producer = producer;
        }

        public IEnumerable<byte> GetBytes()
        {
            long currentOffset = 0;
            foreach (var block in _producer)
            {
                var delta = block.Offset - currentOffset;
                if (delta > 0)
                {
                    // padding
                    for (long i = 0; i < delta; i++)
                    {
                        yield return 0;
                    }
                }

                foreach (var b in block.Data.ToArray())
                {
                    yield return b;
                }
            }
        }
    }
}
