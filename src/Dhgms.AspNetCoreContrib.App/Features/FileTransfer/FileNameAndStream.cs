﻿// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

namespace Dhgms.AspNetCoreContrib.App.Features.FileTransfer
{
    public sealed class FileNameAndStream
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
