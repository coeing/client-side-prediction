// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Server.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Server
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using ClientSidePrediction.Shared;

    public class Server
    {
        #region Fields

        /// <summary>
        ///   Interval with which the physics simulations are updated.
        /// </summary>
        public float PhysicsUpdateInterval;

        /// <summary>
        ///   Connected clients.
        /// </summary>
        private readonly List<ConnectedClient> clients = new List<ConnectedClient>();

        /// <summary>
        ///   Logs the physics simulation steps.
        /// </summary>
        private readonly StreamWriter log;

        /// <summary>
        ///   Network interface.
        /// </summary>
        private readonly Network network;

        /// <summary>
        ///   Physics simulation of clients.
        /// </summary>
        private readonly MovePhysics physics = new MovePhysics();

        /// <summary>
        ///   Accumulated update time.
        /// </summary>
        private float accumulatedTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="network">Network interface.</param>
        public Server(Network network)
        {
            this.network = network;
            this.network.InputReceived += this.OnClientInputReceived;
            this.log = File.CreateText("server.txt");
            this.log.AutoFlush = true;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Conncects the client with the specified id.
        /// </summary>
        /// <param name="clientId">Id of client to connect.</param>
        public void ConnectClient(int clientId)
        {
            // Not connected client, so connect.
            ConnectedClient client = new ConnectedClient { ClientId = clientId };
            this.clients.Add(client);
        }

        /// <summary>
        ///   Returns the move state of the client with the specified id.
        /// </summary>
        /// <param name="clientId">Id of client to get move state for.</param>
        /// <returns>Move state of client with specified id.</returns>
        public MoveState GetClientMoveState(int clientId)
        {
            ConnectedClient client = this.clients.FirstOrDefault(
                connectedClient => connectedClient.ClientId == clientId);
            return client != null ? client.MoveState : null;
        }

        public void Update(float deltaTime)
        {
            this.accumulatedTime += deltaTime;

            while (this.accumulatedTime > this.PhysicsUpdateInterval)
            {
                this.UpdateWorld(this.PhysicsUpdateInterval);
                this.accumulatedTime -= this.PhysicsUpdateInterval;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Callback when client input was received.
        /// </summary>
        /// <param name="clientInput">Received client input.</param>
        private void OnClientInputReceived(ClientInput clientInput)
        {
            // Get character this input is for.
            ConnectedClient client =
                this.clients.FirstOrDefault(connectedClient => connectedClient.ClientId == clientInput.ClientId);
            if (client == null)
            {
                return;
            }
            client.Input = clientInput.Input;
            client.InputNumber = clientInput.InputNumber;
        }

        /// <summary>
        ///   Sends the world state to all clients.
        /// </summary>
        private void SendWorldState()
        {
            WorldState state = new WorldState { ClientStates = new Dictionary<int, ClientState>() };
            foreach (var client in this.clients)
            {
                state.ClientStates[client.ClientId] = new ClientState
                    {
                        InputNumber = client.InputNumber,
                        MoveState = client.MoveState
                    };
            }

            this.network.SendToClients(state);
        }

        private void UpdateWorld(float updateInterval)
        {
            // Update all clients.
            foreach (var client in this.clients)
            {
                client.MoveState = this.physics.TickSimulation(client.MoveState, client.Input, updateInterval);

                if (client.Input != null && client.Input.HorizontalAxis != 0)
                {
                    this.log.WriteLine("{0} {1}", client.Input.HorizontalAxis == -1 ? 'L' : 'R', client.MoveState.X);
                }
            }

            // Send world state to clients.
            this.SendWorldState();
        }

        #endregion
    }
}