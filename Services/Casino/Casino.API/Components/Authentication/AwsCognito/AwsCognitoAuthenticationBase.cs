using Amazon;
using Amazon.CognitoIdentityProvider;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public abstract class AwsCognitoAuthenticationBase
    {

        public string GetClientId()
        {
            return "26qsg5e071phpgoq2fcihaudqp";
        }

        public string GetUserPoolId()
        {
            return "us-east-1_u0W5Auuax";
        }

        public AmazonCognitoIdentityProviderClient GetAmazonCognitoIdentity()
        {
            return new AmazonCognitoIdentityProviderClient(GetRegion());
        }

        public RegionEndpoint GetRegion()
        {
            System.Diagnostics.Debug.WriteLine(RegionEndpoint.USEast1.ToString());
            return RegionEndpoint.USEast1;
        }
    }
}
