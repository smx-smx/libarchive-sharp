#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
﻿using Smx.SharpIO;
using Smx.SharpIO.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libarchive.Managed
{
    public class ArchiveClient : IDisposable
    {
        private readonly TypedPointer<archive> _archive;
        private readonly DisposableGCHandle _instance;

        public ArchiveClient(TypedPointer<archive> archive)
        {
            _archive = archive;
            _instance = DisposableGCHandle.Pin(this);
        }

        public void Dispose()
        {
            _instance.Dispose();
        }
    }
}
