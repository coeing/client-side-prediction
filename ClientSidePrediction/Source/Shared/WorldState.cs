// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldState.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Shared
{
    using System.Collections.Generic;

    public class WorldState
    {
        #region Public Properties

        /// <summary>
        ///   Maps character id to the current move state of the character.
        /// </summary>
        public Dictionary<int, ClientState> ClientStates { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the state for the client with the specified id.
        /// </summary>
        /// <param name="clientId">Id of client to get state for.</param>
        /// <returns>State of client with specified id.</returns>
        public ClientState GetClientState(int clientId)
        {
            ClientState clientState;
            this.ClientStates.TryGetValue(clientId, out clientState);
            return clientState;
        }

        #endregion
    }
}