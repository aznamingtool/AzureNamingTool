using AzNamingTool.Models;
using AzNamingTool.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AzNamingTool.Services;
using AzNamingTool.Attributes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzNamingTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class ImportExportController : ControllerBase
    {
        private ServiceResponse serviceResponse = new();
        // GET: api/<ImportExportController>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ExportConfiguration()
        {
            try
            {
                serviceResponse = await ImportExportService.ExportConfig();
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

        // POST api/<ImportExportController>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ImportConfiguration([FromBody] ConfigurationData configdata)
        {
            try
            {
                serviceResponse = await ImportExportService.PostConfig(configdata);
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
    }
}
