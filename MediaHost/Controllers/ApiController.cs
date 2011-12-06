using System.Collections.Generic;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using MediaHost.Domain.Models;
using MediaHost.Domain.Repository;
using MediaHost.Domain.Storage;

namespace MediaHost.Controllers
{
    [Base(Authorize = true)]
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

        public ContentResult AddEntity(Entity entity)
        {
            if (IsValid(entity))
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
                if (playlist.Id == 0)
                    playlist = _dbRepository.Insert(playlist);
                else
                {
                    if (_dbRepository.Update(playlist))
                    {
                        playlist = _dbRepository.GetPlaylist(playlist.Id);
                    }
                }
            }

            return ContentResult(playlist);
        }

        public ContentResult DeletePlaylist(long id)
        {
            bool success = false;

            Playlist playlist = _dbRepository.GetPlaylist(id);

            if (playlist != null)
            {
                success = _dbRepository.Remove(playlist);
            }

            return ContentResult(success);
        }

        public ContentResult GetPlaylist(long id)
        {
            Playlist playList = _dbRepository.GetPlaylist(id);

            return ContentResult(playList);
        }

        public ContentResult GetPlaylists_ByEntity(long entityId, bool includeFile = true)
        {
            IEnumerable<Playlist> playLists = _dbRepository.GetPlaylists_ByEntity(entityId, includeFile);

            foreach (var playlist in playLists)
            {
                if (playlist.Files != null)
                {
                    foreach (var file in playlist.Files)
                    {
                        file.TemporaryUrl = _storage.GetFileUrl(file.RelativeFilePath, true);
                    }
                }
            }

            return ContentResult(playLists);
        }

        public ContentResult GetPlaylists_ByPlaylistType(long entityId, int type)
        {
            IEnumerable<Playlist> playLists = _dbRepository.GetPlaylists_ByPlaylistType(entityId, type);
            return ContentResult(playLists);
        }

        public ContentResult GetPlaylists_ByName(long entityId, string name)
        {
            IEnumerable<Playlist> playLists = _dbRepository.GetPlaylists_ByName(entityId, name);
            return ContentResult(playLists);
        }

        #endregion Playlist

        #region Playlist File

        public ContentResult RemoveFile(long id)
        {
            var file = _dbRepository.Find<MediaFile>(id);
            bool success = false;

            if (file != null)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    success = _dbRepository.Remove(file);

                    string fileUrl = file.RelativeFilePath;

                    if (success)
                    {
                        success = _storage.RemoveFile(fileUrl);
                    }

                    if (success)
                    {
                        scope.Complete();
                    }
                }
            }

            return ContentResult(success);
        }

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

                if (file.ContentType == "video/mp4" || file.ContentType == "audio/mp3" || file.ContentType == "application/octet-stream")
                {
                    mediaFile.IsStreaming = true;
                }
                else
                {
                    mediaFile.IsStreaming = false;
                }

                mediaFile.RelativeFilePath = _storage.StoreFile(file.InputStream, file.ContentType);

                mediaFile = _dbRepository.Insert(mediaFile);
                //ms.Close();

                if (playlistId != 0)
                {
                    var playlist_mediafile = new Playlist_MediaFile { MediaFileId = mediaFile.Id, PlaylistId = playlistId };
                    _dbRepository.Insert(playlist_mediafile);
                }
            }

            return ContentResult(mediaFile);
        }

        public ContentResult GetFile(long id)
        {
            MediaFile mediaFile = _dbRepository.GetMediaFile(id);
            return ContentResult(mediaFile);
        }

        public ContentResult GetPlaylistFiles(long playlistId, Pager pager, string search)
        {
            Playlist playList = _dbRepository.GetPlaylistFiles(playlistId, pager, search);
            return ContentResult(playList);
        }

        public ContentResult GetTotalPlaylistFiles(long playlistId, string search)
        {
            int total = _dbRepository.GetTotalPlaylistFiles(playlistId, search);
            return ContentResult(total);
        }

        public string StreamingServer()
        {
            return MediaHost.Domain.Helper.AppConfig.StreamingServer;
        }

        #endregion Playlist File
    }
}