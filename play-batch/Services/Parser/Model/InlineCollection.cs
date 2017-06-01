using System.Collections;
using System.Collections.Generic;

namespace platch.Services.Parser.Model
{
    public class InlineCollection : IEnumerable<InlineModel>
    {
        private readonly List<InlineModel> inlines = new List<InlineModel>();

        public int Count => inlines.Count;
        public bool Success => inlines.Count > 0;

        public InlineModel this[int index]
        {
            get { return inlines[index]; }
            set { inlines.Insert(index, value); }
        }

        public IEnumerator<InlineModel> GetEnumerator()
        {
            return inlines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
