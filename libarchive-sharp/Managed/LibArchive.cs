#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libarchive.Managed
{
    public class LibArchive
    {
        public static void Initialize(string dllPath)
        {
#if NETCOREAPP
            NativeLibrary.SetDllImportResolver(
                Assembly.GetExecutingAssembly(),
                (libName, assembly, searchPath) =>
                {
                    return libName switch
                    {
                        "archive" => NativeLibrary.Load(dllPath),
                        _ => 0
                    };
                });
#endif
        }
    }
}
