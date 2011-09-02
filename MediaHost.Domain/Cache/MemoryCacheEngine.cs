using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using MediaHost.Domain.Repository;
using StructureMap;
using MediaHost.Domain.Models;

namespace MediaHost.Domain.Cache
{
    public class MemoryCacheEngine
    {
        private static readonly IDbRepository _dbRepository = ObjectFactory.GetInstance<IDbRepository>();

        public static void CacheAll()
        {
            MemoryCache.Default.Dispose();

            ObjectCache cache = MemoryCache.Default;
            
            var root = new Root();

            root.Entities = _dbRepository.GetAll<Entity>();
            root.Groups = _dbRepository.GetAll<Group>();
            root.MediaFiles = _dbRepository.GetAll<MediaFile>();
            root.Playlists = _dbRepository.GetAll<Playlist>();
            root.Playlists_MediaFiles = _dbRepository.GetAll<Playlist_MediaFile>();

            cache["Root"] = root;
        }
    }
}
