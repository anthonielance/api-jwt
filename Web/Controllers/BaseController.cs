using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
    [Route("api/users")]
    public abstract class BaseController<T> : Controller where T : BaseEntity
    {
        protected IRepository<T> _repository;

        public BaseController(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("me/[controller]/{year:int}")]
        public virtual IActionResult Get(int year)
        {
            return GetFromRepository(User?.Identity?.Name, year);
        }

        [Authorize(Policy = "ADMIN_ACCESS")]
        [HttpGet("{upn:email}/[controller]/{year:int}")]
        public virtual IActionResult Get(int year, string upn)
        {
            return GetFromRepository(upn, year);
        }

        protected abstract IActionResult GetFromRepository(string upn, int year);
    }
}
