using FractalSolutions.Api.Services;
using NUnit.Framework;
using Moq.AutoMock;
using System.Threading.Tasks;
using AutoFixture;
using FractalSolutions.Api.HttpClients;
using System.Linq;
using FractalSolutions.Api.Infrastructure;
using System.Collections.Generic;
using Shouldly;
using Moq;
using FractalSolutions.Api.Dtos.TrueLayer;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using FractalSolutions.Api.Dtos;
using System;

namespace FractalSolutions.Api.Unit.Tests.Services
{
    [TestFixture]
    public class UserAccountServiceTests
    {
        private Fixture _autoFixture;
        private AutoMocker _autoMocker;
        private UserAccountsService _userAccountService;
        
        [SetUp]
        public void TestSetup()
        {
            _autoFixture = new Fixture();
            _autoMocker = new AutoMocker();
            _userAccountService = _autoMocker.CreateInstance<UserAccountsService>();
        }

        [Test]
        public async Task GetUserAccountsAsync_CacheHasUserAccounts_ReturnsUserAccounts()
        {
            //Arrange
            var userId = "some-user-id";

            SetupContextMock(userId);

            var accountTL = _autoFixture
                .CreateMany<AccountTL>(2)
                .ToList();

            var cacheKey = $"{userId}_{CacheKeyConstants.UserAccountsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<AccountTL>>(cmc => cmc.Get<IList<AccountTL>>(cacheKey)).Returns(accountTL);

            //Act
            var expectedAccounts = await _userAccountService.GetUserAccountsAsync();

            //Assert
            expectedAccounts.Count.ShouldBe(2);
            expectedAccounts.Zip(accountTL).ToList().ForEach(accountPair =>
            {
                accountPair.First.AccountId.ShouldBe(accountPair.Second.AccountId);
                accountPair.First.DisplayName.ShouldBe(accountPair.Second.DisplayName);
                accountPair.First.AccountType.ShouldBe(accountPair.Second.AccountType);                
            });
        }


        [Test]
        public async Task GetUserAccountsAsync_CacheHasUserAccounts_DoesNotCallTheTrueLayerDataClient()
        {
            //Arrange
            var userId = "some-user-id";

            SetupContextMock(userId);

            var accountsTL = _autoFixture
                .CreateMany<AccountTL>(2)
                .ToList();

            var cacheKey = $"{userId}_{CacheKeyConstants.UserAccountsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<AccountTL>>(cmc => cmc.Get<IList<AccountTL>>(cacheKey)).Returns(accountsTL);

            //Act
            var expectedAccountss = await _userAccountService.GetUserAccountsAsync();

            //Assert
            _autoMocker.Verify<ITrueLayerDataClient>(tldc => tldc.GetUserAccountsAsync(), Times.Never);
        }

        [Test]
        public async Task GetUserAccountsAsync_CacheDoesNotHaveUserAccounts_ReturnsUserAccountsFromTrueLayerDataClient()
        {
            //Arrange
            var userId = "some-user-id";

            SetupContextMock(userId);

            var accountsTL = _autoFixture
                .CreateMany<AccountTL>(2)
                .ToList();

            var clientResult = new ResponseTL<AccountTL>
            {
                Results = accountsTL,
                Status = ResultStatusTL.Succeeded
            };

            var cacheKey = $"{userId}_{CacheKeyConstants.UserAccountsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<AccountTL>>(cmc => cmc.Get<IList<AccountTL>>(cacheKey))
                .Returns(null as IList<AccountTL>);

            _autoMocker.Setup<ITrueLayerDataClient, Task<ResponseTL<AccountTL>>>(tldc => tldc.GetUserAccountsAsync())
                .ReturnsAsync(clientResult);

            //Act
            var expectedAccounts = await _userAccountService.GetUserAccountsAsync();

            //Assert
            expectedAccounts.Count.ShouldBe(2);
            expectedAccounts.Zip(accountsTL).ToList().ForEach(accountPair =>
            {
                accountPair.First.AccountId.ShouldBe(accountPair.Second.AccountId);
                accountPair.First.DisplayName.ShouldBe(accountPair.Second.DisplayName);
                accountPair.First.AccountType.ShouldBe(accountPair.Second.AccountType);
            });
        }

        [Test]
        public async Task GetUserAccountsAsync_CacheDoesNotHaveAccounts_AccountsReturnsFromTrueDataClientAreAddedToCache()
        {
            //Arrange
            var userId = "some-user-id";

            SetupContextMock(userId);

            var accountsTL = _autoFixture
                .CreateMany<AccountTL>(2)
                .ToList();

            var clientResult = new ResponseTL<AccountTL>
            {
                Results = accountsTL,
                Status = ResultStatusTL.Succeeded
            };

            var cacheKey = $"{userId}_{CacheKeyConstants.UserAccountsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<AccountTL>>(cmc => cmc.Get<IList<AccountTL>>(cacheKey))
                .Returns(null as IList<AccountTL>);

            _autoMocker.Setup<ITrueLayerDataClient, Task<ResponseTL<AccountTL>>>(tldc => tldc.GetUserAccountsAsync())
                .ReturnsAsync(clientResult);

            //Act
            var expectedAccounts = await _userAccountService.GetUserAccountsAsync();

            //Assert
            _autoMocker.Verify<ICustomMemoryCache>(cmc => cmc.Set(cacheKey, clientResult.Results));
        }

        [Test]
        public void GetUserAccountsAsync_TrueLayerDateClientReturnsFailure_ShouldThrowAndException()
        {
            //Arrange
            var userId = "some-user-id";

            SetupContextMock(userId);

            var accountsTL = _autoFixture
                .CreateMany<AccountTL>(2)
                .ToList();

            var clientResult = new ResponseTL<AccountTL>
            {
                Results = accountsTL,
                Status = ResultStatusTL.Failed
            };

            var cacheKey = $"{userId}_{CacheKeyConstants.UserAccountsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<AccountTL>>(cmc => cmc.Get<IList<AccountTL>>(cacheKey))
                .Returns(null as IList<AccountTL>);

            _autoMocker.Setup<ITrueLayerDataClient, Task<ResponseTL<AccountTL>>>(tldc => tldc.GetUserAccountsAsync())
                .ReturnsAsync(clientResult);

            //Act
            Func<Task<IList<Account>>> getAccountsFunc = async () => await _userAccountService.GetUserAccountsAsync();

            //Assert
            getAccountsFunc.ShouldThrow<Exception>();
        }

        private void SetupContextMock(string userId)
        {
            var identity = new ClaimsIdentity();
            if (userId != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            }
            var principal = new ClaimsPrincipal(identity);

            var httpContext = _autoMocker.GetMock<HttpContext>();
            httpContext.Setup(x => x.User).Returns(principal);

            _autoMocker.Setup<IHttpContextAccessor, HttpContext>(hca => hca.HttpContext).Returns(httpContext.Object);
        }
    }
}
