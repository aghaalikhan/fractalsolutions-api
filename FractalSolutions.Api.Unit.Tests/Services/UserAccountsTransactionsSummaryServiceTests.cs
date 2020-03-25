using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Dtos.TrueLayer;
using FractalSolutions.Api.Services;
using FractalSolutions.Api.Services.Interfaces;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Unit.Tests.Services
{
    [TestFixture]
    public class UserAccountsTransactionsSummaryServiceTests
    {  
        private AutoMocker _autoMocker;
        private UserAccountsTransactionsSummaryService _userAccountsTransactionsSummaryService;

        [SetUp]
        public void TestSetup()
        {            
            _autoMocker = new AutoMocker();
            _userAccountsTransactionsSummaryService = _autoMocker.CreateInstance<UserAccountsTransactionsSummaryService>();
        }

        [Test]
        public async Task GetAccountsSummaryAsync_NoAccountTransactions_ReturnsZeroForEachCategory()
        {
            //Arrange
            var accountTransactions = new List<AccountTransactions>();

            _autoMocker.Setup<IUserAccountsTransactionsService, Task<IList<AccountTransactions>>>(uats => uats.GetUserTransactionsAsync())
                .ReturnsAsync(accountTransactions);

            //Act
            var summary = await _userAccountsTransactionsSummaryService.GetAccountsSummaryAsync();

            //Assert
            summary[TransactionCategoryTL.Purchase].ShouldBe(0);
            summary[TransactionCategoryTL.Credit].ShouldBe(0);
            summary[TransactionCategoryTL.Debit].ShouldBe(0);
            summary[TransactionCategoryTL.Bill_Payment].ShouldBe(0);
            summary[TransactionCategoryTL.Direct_Debit].ShouldBe(0);
            summary[TransactionCategoryTL.Atm].ShouldBe(0);
            summary[TransactionCategoryTL.Transfer].ShouldBe(0);
            summary[TransactionCategoryTL.Cash].ShouldBe(0);
        }

        [TestCase(50.5, 22.4, TransactionCategoryTL.Purchase)]
        [TestCase(15.0, 20.0, TransactionCategoryTL.Credit)]
        [TestCase(9.0, 2.0, TransactionCategoryTL.Debit)]
        [TestCase(8.0, 15.0, TransactionCategoryTL.Purchase)]
        [TestCase(17.5, 18.0, TransactionCategoryTL.Bill_Payment)]
        [TestCase(55.0, 2.0, TransactionCategoryTL.Direct_Debit)]
        [TestCase(75.0, 20.0, TransactionCategoryTL.Atm)]
        [TestCase(23.05, 11.23, TransactionCategoryTL.Transfer)]
        [TestCase(7.23, 8.99, TransactionCategoryTL.Cash)]

        public async Task GetAccountsSummaryAsync_TransactionsAreWithinTimeDelta_ReturnsSummaryForEachCategory(decimal transactionOneAmt, decimal transacitonTwoAmt, TransactionCategoryTL category)
        {
            //Arrange
            var today = new DateTimeOffset(2019, 01, 11, 0, 0, 0, TimeSpan.Zero);
            var yesterday = new DateTimeOffset(2019, 01, 10, 0, 0, 0, TimeSpan.Zero);

            var transactionsAccOne = new Transaction
            {
                TransactionCategory = category,
                Timestamp = yesterday,
                Amount = transactionOneAmt
            };

            var transactionsAccTwo = new Transaction
            {
                TransactionCategory = category,
                Timestamp = yesterday,
                Amount = transacitonTwoAmt
            };

            var accountTransactionsOne = new AccountTransactions
            {
                Transactions = new List<Transaction> { transactionsAccOne }
            };

            var accountTransactionsTwo = new AccountTransactions
            {
                Transactions = new List<Transaction> { transactionsAccTwo }
            };

            var accountTransactions = new List<AccountTransactions>
            {
                accountTransactionsOne,
                accountTransactionsTwo
            };

            _autoMocker.Setup<IDateTimeService, DateTimeOffset>(dts => dts.UtcNow).Returns(today);                

            _autoMocker.Setup<IUserAccountsTransactionsService, Task<IList<AccountTransactions>>>(uats => uats.GetUserTransactionsAsync())
                .ReturnsAsync(accountTransactions);

            //Act
            var summary = await _userAccountsTransactionsSummaryService.GetAccountsSummaryAsync();

            //Assert
            summary[category].ShouldBe(transactionOneAmt + transacitonTwoAmt);            
        }

        [Test]
        public async Task GetAccountsSummaryAsync_TransactionsOlderThanAWeek_IsExcludedFromSummary()
        {
            //Arrange
            var today = new DateTimeOffset(2019, 01, 11, 0, 0, 0, TimeSpan.Zero);
            var weekAgo = new DateTimeOffset(2019, 01, 4, 0, 0, 0, TimeSpan.Zero);
            var weekPlusDayAgo = new DateTimeOffset(2019, 01, 3, 0, 0, 0, TimeSpan.Zero);

            var transactionsOne = new Transaction
            {
                TransactionCategory = TransactionCategoryTL.Cash,
                Timestamp = weekAgo,
                Amount = 5.0m
            };

            var transactionTwo = new Transaction
            {
                TransactionCategory = TransactionCategoryTL.Cash,
                Timestamp = weekPlusDayAgo,
                Amount = 15.0m
            };

            var accountTransactions = new AccountTransactions
            {
                Transactions = new List<Transaction> { transactionsOne,  }
            };     

            _autoMocker.Setup<IDateTimeService, DateTimeOffset>(dts => dts.UtcNow).Returns(today);

            _autoMocker.Setup<IUserAccountsTransactionsService, Task<IList<AccountTransactions>>>(uats => uats.GetUserTransactionsAsync())
                .ReturnsAsync(new List<AccountTransactions> { accountTransactions });

            //Act
            var summary = await _userAccountsTransactionsSummaryService.GetAccountsSummaryAsync();

            //Assert
            summary[TransactionCategoryTL.Cash].ShouldBe(5.0m);
        }

        [Test]
        public async Task GetAccountsSummaryAsync_TransactionsToday_AreExcludedFromSummary()
        {
            //Arrange
            var today = new DateTimeOffset(2019, 01, 11, 0, 0, 0, TimeSpan.Zero);           

            var transactionsOne = new Transaction
            {
                TransactionCategory = TransactionCategoryTL.Cash,
                Timestamp = today,
                Amount = 5.0m
            };
        
            var accountTransactions = new AccountTransactions
            {
                Transactions = new List<Transaction> { transactionsOne, }
            };

            _autoMocker.Setup<IDateTimeService, DateTimeOffset>(dts => dts.UtcNow).Returns(today);

            _autoMocker.Setup<IUserAccountsTransactionsService, Task<IList<AccountTransactions>>>(uats => uats.GetUserTransactionsAsync())
                .ReturnsAsync(new List<AccountTransactions> { accountTransactions });

            //Act
            var summary = await _userAccountsTransactionsSummaryService.GetAccountsSummaryAsync();

            //Assert
            summary[TransactionCategoryTL.Cash].ShouldBe(0);
        }
    }       
}           
            
            