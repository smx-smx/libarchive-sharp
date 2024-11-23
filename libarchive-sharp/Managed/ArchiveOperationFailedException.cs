#region License
/*
 * Copyright (C) 2024 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using Smx.SharpIO.Memory;
using static libarchive.Methods;

namespace libarchive.Managed
{
    public class ArchiveOperationFailedException : Exception
    {
        public ArchiveOperationFailedException(TypedPointer<archive> handle, string func, ArchiveError err)
            : base($"{func} failed: {Enum.GetName(err)} ({archive_error_string(handle)})")
        { }
        public ArchiveOperationFailedException(string func, ArchiveError err)
            : base($"{func} failed: {Enum.GetName(err)}")
        { }

        public ArchiveOperationFailedException(string func, string err)
            : base($"{func} failed: {err}")
        { }

        public ArchiveOperationFailedException(TypedPointer<archive> handle, string func)
            : base($"{func} failed: {archive_errno(handle):X}: {archive_error_string(handle)}")
        { }

        public ArchiveOperationFailedException(TypedPointer<archive> handle, string func, string err)
            : base($"{func} failed: {err}. ({archive_errno(handle):X}: {archive_error_string(handle)})")
        { }
    }
}
