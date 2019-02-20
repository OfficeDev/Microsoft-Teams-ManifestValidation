// <copyright file="ValidDomainsValidator.cs" company="Microsoft">
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
    /// Valid domains validator
    /// </summary>
    internal class ValidDomainsValidator : IValidator
    {
        private static readonly IList<string> TunnelSites = new List<string> { "ngrok.io", "openport.io", "portmap.io", "fwd.wf" };
        private static readonly IList<string> HostingSites = new List<string> { "amazonaws.com", "appspot.com", "azurewebsites.net", "cloudapp.net", "dialogflow.com", "glitch.me", "heroku.com", "onmicrosoft.com", "recast.ai", "sharepoint.com" };
        private JObject manifest;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidDomainsValidator"/> class.
        /// </summary>
        /// <param name="manifest">manifest</param>
        public ValidDomainsValidator(JObject manifest)
        {
            this.manifest = manifest;
        }

        /// <inheritdoc/>
        public Task<ValidationResult> ValidateAsync()
        {
            var validDomains = this.manifest["validDomains"] as JToken;
            if (validDomains == null)
            {
                return Task.FromResult(new ValidationResult());
            }

            if (validDomains.Type != JTokenType.Array)
            {
                return Task.FromResult(ValidationResult.FromError(ValidationResult.MessageCode.ValidDomainsType));
            }

            var domains = validDomains as JArray;
            if (!domains.All(this.IsDomainNullEmptyString))
            {
                return Task.FromResult(ValidationResult.FromError(ValidationResult.MessageCode.ValidDomainsType));
            }

            var result = new ValidationResult();
            if (domains.Any(this.IsTunnelSite))
            {
                result.ErrorMessages.Add(ValidationResult.MessageCode.ValidDomainsIsTunnelSite);
            }

            if (domains.Any(this.IsWildcardHostingSite))
            {
                result.ErrorMessages.Add(ValidationResult.MessageCode.ValidDomainsIsWildcardHostingSite);
            }

            return Task.FromResult(result);
        }

        private bool IsDomainNullEmptyString(JToken domain)
        {
            return domain.Type == JTokenType.String && !string.IsNullOrEmpty((domain as JValue).Value as string);
        }

        private bool IsTunnelSite(JToken domain)
        {
            var domainStr = (domain as JValue).Value as string;
            return TunnelSites.Any(tunnelSite => domainStr.EndsWith(tunnelSite, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsWildcardHostingSite(JToken domain)
        {
            var domainStr = (domain as JValue).Value as string;
            if (!domainStr.Contains("*"))
            {
                return false;
            }

            return HostingSites.Any(hostingSite => domainStr.ToLower().Contains(hostingSite));
        }
    }
}
