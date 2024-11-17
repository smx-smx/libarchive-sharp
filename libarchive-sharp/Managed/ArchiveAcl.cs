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
    public class ArchiveAcl
    {
        private readonly TypedPointer<archive_acl> _handle;

        public ArchiveAcl(TypedPointer<archive_acl> handle)
        {
            _handle = handle;
        }

        public ArchiveAcl(TypedPointer<archive_entry> entry)
            : this(NewHandle(entry))
        { }

        private static TypedPointer<archive_acl> NewHandle(TypedPointer<archive_entry> entry)
        {
            var handle = archive_entry_acl(entry);
            if (handle.Address == 0)
            {
                throw new ArchiveOperationFailedException(nameof(archive_entry_acl), "corrupted memory");
            }
            return handle;
        }
    }
}
