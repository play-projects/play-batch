using System.Text.RegularExpressions;
using batch.Services.Parser.Model;

namespace batch.Services.Parser.Controller
{
    class Block
    {
        private readonly Tags Tags = new Tags();
        private readonly Attributs Attributs = new Attributs();

        public BlockModel Get(string str, string tag, int? idx = null)
        {
            var tagMatch = Tags.GetTagPos(tag, str);
            if (!tagMatch.Success) return new BlockModel();

            var count = 1;
            var start = tagMatch.Index + tagMatch.Length;
            var content = tagMatch.Length;

            while (count > 0)
            {
                var sub = str.Substring(start);
                var rStart = new Regex($"<{tag}(\"[^\"]*\"+|'[^']*'+|[^>\"']*)*>").Match(sub);
                var rEnd = new Regex($"</{tag}>").Match(sub);

                if (rStart.Index < rEnd.Index && rStart.Success && rEnd.Success)
                {
                    start += rStart.Index + rStart.Length;
                    count += 1;
                    content += rStart.Index + rStart.Length;
                }
                else
                {
                    start += rEnd.Index + rEnd.Length;
                    count -= 1;
                    content += rEnd.Index + rEnd.Length;
                }
            }

            var attributes = Attributs.GetAttributs(tagMatch.Value);
            var value = str.Substring(tagMatch.Index, content);
            var index = idx ?? tagMatch.Index;
            var length = value.Length;
            var text = str.Substring(tagMatch.Index + tagMatch.Length, length - tagMatch.Length - 3 - tag.Length);
            return new BlockModel(index, attributes, value, text);
        }

        public BlockCollection Gets(string str, string tag)
        {
            var blocks = new BlockCollection();
            var tagMatches = Tags.GetTagsPos(tag, str);
            foreach (var match in tagMatches)
            {
                var sub = str.Substring(match.Index);
                blocks[blocks.Count] = Get(sub, tag, match.Index);
            }
            return blocks;
        }

        public BlockModel GetById(string str, string tag, string id)
        {
            var tagMatch = Tags.GetTagPos(tag, Type.Id, id, str);
            if (!tagMatch.Success) return new BlockModel();
            var sub = str.Substring(tagMatch.Index);
            return Get(sub, tag, tagMatch.Index);
        }

        public BlockCollection GetsById(string str, string tag, string id)
        {
            var blocks = new BlockCollection();
            var tagMatches = Tags.GetTagsPos(tag, Type.Id, id, str);
            foreach (var match in tagMatches)
            {
                var sub = str.Substring(match.Index);
                blocks[blocks.Count] = Get(sub, tag, match.Index);
            }
            return blocks;
        }

        public BlockCollection GetsByClass(string str, string tag, string classe)
        {
            var blocks = new BlockCollection();
            var tagMatches = Tags.GetTagsPos(tag, Type.Class, classe, str);
            foreach (var match in tagMatches)
            {
                var sub = str.Substring(match.Index);
                blocks[blocks.Count] = Get(sub, tag, match.Index);
            }
            return blocks;
        }
    }
}
