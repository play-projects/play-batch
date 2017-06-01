using System.Collections.Generic;

namespace platch.Services.Parser.Model
{
    public class AttributModel
    {
        public readonly string Key;
        public readonly List<string> Values;

        public AttributModel(string key, List<string> values)
        {
            Key = key;
            Values = values;
        }
    }
}
