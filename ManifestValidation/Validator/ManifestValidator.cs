// <copyright file="ManifestValidator.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ManifestValidation.Validator.Helper;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Main entry point for this Manifest Validation project
    /// Takes a manifest json string, except the icons are not paths to icon files.
    /// Icons are Base64 encoded icon content.
    /// </summary>
    public class ManifestValidator : IValidator
    {
        private string manifestJson;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestValidator"/> class.
        /// </summary>
        /// <param name="manifestJson">manifest json</param>
        public ManifestValidator(string manifestJson)
        {
            this.manifestJson = manifestJson;
        }

        /// <inheritdoc/>
        public Task<ValidationResult> ValidateAsync()
        {
            JObject manifest;

            try
            {
                manifest = JObject.Parse(this.manifestJson);
            }
            catch (Exception)
            {
                return Task.FromResult(ValidationResult.FromError(ValidationResult.MessageCode.NotValidJson));
            }

            return new AllValidator(new List<IValidator>
            {
                new ManifestVersionValidator(manifest),
                new NameValidator(manifest),
                new DescriptionValidator(manifest),
                new ValidDomainsValidator(manifest),
                new VersionValidator(manifest),
            })
                .ValidateAsync();
        }
    }
}
