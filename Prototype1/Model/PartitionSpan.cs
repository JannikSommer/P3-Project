using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public struct PartitionSpan
    {
        public int Shelf;
        public int Position;

        public PartitionSpan(int shelf, int position)
        {
            Shelf = shelf;
            Position = position;
        }
    }
}
