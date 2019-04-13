using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monaco.Core.Caching;
using Monaco.Core.EventPublisher;
using Monaco.Data.Core.DbContexts;
using Monaco.Data.Core.Repository;
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
        private readonly IEventPublisher _eventPublisher;
        private readonly IRemoteCacheManager _cacheManager;
        private readonly ILockManager _lockManager;

        public ValuesController(
        IRepository<Class> @class,
        ILogger<ValuesController> logger,
        MonacoDbContext context,
        IMapper mapper,
        IEventPublisher eventPublisher,
        IRemoteCacheManager cacheManager,
        ILockManager lockManager)
        {
            this._class = @class;
            this._logger = logger;
            this._context = context;
            this._mapper = mapper;
            this._eventPublisher = eventPublisher;
            this._cacheManager = cacheManager;
            this._lockManager = lockManager;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            using (TransactionScope trans = new TransactionScope())
            {
                //var result = this._class.Insert(new Class { Name = "11", ClassNo = 1, GradeNo = 1 });
                trans.Complete();
            }

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<SampleDTO> Get(int id)
        {
            using (_lockManager.AcquireLock("temp", TimeSpan.FromSeconds(30)))
            {
                Console.WriteLine("111");
                //Thread.Sleep(15 * 1000);
            }

            var sample = new Sample() { Value = 11 };
            //this._eventPublisher.Publish(sample);

            //_logger.LogWarning("{@sample}", sample);
            _cacheManager.SetAsync<Sample>("sample", sample, 60);
            return _mapper.Map<Sample, SampleDTO>(_cacheManager.GetAsync("sample", () => Task.FromResult(new Sample())).Result);
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
