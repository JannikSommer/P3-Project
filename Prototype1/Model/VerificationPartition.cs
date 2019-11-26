using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class VerificationPartition
    {
        public PartitionState State { get; private set; }
        public PartitionRequsitionState RequsitionState { get; private set; }
        public int TotalNrOFItems { get; private set; }
        public int ItemsCounted { get; private set; }
        public List<Item> Items { get; private set; }
        public List<Location> Locations { get; private set; }

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
            int Index;

            foreach(Location location in item.Locations)
            {
                Index = Locations.FindIndex(x => x.ID == location.ID);

                if(Index < 0)
                {
                    Locations.Add(new Location(location.ID, new List<Item> { item })); //creates copy locations, with only the items included in this VerificationPartition
                }
                else
                {
                    Locations[Index].AddItem(item);
                }
            }
        }

        public int CompareDistance(Item item, LocationComparer locationComparer)
        {
            int TotalDistance = 0;
            int ShortestDistance;
            int x;

            if (item.Locations.Count == 0)
            {
                throw new Exception("Can't compare Distance of to an location with no Assigned Locations");
            }

            if(Locations.Count == 0)
            {
                throw new Exception("Can't compare distance to item when this VerificationPartition doesn't have any assigned Locations yet");
            }

            foreach (Location ThisPartitionsLocation in Locations)
            {
                ShortestDistance = int.MaxValue;

                foreach (Location ItemsLocation in item.Locations)
                {
                    x = ThisPartitionsLocation.CompareDistance(ItemsLocation, locationComparer);

                    if (x < ShortestDistance)
                    {
                        ShortestDistance = x;
                    }
                }

                TotalDistance += ShortestDistance;
            }

            return TotalDistance;
        }
    }
}
