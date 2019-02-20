// <copyright file="ManifestVersionValidator.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation.Validator
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Validates manifest version
    /// </summary>
    internal class ManifestVersionValidator : IValidator
    {
        private static readonly ISet<string> AllowedVersions = new HashSet<string> { "1.0", "1.2", "1.3" };
        private JObject manifest;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestVersionValidator"/> class.
        /// </summary>
        /// <param name="manifest">the entire manifest JObject</param>
        public ManifestVersionValidator(JObject manifest)
        {
            this.manifest = manifest;
        }

        /// <inheritdoc/>
        public Task<ValidationResult> ValidateAsync()
        {
            if (!(this.manifest["manifestVersion"] is JValue))
            {
                return Task.FromResult(
                    ValidationResult.FromError(ValidationResult.MessageCode.ManifestVersionInvalid));
            }

            var manifestVersion = this.manifest["manifestVersion"] as JValue;
            if (!(manifestVersion.Value is string))
            {
                return Task.FromResult(
                    ValidationResult.FromError(ValidationResult.MessageCode.ManifestVersionInvalid));
            }

            var versionString = manifestVersion.Value as string;
            if (!AllowedVersions.Contains(versionString))
            {
                return Task.FromResult(
                    ValidationResult.FromError(ValidationResult.MessageCode.ManifestVersionInvalid));
            }

            return Task.FromResult(new ValidationResult());
        }
    }
}
