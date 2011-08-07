using System;
using System.Web;
using System.Web.Mvc;
using MediaHost.Domain.Models;
using MediaHost.Domain.Repository;
using MediaHost.Domain.Storage;

namespace MediaHost.Controllers
{
    public class ApiController : BaseController
    {
        private readonly IDbRepository _dbRepository;
        private readonly IStorage _storage;

        public ApiController(IDbRepository entityRepository, IStorage storage)
        {
            _dbRepository = entityRepository;
            _storage = storage;
        }

        public string Index()
        {
            return "";
        }

        public ContentResult AddEntity (Entity entity)
        {
            if(IsValid(entity))
            {
                entity = _dbRepository.Insert(entity);
            }

            return ContentResult(entity);
        }

        public ContentResult AddGroup(Group group)
        {
            if (IsValid(group))
            {
                group = _dbRepository.Insert(group);
            }

            return ContentResult(group);
        }

        public ContentResult AddPlaylist(Playlist playlist)
        {
            if (IsValid(playlist))
            {
                playlist = _dbRepository.Insert(playlist);
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
                mediaFile.ContentLength = file.ContentLength;
                mediaFile.ContentType = file.ContentType;
                mediaFile.FileName = file.FileName;
                
                if (file.ContentType == "video/mp4" || file.ContentType == "audio/mp3")
                {
                    mediaFile.RelativeFilePath = _storage.StoreStreamingFile(file.InputStream, file.ContentType);
                    mediaFile.IsStreaming = true;
                }
                else
                {
                    mediaFile.RelativeFilePath = _storage.StoreFile(file.InputStream);
                    mediaFile.IsStreaming = false;
                }

                mediaFile = _dbRepository.Insert(mediaFile);
            }

            return ContentResult(mediaFile);
        }
    }
}
