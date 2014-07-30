// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientInput.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ClientSidePrediction.Shared
{
    public class ClientInput
    {
        #region Public Properties

        /// <summary>
        ///   Id of client who send the input.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        ///   Input data.
        /// </summary>
        public Input Input { get; set; }

        /// <summary>
        ///   Unique, consecutive input number.
        /// </summary>
        public int InputNumber { get; set; }
        
        #endregion
    }
}