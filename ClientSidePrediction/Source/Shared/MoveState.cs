// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoveState.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Shared
{
    using System;

    public class MoveState
    {
        #region Constants

        #endregion

        #region Fields

        /// <summary>
        ///   Position.
        /// </summary>
        public float X;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Linear interpolation between two move states.
        /// </summary>
        /// <param name="moveStateA">First move state.</param>
        /// <param name="moveStateB">Second move state.</param>
        /// <param name="t">Factor of linear interpolation.</param>
        /// <returns>Linear interpolated move state between the two specified move states.</returns>
        public MoveState Lerp(MoveState moveStateA, MoveState moveStateB, float t)
        {
            return new MoveState { X = moveStateA.X * (1 - t) + moveStateB.X * t };
        }

        public override string ToString()
        {
            return string.Format("X: {0,-6}", Math.Round(this.X, 2));
        }

        public string Visualize(int range)
        {
            int position = (int)this.X + range / 2;
            if (position < 0)
            {
                position = 0;
            }
            if (position > range)
            {
                position = range;
            }
            return new string(' ', position) + "O" + new string(' ', range - position);
        }

        #endregion
    }
}