using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class VerificationPartition {
        public VerificationPartition() {
            State = PartitionState.NotCounted;
            RequsitionState = PartitionRequsitionState.Requested;
            TotalNrOFItems = 0;
            ItemsCounted = 0;
            Items = new List<Item>();
            Locations = new List<Location>();
        }

        public int TotalNrOFItems { get; set; }
        public int ItemsCounted { get; set; }
        public PartitionState State { get; set; }
        public PartitionRequsitionState RequsitionState { get; set; }
        public List<Item> Items { get; private set; }
        public List<Location> Locations { get; private set; }


        public void AddItem(Item item) {
            int Index;

            if(Items.Exists(x => x.ID == item.ID)) {
                throw new Exception("Item Already exists in this Verification Partition");
            }

            item.CountedQuantity = -1;
            Items.Add(item);
            TotalNrOFItems++;

            foreach (Location location in item.Locations) {
                Index = Locations.FindIndex(x => x.ID == location.ID);

                if(Index < 0) {
                    Locations.Add(new Location(location.ID, new List<Item> { item })); //creates copy locations, with only the items included in this VerificationPartition
                } else {
                    Locations[Index].AddItem(item);
                }
            }
        }

        public int CompareDistance(Item item, LocationComparer locationComparer) {
            int TotalDistance = 0;
            int ShortestDistance;
            int x;

            if (item.Locations.Count == 0) {
                throw new Exception("Can't compare Distance of to an location with no Assigned Locations");
            }
            if(Locations.Count == 0) {
                throw new Exception("Can't compare distance to item when this VerificationPartition doesn't have any assigned Locations yet");
            }

            foreach (Location ItemsLocation in item.Locations) {
                ShortestDistance = int.MaxValue;

                foreach (Location ThisPartitionsLocation in Locations) {
                    x = ThisPartitionsLocation.CompareDistance(ItemsLocation, locationComparer);

                    if (x < ShortestDistance) {
                        ShortestDistance = x;
                    }
                }

                TotalDistance += ShortestDistance;
            }

            return TotalDistance;
        }
    }
}
