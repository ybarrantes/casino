using Amazon.CognitoIdentityProvider.Model;
using System.Threading.Tasks;
using System;
using Casino.Services.WebApi;
using Amazon.CognitoIdentityProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Casino.Services.Authentication.Contracts;

namespace Casino.Services.Authentication.AwsCognito
{
    public class Signup : AuthenticationBase, ISignup
    {

        public Signup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<string> SignUpUser(ISignupModelUser userDTO)
        {
            // try signup user in aws cognito
            SignUpResponse response = await TrySignUpUser(userDTO);

            // if signup is successful then try add user to default group
            await TryAddUserToGroup(userDTO.Username);

            return response.UserSub;
        }

        private async Task<SignUpResponse> TrySignUpUser(ISignupModelUser user)
        {
            try
            {
                SignUpRequest request = GetSignUpRequest(user);
                AmazonCognitoIdentityProviderClient client = GetAmazonCognitoIdentity();
                SignUpResponse response = await client.SignUpAsync(request);

                return response;
            }
            catch (UsernameExistsException e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
            catch (InvalidPasswordException e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (InvalidParameterException e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (Exception e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private async Task TryAddUserToGroup(string Username)
        {
            try
            {
                AwsCognitoUserGroups awsCognitoUserGroup = new AwsCognitoUserGroups(base._configuration);
                await awsCognitoUserGroup.AddUserToGroup(Username, awsCognitoUserGroup.DefaultCognitoGroup);
            }
            catch (Exception)
            {
            }
        }

        private SignUpRequest GetSignUpRequest(ISignupModelUser user)
        {
            SignUpRequest request = new SignUpRequest
            {
                ClientId = GetClientId(),
                Password = user.Password,
                Username = user.Username
            };

            request.UserAttributes.Add(CreateAttributeType(AuthenticationParameters.NAME, user.Name));
            request.UserAttributes.Add(CreateAttributeType(AuthenticationParameters.MIDDLE_NAME, user.MiddleName));
            request.UserAttributes.Add(CreateAttributeType(AuthenticationParameters.EMAIL, user.Email));
            request.UserAttributes.Add(CreateAttributeType(AuthenticationParameters.BIRTHDAY, user.BirthDate.ToString("dd/MM/yyyy")));

            return request;
        }

        private AttributeType CreateAttributeType(string name, string value)
        {
            return new AttributeType
            {
                Name = name,
                Value = value
            };
        }
    }
}
