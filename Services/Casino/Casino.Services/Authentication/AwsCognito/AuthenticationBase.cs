using Amazon;
using Amazon.CognitoIdentityProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Casino.Services.Authentication.AwsCognito
{
    public abstract class AuthenticationBase
    {
        public string _defaultCognitoGroup;

        protected readonly IConfiguration _configuration;

        public AuthenticationBase(IConfiguration configuration)
        {
            _configuration = configuration;
            _defaultCognitoGroup = configuration.GetSection("AWS:Cognito:DefaultGroup").Value;
        }

        public string DefaultCognitoGroup { get => _defaultCognitoGroup; }

        public List<string> GetAuthorizedGroups()
        {
            return _configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();
        }

        public string GetClientId()
        {
            return _configuration["AWS:Cognito:AppClientId"];
        }

        public string GetUserPoolId()
        {
            return _configuration["AWS:Cognito:Region"] + "_" + _configuration["AWS:Cognito:PoolId"];
        }

        public string GetAccessKeyId()
        {
            return _configuration["AWS:Credentials:AppAccessKeyId"];
        }

        public string GetSecretAccessKey()
        {
            return _configuration["AWS:Credentials:SecretAccessKey"];
        }

        public AmazonCognitoIdentityProviderClient GetAmazonCognitoIdentity()
        {
            return new AmazonCognitoIdentityProviderClient(GetRegion());
        }

        public RegionEndpoint GetRegion()
        {
            return RegionEndpoint.USEast1;
        }
    }
}
