using batch.Services.Parser.Controller;
using batch.Services.Parser.Model;

namespace batch.Services.Parser
{
    public class ParserFacade
    {
        public static ParserFacade Instance = new ParserFacade();

        private readonly Block block = new Block();
        private readonly Inline inline = new Inline();

		private ParserFacade() { }

        /// <summary>
        /// Récupère la première occurrence de la balise `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public BlockModel GetTag(string context, string tag)
        {
            return block.Get(context, tag);
        }

        /// <summary>
        /// Récupère l'ensemble des occurrences de la balise `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public BlockCollection GetTags(string context, string tag)
        {
            return block.Gets(context, tag);
        }

        /// <summary>
        /// Récupère la première occurrence de l'`id` de la balise `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public BlockModel GetTagById(string context, string tag, string id)
        {
            return block.GetById(context, tag, id);
        }

        /// <summary>
        /// Récupère les occurrences de l'`id` de la balise `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public BlockCollection GetTagsById(string context, string tag, string id)
        {
            return block.GetsById(context, tag, id);
        }

        /// <summary>
        /// Récupère les occurrences de la `class` de la balise `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <param name="classe"></param>
        /// <returns></returns>
        public BlockCollection GetTagsByClass(string context, string tag, string classe)
        {
            return block.GetsByClass(context, tag, classe);
        }

        /// <summary>
        /// Récupère la première occurence de la balise auto-fermante `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public InlineModel GetSelfClosing(string context, string tag)
        {
            return inline.Get(context, tag);
        }

        /// <summary>
        /// Récupèration des occurrences de la balise auto-fermante `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public InlineCollection GetSelfClosings(string context, string tag)
        {
            return inline.Gets(context, tag);
        }

        /// <summary>
        /// Récupèration de la première occurrence de l'`id` de la balise `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public InlineModel GetSelfClosingById(string context, string tag, string id)
        {
            return inline.GetById(context, tag, id);
        }

        /// <summary>
        /// Récupèration des occurrences de l'`id` de la balise `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public InlineCollection GetSelfClosingsById(string context, string tag, string id)
        {
            return inline.GetsById(context, tag, id);
        }

        /// <summary>
        /// Récupèration des occurrences de la `class` de la balise `tag` dans le `context`.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <param name="classe"></param>
        /// <returns></returns>
        public InlineCollection GetSelfClosingsByClass(string context, string tag, string classe)
        {
            return inline.GetsByClass(context, tag, classe);
        }
    }
}
