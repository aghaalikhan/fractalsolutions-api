using FractalSolutions.Api.Controllers;
using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Dtos.TrueLayer;
using FractalSolutions.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Unit.Tests.Services
{
    [TestFixture]
    public class RedirectControllerTests
    {
        private AutoMocker _autoMocker;
        private RedirectController _redirectController;

        [SetUp]
        public void Setup()
        {            
            _autoMocker = new AutoMocker();
            _redirectController = _autoMocker.CreateInstance<RedirectController>();
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task Get_EmptyNullOrWhiteSpaceCode(string code)
        {
            //Arrange
            //Act
            var response = await _redirectController.Get(code);

            //Assert
            response.ShouldBeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Get_CallsAuthorisationService()
        {
            //Arrange           
            var code = "some-code";

            //Act
            await _redirectController.Get(code);

            //Assert            
            _autoMocker.Verify<IAuthorisationService>(ass => ass.GetAccessTokenAsync(code));
        }

        [Test]
        public async Task Get_AuthorisationServiceCallSucceeds_ReturnsTokenInfo()
        {
            //Arrange
            var code = "some-code";
            var tokenInfo = new TokenInfoTL 
            { 
                AccessToken = "my-token"
            };

            _autoMocker.Setup<IAuthorisationService, Task<TokenInfoTL>>(ass => ass.GetAccessTokenAsync(code))
                .ReturnsAsync(tokenInfo);


            //Act
            var response = await _redirectController.Get(code);

            //Assert
            response.ShouldBeOfType<OkObjectResult>();
            var result = (OkObjectResult)response;
            result.Value.ShouldBeSameAs(tokenInfo);
        }      
    }
}
