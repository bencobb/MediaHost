using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MediaHost.Domain.Models;
using MediaHost.Domain.Repository;
using MediaHost.Domain.Storage;

namespace MediaHost.Controllers
{
    [Base]
    public class TestController : BaseController
    {
        //
        // GET: /Test/
        private readonly IDbRepository _dbRepository;
        private readonly IStorage _storage;

        public TestController(IDbRepository entityRepository, IStorage storage)
        {
            _dbRepository = entityRepository;
            _storage = storage;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowFiles(int id)
        {
            var data = _dbRepository.Find<MediaFile>(id);
            
            return View(data);
        }
    }
}
