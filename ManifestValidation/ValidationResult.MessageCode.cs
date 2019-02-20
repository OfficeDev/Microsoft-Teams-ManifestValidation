// <copyright file="ValidationResult.MessageCode.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.ManifestValidation
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// <see cref="ValidationResult"/>
    /// </summary>
    public partial class ValidationResult
    {
        /// <summary>
        /// Message codes
        /// </summary>
        [JsonConverter(typeof(MessageCodeJsonConverter))]
        public sealed class MessageCode
        {
            /// <summary>
            /// Manifest does not contain valid json
            /// </summary>
            public static readonly MessageCode NotValidJson = new MessageCode("not_valid_json");

            /// <summary>
            /// Manfiest version can't be empty
            /// </summary>
            public static readonly MessageCode ManifestVersionInvalid = new MessageCode("manifest_version.invalid");

            /// <summary>
            /// Manfiest should have a name object
            /// </summary>
            public static readonly MessageCode NameObjectRequired = new MessageCode("name.required");

            /// <summary>
            /// name.short is required
            /// </summary>
            public static readonly MessageCode ShortNameRequired = new MessageCode("name.short.required");

            /// <summary>
            /// name.short needs to be a string
            /// </summary>
            public static readonly MessageCode ShortNameType = new MessageCode("name.short.type");

            /// <summary>
            /// name.short should not look like staging
            /// </summary>
            public static readonly MessageCode ShortNameStaging = new MessageCode("name.short.looks_like_staging");

            /// <summary>
            /// name.short contains Microsoft keywords
            /// </summary>
            public static readonly MessageCode ShortNameMicrosoft = new MessageCode("name.short.microsoft");

            /// <summary>
            /// name.full needs to be a string
            /// </summary>
            public static readonly MessageCode FullNameType = new MessageCode("name.full.type");

            /// <summary>
            /// name.full should not look like staging
            /// </summary>
            public static readonly MessageCode FullNameStaging = new MessageCode("name.full.looks_like_staging");

            /// <summary>
            /// name.full contains Microsoft keywords
            /// </summary>
            public static readonly MessageCode FullNameMicrosoft = new MessageCode("name.full.microsoft");

            /// <summary>
            /// description is required and must be a object
            /// </summary>
            public static readonly MessageCode DescriptionObjectRequired = new MessageCode("description.required");

            /// <summary>
            /// description.short is required
            /// </summary>
            public static readonly MessageCode ShortDescriptionRequired = new MessageCode("description.short.required");

            /// <summary>
            /// description.short needs to be a string
            /// </summary>
            public static readonly MessageCode ShortDescriptionType = new MessageCode("description.short.type");

            /// <summary>
            /// description.short contains competitor keyword
            /// </summary>
            public static readonly MessageCode ShortDescriptionCompetitor = new MessageCode("description.short.competitor_keyword");

            /// <summary>
            /// description.full is required
            /// </summary>
            public static readonly MessageCode FullDescriptionRequired = new MessageCode("description.full.required");

            /// <summary>
            /// description.full needs to be a string
            /// </summary>
            public static readonly MessageCode FullDescriptionType = new MessageCode("description.full.type");

            /// <summary>
            /// description.full contains competitor keyword
            /// </summary>
            public static readonly MessageCode FullDescriptionCompetitor = new MessageCode("description.full.competitor_keyword");

            /// <summary>
            /// description.full contains short description, which should not happen
            /// </summary>
            public static readonly MessageCode FullDescriptionContainsShortDescription = new MessageCode("description.full.contains_short");

            /// <summary>
            /// validdomains should be an array of strings
            /// </summary>
            public static readonly MessageCode ValidDomainsType = new MessageCode("validdomains.type");

            /// <summary>
            /// validdomains cannot contain tunnel sites
            /// </summary>
            public static readonly MessageCode ValidDomainsIsTunnelSite = new MessageCode("validdomains.is_tunnel_site");

            /// <summary>
            /// validdomains cannot contain wildcard hosting sites
            /// </summary>
            public static readonly MessageCode ValidDomainsIsWildcardHostingSite = new MessageCode("validdomains.is_wildcard_hosting_site");

            /// <summary>
            /// version is required
            /// </summary>
            public static readonly MessageCode VersionRequired = new MessageCode("version.required");

            /// <summary>
            /// version needs to be a non-empty string. It also needs to be parsable by <see cref="Version.TryParse(string, out Version)"/>
            /// </summary>
            public static readonly MessageCode VersionType = new MessageCode("version.type");

            /// <summary>
            /// major version should not be less than 1
            /// </summary>
            public static readonly MessageCode VersionMajorLessThanOne = new MessageCode("version.major_less_than_one");

            private readonly string messageCode;

            private MessageCode(string messageCode)
            {
                this.messageCode = messageCode;
            }

            /// <summary>
            /// Override ==
            /// </summary>
            /// <param name="a">left hand ops</param>
            /// <param name="b">right hand ops</param>
            /// <returns>true if they're equal</returns>
            public static bool operator ==(MessageCode a, MessageCode b)
            {
                if (a == null && b == null)
                {
                    return true;
                }

                if (a == null && b != null)
                {
                    return false;
                }

                if (a != null && b == null)
                {
                    return false;
                }

                return a.Equals(b);
            }

            /// <summary>
            /// Override !=
            /// </summary>
            /// <param name="a">left hand ops</param>
            /// <param name="b">right hand ops</param>
            /// <returns>true if they're not equal</returns>
            public static bool operator !=(MessageCode a, MessageCode b)
            {
                return !(a == b);
            }

            /// <summary>
            /// Override equals
            /// </summary>
            /// <param name="obj">obj to compare to this one</param>
            /// <returns>true if they're equal</returns>
            public override bool Equals(object obj)
            {
                if (obj is MessageCode other)
                {
                    return string.Equals(this.messageCode, other.messageCode);
                }

                return base.Equals(obj);
            }

            /// <summary>
            /// Override GetHashCode
            /// </summary>
            /// <returns>hash code</returns>
            public override int GetHashCode()
            {
                return this.messageCode.GetHashCode();
            }

            /// <summary>
            /// Implements <see cref="JsonConverter{T}"/> for <see cref="MessageCode"/>
            /// </summary>
            public class MessageCodeJsonConverter : JsonConverter<MessageCode>
            {
                /// <inheritdoc/>
                public override void WriteJson(JsonWriter writer, MessageCode value, JsonSerializer serializer)
                {
                    JToken.FromObject(value.messageCode).WriteTo(writer);
                }

                /// <inheritdoc/>
                public override MessageCode ReadJson(JsonReader reader, Type objectType, MessageCode existingValue, bool hasExistingValue, JsonSerializer serializer)
                {
                    // Message code is passed from server to client, never the other way around.
                    // We should never need to deserialize it
                    throw new NotImplementedException();
                }
            }
        }
    }
}
