using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth_service.Models;
using auth_service.Services.DbClient;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace auth_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMongoCollection<Account> _accounts;
        public AccountController(IDbClient dbClient)
        {
            _accounts = dbClient.GetAccountCollection();
        }
    }
}
