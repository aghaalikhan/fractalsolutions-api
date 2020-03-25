using AutoFixture;
using FractalSolutions.Api.Dtos.TrueLayer;
using FractalSolutions.Api.HttpClients;
using FractalSolutions.Api.Infrastructure;
using FractalSolutions.Api.Services;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Unit.Tests.Services
{
    [TestFixture]
    public class AccountTransactionServiceTests
    {
        private Fixture _autoFixture;
        private AutoMocker _autoMocker;
        private AccountTransactionsService _accountTransactionsService;
        
        [SetUp]
        public void TestSetup()
        {
            _autoFixture = new Fixture();
            _autoMocker = new AutoMocker();
            _accountTransactionsService = _autoMocker.CreateInstance<AccountTransactionsService>();
        }

        [Test]
        public async Task GetAccountTransactionsAsync_CacheHasTransactionsForAccountIdSupplied_ReturnsTransactions()
        {
            //Arrange
            var accountId = "some-acc-id";

            var transactionsTL = _autoFixture
                .CreateMany<TransactionTL>(2)
                .ToList();

            var cacheKey = $"{accountId}_{CacheKeyConstants.AccountTransactionsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<TransactionTL>>(cmc => cmc.Get<IList<TransactionTL>>(cacheKey)).Returns(transactionsTL);

            //Act
            var expectedTransactions = await _accountTransactionsService.GetAccountTransactionsAsync(accountId);

            //Assert
            expectedTransactions.Count.ShouldBe(2);
            expectedTransactions.Zip(transactionsTL).ToList().ForEach(transactionPair =>
            {
                transactionPair.First.Amount.ShouldBe(transactionPair.Second.Amount);
                transactionPair.First.TransactionId.ShouldBe(transactionPair.Second.TransactionId);
                transactionPair.First.TransactionCategory.ShouldBe(transactionPair.Second.TransactionCategory);
                transactionPair.First.Timestamp.ShouldBe(transactionPair.Second.Timestamp);
            });
        }


        [Test]
        public async Task GetAccountTransactionsAsync_CacheHasTransactionsForAccountIdSupplied_DoesNotCallTrueLayerDataClient()
        {
            //Arrange
            var accountId = "some-acc-id";

            var transactionsTL = _autoFixture
                .CreateMany<TransactionTL>(2)
                .ToList();

            var cacheKey = $"{accountId}_{CacheKeyConstants.AccountTransactionsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<TransactionTL>>(cmc => cmc.Get<IList<TransactionTL>>(cacheKey)).Returns(transactionsTL);

            //Act
            var expectedTransactions = await _accountTransactionsService.GetAccountTransactionsAsync(accountId);

            //Assert
            _autoMocker.Verify<ITrueLayerDataClient>(tldc => tldc.GetAccountTransactionsAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task GetAccountTransactionsAsync__TrueLayerDateClientReturnsSuccess_ReturnsTransactionsFromTrueLayerDateClient()
        {
            //Arrange
            var accountId = "some-acc-id";

            var transactionsTL = _autoFixture
                .CreateMany<TransactionTL>(2)
                .ToList();

            var clientResult = new ResponseTL<TransactionTL>
            {
                Results = transactionsTL,
                Status = ResultStatusTL.Succeeded
            };

            var cacheKey = $"{accountId}_{CacheKeyConstants.AccountTransactionsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<TransactionTL>>(cmc => cmc.Get<IList<TransactionTL>>(cacheKey))
                .Returns(null as IList<TransactionTL>);

            _autoMocker.Setup<ITrueLayerDataClient, Task<ResponseTL<TransactionTL>>>(tldc => tldc.GetAccountTransactionsAsync(accountId))
                .ReturnsAsync(clientResult);

            //Act
            var expectedTransactions = await _accountTransactionsService.GetAccountTransactionsAsync(accountId);

            //Assert
            expectedTransactions.Count.ShouldBe(2);
            expectedTransactions.Zip(transactionsTL).ToList().ForEach(transactionPair =>
            {
                transactionPair.First.Amount.ShouldBe(transactionPair.Second.Amount);
                transactionPair.First.TransactionId.ShouldBe(transactionPair.Second.TransactionId);
                transactionPair.First.TransactionCategory.ShouldBe(transactionPair.Second.TransactionCategory);
                transactionPair.First.Timestamp.ShouldBe(transactionPair.Second.Timestamp);
            });
        }

        [Test]
        public async Task GetAccountTransactionsAsync_CacheDoesNotHaveTransactionsForAccountIdSupplied_TransactionsFromTrueLayerDataClientAreAddedToCache()
        {
            //Arrange
            var accountId = "some-acc-id";

            var transactionsTL = _autoFixture
                .CreateMany<TransactionTL>(2)
                .ToList();

            var clientResult = new ResponseTL<TransactionTL>
            {
                Results = transactionsTL,
                Status = ResultStatusTL.Succeeded
            };

            var cacheKey = $"{accountId}_{CacheKeyConstants.AccountTransactionsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<TransactionTL>>(cmc => cmc.Get<IList<TransactionTL>>(cacheKey))
                .Returns(null as IList<TransactionTL>);

            _autoMocker.Setup<ITrueLayerDataClient, Task<ResponseTL<TransactionTL>>>(tldc => tldc.GetAccountTransactionsAsync(accountId))
                .ReturnsAsync(clientResult);

            //Act
            var expectedTransactions = await _accountTransactionsService.GetAccountTransactionsAsync(accountId);

            //Assert
            _autoMocker.Verify<ICustomMemoryCache>(cmc => cmc.Set(cacheKey, clientResult.Results));
        }

        [Test]
        public void GetAccountTransactionsAsync_TrueLayerDateClientReturnsFailure_ShouldThrowAndException()
        {
            //Arrange
            var accountId = "some-acc-id";

            var transactionsTL = _autoFixture
                .CreateMany<TransactionTL>(2)
                .ToList();

            var clientResult = new ResponseTL<TransactionTL>
            {
                Results = transactionsTL,
                Status = ResultStatusTL.Failed
            };

            var cacheKey = $"{accountId}_{CacheKeyConstants.AccountTransactionsKey}";

            _autoMocker.Setup<ICustomMemoryCache, IList<TransactionTL>>(cmc => cmc.Get<IList<TransactionTL>>(cacheKey))
                .Returns(null as IList<TransactionTL>);

            _autoMocker.Setup<ITrueLayerDataClient, Task<ResponseTL<TransactionTL>>>(tldc => tldc.GetAccountTransactionsAsync(accountId))
                .ReturnsAsync(clientResult);

            //Act
            Func<Task<IList<Dtos.Transaction>>> getTransactionsFunc = async () => await _accountTransactionsService.GetAccountTransactionsAsync(accountId);

            //Assert
            getTransactionsFunc.ShouldThrow<Exception>();
        }
    }

}
