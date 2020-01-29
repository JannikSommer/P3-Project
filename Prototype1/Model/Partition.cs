using System;
using System.Collections.Generic;

namespace Model {
    public enum PartitionState { NotCounted, Counted, Verified}
    public enum PartitionRequsitionState { Requested, Uploaded, Recieved}

    
    public class Partition : IComparable {
        public Partition() {
            State = PartitionState.NotCounted;
            RequsitionState = PartitionRequsitionState.Requested;
            ItemsCounted = 0;
            Locations = new List<Location>();
        }
        public Partition(bool _IsMultiLocationItemPartition) : this() {
            IsMultiLocationItemPartition = _IsMultiLocationItemPartition;
        }
        public Partition(Location _Location)  {
            State = PartitionState.NotCounted;
            RequsitionState = PartitionRequsitionState.Requested;
            ItemsCounted = 0;
            Locations = new List<Location>();
            IsMultiLocationItemPartition = _Location.HasMultilocationItem;
            AddLocation(_Location);
        }

        public PartitionState State { get; set; }
        public int ItemsCounted { get; set; }
        public bool IsMultiLocationItemPartition { get; set; } = false;
        public PartitionRequsitionState RequsitionState { get; set; }
        public List<Location> Locations { get; set; }
        public PartitionSpan Span { get; set; } = new PartitionSpan(-1, -1);
        public string AssignedUser = "none";


        public void AddLocation(Location _Location) {
            if (!Locations.Exists(x => x.ID == _Location.ID)) {
                if (!IsMultiLocationItemPartition) {
                    if (!_Location.HasMultilocationItem) {
                        if (Span.Shelf == -1) {
                            Locations.Add(_Location);
                            Span = new PartitionSpan(_Location.Shelf, _Location.Position);
                        } else if (Span.Shelf == _Location.Shelf && Span.Position == _Location.Position) {
                            Locations.Add(_Location);
                        } else {
                            throw new Exception("Can't add location outside of Partition.Span");
                        }
                    } else {
                        throw new Exception("Trying to add a location with multiLocationItemsLocation to a non-MultiLocationItemPartition");
                    }
                } else {
                    Locations.Add(_Location);
                }
            }
        }

        public int CompareTo(object obj) {
            Partition OtherPartition = (Partition) obj;
            int x = Span.Shelf.CompareTo(OtherPartition.Span.Shelf);

            if(x == 0) {
                return Span.Position.CompareTo(OtherPartition.Span.Position);
            }

            return x;
        }
    }
}
