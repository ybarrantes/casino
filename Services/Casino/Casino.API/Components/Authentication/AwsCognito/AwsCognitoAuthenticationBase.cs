using Amazon;
using Amazon.CognitoIdentityProvider;
using Casino.API.Config;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public abstract class AwsCognitoAuthenticationBase
    {
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
