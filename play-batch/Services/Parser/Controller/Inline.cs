using batch.Services.Parser.Model;

namespace batch.Services.Parser.Controller
{
    class Inline
    {
        private readonly Tags Tags = new Tags();
        private readonly Attributs Attributs = new Attributs();

        public InlineModel Get(string str, string tag)
        {
            var tagMatch = Tags.GetTagPos(tag, str, true);
            if (tagMatch.Success)
            {
                var index = tagMatch.Index;
                var attributes = Attributs.GetAttributs(tagMatch.Value);
                var value = tagMatch.Value;
                return new InlineModel(index, attributes, value);
            }
            return new InlineModel();
        }

        public InlineCollection Gets(string str, string tag)
        {
            var inlines = new InlineCollection();
            var tagMatches = Tags.GetTagsPos(tag, str, true);
            foreach (var match in tagMatches)
            {
                var sub = str.Substring(match.Index);
                inlines[inlines.Count] = Get(sub, tag);
            }
            return inlines;
        }

        public InlineModel GetById(string str, string tag, string id)
        {
            var tagMatch = Tags.GetTagPos(tag, Type.Id, id, str, true);
            if (!tagMatch.Success) return new InlineModel();
            var sub = str.Substring(tagMatch.Index);
            return Get(sub, tag);
        }

        public InlineCollection GetsById(string str, string tag, string id)
        {
            var inlines = new InlineCollection();
            var tagMatches = Tags.GetTagsPos(tag, Type.Id, id, str, true);
            foreach (var match in tagMatches)
            {
                var sub = str.Substring(match.Index);
                inlines[inlines.Count] = Get(sub, tag);
            }
            return inlines;
        }

        public InlineCollection GetsByClass(string str, string tag, string classe)
        {
            var inlines = new InlineCollection();
            var tagMatches = Tags.GetTagsPos(tag, Type.Class, classe, str, true);
            foreach (var match in tagMatches)
            {
                var sub = str.Substring(match.Index);
                inlines[inlines.Count] = Get(sub, tag);
            }
            return inlines;
        }
    }
}
