// <copyright file="NameValidator.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Validates name
    /// </summary>
    internal class NameValidator : IValidator
    {
        private static readonly IList<string> StagingKeywords = new List<string> { "staging", "stg", "stag", "prod" };
        private static readonly IList<string> MicrosoftKeywords = new List<string> { "microsoft", "teams", "microsoft teams" };
        private JObject manifest;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameValidator"/> class.
        /// </summary>
        /// <param name="manifest">manifest</param>
        public NameValidator(JObject manifest)
        {
            this.manifest = manifest;
        }

        /// <inheritdoc/>
        public Task<ValidationResult> ValidateAsync()
        {
            var name = this.manifest["name"] as JObject;
            if (name == null)
            {
                return Task.FromResult(ValidationResult.FromError(ValidationResult.MessageCode.NameObjectRequired));
            }

            var result = new ValidationResult();

            var shortName = name["short"] as JValue;
            if (shortName == null)
            {
                result.ErrorMessages.Add(ValidationResult.MessageCode.ShortNameRequired);
            }
            else if (shortName.Type != JTokenType.String || string.IsNullOrEmpty(shortName.Value as string))
            {
                result.ErrorMessages.Add(ValidationResult.MessageCode.ShortNameType);
            }
            else
            {
                if (this.LookLikeStaging(shortName.Value as string))
                {
                    result.WarningMessages.Add(ValidationResult.MessageCode.ShortNameStaging);
                }

                if (this.ContainsMicrosoftKeywords(shortName.Value as string))
                {
                    result.WarningMessages.Add(ValidationResult.MessageCode.ShortNameMicrosoft);
                }
            }

            if (name["full"] is JValue fullName)
            {
                if (fullName.Type != JTokenType.String || string.IsNullOrEmpty(fullName.Value as string))
                {
                    result.ErrorMessages.Add(ValidationResult.MessageCode.FullNameType);
                }
                else
                {
                    if (this.LookLikeStaging(fullName.Value as string))
                    {
                        result.WarningMessages.Add(ValidationResult.MessageCode.FullNameStaging);
                    }

                    if (this.ContainsMicrosoftKeywords(fullName.Value as string))
                    {
                        result.WarningMessages.Add(ValidationResult.MessageCode.FullNameMicrosoft);
                    }
                }
            }

            return Task.FromResult(result);
        }

        private bool LookLikeStaging(string name)
        {
            name = name.ToLower();
            return StagingKeywords.Any(kw => name.Contains(kw));
        }

        private bool ContainsMicrosoftKeywords(string name)
        {
            name = name.ToLower();
            return MicrosoftKeywords.Any(kw => name.Contains(kw));
        }
    }
}
