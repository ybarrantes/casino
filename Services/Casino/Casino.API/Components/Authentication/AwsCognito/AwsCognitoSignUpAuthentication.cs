﻿using Amazon.CognitoIdentityProvider.Model;
using System.Threading.Tasks;
using System;
using Casino.Services.WebApi;
using Amazon.CognitoIdentityProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Casino.Data.Models.DTO;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoSignUpAuthentication : AwsCognitoAuthenticationBase, ISignUpRequest
    {

        public AwsCognitoSignUpAuthentication(IConfiguration configuration, ILogger logger)
            : base(configuration, logger)
        {
        }

        async Task<string> ISignUpRequest.SignUpUser(UserSignUpDTO userDTO)
        {
            // try signup user in aws cognito
            SignUpResponse response = await TrySignUpUser(userDTO);

            // if signup is successful then try add user to default group
            await TryAddUserToGroup(userDTO.Username);

            return response.UserSub;
        }

        private async Task<SignUpResponse> TrySignUpUser(UserSignUpDTO user)
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
                AwsCognitoUserGroupAuthentication awsCognitoUserGroup = new AwsCognitoUserGroupAuthentication(base._configuration, base._logger);
                AdminAddUserToGroupResponse responseAddUserToGroup = await awsCognitoUserGroup.AddUserToGroup(Username, awsCognitoUserGroup.DefaultCognitoGroup);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error trying to assign group to user");
            }
        }

        private SignUpRequest GetSignUpRequest(UserSignUpDTO user)
        {
            SignUpRequest request = new SignUpRequest
            {
                ClientId = GetClientId(),
                Password = user.Password,
                Username = user.Username
            };

            request.UserAttributes.Add(CreateAttributeType(AwsCognitoAuthenticationParameters.NAME, user.Name));
            request.UserAttributes.Add(CreateAttributeType(AwsCognitoAuthenticationParameters.MIDDLE_NAME, user.MiddleName));
            request.UserAttributes.Add(CreateAttributeType(AwsCognitoAuthenticationParameters.EMAIL, user.Email));
            request.UserAttributes.Add(CreateAttributeType(AwsCognitoAuthenticationParameters.BIRTHDAY, user.BirthDate.ToString("dd/MM/yyyy")));

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

        public async Task SignUpUserConfirmation(UserConfirmationSignUpDTO confirmation)
        {
            try
            {
                ConfirmSignUpRequest confirmRequest = new ConfirmSignUpRequest()
                {
                    Username = confirmation.Username,
                    ConfirmationCode = confirmation.ConfirmationCode,
                    ClientId = GetClientId(),
                };

                AmazonCognitoIdentityProviderClient client = GetAmazonCognitoIdentity();
                ConfirmSignUpResponse confirmResult = await client.ConfirmSignUpAsync(confirmRequest);
            }
            catch(CodeMismatchException e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
            catch (ExpiredCodeException e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }            
            catch (Exception e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
