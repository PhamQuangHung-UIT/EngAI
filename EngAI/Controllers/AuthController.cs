using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using EngAI.Models;
using EngAI.Models.Request;
using EngAI.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EngAI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAmazonCognitoIdentityProvider cognito, IConfiguration config, UserService service) : ControllerBase
{
    private readonly IAmazonCognitoIdentityProvider _cognito = cognito;
    private readonly IConfiguration _configuration = config;
    private readonly UserService _service = service;

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            // Create a CognitoUserPool and CognitoUser instance
            var userPool = new CognitoUserPool(
                _configuration.GetSection("AWS")["UserPoolId"],
                _configuration.GetSection("AWS")["UserPoolClientId"],
                _cognito
            );
            var user = new CognitoUser(
                request.Email,
                _configuration.GetSection("AWS")["UserPoolClientId"],
                userPool,
                _cognito
            );

            // Authenticate the user using SRP
            var authRequest = new InitiateSrpAuthRequest
            {
                Password = request.Password
            };

            var authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

            // Return the authentication tokens
            return Ok(new AuthResponse
            {
                AccessToken = authResponse.AuthenticationResult.AccessToken,
                IdToken = authResponse.AuthenticationResult.IdToken,
                RefreshToken = authResponse.AuthenticationResult.RefreshToken
            });
        }
        catch (UserNotFoundException)
        {
            return Unauthorized(new { error = "User not found" });
        }
        catch (NotAuthorizedException)
        {
            return Unauthorized(new { error = "Invalid credentials" });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    //[HttpPost]
    //public async Task<IActionResult> SocialLogin([FromBody] ThirdPartyLoginRequest request)
    //{
    //    User userInfo;

    //    try
    //    {
    //        userInfo = request.Provider.ToLower() switch
    //        {
    //            "google" => await _service.GetGoogleUserInfoAsync(request.AccessToken),
    //            "facebook" => await _service.GetFacebookUserInfoAsync(request.AccessToken),
    //            _ => throw new ArgumentException("Unsupported provider")
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(new { message = $"Invalid access token or provider: {ex.Message}" });
    //    }
    //}

    private async Task EnsureUserInCognitoAsync(User userInfo)
    {
        var userPoolId = config.GetSection("AWS")["UserPoolId"];
        try
        {
            await _cognito.AdminGetUserAsync(new AdminGetUserRequest
            {
                UserPoolId = userPoolId,
                Username = userInfo.Email
            });
        }
        catch (UserNotFoundException)
        {
            var createRequest = new AdminCreateUserRequest
            {
                Username = userInfo.Email,
                UserAttributes =
                [
                    new() { Name = "email", Value = userInfo.Email },
                    new() { Name = "name", Value = userInfo.Name },
                    new() { Name = "email_verified", Value = "true" }
                ],
                MessageAction = MessageActionType.SUPPRESS
            };

            var result = await _cognito.AdminCreateUserAsync(createRequest);

            if (result.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Failed to create user in Cognito");
            }

        }
    }
}
