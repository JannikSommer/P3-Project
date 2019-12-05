using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public enum PartitionState { NotCounted, Counted, Verified}
    public enum PartitionRequsitionState { Requested, Uploaded, Recieved}

    
    public class Partition : IComparable
    {
        public PartitionState State { get; set; }
        public int TotalNrOFItems { get; private set; }
        public int ItemsCounted { get; private set; }
        public bool IsMultiLocationItemPartition { get; private set; } = false;
        public PartitionRequsitionState RequsitionState { get; private set; }
        public List<Location> Locations { get; private set; }
        public PartitionSpan Span { get; private set; } = new PartitionSpan(-1, -1);

        public Partition()
        {
            State = PartitionState.NotCounted;
            RequsitionState = PartitionRequsitionState.Requested;
            TotalNrOFItems = 0;
            ItemsCounted = 0;
            Locations = new List<Location>();
        }

        public Partition(bool _IsMultiLocationItemPartition)
        {
            State = PartitionState.NotCounted;
            RequsitionState = PartitionRequsitionState.Requested;
            TotalNrOFItems = 0;
            ItemsCounted = 0;
            IsMultiLocationItemPartition = _IsMultiLocationItemPartition;
            Locations = new List<Location>();
        }

        public Partition(Location _Location)
        {
            State = PartitionState.NotCounted;
            RequsitionState = PartitionRequsitionState.Requested;
            TotalNrOFItems = 0;
            ItemsCounted = 0;
            Locations = new List<Location>();

            if (_Location.HasMultilocationItem)
            {
                IsMultiLocationItemPartition = true;
            }

            AddLocation(_Location);
        }

        public void AddLocation(Location _Location)
        {
            if (!IsMultiLocationItemPartition)
            {
                if (!_Location.HasMultilocationItem)
                {
                    if (Span.Shelf == -1)
                    {
                        Locations.Add(_Location);
                        TotalNrOFItems += _Location.Items.Count;

                        Span = new PartitionSpan(_Location.Shelf, _Location.Position);
                    }
                    else if (Span.Shelf == _Location.Shelf && Span.Position == _Location.Position)
                    {
                        Locations.Add(_Location);
                        TotalNrOFItems += _Location.Items.Count;
                    }
                    else
                    {
                        throw new Exception("Can't add location outside of Partition.Span");
                    }
                }
                else
                {
                    throw new Exception("Trying to add a location with multiLocationItemsLocation to a non-MultiLocationItemPartition");
                }
            }
            else
            {
                Locations.Add(_Location);
                TotalNrOFItems += _Location.Items.Count; //this will count MultilocationItems as once per location, rather then just once
            }
            
        }

        public int CompareTo(object obj)
        {
            Partition OtherPartition = (Partition) obj;

            int x = Span.Shelf.CompareTo(OtherPartition.Span.Shelf);

            if(x == 0)
            {
                return Span.Position.CompareTo(OtherPartition.Span.Position);
            }

            return x;
        }

        public bool ContainsBarcode(string barcode) {
            if(char.IsLetter(barcode[3])) { 
                foreach(var location in Locations) {
                    if(location.ID == barcode) {
                        return true;
                    }
                }
            } else if(char.IsDigit(barcode[3])) {
                foreach(var location in Locations) {
                    foreach(var item in location.Items) {
                        if(item.CheckSum == barcode) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


    }
}
