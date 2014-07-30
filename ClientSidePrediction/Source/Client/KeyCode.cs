// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyCode.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Client
{
    /// <summary>
    ///   Codes representing keyboard keys.
    /// </summary>
    /// <remarks>
    ///   Key code documentation:
    ///   http://msdn.microsoft.com/en-us/library/dd375731%28v=VS.85%29.aspx
    /// </remarks>
    internal enum KeyCode
    {
        /// <summary>
        ///   The left arrow key.
        /// </summary>
        Left = 0x25,

        /// <summary>
        ///   The up arrow key.
        /// </summary>
        Up,

        /// <summary>
        ///   The right arrow key.
        /// </summary>
        Right,

        /// <summary>
        ///   The down arrow key.
        /// </summary>
        Down,
    }
}