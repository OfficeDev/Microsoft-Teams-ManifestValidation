// <copyright file="VersionValidator.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace ManifestValidation.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Version validator
    /// </summary>
    internal class VersionValidator : IValidator
    {
        private JObject manifest;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionValidator"/> class.
        /// </summary>
        /// <param name="manifest">manifest</param>
        public VersionValidator(JObject manifest)
        {
            this.manifest = manifest;
        }

        /// <inheritdoc/>
        public Task<ValidationResult> ValidateAsync()
        {
            var version = this.manifest["version"] as JValue;
            if (version == null)
            {
                return Task.FromResult(ValidationResult.FromError(ValidationResult.MessageCode.VersionRequired));
            }

            if (version.Type != JTokenType.String || string.IsNullOrEmpty(version.Value as string))
            {
                return Task.FromResult(ValidationResult.FromError(ValidationResult.MessageCode.VersionType));
            }

            if (!Version.TryParse(version.Value as string, out var versionObj))
            {
                return Task.FromResult(ValidationResult.FromError(ValidationResult.MessageCode.VersionType));
            }

            if (versionObj.Major < 1)
            {
                return Task.FromResult(ValidationResult.FromWarning(ValidationResult.MessageCode.VersionMajorLessThanOne));
            }

            return Task.FromResult(new ValidationResult());
        }
    }
}
