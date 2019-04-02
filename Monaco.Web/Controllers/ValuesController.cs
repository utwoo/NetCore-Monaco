﻿using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monaco.Data.Core.DbContexts;
using Monaco.Data.Core.Repositories;
using Monaco.Data.Test;
using Monaco.Web.Models;

namespace Monaco.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IRepository<Class> _class;
        private readonly ILogger<ValuesController> _logger;
        private readonly MonacoDbContext _context;
        private readonly IMapper _mapper;

        public ValuesController(
        IRepository<Class> @class,
        ILogger<ValuesController> logger,
        MonacoDbContext context,
        IMapper mapper)
        {
            this._class = @class;
            this._logger = logger;
            this._context = context;
            this._mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.LogInformation("ValuesController-GET");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<SampleDTO> Get(int id)
        {
            var sample = new Sample() { Value = 11 };
            return _mapper.Map<Sample, SampleDTO>(sample);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
