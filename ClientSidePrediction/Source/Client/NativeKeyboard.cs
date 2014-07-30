// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeKeyboard.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Client
{
    using System.Runtime.InteropServices;

    /// <summary>
    ///   Provides keyboard access.
    /// </summary>
    internal static class NativeKeyboard
    {
        #region Constants

        /// <summary>
        ///   A positional bit flag indicating the part of a key state denoting
        ///   key pressed.
        /// </summary>
        private const int KeyPressed = 0x8000;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns a value indicating if a given key is pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>
        ///   <c>true</c> if the key is pressed, otherwise <c>false</c>.
        /// </returns>
        public static bool IsKeyDown(KeyCode key)
        {
            return (GetKeyState((int)key) & KeyPressed) != 0;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Gets the key state of a key.
        /// </summary>
        /// <param name="key">Virtuak-key code for key.</param>
        /// <returns>The state of the key.</returns>
        [DllImport("user32.dll")]
        private static extern short GetKeyState(int key);

        #endregion
    }
}