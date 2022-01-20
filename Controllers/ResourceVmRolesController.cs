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
    public class ResourceVmRolesController : ControllerBase
    {
        private ServiceResponse serviceResponse = new();
        // GET: api/<ResourceVmRolesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Get list of items
                var items = await GeneralHelper.GetList<ResourceVmRole>();

                return Ok(items.OrderBy(x => x.SortOrder));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<ResourceVmRolesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                // Get list of items
                var data = await GeneralHelper.GetList<ResourceVmRole>();
                var item = data.Find(x => x.Id == id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<ResourceVmRolesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceVmRole item)
        {
            try
            {
                serviceResponse = await ResourceVmRoleService.PostItem(item);
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

        // POST api/<ResourceVmRolesController>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceVmRole> items)
        {
            try
            {
                serviceResponse = await ResourceVmRoleService.PostConfig(items);
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

        // DELETE api/<ResourceVmRolesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                serviceResponse = await ResourceVmRoleService.DeleteItem(id);
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