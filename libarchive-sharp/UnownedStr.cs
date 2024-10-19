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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace libarchive
{
    public class UnownedStr : ICustomMarshaler
    {
        public const string LPStr = "LPStr";
        public const string LPWStr = "LPWStr";
        public const string LPUTF8Str = "LPUTF8Str";

        private readonly UnmanagedType _strType;

        public UnownedStr(UnmanagedType strType)
        {
            _strType = strType;
        }

        public static ICustomMarshaler GetInstance(string pstrCookie)
        {
            return new UnownedStr(Enum.Parse<UnmanagedType>(pstrCookie));
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(nint pNativeData)
        {
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public nint MarshalManagedToNative(object ManagedObj)
        {
            if (!(ManagedObj is string str))
            {
                throw new ArgumentException(ManagedObj.GetType().ToString());
            }

            switch (_strType)
            {
                case UnmanagedType.LPStr:
                    return Marshal.StringToCoTaskMemAnsi(str);
                case UnmanagedType.LPWStr:
                    return Marshal.StringToCoTaskMemUni(str);
                case UnmanagedType.LPUTF8Str:
                    return Marshal.StringToCoTaskMemUTF8(str);
                default:
                    throw new ArgumentException(Enum.GetName(_strType));
            }
        }

        public object MarshalNativeToManaged(nint pNativeData)
        {
            switch (_strType)
            {
                case UnmanagedType.LPStr:
                    return Marshal.PtrToStringAnsi(pNativeData)!;
                case UnmanagedType.LPWStr:
                    return Marshal.PtrToStringUni(pNativeData)!;
                case UnmanagedType.LPUTF8Str:
                    return Marshal.PtrToStringUTF8(pNativeData)!;
                default:
                    throw new ArgumentException(Enum.GetName(_strType));
            }
        }
    }
}
