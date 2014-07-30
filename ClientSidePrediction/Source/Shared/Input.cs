// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Input.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Shared
{
    public class Input
    {
        #region Fields

        /// <summary>
        ///   -1, 0, 1
        /// </summary>
        public readonly int HorizontalAxis;

        #endregion

        #region Constructors and Destructors

        public Input(int horizontalAxis)
        {
            this.HorizontalAxis = horizontalAxis;
        }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((Input)obj);
        }

        public override int GetHashCode()
        {
            return this.HorizontalAxis;
        }

        #endregion

        #region Methods

        protected bool Equals(Input other)
        {
            return this.HorizontalAxis == other.HorizontalAxis;
        }

        #endregion
    }
}