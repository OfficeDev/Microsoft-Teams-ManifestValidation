// <copyright file="DescriptionValidatorTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation.Tests.Validator
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Teams.ManifestValidation;
    using Microsoft.Teams.ManifestValidation.Validator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class DescriptionValidatorTest
    {
        [TestMethod]
        public async Task Error_description_not_exist()
        {
            var manifest = JObject.Parse(@"
{
    ""no_description_key"": 111
}
");
            var validator = new DescriptionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.DescriptionObjectRequired, result.ErrorMessages.First());
        }

        [TestMethod]
        public async Task Error_description_not_object()
        {
            var manifest = JObject.Parse(@"
{
    ""description"": ""not an object""
}
");
            var validator = new DescriptionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.AreEqual(ValidationResult.MessageCode.DescriptionObjectRequired, result.ErrorMessages.First());
        }

        [TestMethod]
        public async Task Error_no_short_description()
        {
            var manifest = JObject.Parse(@"
{
    ""description"": {
        ""no_short_desc_key"": 1
    }
}
");
            var validator = new DescriptionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.ShortDescriptionRequired));
        }

        [TestMethod]
        public async Task Error_short_description_not_string()
        {
            var manifest = JObject.Parse(@"
{
    ""description"": {
        ""short"": 12345
    }
}
");
            var validator = new DescriptionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.ShortDescriptionType));
        }

        [TestMethod]
        public async Task Warning_short_description_contains_competitor_keyword()
        {
            var manifest = JObject.Parse(@"
{
    ""description"": {
        ""short"": ""awesome app for slack""
    }
}
");
            var validator = new DescriptionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.IsTrue(result.WarningMessages.Contains(ValidationResult.MessageCode.ShortDescriptionCompetitor));
        }

        [TestMethod]
        public async Task Error_no_full_description()
        {
            var manifest = JObject.Parse(@"
{
    ""description"": {
        ""no_full_desc_key"": 1
    }
}
");
            var validator = new DescriptionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.FullDescriptionRequired));
        }

        [TestMethod]
        public async Task Error_full_description_not_string()
        {
            var manifest = JObject.Parse(@"
{
    ""description"": {
        ""full"": 12345
    }
}
");
            var validator = new DescriptionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.FullDescriptionType));
        }

        [TestMethod]
        public async Task Warning_full_description_contains_competitor_keyword()
        {
            var manifest = JObject.Parse(@"
{
    ""description"": {
        ""full"": ""awesome app for workplace""
    }
}
");
            var validator = new DescriptionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.IsTrue(result.WarningMessages.Contains(ValidationResult.MessageCode.FullDescriptionCompetitor));
        }

        [TestMethod]
        public async Task Warning_full_description_contains_short_description()
        {
            var manifest = JObject.Parse(@"
{
    ""description"": {
        ""full"": ""Full description for my app"",
        ""short"": ""my app""
    }
}
");
            var validator = new DescriptionValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.IsTrue(result.WarningMessages.Contains(ValidationResult.MessageCode.FullDescriptionContainsShortDescription));
        }
    }
}
