using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using Persistence;
using Appliction.Activities;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    
    public class ActivitiesController : BaseApiController
    {

        [HttpGet]
        public async Task <IActionResult> GetActivities()
        {
            return HandleResult(await Mediator.Send(new List.Query()));

        }
        [HttpGet("{id}")]
        public async Task <IActionResult> GetActivitie(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query{Id=id}));
        }

        [HttpPost]
        public async Task<IActionResult>CreateActivity(Activity activity)
        {
            return HandleResult(await Mediator.Send(new Create.Command{Activity=activity}));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
            public async Task<IActionResult>EditActivity(Guid id,Activity activity)
            {
                activity.Id =id;
                return HandleResult(await Mediator.Send(new Edit.Command{Activity=activity}));
            }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command{Id =id }));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult>Attend(Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command{Id=id})); 
        }
    }
}