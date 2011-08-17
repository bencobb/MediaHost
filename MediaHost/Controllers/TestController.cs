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
            var model = new ViewModel.MediaFileView.Add();
            model.EntityId = 35;
            model.Playlists = _dbRepository.GetPlaylists_ByEntity(model.EntityId);

            return View(model);
        }

        public ActionResult ShowFiles(int id)
        {
            var data = _dbRepository.Find<MediaFile>(id);
            
            return View(data);
        }
    }
}
