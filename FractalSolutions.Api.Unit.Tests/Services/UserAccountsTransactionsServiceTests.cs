using AutoFixture;
using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Services;
using FractalSolutions.Api.Services.Interfaces;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Unit.Tests.Services
{
    [TestFixture]
    public class UserAccountsTransactionsServiceTests
    {
        private Fixture _autoFixture;
        private AutoMocker _autoMocker;
        private UserAccountsTransactionsService _userAccountsTransactionsService;

        [SetUp]
        public void TestSetup()
        {
            _autoFixture = new Fixture();
            _autoMocker = new AutoMocker();
            _userAccountsTransactionsService = _autoMocker.CreateInstance<UserAccountsTransactionsService>();
        }

        [Test]
        public async Task GetUserTransactionsAsync_TransationsExistForAccounts_ReturnsAccountTransactions()
        {
            //Arrange
            var accountOne = _autoFixture.Create<Account>();
            var accountTwo = _autoFixture.Create<Account>();

            var accountOneTransactions = _autoFixture.CreateMany<Transaction>(2)
              .ToList();

            var accountTwoTransactions = _autoFixture.CreateMany<Transaction>(2)
              .ToList();

            _autoMocker.Setup<IUserAccountsService, Task<IList<Account>>>(uas => uas.GetUserAccountsAsync())
              .ReturnsAsync(new List<Account> { accountOne, accountTwo });

            _autoMocker.Setup<IAccountTransactionsService, Task<IList<Transaction>>>(uas => uas.GetAccountTransactionsAsync(accountOne.AccountId))
                    .ReturnsAsync(accountOneTransactions);

            _autoMocker.Setup<IAccountTransactionsService, Task<IList<Transaction>>>(uas => uas.GetAccountTransactionsAsync(accountTwo.AccountId))
                    .ReturnsAsync(accountTwoTransactions);

            //Act
            var accountTransactions = await _userAccountsTransactionsService.GetUserTransactionsAsync();

            //Assert
            accountTransactions.Count.ShouldBe(2);
            
            accountTransactions[0].AccountId.ShouldBe(accountOne.AccountId);
            accountTransactions[0].Transactions.ShouldBe(accountOneTransactions);

            accountTransactions[1].AccountId.ShouldBe(accountTwo.AccountId);
            accountTransactions[1].Transactions.ShouldBe(accountTwoTransactions);
        }
    }
}
