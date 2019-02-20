// <copyright file="DescriptionValidator.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation.Validator
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Validates manifest description property
    /// </summary>
    internal class DescriptionValidator : IValidator
    {
        private static readonly IList<string> CompetitorNames = new List<string> { "slack", "workplace", "workplace by facebook", "flock", "stride", "hipchat", "hangouts meet", "g suite", "gsuite" };
        private JObject manifest;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionValidator"/> class.
        /// </summary>
        /// <param name="manifest">manifest object</param>
        public DescriptionValidator(JObject manifest)
        {
            this.manifest = manifest;
        }

        /// <inheritdoc/>
        public Task<ValidationResult> ValidateAsync()
        {
            var description = this.manifest["description"] as JObject;
            if (description == null)
            {
                return Task.FromResult(ValidationResult.FromError(ValidationResult.MessageCode.DescriptionObjectRequired));
            }

            var result = new ValidationResult();
            var hasShortDesc = false;
            var hasFullDesc = false;

            var shortDesc = description["short"] as JValue;
            if (shortDesc == null)
            {
                result.ErrorMessages.Add(ValidationResult.MessageCode.ShortDescriptionRequired);
            }
            else if (shortDesc.Type != JTokenType.String || string.IsNullOrEmpty(shortDesc.Value as string))
            {
                result.ErrorMessages.Add(ValidationResult.MessageCode.ShortDescriptionType);
            }
            else
            {
                hasShortDesc = true;
                if (this.ContainsCompetitorKeywords(shortDesc.Value as string))
                {
                    result.WarningMessages.Add(ValidationResult.MessageCode.ShortDescriptionCompetitor);
                }
            }

            var fullDesc = description["full"] as JValue;
            if (fullDesc == null)
            {
                result.ErrorMessages.Add(ValidationResult.MessageCode.FullDescriptionRequired);
            }
            else if (fullDesc.Type != JTokenType.String || string.IsNullOrEmpty(fullDesc.Value as string))
            {
                result.ErrorMessages.Add(ValidationResult.MessageCode.FullDescriptionType);
            }
            else
            {
                hasFullDesc = true;
                if (this.ContainsCompetitorKeywords(fullDesc.Value as string))
                {
                    result.WarningMessages.Add(ValidationResult.MessageCode.FullDescriptionCompetitor);
                }
            }

            if (hasShortDesc && hasFullDesc)
            {
                if ((fullDesc.Value as string).Contains(shortDesc.Value as string))
                {
                    result.WarningMessages.Add(ValidationResult.MessageCode.FullDescriptionContainsShortDescription);
                }
            }

            return Task.FromResult(result);
        }

        private bool ContainsCompetitorKeywords(string text)
        {
            text = text.ToLower();
            return CompetitorNames.Any(kw => text.Contains(kw));
        }
    }
}
