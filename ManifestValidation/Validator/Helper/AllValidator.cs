// <copyright file="AllValidator.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace ManifestValidation.Validator.Helper
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Run all child validators and aggregate results
    /// </summary>
    internal class AllValidator : IValidator
    {
        private IEnumerable<IValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllValidator"/> class.
        /// </summary>
        /// <param name="validators">aggregated validators</param>
        public AllValidator(IEnumerable<IValidator> validators)
        {
            this.validators = validators;
        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateAsync()
        {
            var result = new ValidationResult();
            foreach (var validator in this.validators)
            {
                var r = await validator.ValidateAsync();
                result.InfoMessages.AddRange(r.InfoMessages);
                result.WarningMessages.AddRange(r.WarningMessages);
                result.ErrorMessages.AddRange(r.ErrorMessages);
            }

            return result;
        }
    }
}
