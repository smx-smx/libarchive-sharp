#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
﻿using Smx.SharpIO.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libarchive;

public class StringPool
{
    private IList<string> _items = new List<string>();

    public void Add(string item)
    {
        _items.Add(item);
    }

    public NativeMemoryHandle ToNative(Encoding encoding)
    {
        var encoder = encoding.GetEncoder();
        var nullSize = encoder.GetByteCount("\0", true);

        var size_pointers = (_items.Count + 1) * nint.Size;
        var size_data = _items.Sum(s => encoder.GetByteCount(s, true) + nullSize);
        var size = size_pointers + size_data;

        var mem = MemoryHGlobal.Alloc(size);
        var span_data = mem.Span.Slice(size_pointers);
        var data_ptr = mem.Address + size_pointers;

        int i;
        for (i = 0; i < _items.Count; i++)
        {
            /** convert and copy string **/
            var bytes = encoding.GetBytes(_items[i]);
            bytes.CopyTo(span_data);

            /** write string pointer **/
            Marshal.WriteIntPtr(
                mem.Address + (i * nint.Size),
                data_ptr
            );

            /** advance memory view **/
            span_data = span_data.Slice(bytes.Length);
            data_ptr += bytes.Length;
        }
        Marshal.WriteIntPtr(mem.Address + (i * nint.Size), 0);
        return mem;
    }
}
