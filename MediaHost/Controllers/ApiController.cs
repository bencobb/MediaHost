using System;
using System.Web;
using System.Web.Mvc;
using MediaHost.Domain.Models;
using MediaHost.Domain.Repository;

namespace MediaHost.Controllers
{
    public class ApiController : BaseController
    {
        private readonly IDbRepository _dbRepository;

        public ApiController(IDbRepository entityRepository)
        {
            _dbRepository = entityRepository;
        }

        public string Index()
        {
            return "";
        }

        public ContentResult AddEntity (Entity entity)
        {
            if(IsValid(entity))
            {
                entity = _dbRepository.InsertEntity(entity);
            }

            return ContentResult(entity);
        }


        public ContentResult AddGroup(Group group)
        {
            if (IsValid(group))
            {
                group = _dbRepository.InsertGroup(group);
            }

            return ContentResult(group);
        }

        public ContentResult AddPlaylist(Playlist playlist)
        {
            if (IsValid(playlist))
            {
                playlist = _dbRepository.InsertPlaylist(playlist);
            }

            return ContentResult(playlist);
        }

        public ContentResult AddFile(MediaFile mediaFile, HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                ModelState.AddModelError("FileUploaded", "Exception: File Upload Required");
            }

            if (IsValid(mediaFile))
            {
                mediaFile = _dbRepository.InsertFile(mediaFile);
            }

            return ContentResult(mediaFile);
        }
    }
}
