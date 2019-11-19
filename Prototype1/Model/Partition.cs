using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public enum PartitionState { NotCounted, Counted, Verified}
    public enum PartitionRequsitionState { Requested, Uploaded, Recieved}

    public class Partition
    {
        public PartitionState State { get; private set; }
        public int TotalNrOFItems { get; private set; }
        public int ItemsCounted { get; private set; }
        public PartitionRequsitionState RequsitionState { get; private set; }
        public List<Location> Locations { get; private set; }

        public Partition()
        {
            State = PartitionState.NotCounted;
            RequsitionState = PartitionRequsitionState.Requested;
            TotalNrOFItems = 0;
            ItemsCounted = 0;
            Locations = new List<Location>();
        }

        public void AddLocation(Location _Location)
        {
            Locations.Add(_Location);
            TotalNrOFItems += _Location.Items.Count;
        }
    }
}
