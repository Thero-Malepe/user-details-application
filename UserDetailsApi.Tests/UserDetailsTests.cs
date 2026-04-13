using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserDetailsApi.Controllers;
using UserDetailsApi.DTOs;
using UserDetailsApi.DTOs.AuthDtos;
using UserDetailsApi.Interfaces;
using UserDetailsApi.Models;

namespace UserDetailsApi.Tests
{
    public class UserDetailsTests
    {
        private readonly Mock<IAuthManagerService> _authMock;
        private readonly Mock<IUserDetailsService> _userDetailsMock;
        private readonly AuthController _authController;
        private readonly UserDetailsController _userDetailcontroller;

        public UserDetailsTests()
        {
            _authMock = new Mock<IAuthManagerService>();
            _userDetailsMock = new Mock<IUserDetailsService>();

            _userDetailcontroller = new UserDetailsController(
                _userDetailsMock.Object
            );

            _authController = new AuthController(
                _authMock.Object
            );
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsCreated()
        {
            // Arrange
            var request = new UserDto 
            {   
                Email = "test@example.com",
                Password = "password",
                FirstName = "Thero",
                LastName = "Malepe"
            };

            var createdUser = new User { Email = "test@example.com" };

            _authMock
                .Setup(x => x.Register(request))
                .ReturnsAsync(createdUser);

            // Act
            var result = await _authController.Register(request);

            // Assert
            var ok = result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(createdUser);
        }

        [Fact]
        public async Task Register_UserExists_ReturnsBadRequest()
        {
            // Arrange
            var request = new UserDto 
            {
                Email = "test@example.com",
                Password = "password",
                FirstName = "Thero",
                LastName = "Malepe"
            };

            _authMock
                .Setup(x => x.Register(request))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _authController.Register(request);

            // Assert
            var bad = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            bad.Value.Should().Be("User already exists");

        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new LoginDto { Email = "test@example.com", Password = "123" };
            var tokenResponse = new TokenResponseDto { AccessToken = "abc", RefreshToken = "xyz" };

            _authMock
                .Setup(x => x.Login(request))
                .ReturnsAsync(tokenResponse);

            // Act
            var result = await _authController.Login(request);

            // Assert
            var ok = result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(tokenResponse);
        }

        [Fact]
        public async Task Login_UserDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginDto { Email = "wrong@example.com", Password = "bad" };

            _authMock
                .Setup(x => x.Login(request))
                .ReturnsAsync((TokenResponseDto?)null);

            // Act
            var result = await _authController.Login(request);

            // Assert
            var bad = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            bad.Value.Should().Be("Email or password is incorrect");

        }

        [Fact]
        public async Task RefreshToken_ShouldReturnOk_WhenTokensAreValid()
        {
            // Arrange
            var request = new RefreshTokenDto { AccessToken = "old", RefreshToken = "refresh" };
            var newTokens = new TokenResponseDto { AccessToken = "new", RefreshToken = "newRefresh" };

            _authMock
                .Setup(x => x.RefreshToken(request))
                .ReturnsAsync(newTokens);

            // Act
            var result = await _authController.RefreshToken(request);

            // Assert
            var ok = result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(newTokens);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnUnauthorized_WhenRefreshTokenInvalid()
        {
            // Arrange
            var request = new RefreshTokenDto { AccessToken = "old", RefreshToken = "expired" };

            _authMock
                .Setup(x => x.RefreshToken(request))
                .ReturnsAsync((TokenResponseDto?)null);

            // Act
            var result = await _authController.RefreshToken(request);

            // Assert
            var unauthorized = result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
            unauthorized.Value.Should().Be("Invalid refresh token");
        }


        [Fact]
        public async Task GetUserDetails_ShouldReturnOk_WhenEmailIsValid()
        {
            // Arrange
            var email = "wrong@example.com";
            var detailsResponse = new UserDetailsDto { Email = "wrong@example.com" };

            _userDetailsMock
                .Setup(x => x.GetUserDetails(email))
                .ReturnsAsync(detailsResponse);

            // Act
            var result = await _userDetailcontroller.GetUserDetails(email);

            // Assert
            var ok = result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(detailsResponse);
        }


        [Fact]
        public async Task GetUserDetails_ShouldReturnNotFound_WhenEmailDoesNotExist()
        {
            // Arrange
            var email = "wrong@example.com";

            _userDetailsMock
                .Setup(x => x.GetUserDetails(email))
                .ReturnsAsync((UserDetailsDto?)null);

            // Act
            var result = await _userDetailcontroller.GetUserDetails(email);

            // Assert
            var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFound.Value.Should().Be("User details not found");
        }
    }
}