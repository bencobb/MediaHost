using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaHost.Domain.Models;

namespace MediaHost.Domain.Repository
{
    public interface IDbRepository
    {
        //(reason for where T: class) T must be a reference type to use in later generic method calls, i.e. - the SqlMapperExtensions Insert<T>
        T Find<T>(long id) where T : class, IActiveRecord;
        IEnumerable<T> GetAll<T>() where T : class, IActiveRecord;
        T Insert<T>(T record) where T: class, IActiveRecord;
        bool Update<T>(T record) where T : class, IActiveRecord;
        bool Remove<T>(T record) where T : class, IActiveRecord;

        Playlist GetPlaylist(long id);
        IEnumerable<Playlist> GetPlaylists_ByEntity(long entityId, bool includeFiles);
    }
}
