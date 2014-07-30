// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Network.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Shared
{
    using System;
    using System.Collections.Generic;

    public sealed class Network
    {
        #region Fields

        /// <summary>
        ///   Delayed actions to perform after a lag.
        /// </summary>
        private readonly List<DelayedAction> actions = new List<DelayedAction>();

        /// <summary>
        ///   Lag (in s).
        /// </summary>
        private readonly float lag;

        #endregion

        #region Constructors and Destructors

        public Network(float lag)
        {
            this.lag = lag;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Called when client input was received.
        /// </summary>
        public event Action<ClientInput> InputReceived;

        /// <summary>
        ///   Called when a world state was received.
        /// </summary>
        public event Action<WorldState> StateReceived;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Sends the specified world state to all clients.
        /// </summary>
        /// <param name="state">State to send.</param>
        public void SendToClients(WorldState state)
        {
            this.AddLagAction(() => this.OnStateReceived(state), this.lag);
        }

        /// <summary>
        ///   Sends the specified client input to the server.
        /// </summary>
        /// <param name="clientInput">Client input to send.</param>
        public void SendToServer(ClientInput clientInput)
        {
            this.AddLagAction(() => this.OnInputReceived(clientInput), this.lag);
        }

        public void Update(float deltaTime)
        {
            // Check delayed actions.
            for (int index = this.actions.Count - 1; index >= 0; --index)
            {
                var delayedAction = this.actions[index];
                delayedAction.RemainingDelay -= deltaTime;
                if (delayedAction.RemainingDelay <= 0.0f)
                {
                    delayedAction.Action();
                    this.actions.RemoveAt(index);
                }
            }
        }

        #endregion

        #region Methods

        private void AddLagAction(Action action, float actionLag)
        {
            this.actions.Add(new DelayedAction { RemainingDelay = actionLag, Action = action });
        }

        private void OnInputReceived(ClientInput obj)
        {
            Action<ClientInput> handler = this.InputReceived;
            if (handler != null)
            {
                handler(obj);
            }
        }

        private void OnStateReceived(WorldState obj)
        {
            Action<WorldState> handler = this.StateReceived;
            if (handler != null)
            {
                handler(obj);
            }
        }

        #endregion

        private class DelayedAction
        {
            #region Fields

            /// <summary>
            ///   Action to perform after delay.
            /// </summary>
            public Action Action;

            /// <summary>
            ///   Remaining delay of action (in s).
            /// </summary>
            public float RemainingDelay;

            #endregion
        }
    }
}