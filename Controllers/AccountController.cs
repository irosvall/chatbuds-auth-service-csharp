using System;
using System.Threading.Tasks;
using auth_service.Models;
using auth_service.Services.AccountService;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Controllers
{
	[ApiController]
	[Route("account")]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			this._accountService = accountService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(Account account)
		{
			try
			{
				await this._accountService.RegisterAccount(account);
				return this.Created("", new
				{
					account.Id,
					account.Email,
					account.Username
				});
			}
			catch (ValidationException e)
			{
				return this.BadRequest(e.Errors);
			}
			catch (Exception)
			{
				return this.StatusCode(500);
			}
		}
	}
}
