using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public struct PartitionSpan {
        public PartitionSpan(int shelf, int position) {
            Shelf = shelf;
            Position = position;
        }

        public int Shelf;
        public int Position;
    }
}
