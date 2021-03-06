﻿using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Dtos.TrueLayer;
using FractalSolutions.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Controllers
{
    [Route("api/user/transactions/")]
    public class UserTransactionsController : Controller
    {
        private readonly IUserAccountsTransactionsService _userAccountsTransactionService;
        private readonly IUserAccountsTransactionsSummaryService _userAccountsTransactionsSummaryService;

        public UserTransactionsController(
            IUserAccountsTransactionsService userAccountsTransactionService,
            IUserAccountsTransactionsSummaryService userAccountsTransactionsSummaryService)
        {
            _userAccountsTransactionService = userAccountsTransactionService;
            _userAccountsTransactionsSummaryService = userAccountsTransactionsSummaryService;
        }


        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IList<AccountTransactions>), StatusCodes.Status200OK)]        
        public async Task<IActionResult> Get()
        {
            return Ok(await _userAccountsTransactionService.GetUserTransactionsAsync());
        }

        [HttpGet("summary")]
        [Authorize]
        [ProducesResponseType(typeof(IDictionary<TransactionCategoryTL, decimal>), StatusCodes.Status200OK)]        
        public async Task<IActionResult> GetSummary()
        {
            return Ok(await _userAccountsTransactionsSummaryService.GetAccountsSummaryAsync());
        }
    }
}
