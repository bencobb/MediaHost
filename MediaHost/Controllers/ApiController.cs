using System;
using System.Web;
using System.Web.Mvc;
using MediaHost.Domain.Models;
using MediaHost.Domain.Repository;
using MediaHost.Domain.Storage;
using System.Collections.Generic;

namespace MediaHost.Controllers
{
    [Base(Authorize=true)]
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

        #region Playlist
        public ContentResult AddPlaylist(Playlist playlist)
        {
            if (IsValid(playlist))
            {
                playlist = _dbRepository.Insert(playlist);
            }

            return ContentResult(playlist);
        }

        public ContentResult GetPlaylist(long id)
        {
            Playlist playList = _dbRepository.GetPlaylist(id);

            return ContentResult(playList);
        }

        public ContentResult GetPlaylists_ByEntity(long entityId)
        {
            IEnumerable<Playlist> playList = _dbRepository.GetPlaylists_ByEntity(entityId);

            return ContentResult(playList);
        }
        #endregion

        public ContentResult AddFile(MediaFile mediaFile, long playlistId, HttpPostedFileBase file)
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

                if (playlistId != 0)
                {
                    var playlist_mediafile = new Playlist_MediaFile { MediaFileId = mediaFile.Id, PlaylistId = playlistId };
                    _dbRepository.Insert(playlist_mediafile);
                }
            }

            return ContentResult(mediaFile);
        }
    }
}
