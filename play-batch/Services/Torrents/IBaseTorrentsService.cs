using System.Collections.Generic;
using batch.Models;
using batch.Services.Parser;

namespace batch.Services.Torrents
{
    public abstract class ABaseTorrentsService
    {
        protected readonly ParserFacade parser = ParserFacade.Instance;
        protected abstract List<Torrent> GetSearch(string url);
    }
}
