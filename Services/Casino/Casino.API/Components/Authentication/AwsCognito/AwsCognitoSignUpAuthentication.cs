using Amazon.CognitoIdentityProvider.Model;
using Casino.API.Data.Models.Usuario;
using System.Threading.Tasks;
using System;
using Casino.API.Exceptions;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoSignUpAuthentication : AwsCognitoAuthenticationBase, ISignUpRequest
    {

        async Task ISignUpRequest.SignUpUser(UsuarioSignUpDTO user)
        {
            try
            {
                SignUpRequest request = GetSignUpRequest(user);
                SignUpResponse response = await GetAmazonCognitoIdentity().SignUpAsync(request);
            }
            catch (UsernameExistsException e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
            catch (InvalidPasswordException e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (InvalidParameterException e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private SignUpRequest GetSignUpRequest(UsuarioSignUpDTO user)
        {
            SignUpRequest request = new SignUpRequest
            {
                ClientId = GetClientId(),
                Password = user.Password,
                Username = user.Username
            };

            request.UserAttributes.Add(GetAttributeTypeFromNameAndValue(AwsCognitoAuthenticationParameters.NAME, user.Name));
            request.UserAttributes.Add(GetAttributeTypeFromNameAndValue(AwsCognitoAuthenticationParameters.MIDDLE_NAME, user.MiddleName));
            request.UserAttributes.Add(GetAttributeTypeFromNameAndValue(AwsCognitoAuthenticationParameters.EMAIL, user.Email));
            request.UserAttributes.Add(GetAttributeTypeFromNameAndValue(AwsCognitoAuthenticationParameters.BIRTHDAY, user.BirthDate.ToString("dd/MM/yyyy")));

            return request;
        }

        private AttributeType GetAttributeTypeFromNameAndValue(string name, string value)
        {
            return new AttributeType
            {
                Name = name,
                Value = value
            };
        }

        public async Task SignUpUserConfirmation(UsuarioConfirmationSignUpDTO confirmation)
        {
            try
            {
                ConfirmSignUpRequest confirmRequest = new ConfirmSignUpRequest()
                {
                    Username = confirmation.Username,
                    ConfirmationCode = confirmation.ConfirmationCode,
                    ClientId = GetClientId(),
                };

                ConfirmSignUpResponse confirmResult = await GetAmazonCognitoIdentity().ConfirmSignUpAsync(confirmRequest);
            }
            catch(CodeMismatchException e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
            catch (ExpiredCodeException e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }            
            catch (Exception e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
