// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovePhysics.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Shared
{
    public class MovePhysics
    {
        #region Constants

        /// <summary>
        ///   Fixed speed if moved.
        /// </summary>
        public const float Speed = 10.0f;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Performs one simulation step for the specified move state, input and time delta.
        /// </summary>
        /// <param name="moveState">Current move state.</param>
        /// <param name="input">Current input.</param>
        /// <param name="deltaTime">Delta time to simulate.</param>
        /// <returns>New move state after simulation step.</returns>
        public MoveState TickSimulation(MoveState moveState, Input input, float deltaTime)
        {
            MoveState newState = new MoveState();
            if (input != null)
            {
                newState.X = moveState.X + deltaTime * input.HorizontalAxis * Speed;
            }
            return newState;
        }

        #endregion
    }
}