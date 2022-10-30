using Application.Constants;
using MailingFB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMailingService mailingService;

        public TestController(IMailingService MailingService)
        {
            mailingService = MailingService;
        }

        [HttpGet("SendMail")]
        [AllowAnonymous]
        public async Task  SendMail()
        {
            try
            {
                await mailingService.SendEmailAsync("Faouzi.Benamor@mssolutions-group.com", "test", "test");

            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
        }

        // GET: api/<TestController>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        [Authorize(Permissions.Products.View)]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestController>
        [HttpPost]
        [Authorize(Permissions.Roles.Create)]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        [Authorize(Permissions.Products.Edit)]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        [Authorize(Permissions.Products.Delete)]
        public void Delete(int id)
        {
        }
    }
}
