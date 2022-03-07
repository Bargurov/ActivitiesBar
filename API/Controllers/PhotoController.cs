using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appliction.Photos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PhotoController:BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command{Id=id}));
        }
    

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            return HandleResult(await Mediator.Send(new SetMain.Command{Id=id}));
        }
    }
}