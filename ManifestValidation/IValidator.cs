// <copyright file="IValidator.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation
{
    using System.Threading.Tasks;

    /// <summary>
    /// IValidator
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Run this validator
        /// </summary>
        /// <returns><see cref="ValidationResult"/></returns>
        Task<ValidationResult> ValidateAsync();
    }
}
