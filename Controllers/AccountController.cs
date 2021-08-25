using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth_service.Models;
using auth_service.Services.AccountService;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public IActionResult Register(Account account)
        {
            try
            {
                _accountService.CreateAccount(account);
                return Created("", new
                {
                    account.Id,
                    account.Email,
                    account.Username
                });
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
