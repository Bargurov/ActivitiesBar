using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Appliction.Core;
using API.Extensions;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BaseApiController: ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices
        .GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result) 
        {
            if(result == null) return NotFound();
            if(result.IsSuccess&& result.Value != null)
                return Ok(result.Value);
            if(result.IsSuccess&& result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
            
        }
        protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
            {
                Response.AddHeader(result.Value.CurrentPage, result.Value.PageSize, 
                    result.Value.TotalCount, result.Value.TotalPages);
                return Ok(result.Value);
            }
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }
    }
}


