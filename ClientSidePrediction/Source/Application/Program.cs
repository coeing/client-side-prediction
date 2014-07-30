// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Application
{
    using System;
    using System.Diagnostics;

    using ClientSidePrediction.Client;
    using ClientSidePrediction.Server;
    using ClientSidePrediction.Shared;

    internal class Program
    {
        #region Constants

        /// <summary>
        ///   Lag (in s).
        /// </summary>
        private const float Lag = 0.15f;

        /// <summary>
        ///   Interval with which the physics simulation is updated.
        /// </summary>
        private const float PhysicsUpdateInterval = 0.1f;

        /// <summary>
        ///   Interval with which the console is updated (in s).
        /// </summary>
        private const float RenderInterval = 0.02f;

        #endregion

        #region Fields

        /// <summary>
        ///   Client.
        /// </summary>
        private readonly Client client;

        /// <summary>
        ///   Network interface.
        /// </summary>
        private readonly Network network;

        /// <summary>
        ///   Server.
        /// </summary>
        private readonly Server server;

        /// <summary>
        ///   Accumulated time which was not used for rendering yet (in s).
        /// </summary>
        private float accumulatedTime;

        #endregion

        #region Constructors and Destructors

        public Program()
        {
            // Setup server/client.
            this.network = new Network(Lag);
            this.server = new Server(this.network) { PhysicsUpdateInterval = PhysicsUpdateInterval };
            this.client = new Client(this.network) { ClientId = 1, PhysicsUpdateInterval = PhysicsUpdateInterval };
            this.server.ConnectClient(this.client.ClientId);
        }

        #endregion

        #region Public Methods and Operators

        public void Render()
        {
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Usage");
            Console.WriteLine("-----");
            Console.WriteLine("Use the arrow keys (left|right) to move the character on the client.");
            Console.WriteLine("Turn prediction/reconciliation on/off with P/R.");

            Console.WriteLine();
            Console.WriteLine("Settings");
            Console.WriteLine("--------");
            Console.WriteLine("Lag: {0}, Update Interval: {1}", Lag, PhysicsUpdateInterval);
            Console.WriteLine("[P] Prediction: {0,-5}", this.client.Prediction);
            Console.WriteLine("[R] Reconciliation: {0,-5}", this.client.Reconciliation);

            Console.WriteLine();
            Console.WriteLine("Info");
            Console.WriteLine("----");
            Console.WriteLine("Unacknowledged Inputs: " + this.client.UnacknowledgedInputsCount);

            Console.WriteLine();

            const int ConsoleRange = 100;
            Console.WriteLine("Client: " + this.client.MoveState);
            Console.WriteLine(this.client.MoveState.Visualize(ConsoleRange));
            MoveState moveStateServer = this.server.GetClientMoveState(this.client.ClientId);
            Console.WriteLine("Server: " + moveStateServer);
            if (moveStateServer != null)
            {
                Console.WriteLine(moveStateServer.Visualize(ConsoleRange));
            }
        }

        #endregion

        #region Methods

        private static void Main()
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            // Update loop.
            Stopwatch stopwatch = Stopwatch.StartNew();
            long lastTime = 0;
            bool done = false;
            while (!done)
            {
                long time = stopwatch.ElapsedMilliseconds;
                float deltaTime = (time - lastTime) / 1000.0f;
                lastTime = time;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.P)
                    {
                        this.client.Prediction = !this.client.Prediction;
                    }
                    if (keyInfo.Key == ConsoleKey.R)
                    {
                        this.client.Reconciliation = !this.client.Reconciliation;
                    }
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        done = true;
                    }
                }

                this.client.Update(deltaTime);
                this.server.Update(deltaTime);
                this.network.Update(deltaTime);

                // Render.
                this.accumulatedTime += deltaTime;
                if (this.accumulatedTime >= RenderInterval)
                {
                    this.Render();
                    this.accumulatedTime = this.accumulatedTime % RenderInterval;
                }
            }
        }

        #endregion
    }
}