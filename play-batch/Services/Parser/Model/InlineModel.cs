using System.Collections.Generic;

namespace platch.Services.Parser.Model
{
    public class InlineModel
    {
        public readonly int Index;
        public readonly List<AttributModel> Attributes;
        public readonly string Value;
        public int Length => Value.Length;
        public readonly bool Success;

        public InlineModel()
        {
            Index = 0;
            Attributes = new List<AttributModel>();
            Value = string.Empty;
            Success = false;
        }

        public InlineModel(int index, List<AttributModel> attributes, string value)
        {
            Index = index;
            Attributes = attributes;
            Value = value;
            Success = true;
        }
    }
}
