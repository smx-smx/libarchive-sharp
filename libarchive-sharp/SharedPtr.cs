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

namespace libarchive
{
    /// <summary>
    /// Used to implement deterministic destruction of certain objects
    /// The idea is that anyone wishing to use the object must call <see cref="AddRef"/> to obtain the instance.
    /// After calling <see cref="Release"/>, the object will be disposed if the refcount is 0
    /// If the object should be kept alive, <see cref="AddRef" /> must be called after the ctor
    /// to bump the refcount to 1
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SharedPtr<T> where T : IDisposable
    {
        private T _obj;
        private uint _refCount;
        private bool _disposed;

        public SharedPtr(T obj)
        {
            _obj = obj;
            _refCount = 0;
            _disposed = false;
        }

        public T AddRef()
        {
            if (_disposed)
            {
                throw new InvalidOperationException("object already disposed");
            }
            Interlocked.Add(ref _refCount, 1);
            return _obj;
        }

        public void Release()
        {
            if (Interlocked.Decrement(ref _refCount) == 0)
            {
                _obj.Dispose();
                _disposed = true;
            }
        }
    }
}
