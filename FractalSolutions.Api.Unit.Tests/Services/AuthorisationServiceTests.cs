using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.HttpClients;
using FractalSolutions.Api.Repositories;
using FractalSolutions.Api.Services;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Unit.Tests.Services
{
    public class AuthorisationServiceTests
    {
        //jwt created on jwt io with subject value "123"
        private const string _jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjMiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjJ9.z__LV80pBSF1gtPyyx743i3i14R9lrXMlPnlIxYUNnA";
        private AutoMocker _autoMocker;
        private AuthorisationService _authorisationService;

        [SetUp]
        public void TestSetup()
        {
            _autoMocker = new AutoMocker();
            _authorisationService = _autoMocker.CreateInstance<AuthorisationService>();
        }

        [Test]
        public async Task GetAccessTokenAsync_TokenInfoReturnedFromTrueLayerAuthClient_SavesUserNameToRepository()
        {
            //Arrange
            var code = "some-code";

            var tokenInfo = new TokenInfoTL
            {
                AccessToken = _jwt
            };

            _autoMocker.Setup<ITrueLayerAuthClient, Task<TokenInfoTL>>(tlac => tlac.GetTokenAsync(code)).
                ReturnsAsync(tokenInfo);

            //Act
            await _authorisationService.GetAccessTokenAsync(code);

            //Assert
            // hard coded jwt has value "agha" for subject
            _autoMocker.Verify<IUserRepository>(ur => ur.AddUser(It.Is<Entities.UserEntity>(user => user.UserId == "123")));
        }

        [Test]
        public async Task GetAccessTokenAsync_TokenInfoReturnedFromTrueLayerAuthClient_ReturnsTokenInfo()
        {
            //Arrange
            var code = "some-code";

            var tokenInfo = new TokenInfoTL
            {
                AccessToken = _jwt
            };

            _autoMocker.Setup<ITrueLayerAuthClient, Task<TokenInfoTL>>(tlac => tlac.GetTokenAsync(code)).
                ReturnsAsync(tokenInfo);

            //Act
            var result = await _authorisationService.GetAccessTokenAsync(code);

            //Assert
            result.ShouldBe(tokenInfo);
        }
    }
}
