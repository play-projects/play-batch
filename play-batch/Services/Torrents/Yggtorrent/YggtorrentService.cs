using System;
using System.Collections.Generic;
using batch.Models;
using batch.Services.Torrents;

namespace play.Services.Torrents.Yggtorrent
{
    public class YggtorrentService : ABaseTorrentsService
    {
        private readonly string _url;

        public YggtorrentService(string url)
        {
            _url = url;
        }

        public override List<Torrent> GetMovieTorrents()
        {
            throw new NotImplementedException();
        }
    }
}
