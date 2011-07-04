using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaHost.Domain.Models;

namespace MediaHost.Domain.Repository
{
    public interface IDbRepository
    {
        Entity InsertEntity(Entity entity);
        Group InsertGroup (Group group);
        Playlist InsertPlaylist(Playlist playlist);
        MediaFile InsertFile(MediaFile file);
    }
}
