// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientState.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Shared
{
    public class ClientState
    {
        #region Fields

        /// <summary>
        ///   Latest input which was applied to the move state.
        /// </summary>
        public int InputNumber;

        /// <summary>
        ///   Current move state on server.
        /// </summary>
        public MoveState MoveState;
        
        #endregion
    }
}