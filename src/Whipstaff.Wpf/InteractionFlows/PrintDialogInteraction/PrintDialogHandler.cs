﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Whipstaff.Wpf.InteractionFlows.PrintDialogInteraction
{
    /// <summary>
    /// Handling logic for a FileOpenDialog interaction request.
    /// </summary>
    public static class PrintDialogHandler
    {
        /// <summary>
        /// Helper for handling the Windows Print Dialog.
        /// </summary>
        /// <param name="request">Request settings for the Print Dialog.</param>
        /// <returns>Whether the Print Dialog was confirmed or cancelled.</returns>
        public static Task<PrintDialogResult> OnPrintDialog(PrintDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            PrintDialog dialog = new();

            var dialogResult = dialog.ShowDialog() ?? false;

            if (!dialogResult)
            {
                return Task.FromResult(PrintDialogResult.Cancelled());
            }

            return Task.FromResult(PrintDialogResult.Proceed());
        }
    }
}