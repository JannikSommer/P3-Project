using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Central_Controller
{
    public partial class Controller
    {
        public int TotalNumberOfItems { get; private set; } = 0;
        public int ItemsVerified { get; private set; } = 0;
        private readonly int MaxSizeForPartitions = 20; //MaxSizeForPartitions is a lie, anything adding multiple locations at once can exceed this limit
        private readonly TimeSpan TimeBeforeAFK = new TimeSpan(0, 30, 0);
        public SortedList<string, Location> UnPartitionedLocations { get; private set; } = new SortedList<string, Location>();
        private List<Location> MultiLocationsItem_Locations = new List<Location>();
        //private List<Item> MultilocationItems = new List<Item>();
        private List<Client> Active_Clients = new List<Client>();
        private List<Shelf> AvailebleShelfs = new List<Shelf>();
        private List<Shelf> OccopiedShelfs = new List<Shelf>();
        private List<Item> ItemsForVerification = new List<Item>();
        private List<Item> MultiLocationItemsForVerification = new List<Item>();
        private LocationComparer Location_Comparer;

        /* first Send Next Partition Implimentation
        public Partition SendNextPartition(Client client)
        {
            int Index;

            Index = HasAssignedShelf(client);

            if(Index < 0)
            {
                AssignUserNewShelf(client);
                return NextPartition(OccopiedShelfs.Count - 1, client);
            }
            else
            {
                return NextPartition(Index, client);
            }
        }

        private Partition NextPartition(int index, Client client)
        {
            if(OccopiedShelfs[index].Partitions.Count != 0)
            {
                client.CurrentPartition = OccopiedShelfs[index].Partitions[0];
                OccopiedShelfs[index].Partitions.RemoveAt(0);
                return client.CurrentPartition;
            }
            else
            {
                OccopiedShelfs.RemoveAt(index);
                AssignUserNewShelf(client);

                client.CurrentPartition = OccopiedShelfs[OccopiedShelfs.Count - 1].Partitions[0];
                OccopiedShelfs[OccopiedShelfs.Count - 1].Partitions.RemoveAt(0);
                return client.CurrentPartition;
            }
        }

        private void AssignUserNewShelf(Client client)
        {
            OccopiedShelfs.Add(AvailebleShelfs[0]);
            OccopiedShelfs[OccopiedShelfs.Count - 1].BeingCountedByUser = client;
            AvailebleShelfs.RemoveAt(0);
        }

        private int HasAssignedShelf(Client client)
        {
            int x = 0;

            foreach(Shelf shelf in OccopiedShelfs)
            {
                if(shelf.BeingCountedByUser == client)
                {
                    return x;
                }
                x++;
            }

            return -1;
        }
        */

        public Partition NextPartition(Client client)
        {
            int ClientsIndex = IndexOfClient(client);

            int ShelfsIndex = OccopiedShelfs.FindIndex(x => x.HasClient(client));

            if(ShelfsIndex >= 0) //checks if client has a shelf and if its empty
            {
                if(OccopiedShelfs[ShelfsIndex].Partitions.Count == 0)
                {
                    OccopiedShelfs.RemoveAt(ShelfsIndex);
                    ShelfsIndex = -1;
                }
            }

            if(ShelfsIndex < 0) //if the client didn't have an assigned shelf or if it was empty, assign a new one
            {
                if(AvailebleShelfs.Count != 0) //if there are availeble shelfs, assigns the new client the first in the list
                {
                    MoveElementFromListToOtherList<Shelf>(AvailebleShelfs, 0, OccopiedShelfs);
                    ShelfsIndex = OccopiedShelfs.Count - 1;
                    OccopiedShelfs[OccopiedShelfs.Count - 1].AddClient(client);
                }
                else //if no availeble shelfs assign the client to the shelf with the most room;
                {
                    int LargestGap = -1;
                    
                    for(int Counter = 0; Counter < OccopiedShelfs.Count; Counter++)
                    {
                        if (OccopiedShelfs[Counter].FindLargestGapInPartitions_size() > LargestGap)
                        {
                            LargestGap = OccopiedShelfs[Counter].FindLargestGapInPartitions_size();
                            ShelfsIndex = Counter;
                        }
                    }

                    if(ShelfsIndex < 0) //if there are no more Shelfs
                    {
                        return null;
                    }

                    OccopiedShelfs[ShelfsIndex].AddClient(client);
                }

            }

            return OccopiedShelfs[ShelfsIndex].ClientsNextPartition(client);
        }
        
        private int IndexOfClient(Client client)
        {
            int index = Active_Clients.FindIndex(x => x.ID == client.ID);

            if(index < 0)
            {
                Active_Clients.Add(client);
                index = Active_Clients.Count - 1;
            }

            return index;
        }

        public void InitialAddItem(Item item, List<string> Location_IDs)
        {
            int Index;
            Location NewLocation;

            foreach(string Location_ID in Location_IDs)
            {
                Index = UnPartitionedLocations.IndexOfKey(Location_ID);
                if(Index < 0)
                {
                    NewLocation = new Location(Location_ID);
                    UnPartitionedLocations.Add(Location_ID, NewLocation);
                    item.AddLocation(NewLocation);
                }
                else
                {
                    UnPartitionedLocations.ElementAt(Index).Value.AddItem(item);
                }
            }

            TotalNumberOfItems++;
        }

        private void InitilizeLocationComparer() //THIS MIGHT NEED TO BE REWORKED AND REMOVED, + remember that its called in InitialPartitionUnpartitionedLocations when/if reworking this.
        {
            Location_Comparer = new LocationComparer(UnPartitionedLocations.Values[UnPartitionedLocations.Count - 1].Shelf);
        }

        public void SortMultiLocationItem_Locations()
        {
            for(int x = 0; x < UnPartitionedLocations.Count; x++)
            {
                if (UnPartitionedLocations.Values[x].HasMultilocationItem)
                {
                    MultiLocationsItem_Locations.Add(UnPartitionedLocations.Values[x]);
                    UnPartitionedLocations.RemoveAt(x);
                }
            }
        }

        public void InitialPartitionUnpartitionedLocations()
        {
            int FormerShelf = -1;
            int FormerPosition = -1;
            List<Location> Locations = UnPartitionedLocations.Values.ToList();

            InitilizeLocationComparer(); //THIS MIGHT NEED TO BE REWORKED AND REMOVED
            Locations.Sort(Location_Comparer); //The initial AddItem doesn't need to use a sorted list anymore, because compareing the strings isn't good enough anymore

            if (AvailebleShelfs.Count != 0)
            {
                throw new Exception("AvailebleShelfs has to be empty to partition useing this algorithm");
            }


            foreach(Location location in Locations)
            {
                if(FormerShelf == location.Shelf)
                {
                    if(FormerPosition == location.Position)
                    {
                        //adds location to last shelf in Availebleshelfs lists, last partition in Partition list.
                        AvailebleShelfs.Last().Partitions.Last().AddLocation(location);
                    }
                    else
                    {
                        FormerPosition = location.Position;

                        //adds new partition to last shelf in Availebleshelf list
                        AvailebleShelfs.Last().Partitions.Add(new Partition(location));
                    }
                }
                else
                {
                    FormerShelf = location.Shelf;
                    FormerPosition = location.Position;

                    //adds new shelf to availeble shelf list, and adds new partition to said shelf.
                    AvailebleShelfs.Add(new Shelf(location.Shelf));
                    AvailebleShelfs.Last().Partitions.Add(new Partition(location));
                }
            }

            UnPartitionedLocations.Clear();
        }

        public void AddClient(Client client)
        {
            if (!Active_Clients.Exists(x => x.ID == client.ID))
            {
                Active_Clients.Add(client);
            }
            else
            {
                throw new Exception("Client already exists");
            }
        }

        public void CheckForAFKclients()
        {
            for(int x = 0; x < Active_Clients.Count; x++)
            {
                if (Active_Clients[x].IsAFK(TimeBeforeAFK))
                {
                    RemoveInactiveClient(x);
                }
            }
        }

        public void RemoveInactiveClient(int UsersIndex)
        {
            for(int x = 0; x < OccopiedShelfs.Count; x++)
            {
                if(OccopiedShelfs[x].HasClient(Active_Clients[UsersIndex]))
                {
                    OccopiedShelfs[x].RemoveInactiveClients(Active_Clients[UsersIndex]);

                    if(OccopiedShelfs[x].NumberOfClients == 0)
                    {
                        MoveElementFromListToOtherList(OccopiedShelfs, x, AvailebleShelfs);
                    }
                    
                    break;
                }
            }

            Active_Clients.RemoveAt(UsersIndex);
        }

        private void MoveElementFromListToOtherList<T>(List<T> MoveFromList, int Index, List<T> MoveToList) where T : IComparable
        {
            MoveToList.Add(MoveFromList[Index]);

            MoveToList.Sort();

            MoveFromList.RemoveAt(Index);
        }

        public void CheckCountedItem(Item item) //checks an item returned for a client, to find out if it should be added into normal rotation, to verification rotaion, or if its should be removed
        {
            Shelf shelf;

            if (item.CountedQuantity < 0)
            {
                //add items back to normal count
                foreach(Location location in item.Locations)
                {
                    shelf = FindShelf(AvailebleShelfs, location.Shelf);
                    if (shelf != null)
                    {
                        shelf.AddLocation(location);
                    }

                    shelf = FindShelf(OccopiedShelfs, location.Shelf);
                    if (shelf != null)
                    {
                        shelf.AddLocation(location);
                    }

                    else
                    {
                        AvailebleShelfs.Add(new Shelf(location.Shelf));
                        AvailebleShelfs[AvailebleShelfs.Count - 1].AddLocation(location);
                        AvailebleShelfs.Sort(ShelfSort);
                    }
                }
            }
            else if (item.ServerQuantity != item.CountedQuantity)
            {
                if (item.HasMultiLocation)
                {
                    MultiLocationItemsForVerification.Add(item);
                }
                else
                {
                    ItemsForVerification.Add(item);
                }
            }
            else
            {
                ItemsVerified++;
            }
        }

        public VerificationPartition CreateVerificationPartition()
        {
            VerificationPartition verificationPartition = new VerificationPartition();
            int Distance;
            int ShortestMultiLocDistance; 
            int ShortestSingleLocDistance;
            int IndexOfMultiLoc;
            int IndexOfSingleLoc;

            if (MultiLocationItemsForVerification.Count != 0)
            {
                verificationPartition.AddItem(MultiLocationItemsForVerification[0]);

                MultiLocationItemsForVerification.RemoveAt(0);
            }
            else if(ItemsForVerification.Count != 0)
            {
                verificationPartition.AddItem(ItemsForVerification[0]);

                ItemsForVerification.RemoveAt(0);
            }
            else
            {
                return null;
            }

            while(verificationPartition.Locations.Count < MaxSizeForPartitions && (MultiLocationItemsForVerification.Count != 0 || ItemsForVerification.Count != 0))
            {
                ShortestMultiLocDistance = int.MaxValue;
                IndexOfMultiLoc = -1;

                for(int x = 0; x < MultiLocationItemsForVerification.Count; x++)
                {
                    Distance = verificationPartition.CompareDistance(MultiLocationItemsForVerification[x], Location_Comparer);

                    if(Distance < ShortestMultiLocDistance)
                    {
                        ShortestMultiLocDistance = Distance;
                        IndexOfMultiLoc = x;
                    }
                }

                if(ShortestMultiLocDistance < 1000) //if a MultiLocation item can be checked without Visiting new Shelfs, its given priority over single Location items
                {
                    verificationPartition.AddItem(MultiLocationItemsForVerification[IndexOfMultiLoc]);

                    MultiLocationItemsForVerification.RemoveAt(IndexOfMultiLoc);

                    continue;
                }

                ShortestSingleLocDistance = int.MaxValue;
                IndexOfSingleLoc = -1;

                for (int x = 0; x < ItemsForVerification.Count; x++)
                {
                    Distance = verificationPartition.CompareDistance(ItemsForVerification[x], Location_Comparer);

                    if (Distance < ShortestSingleLocDistance)
                    {
                        ShortestSingleLocDistance = Distance;
                        IndexOfSingleLoc = x;
                    }
                }

                if(ShortestMultiLocDistance / 1000 < ShortestSingleLocDistance / 1000) //if a multilocation item is the same nr of shelfs away as a single location item, its prioritized
                {
                    verificationPartition.AddItem(MultiLocationItemsForVerification[IndexOfMultiLoc]);

                    MultiLocationItemsForVerification.RemoveAt(IndexOfMultiLoc);
                }
                else
                {
                    verificationPartition.AddItem(ItemsForVerification[IndexOfSingleLoc]);

                    ItemsForVerification.RemoveAt(IndexOfSingleLoc);
                }
            }

            verificationPartition.Locations.Sort(Location_Comparer);

            return verificationPartition;
        }

        private Shelf FindShelf(List<Shelf> ShelfList, int Index)
        {
            return ShelfList.Find(x => x.ShelfIndex == Index);
        }

        private int ShelfSort(Shelf a, Shelf b)
        {
            int aHieraky = Location_Comparer.ShelfHierarchyOf(a.ShelfIndex);
            int bHieraky = Location_Comparer.ShelfHierarchyOf(b.ShelfIndex);

            return aHieraky.CompareTo(bHieraky);
        }

        public void InitialPartition_Unpartitioned_MultilocationItemLocations() 
        {
            List<List<Location>> Paths = new List<List<Location>>();
            List<Location> NewList;

            while(MultiLocationsItem_Locations.Count != 0) //All locations connected by items, is turned into a list and then added to Paths
            {
                NewList = new List<Location> {};

                AddConnectedItems(MultiLocationsItem_Locations[0], NewList);

                RemoveLocationInListA_From_ListB(NewList, MultiLocationsItem_Locations);

                NewList.Sort(Location_Comparer);

                Paths.Add(NewList);
            }

            /* NOT IMPLIMENTED */ DivideLargerPaths(Paths); //paths larger then MaxSizeForPartitions is devided into multiple paths

            CombineShorterPaths(Paths); //paths lower then MaxSizeForPartition is combined with others combinable paths. (paths are considered combinable, if all the location in both paths are contained within the same shelves)

            /* NOT IMPLIMENTED */ FillPaths(Paths); //Paths still lower then MaxSizeForPartition is filled with Locations that doesn't have MultiLocationItems
        }

        private void DivideLargerPaths(List<List<Location>> paths)
        {
            throw new NotImplementedException();
        }

        private void FillPaths(List<List<Location>> paths)
        {
            throw new NotImplementedException();
        }

        private void CombineShorterPaths(List<List<Location>> Paths) 
        //combines paths of linked MultiLocationItemLocations containted within Paths, 
        //as long as they don't become longer then MaxSizeForPartitions
        {
            for(int x = 0; x < Paths.Count; x++)
            {
                if (Paths[x].Count < MaxSizeForPartitions - 1)
                {
                    for (int y = x + 1; y < Paths.Count; y++)
                    {
                        if (Paths[x].Count + Paths[y].Count <= MaxSizeForPartitions && PathsIsCombinable(Paths[x], Paths[y]))
                        {
                            foreach (Location location in Paths[y])
                            {
                                Paths[x].Add(location);
                            }

                            Paths.RemoveAt(y);
                            y--;

                            if(Paths[x].Count >= MaxSizeForPartitions - 1)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private bool PathsIsCombinable(List<Location> PathA, List<Location> PathB) 
        //tests 2 list of locations, to test if PathA only contain locations with Shelves also contained in PathB, or vise versa
        {
            bool PathA_HasAShelfOutsideB = false;
            bool LocationAExistInB_Test;
            List<bool> LocationsInBThatExistInA = new List<bool>();

            for(int x = 0; x < PathA.Count; x++ )
            {
                LocationsInBThatExistInA.Add(false);
            }

            foreach (Location LocationInA in PathA)
            {
                LocationAExistInB_Test = false;

                for(int x = 0; x < PathB.Count; x++)
                {
                    if(LocationInA.ID == PathB[x].ID)
                    {
                        LocationAExistInB_Test = true;
                        LocationsInBThatExistInA[x] = true;
                    }
                }

                if (!LocationAExistInB_Test)
                {
                    PathA_HasAShelfOutsideB = true;
                }
            }

            if (!PathA_HasAShelfOutsideB || !(LocationsInBThatExistInA.Exists(x => x == false)))
            {
                return true;
            }

            return false;
        }

        private void AddConnectedItems(Location StartLocation, List<Location> CombinedList) //Turns Locations connected by commen MultiLocationItems into one list
        {
            CombinedList.Add(StartLocation);

            foreach (Item item in StartLocation.Items)
            {
                if (item.HasMultiLocation)
                {
                    foreach(Location location in item.Locations)
                    {
                        if(!CombinedList.Exists(x => x.ID == location.ID))
                        {
                            AddConnectedItems(location, CombinedList);
                        }
                    }
                }
            }
        }

        private void RemoveLocationInListA_From_ListB(List<Location> ListA, List<Location> ListB)
        {
            Location LocationInB;

            for(int n = 0; n < ListB.Count; n++)
            {
                LocationInB = ListB[n];
                
                if(ListA.Exists(x => x.ID == LocationInB.ID))
                {
                    ListB.RemoveAt(n);
                    n--;
                }
            }
        }

        private List<Item> ConvertLocationListToItemList(List<Location> LocationList) //Runs 3 loops
        {
            List<Item> ItemList = new List<Item>();
            
            foreach(Location location in LocationList)
            {
                foreach(Item item in location.Items)
                {
                    if (!ItemList.Exists(x => x == item))
                    {
                        ItemList.Add(item);
                    }
                }
            }

            return ItemList;
        }

        private List<Location> ConvertItemListToLocationList(List<Item> ItemList) //Runs 3 loops
        {
            List<Location> LocationList = new List<Location>();

            foreach(Item item in ItemList)
            {
                foreach(Location location in item.Locations)
                {
                    if(!LocationList.Exists(x => x == location))
                    {
                        LocationList.Add(location);
                    }
                }
            }

            return LocationList;
        }

        /*public void AddItem(Item item, List<string> Location_IDs)
        {
            Location location;
            ShelfSearch shelfSearch = new ShelfSearch(-1);
            int index;

            if (Location_IDs.Count == 1)
            {
                foreach (string Location_ID in Location_IDs)
                {
                    location = new Location(Location_ID, new List<Item> { item });
                    shelfSearch.Index = location.Shelf;
                    index = AvailebleShelfs.FindIndex(shelfSearch.HasIndex);

                    if (index < 0)
                    {
                        AvailebleShelfs.Add(new Shelf(index));

                        AvailebleShelfs[AvailebleShelfs.Count - 1].Partitions.Add(new Partition());

                        AvailebleShelfs[AvailebleShelfs.Count - 1].Partitions[0].AddLocation(location);

                        AvailebleShelfs.Sort();
                    }
                    else
                    {
                        AvailebleShelfs[index].AddLocation(location);
                    }

                }
            }
            //else{ }
        }*/

        /*private char NextLetter(char Letter)
        {
            if (Letter == 'z')
                return 'A';
            else if (Letter == 'Z')
                return 'a';
            else
                return (char) ((int)Letter + 1);
        }*/
    }
}
