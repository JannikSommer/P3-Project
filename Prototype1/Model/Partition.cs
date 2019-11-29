using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public enum PartitionState { NotCounted, Counted, Verified}
    public enum PartitionRequsitionState { Requested, Uploaded, Recieved}

    public class Partition : PartitionBase
    {
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
