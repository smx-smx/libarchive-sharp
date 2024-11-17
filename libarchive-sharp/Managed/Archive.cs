#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using Smx.SharpIO.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class Archive : IDisposable
    {
        private readonly TypedPointer<archive> _handle;
        protected bool _disposed;
        protected readonly bool _owned;

        protected Archive(TypedPointer<archive> handle, bool owned)
        {
            _handle = handle;
            _owned = owned;
        }

        public int LastError => archive_errno(_handle);
        public string LastErrorString => archive_error_string(_handle);

        public void SetError(int errorNumber, string errorMessage)
        {
            // escape sprintf string
            errorMessage = errorMessage.Replace("%", "%%");
            archive_set_error(_handle, errorNumber, errorMessage, __arglist());
        }

        public void ClearError()
        {
            archive_clear_error(_handle);
        }

        public int FileCount => archive_file_count(_handle);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }

                if (_owned)
                {
                    archive_free(_handle);
                }

                _disposed = true;
            }
        }

        ~Archive()
        {
            Dispose(disposing: false);
        }

        public virtual void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
