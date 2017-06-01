using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace batch.Services.Parser.Model
{
    public class BlockModel: InlineModel
    {
		public readonly string Text;

        public BlockModel()
        {
            Text = string.Empty;
        }

        public BlockModel(int index, List<AttributModel> attributes, string value, string text): base(index, attributes, value)
        {
            Text = text;
        }

        public string GetText()
        {
            var pattern = "<(\"+[^\"]*\"+|'+[^']*'+|[^>\"']*)*>";
            var regex = new Regex(pattern);
            return regex.Replace(Text, string.Empty).Trim();
        }
    }
}
