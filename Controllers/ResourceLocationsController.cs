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
    public class ResourceLocationsController : ControllerBase
    {
        private ServiceResponse serviceResponse = new();
        // GET: api/<ResourceLocationsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                serviceResponse = await ResourceLocationService.GetItems();
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

        // GET api/<ResourceLocationsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                serviceResponse = await ResourceLocationService.GetItem(id);
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

        //// POST api/<ResourceLocationsController>
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] ResourceLocation item)
        //{
        //    try
        //    {
        //        serviceResponse = await ResourceLocationService.PostItem(item);
        //        if (serviceResponse.Success)
        //        {
        //            return Ok(serviceResponse.ResponseObject);
        //        }
        //        else
        //        {
        //            return BadRequest(serviceResponse.ResponseObject);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        // POST api/<ResourceLocationsController>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceLocation> items)
        {
            try
            {
                serviceResponse = await ResourceLocationService.PostConfig(items);
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

        //// DELETE api/<ResourceLocationsController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        serviceResponse = await ResourceLocationService.DeleteItem(id);
        //        if (serviceResponse.Success)
        //        {
        //            return Ok(serviceResponse.ResponseObject);
        //        }
        //        else
        //        {
        //            return BadRequest(serviceResponse.ResponseObject);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
    }
}
