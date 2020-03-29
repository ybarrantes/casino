using Amazon;
using Amazon.CognitoIdentityProvider;
using Casino.API.Config;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public abstract class AwsCognitoAuthenticationBase
    {
        public readonly string DEFAULT_COGNITO_GROUP = ApiConfig.Singleton.Configuration.GetSection("AWS:Cognito:DefaultGroup").Value;

        public List<string> GetAuthorizedGroups()
        {
            return ApiConfig.Singleton.Configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();
        }

        public string GetClientId()
        {
            return ApiConfig.Singleton.Configuration["AWS:Cognito:AppClientId"];
        }

        public string GetUserPoolId()
        {
            return ApiConfig.Singleton.Configuration["AWS:Cognito:Region"] + "_" + ApiConfig.Singleton.Configuration["AWS:Cognito:PoolId"];
        }

        public string GetAccessKeyId()
        {
            return ApiConfig.Singleton.Configuration["AWS:Credentials:AppAccessKeyId"];
        }

        public string GetSecretAccessKey()
        {
            return ApiConfig.Singleton.Configuration["AWS:Credentials:SecretAccessKey"];
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
