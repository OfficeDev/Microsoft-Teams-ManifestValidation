// <copyright file="ManifestVersionValidatorTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ManifestValidation.Tests.Validator
{
    using System.Threading.Tasks;
    using ManifestValidation.Validator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class ManifestVersionValidatorTest
    {
        [TestMethod]
        public async Task No_error_or_warning_manifest_version_is_1_2()
        {
            var manifest = JObject.Parse(@"
{
    ""manifestVersion"": ""1.2""
}
");

            var validtor = new ManifestVersionValidator(manifest);
            var res = await validtor.ValidateAsync();

            Assert.AreEqual(0, res.ErrorMessages.Count);
            Assert.AreEqual(0, res.WarningMessages.Count);
        }

        [TestMethod]
        public async Task Error_manifest_version_is_0_9()
        {
            var manifest = JObject.Parse(@"
{
""manifestVersion"": ""0.9""
}
");

            var validtor = new ManifestVersionValidator(manifest);
            var res = await validtor.ValidateAsync();

            Assert.AreEqual(1, res.ErrorMessages.Count);
            Assert.IsTrue(res.ErrorMessages.Contains(ValidationResult.MessageCode.ManifestVersionInvalid));
        }
    }
}
