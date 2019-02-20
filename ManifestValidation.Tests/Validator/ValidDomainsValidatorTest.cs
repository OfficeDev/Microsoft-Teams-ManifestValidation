// <copyright file="ValidDomainsValidatorTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ManifestValidation.Tests.Validator
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ManifestValidation.Validator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class ValidDomainsValidatorTest
    {
        [TestMethod]
        public async Task Okay_no_valid_domains_key()
        {
            var manifest = JObject.Parse(@"
{
    ""no_valid_domains_key"": 111
}
");
            var validator = new ValidDomainsValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(0, result.ErrorMessages.Count);
            Assert.AreEqual(0, result.WarningMessages.Count);
        }

        [TestMethod]
        public async Task Error_valid_domains_not_array()
        {
            var manifest = JObject.Parse(@"
{
    ""validDomains"": ""not an array""
}
");
            var validator = new ValidDomainsValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.ValidDomainsType, result.ErrorMessages.First());
        }

        [TestMethod]
        public async Task Okay_valid_domains_empty_array()
        {
            var manifest = JObject.Parse(@"
{
    ""validDomains"": []
}
");
            var validator = new ValidDomainsValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(0, result.ErrorMessages.Count);
            Assert.AreEqual(0, result.WarningMessages.Count);
        }

        [TestMethod]
        public async Task Error_valid_domain_empty_string()
        {
            var manifest = JObject.Parse(@"
{
    ""validDomains"": [""""]
}
");
            var validator = new ValidDomainsValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.ValidDomainsType, result.ErrorMessages.First());
        }

        [TestMethod]
        public async Task Error_valid_domain_is_tunnel_site()
        {
            var manifest = JObject.Parse(@"
{
    ""validDomains"": [""avalidsite.com"", ""something.ngrok.io""]
}
");
            var validator = new ValidDomainsValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.ValidDomainsIsTunnelSite, result.ErrorMessages.First());
        }

        [TestMethod]
        public async Task Error_valid_domain_is_wildcard_hosting_site()
        {
            var manifest = JObject.Parse(@"
{
    ""validDomains"": [""*.azurewebsites.net""]
}
");
            var validator = new ValidDomainsValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.ValidDomainsIsWildcardHostingSite, result.ErrorMessages.First());
        }
    }
}
