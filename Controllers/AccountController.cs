﻿using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using auth_service.Models;
using auth_service.Services.AccountService;
using auth_service.Services.JwtService;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Controllers
{
	[ApiController]
	[Route("account")]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;
		private readonly IJwtService _jwtService;

		public AccountController(IAccountService accountService, IJwtService jwtService)
		{
			this._accountService = accountService;
			this._jwtService = jwtService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(string email, string password)
		{
			try
			{
				var account = await this._accountService.AuthenticateAccount(email, password);

				return this.Ok(new
				{
					user = new
					{
						account.Id,
						account.Email,
						account.Username
					},
					access_token = this._jwtService.CreateJwt(account.Id, account.Username)
				});
			}
			catch (AuthenticationException)
			{
				return this.Unauthorized();
			}
			catch (Exception)
			{
				return this.StatusCode(500);
			}
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
