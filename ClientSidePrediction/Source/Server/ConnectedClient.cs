// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectedClient.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Server
{
    using ClientSidePrediction.Shared;

    public class ConnectedClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public ConnectedClient()
        {
            this.MoveState = new MoveState();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Id of connected client.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        ///   Most recent input data.
        /// </summary>
        public Input Input { get; set; }

        /// <summary>
        ///   Number of last received input.
        /// </summary>
        public int InputNumber { get; set; }

        /// <summary>
        ///   Client move state on server.
        /// </summary>
        public MoveState MoveState { get; set; }

        #endregion
    }
}