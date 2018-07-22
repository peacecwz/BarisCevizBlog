using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Api.Controllers
{
    public class IoTController : Controller
    {
        Repositories.Repository<EF.Tables.IoTHub> IoTRepo = new Repositories.Repository<EF.Tables.IoTHub>();

        [HttpGet]
        [Route("api/iot/get")]
        public string GetUrl()
        {
            EF.Tables.IoTHub hub = IoTRepo.First();
            return hub?.Url;
        }

        [Route("api/iot/change")]
        [HttpPost]
        public IActionResult Change([FromBody]string url)
        {
            bool isSuccess = false;
            EF.Tables.IoTHub hub = IoTRepo.First();
            if (hub == null)
            {
                hub = new EF.Tables.IoTHub()
                {
                    Url = url
                };
                isSuccess = IoTRepo.Add(hub);
            }
            else
            {
                hub.Url = url;
                isSuccess = IoTRepo.Update(hub);
            }
            if (isSuccess)
                return Ok();
            else
                return BadRequest();
        }
    }
}
