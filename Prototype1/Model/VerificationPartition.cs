using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class VerificationPartition : PartitionBase
    {
        public List<Item> Items { get; private set; }

        public VerificationPartition()
        {
            State = PartitionState.NotCounted;
            RequsitionState = PartitionRequsitionState.Requested;
            TotalNrOFItems = 0;
            ItemsCounted = 0;
            Items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
            TotalNrOFItems++;
        }
    }
}
