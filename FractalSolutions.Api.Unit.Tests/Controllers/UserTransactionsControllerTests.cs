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

namespace FractalSolutions.Api.Unit.Tests.Controllers
{
    [TestFixture]
    public class UserTransactionsControllerTests
    {
        private AutoMocker _autoMocker;
        private UserTransactionsController _userTransactionsController;

        [SetUp]
        public void Setup()
        {
            _autoMocker = new AutoMocker();
            _userTransactionsController = _autoMocker.CreateInstance<UserTransactionsController>();
        }

        [Test]
        public async Task Get_CallsUserAccountsTransactionService()
        {
            //Arrange
            //Act
            await _userTransactionsController.Get();

            //Assert
            _autoMocker.Verify<IUserAccountsTransactionsService>(uats => uats.GetUserTransactionsAsync());
        }

        [Test]
        public async Task Get_UserAccountsTransactionServiceSucceeds_ReturnsOkResultsWithUserTransactions()
        {
            //Arrange
            IList<AccountTransactions> transactions = new List<AccountTransactions>
            {
                new AccountTransactions()
            };

            _autoMocker.Setup<IUserAccountsTransactionsService, Task<IList<AccountTransactions>>>(uats => uats.GetUserTransactionsAsync())
                .ReturnsAsync(transactions);

            //Act
            var response = await _userTransactionsController.Get();

            //Assert            
            response.ShouldBeOfType<OkObjectResult>();
            var result = (OkObjectResult)response;
            result.Value.ShouldBeSameAs(transactions);
        }

        [Test]
        public async Task GetSummary_CallsUserAccountsTransactionsSummaryService()
        {
            //Arrange
            //Act
            await _userTransactionsController.GetSummary();

            //Assert
            _autoMocker.Verify<IUserAccountsTransactionsSummaryService>(uatss => uatss.GetAccountsSummaryAsync());
        }

        [Test]
        public async Task GetSummary_UserAccountsTransactionsSummaryServiceSucceeds_ReturnsOkResultsWithTransactionsSummary()
        {
            //Arrange
            IDictionary<TransactionCategoryTL, decimal> transactionSummary = new Dictionary<TransactionCategoryTL, decimal>
            {
                { TransactionCategoryTL.Atm, 0.0m }
            };

            _autoMocker.Setup<IUserAccountsTransactionsSummaryService, Task<IDictionary<TransactionCategoryTL, decimal>>>(uatss => uatss.GetAccountsSummaryAsync())
                .ReturnsAsync(transactionSummary);

            //Act
            var response = await _userTransactionsController.GetSummary();

            //Assert            
            response.ShouldBeOfType<OkObjectResult>();
            var result = (OkObjectResult)response;
            result.Value.ShouldBeSameAs(transactionSummary);
        }
    }
}
