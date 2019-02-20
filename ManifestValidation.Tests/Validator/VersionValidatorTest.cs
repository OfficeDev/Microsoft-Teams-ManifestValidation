// <copyright file="VersionValidatorTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation.Tests.Validator
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Teams.ManifestValidation;
    using Microsoft.Teams.ManifestValidation.Validator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class VersionValidatorTest
    {
        [TestMethod]
        public async Task Error_if_no_version_key()
        {
            var manifest = JObject.Parse(@"
{
    ""no_version_key"": 1
}
");
            var validator = new VersionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.VersionRequired, result.ErrorMessages.First());
        }

        [TestMethod]
        public async Task Error_if_version_is_not_string()
        {
            var manifest = JObject.Parse(@"
{
    ""version"": true
}
");
            var validator = new VersionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.VersionType, result.ErrorMessages.First());
        }

        [TestMethod]
        public async Task Error_if_version_is_not_semver()
        {
            var manifest = JObject.Parse(@"
{
    ""version"": ""string but not semver (should be something like 1.2.3)""
}
");
            var validator = new VersionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.VersionType, result.ErrorMessages.First());
        }

        [TestMethod]
        public async Task Warning_if_major_is_less_than_1()
        {
            var manifest = JObject.Parse(@"
{
    ""version"": ""0.2.3""
}
");
            var validator = new VersionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(0, result.ErrorMessages.Count);
            Assert.AreEqual(1, result.WarningMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.VersionMajorLessThanOne, result.WarningMessages.First());
        }
    }
}
