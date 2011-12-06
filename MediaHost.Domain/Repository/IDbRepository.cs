using System.Collections.Generic;
using MediaHost.Domain.Models;

namespace MediaHost.Domain.Repository
{
    public interface IDbRepository
    {
        //(reason for where T: class) T must be a reference type to use in later generic method calls, i.e. - the SqlMapperExtensions Insert<T>
        T Find<T>(long id) where T : class, IActiveRecord;

        IEnumerable<T> GetAll<T>() where T : class, IActiveRecord;

        IEnumerable<T> GetAll<T>(out int recordCount, string where = "", dynamic param = null, int page = 0, int pagesize = 0, string orderBy = "", bool isOrderByAsc = true) where T : class, IActiveRecord;

        T Insert<T>(T record) where T : class, IActiveRecord;

        bool Update<T>(T record) where T : class, IActiveRecord;

        bool Remove<T>(T record) where T : class, IActiveRecord;

        Playlist GetPlaylist(long id);

        IEnumerable<Playlist> GetPlaylists_ByEntity(long entityId, bool includeFiles);

        IEnumerable<Playlist> GetPlaylists_ByPlaylistType(long entityId, int type);

        IEnumerable<Playlist> GetPlaylists_ByName(long entityId, string name);

        MediaFile GetMediaFile(long id);

        Playlist GetPlaylistFiles(long playlistId, Pager pager, string search = "");

        int GetTotalPlaylistFiles(long playlistId, string search);
    }
}