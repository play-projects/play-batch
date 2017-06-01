using System.Collections;
using System.Collections.Generic;

namespace batch.Services.Parser.Model
{
    public class BlockCollection : IEnumerable<BlockModel>
    {
        private readonly List<BlockModel> blocks = new List<BlockModel>();

        public int Count => blocks.Count;
        public bool Success => blocks.Count > 0;

        public BlockModel this[int index]
        {
            get { return blocks[index]; }
            set { blocks.Insert(index, value); }
        }

        public IEnumerator<BlockModel> GetEnumerator()
        {
            return blocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
