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
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveCallbackData
    {
        private readonly TypedPointer<archive> _handle;

        public ArchiveCallbackData(TypedPointer<archive> handle)
        {
            _handle = handle;
        }

        public void Set(nint client_data)
        {
            var err = archive_read_set_callback_data(_handle, client_data);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_set_callback_data), err);
            }
        }

        public void Set(nint client_data, uint index)
        {
            var err = archive_read_set_callback_data2(_handle, client_data, index);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_set_callback_data2), err);
            }
        }

        public void Add(nint client_data, uint index)
        {
            var err = archive_read_add_callback_data(_handle, client_data, index);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_add_callback_data), err);
            }
        }

        public void Append(nint client_data)
        {
            var err = archive_read_append_callback_data(_handle, client_data);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_append_callback_data), err);
            }
        }

        public void Prepend(nint client_data)
        {
            var err = archive_read_prepend_callback_data(_handle, client_data);
            if (err != ArchiveError.OK)
            {
                throw new ArchiveOperationFailedException(_handle, nameof(archive_read_prepend_callback_data), err);
            }
        }

    }
}
