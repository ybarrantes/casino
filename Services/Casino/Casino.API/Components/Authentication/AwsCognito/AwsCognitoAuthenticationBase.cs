using Amazon;
using Amazon.CognitoIdentityProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public abstract class AwsCognitoAuthenticationBase
    {
        public string defaultCognitoGroup;

        protected readonly IConfiguration configuration;
        protected readonly ILogger logger;

        public AwsCognitoAuthenticationBase(IConfiguration configuration, ILogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.defaultCognitoGroup = configuration.GetSection("AWS:Cognito:DefaultGroup").Value;
        }

        public string DefaultCognitoGroup { get => defaultCognitoGroup; }

        public List<string> GetAuthorizedGroups()
        {
            return configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();
        }

        public string GetClientId()
        {
            return configuration["AWS:Cognito:AppClientId"];
        }

        public string GetUserPoolId()
        {
            return configuration["AWS:Cognito:Region"] + "_" + configuration["AWS:Cognito:PoolId"];
        }

        public string GetAccessKeyId()
        {
            return configuration["AWS:Credentials:AppAccessKeyId"];
        }

        public string GetSecretAccessKey()
        {
            return configuration["AWS:Credentials:SecretAccessKey"];
        }

        public AmazonCognitoIdentityProviderClient GetAmazonCognitoIdentity()
        {
            return new AmazonCognitoIdentityProviderClient(GetRegion());
            //return new AmazonCognitoIdentityProviderClient(GetAccessKeyId(), GetSecretAccessKey(), GetRegion());
        }

        public RegionEndpoint GetRegion()
        {
            return RegionEndpoint.USEast1;
        }
    }
}
