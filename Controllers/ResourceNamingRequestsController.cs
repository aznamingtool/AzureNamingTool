using AzNamingTool.Models;
using AzNamingTool.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Threading;
using AzNamingTool.Services;
using AzNamingTool.Attributes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzNamingTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class ResourceNamingRequestsController : ControllerBase
    {
        // POST api/<ResourceNamingRequestsController>
        /// <summary>
        /// This function will generate a resoure type name for specifed component values. 
        /// </summary>
        /// <param name="request">json - Resource Name Request data</param>
        /// <returns>string - Name generation response</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RequestName([FromBody] ResourceNameRequest request)
        {
            try
            {
                ResourceNameResponse resourceNameRequestResponse = await ResourceNamingRequestService.RequestName(request);

                if (resourceNameRequestResponse.Success)
                {
                    return Ok(resourceNameRequestResponse);
                }
                else
                {
                    return BadRequest(resourceNameRequestResponse);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
