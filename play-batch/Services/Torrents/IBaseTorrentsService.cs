using System.Collections.Generic;
using batch.Models;
using batch.Services.Parser;

namespace batch.Services.Torrents
{
    public abstract class ABaseTorrentsService
    {
        protected readonly ParserFacade Parser = ParserFacade.Instance;
        public abstract List<Torrent> GetMovieTorrents();
    }
}
