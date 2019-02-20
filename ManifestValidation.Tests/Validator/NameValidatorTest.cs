// <copyright file="NameValidatorTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation.Tests.Validator
{
    using System.Threading.Tasks;
    using Microsoft.Teams.ManifestValidation;
    using Microsoft.Teams.ManifestValidation.Validator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class NameValidatorTest
    {
        [TestMethod]
        public async Task Error_name_not_exist()
        {
            var manifest = JObject.Parse(@"
{
    ""no_name_key"": 111
}
");
            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.NameObjectRequired));
        }

        [TestMethod]
        public async Task Error_name_is_null()
        {
            var manifest = JObject.Parse(@"
{
""name"": null 
}
");
            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.NameObjectRequired));
        }

        [TestMethod]
        public async Task Error_name_is_array()
        {
            var manifest = JObject.Parse(@"
{
""name"": [1,2,3]
}
");
            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.NameObjectRequired));
        }

        [TestMethod]
        public async Task Error_name_is_string()
        {
            var manifest = JObject.Parse(@"
{
""name"": ""no an object""
}
");
            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.NameObjectRequired));
        }

        [TestMethod]
        public async Task Error_no_short_name()
        {
            var manifest = JObject.Parse(@"
{
""name"": {
        ""full"": ""a full name""
    }
}
");

            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.ShortNameRequired));
        }

        [TestMethod]
        public async Task Error_short_name_not_string()
        {
            var manifest = JObject.Parse(@"
{
""name"": {
        ""short"": 123
    }
}
");

            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.ShortNameType));
        }

        [TestMethod]
        public async Task Error_Short_name_empty_string()
        {
            var manifest = JObject.Parse(@"
{
""name"": {
        ""short"": """"
    }
}
");

            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.ShortNameType));
        }

        [TestMethod]
        public async Task Warn_short_name_look_like_staging()
        {
            var manifest = JObject.Parse(@"
{
""name"": {
        ""short"": ""my sTaGing app""
    }
}
");

            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.WarningMessages.Count);
            Assert.IsTrue(result.WarningMessages.Contains(ValidationResult.MessageCode.ShortNameStaging));
        }

        [TestMethod]
        public async Task Warn_short_name_contains_microsoft_keywords()
        {
            var manifest = JObject.Parse(@"
{
""name"": {
        ""short"": ""my awesome app for TEAMS""
    }
}
");

            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.WarningMessages.Count);
            Assert.IsTrue(result.WarningMessages.Contains(ValidationResult.MessageCode.ShortNameMicrosoft));
        }

        [TestMethod]
        public async Task Error_full_name_not_string()
        {
            var manifest = JObject.Parse(@"
{
""name"": {
        ""short"": ""my app"",
        ""full"": 12345
    }
}
");

            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.ErrorMessages.Count);
            Assert.IsTrue(result.ErrorMessages.Contains(ValidationResult.MessageCode.FullNameType));
        }

        [TestMethod]
        public async Task Warn_full_name_look_like_staing()
        {
            var manifest = JObject.Parse(@"
{
""name"": {
        ""short"": ""my app"",
        ""full"": ""the app (stg)""
    }
}
");

            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.WarningMessages.Count);
            Assert.IsTrue(result.WarningMessages.Contains(ValidationResult.MessageCode.FullNameStaging));
        }

        [TestMethod]
        public async Task Warn_full_name_contains_microsoft_keywords()
        {
            var manifest = JObject.Parse(@"
{
""name"": {
        ""short"": ""my app"",
        ""full"": ""the app for Microsoft teams""
    }
}
");

            var validator = new NameValidator(manifest);
            var result = await validator.ValidateAsync();

            Assert.AreEqual(1, result.WarningMessages.Count);
            Assert.IsTrue(result.WarningMessages.Contains(ValidationResult.MessageCode.FullNameMicrosoft));
        }
    }
}
