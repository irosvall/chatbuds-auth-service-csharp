using System;
using System.Data;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using auth_service.Models;
using auth_service.Services.AccountService;
using auth_service.Services.JwtService;
using FluentValidation;
using FluentValidation.Results;
using JWT.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Controllers
{
	[ApiController]
	[Route("api/v1")]
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
		public async Task<IActionResult> Login(AccountLogin account)
		{
			try
			{
				var authenticatedAccount =
					await this._accountService.AuthenticateAccount(account.Email, account.Password);

				return this.Ok(new
				{
					user = this.FilterAccountInformation(authenticatedAccount),
					access_token = this._jwtService.CreateJwt(authenticatedAccount.Id, authenticatedAccount.Username)
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
				return this.Created("", this.FilterAccountInformation(account));
			}
			catch (ValidationException e)
			{
				return this.HandleValidationException(e);
			}
			catch (Exception)
			{
				return this.StatusCode(500);
			}
		}

		[HttpDelete("user")]
		public async Task<IActionResult> Delete()
		{
			try
			{
				var account = this._jwtService.AuthenticateJwt(this.Request.Headers);

				await this._accountService.DeleteAccount(account);
				return this.NoContent();
			}
			catch (DataException)
			{
				return this.NotFound();
			}
			catch (AuthenticationException)
			{
				return this.Unauthorized();
			}
			catch (Exception e)
			{
				if (e is TokenExpiredException or SignatureVerificationException)
				{
					return this.StatusCode(403);
				}

				return this.StatusCode(500);
			}
		}

		private Account FilterAccountInformation(Account account)
		{
			return new()
			{
				Id = account.Id,
				Email = account.Email,
				Username = account.Username
			};
		}

		private IActionResult HandleValidationException(ValidationException e)
		{
			var validationFailure = e.Errors.First();

			if (this.IsDuplicationValidationError(validationFailure))
			{
				return this.Conflict(new {message = validationFailure.ErrorMessage});
			}

			return this.BadRequest(validationFailure);
		}

		private bool IsDuplicationValidationError(ValidationFailure validationFailure)
		{
			return validationFailure.ErrorCode == "DuplicationError";
		}
	}
}
