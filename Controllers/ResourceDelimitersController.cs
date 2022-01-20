using AzNamingTool.Attributes;
using AzNamingTool.Helpers;
using AzNamingTool.Models;
using AzNamingTool.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzNamingTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class ResourceDelimitersController : ControllerBase
    {
        private ServiceResponse serviceResponse = new();
        [HttpGet]
        public async Task<IActionResult> Get(bool admin = false)
        {
            try
            {
                serviceResponse = await ResourceDelimiterService.GetItems(admin);
                if (serviceResponse.Success)
                {
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<ResourceDelimitersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceDelimiter item)
        {
            try
            {
                serviceResponse = await ResourceDelimiterService.PostItem(item);
                if (serviceResponse.Success)
                {
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<resourcedelimitersController>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceDelimiter> items)
        {
            try
            {
                serviceResponse = await ResourceDelimiterService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
