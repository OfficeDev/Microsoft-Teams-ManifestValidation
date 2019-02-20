// <copyright file="ValidationResult.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation
{
    using System.Collections.Generic;

    /// <summary>
    /// Result returned from <see cref="IValidator.ValidateAsync"/>
    /// </summary>
    public partial class ValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        public ValidationResult()
        {
            this.InfoMessages = new List<MessageCode>();
            this.WarningMessages = new List<MessageCode>();
            this.ErrorMessages = new List<MessageCode>();
        }

        /// <summary>
        /// Gets info messages
        /// </summary>
        public List<MessageCode> InfoMessages { get; internal set; }

        /// <summary>
        /// Gets warning messages
        /// </summary>
        public List<MessageCode> WarningMessages { get; internal set; }

        /// <summary>
        /// Gets error messages
        /// </summary>
        public List<MessageCode> ErrorMessages { get; internal set; }

        /// <summary>
        /// Helper function to construct a result
        /// </summary>
        /// <param name="firstInfoMessage">first info message</param>
        /// <param name="infoMessages">info messages</param>
        /// <returns>new ValidationResult instance</returns>
        public static ValidationResult FromInfo(MessageCode firstInfoMessage, params MessageCode[] infoMessages)
        {
            var res = new ValidationResult();
            res.InfoMessages.Add(firstInfoMessage);
            res.InfoMessages.AddRange(infoMessages);
            return res;
        }

        /// <summary>
        /// Helper function to construct a result
        /// </summary>
        /// <param name="firstWarningMessage">first warning message</param>
        /// <param name="warningMessages">warning messages</param>
        /// <returns>new ValidationResult instance</returns>
        public static ValidationResult FromWarning(MessageCode firstWarningMessage, params MessageCode[] warningMessages)
        {
            var res = new ValidationResult();
            res.WarningMessages.Add(firstWarningMessage);
            res.WarningMessages.AddRange(warningMessages);
            return res;
        }

        /// <summary>
        /// Helper function to construct a result
        /// </summary>
        /// <param name="firstErrorMessage">first error messages</param>
        /// <param name="errorMessages">error messages</param>
        /// <returns>new ValidationResult instance</returns>
        public static ValidationResult FromError(MessageCode firstErrorMessage, params MessageCode[] errorMessages)
        {
            var res = new ValidationResult();
            res.ErrorMessages.Add(firstErrorMessage);
            res.ErrorMessages.AddRange(errorMessages);
            return res;
        }
    }
}
