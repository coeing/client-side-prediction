// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Client.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Client
{
    using System.Collections.Generic;
    using System.IO;

    using ClientSidePrediction.Shared;

    public class Client
    {
        #region Fields

        /// <summary>
        ///   Interval with which the physics simulation is updated.
        /// </summary>
        public float PhysicsUpdateInterval;

        /// <summary>
        ///   Indicates if prediction is on/off.
        /// </summary>
        public bool Prediction;

        /// <summary>
        ///   Indicates if reconciliation is on/off.
        /// </summary>
        public bool Reconciliation;

        /// <summary>
        ///   Logs the physics simulation steps.
        /// </summary>
        private readonly StreamWriter log;

        /// <summary>
        ///   Network interface.
        /// </summary>
        private readonly Network network;

        /// <summary>
        ///   Physics simulation.
        /// </summary>
        private readonly MovePhysics physics = new MovePhysics();

        /// <summary>
        ///   Sent but unacknowledged inputs.
        /// </summary>
        private readonly Queue<ClientInput> unacknowledgedInputs = new Queue<ClientInput>();

        /// <summary>
        ///   Accumulated time which was not used for physics simulation yet (in s).
        /// </summary>
        private float accumulatedPhysicsTime;

        /// <summary>
        ///   Counts the inputs.
        /// </summary>
        private int inputCounter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="network">Network interface.</param>
        public Client(Network network)
        {
            this.MoveState = new MoveState();
            this.network = network;
            this.network.StateReceived += this.OnStateReceived;

            this.log = File.CreateText("client.txt");
            this.log.AutoFlush = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Unique client id.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        ///   Current move state on client.
        /// </summary>
        public MoveState MoveState { get; private set; }

        /// <summary>
        ///   Number of unachknowledged inputs.
        /// </summary>
        public int UnacknowledgedInputsCount
        {
            get
            {
                return this.unacknowledgedInputs.Count;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Update(float deltaTime)
        {
            this.accumulatedPhysicsTime += deltaTime;

            while (this.accumulatedPhysicsTime >= this.PhysicsUpdateInterval)
            {
                this.FixedUpdate(this.PhysicsUpdateInterval);

                this.accumulatedPhysicsTime -= this.PhysicsUpdateInterval;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Fetches the current input on client.
        /// </summary>
        /// <returns>Current input.</returns>
        private Input FetchInput()
        {
            Input input;
            if (NativeKeyboard.IsKeyDown(KeyCode.Left))
            {
                input = new Input(-1);
            }
            else if (NativeKeyboard.IsKeyDown(KeyCode.Right))
            {
                input = new Input(1);
            }
            else
            {
                input = new Input(0);
            }
            return input;
        }

        private void FixedUpdate(float updateInterval)
        {
            // Get input.
            Input input = this.FetchInput();

            // Send to server.
            ClientInput clientInput = new ClientInput
                {
                    InputNumber = this.inputCounter++,
                    Input = input,
                    ClientId = this.ClientId
                };
            this.network.SendToServer(clientInput);

            if (this.Prediction)
            {
                this.unacknowledgedInputs.Enqueue(clientInput);

                // Apply input to state.
                this.MoveState = this.physics.TickSimulation(this.MoveState, input, updateInterval);

                if (input.HorizontalAxis != 0)
                {
                    this.log.WriteLine("{0} {1}", input.HorizontalAxis == -1 ? 'L' : 'R', this.MoveState.X);
                }
            }
        }

        /// <summary>
        ///   Callback when world state was received via network.
        /// </summary>
        /// <param name="state">Received world state.</param>
        private void OnStateReceived(WorldState state)
        {
            // Get own state.
            ClientState clientState = state.GetClientState(this.ClientId);
            if (clientState == null)
            {
                return;
            }

            // Remove inputs which arrived at server.
            while (this.unacknowledgedInputs.Count > 0
                   && this.unacknowledgedInputs.Peek().InputNumber <= clientState.InputNumber)
            {
                this.unacknowledgedInputs.Dequeue();
            }

            MoveState newMoveState = clientState.MoveState;

            if (this.Reconciliation)
            {
                // Re-apply unacknowledged inputs.
                foreach (var unacknowledgedInput in this.unacknowledgedInputs)
                {
                    newMoveState = this.physics.TickSimulation(
                        newMoveState, unacknowledgedInput.Input, this.PhysicsUpdateInterval);
                }
            }

            // Set new move state.
            this.MoveState = newMoveState;
        }

        #endregion
    }
}