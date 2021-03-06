﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Central_Controller.IO;
using PrestaSharpAPI;
using System.ComponentModel;
using System.Threading;
using Model.Log;
using Newtonsoft.Json;
using System.IO;

namespace Central_Controller.Central_Controller {
    public partial class Controller : INotifyPropertyChanged {

        public Controller() {
            IOController io = new IOController();
            List<Item> items = DownloadFromServer();
            Cycle = io.LoadCycle(items);
            Location_Comparer = new LocationComparer(io.LoadShelves());
            foreach (Item item in Cycle.AllItems)
            {
                InitialAddItem(item, LocationListToStringList(item.Locations));
            }
        }

        public Cycle Cycle { get; set; } = new Cycle();
        public event PropertyChangedEventHandler PropertyChanged;
        public int NumberOfActiveUsers {
            get {
                return _numberOfActiveUsers;
            }
            set {
                _numberOfActiveUsers = value;
                OnPropertyChanged("NumberOfActiveUsers");
            }
        }

        private int _numberOfActiveUsers = 0;

        public int TotalNumberOfItems { get; private set; } = 0;
        public int NumOfItemsVerified { get; private set; } = 0;
        public int MaxSizeForPartitions = 20; //MaxSizeForPartitions is a lie, anything adding multiple locations at once can exceed this limit
        public TimeSpan TimeBeforeAFK = new TimeSpan(0, 30, 0);
        public SortedList<string, Location> UnPartitionedLocations { get; private set; } = new SortedList<string, Location>();
        private List<Location> MultiLocationsItem_Locations = new List<Location>();
        public List<User> ActiveUsers { get; private set; } = new List<User>();
        private List<Shelf> AvailebleShelfs = new List<Shelf>();
        private List<Shelf> OccopiedShelfs = new List<Shelf>();
        private List<Item> ItemsForVerification = new List<Item>();
        private List<Item> MultiLocationItemsForVerification = new List<Item>();
        public LocationComparer Location_Comparer = new LocationComparer(19);
        public List<List<Partition>> MultiLocationPartitions { get; private set; } = new List<List<Partition>>();
        public List<Partition> PriorityPartitions { get; private set; } = new List<Partition>();
        public List<Tuple<Item, bool[]>> PartiallyCountedItems { get; private set; } = new List<Tuple<Item, bool[]>>();
        public List<Item> VerifiedItems = new List<Item>();
        private ProductAPI _productAPI = new ProductAPI();
        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private List<Item> DownloadFromServer() {
            var path = Environment.CurrentDirectory + @"\SaveData\APIData.txt";
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Item>>(json, Settings);
            return _productAPI.GetAllItems();
        }

        public void InitialAddItems(List<Item> allItems, List<string> locationIds) {
            foreach(Item item in allItems) {
                InitialAddItem(item, locationIds);
            }
        }
        
        private List<string> LocationListToStringList(List<Location> LocationList)
        {
            List<string> StringList = new List<string>();
            foreach (Location _location in LocationList)
            {
                StringList.Add(_location.ID);
            }
            return StringList;
        }

        public Partition NextPartition(User user)
        {
            Partition ClientsNextPartition = null;
            User existingUser;

            int userIndex = ActiveUsers.FindIndex(x => x.ID == user.ID);

            if(userIndex >= 0) {
                existingUser = ActiveUsers[userIndex];
            } else {
                ActiveUsers.Add(user);
                NumberOfActiveUsers++;
                existingUser = ActiveUsers.Last();
            }

            if(PriorityPartitions.Count != 0)
            {
                ClientsNextPartition = PriorityPartitions[0];

                existingUser.CurrentPartition = PriorityPartitions[0];

                PriorityPartitions.RemoveAt(0);
            }

            if(ClientsNextPartition == null)
            {
                ClientsNextPartition = NextMultiLocationPartition(existingUser);
            }

            if(ClientsNextPartition == null)
            {
                ClientsNextPartition = NextSingleLocationPartition(existingUser);
            }

            if(ClientsNextPartition == null && MultiLocationPartitions.Count != 0)
            {
                ClientsNextPartition = MultiLocationPartitions.Last()[0];

                MultiLocationPartitions.Last().RemoveAt(0);

                if(MultiLocationPartitions.Last().Count != 0)
                {
                    PriorityPartitions = MultiLocationPartitions.Last();
                }
                else
                {
                    MultiLocationPartitions.RemoveAt(MultiLocationPartitions.Count - 1);
                }

                existingUser.CurrentPartition = ClientsNextPartition;
            }

            if(ClientsNextPartition != null)
            {
                ClientsNextPartition.AssignedUser = user.ID;
            }

            return ClientsNextPartition;
        }

        private Partition NextMultiLocationPartition(User client)
        {
            Partition partition = null;

            for(int x = 0; x < MultiLocationPartitions.Count; x++)
            {
                if(MultiLocationPartitions[x].Count <= ActiveUsers.Count)
                {
                    partition = MultiLocationPartitions[x][0];

                    MultiLocationPartitions[x].RemoveAt(0);

                    if (MultiLocationPartitions[x].Count != 0)
                    {
                        PriorityPartitions = MultiLocationPartitions[x];
                    }

                    MultiLocationPartitions.RemoveAt(x);

                    client.CurrentPartition = partition;

                    break;
                }
            }

            return partition;
        }

        private Partition NextSingleLocationPartition(User client)
        {
            int ClientsIndex = IndexOfClient(client);

            int ShelfsIndex = OccopiedShelfs.FindIndex(x => x.HasUsers(client));

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
                    OccopiedShelfs[OccopiedShelfs.Count - 1].AddUser(client);
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

                    OccopiedShelfs[ShelfsIndex].AddUser(client);
                }

            }

            return OccopiedShelfs[ShelfsIndex].UsersNextPartition(client);
        }

        private int IndexOfClient(User client)
        {
            int index = ActiveUsers.FindIndex(x => x.ID == client.ID);

            if(index < 0)
            {
                ActiveUsers.Add(client);
                index = ActiveUsers.Count - 1;
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

        public void InitilizeLocationComparer(int[] ShelfHierachy)
        {
            Location_Comparer = new LocationComparer(ShelfHierachy);
        }

        private void SortMultiLocationItem_Locations()
        {
            for(int x = 0; x < UnPartitionedLocations.Count; x++)
            {
                if (UnPartitionedLocations.Values[x].HasMultilocationItem)
                {
                    MultiLocationsItem_Locations.Add(UnPartitionedLocations.Values[x]);
                    UnPartitionedLocations.RemoveAt(x);
                    x--;
                }
            }
        }

        public void InitialPartitioningOfLocations()
        {
            SortMultiLocationItem_Locations();

            InitialPartitioningOfSingleLocationItems();

            InitialPartitioningOfMultiLocationItems();
        }

        private void InitialPartitioningOfSingleLocationItems()
        {
            int FormerShelf = -1;
            int FormerPosition = -1;
            List<Location> Locations = UnPartitionedLocations.Values.ToList();

            Locations.Sort(Location_Comparer); //Needs to sort incase the shelf hieraky isn't following regular numeric sorting.

            if (AvailebleShelfs.Count != 0)
            {
                throw new Exception("AvailebleShelfs has to be empty to partition useing this algorithm");
            }


            foreach(Location location in Locations) //Sorts the locations into Seperate shelves, with partitions representing a collum on said shelf
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

        public void AddUser(User user)
        {
            if (!ActiveUsers.Exists(x => x.ID == user.ID))
            {
                ActiveUsers.Add(user);
            }
            else
            {
                throw new Exception("Client already exists");
            }
        }

        public void CheckForAFKclients()
        {
            for(int x = 0; x < ActiveUsers.Count; x++)
            {
                if (ActiveUsers[x].IsAFK(TimeBeforeAFK))
                {
                    RemoveUser(x);
                }
            }
        }

        public void RemoveUser(int UsersIndex)
        {
            if (ActiveUsers[UsersIndex].CurrentPartition != null)
            {
                bool ClientFoundOnActiveShelf = false;

                for (int x = 0; x < OccopiedShelfs.Count; x++)
                {
                    if (OccopiedShelfs[x].HasUsers(ActiveUsers[UsersIndex]))
                    {
                        ClientFoundOnActiveShelf = true;
                        OccopiedShelfs[x].RemoveInactiveUsers(ActiveUsers[UsersIndex]);

                        if (OccopiedShelfs[x].NumberOfClients == 0)
                        {
                            MoveElementFromListToOtherList(OccopiedShelfs, x, AvailebleShelfs);
                        }

                        break;
                    }
                }

                if (!ClientFoundOnActiveShelf)
                {
                    CheckPartition(ActiveUsers[UsersIndex].CurrentPartition);
                }
            }

            if(!(ActiveUsers[UsersIndex].CurrentVerificationPartition == null))
            {
                CheckVerificationPartition(ActiveUsers[UsersIndex].CurrentVerificationPartition);
            }

            ActiveUsers.RemoveAt(UsersIndex);
            NumberOfActiveUsers--;
        }

        private void MoveElementFromListToOtherList<T>(List<T> MoveFromList, int Index, List<T> MoveToList) where T : IComparable
        {
            MoveToList.Add(MoveFromList[Index]);

            MoveToList.Sort();

            MoveFromList.RemoveAt(Index);
        }

        public void CheckVerificationPartition(VerificationPartition verificationPartition)
        {
            foreach(Item item in verificationPartition.Items)
            {
                Cycle.Log.AddMessage(new VerificationLogMessage(verificationPartition.AssignedUser, item.ID, true));
                
                if(item.CountedQuantity < 0) //less then 0 means i was never counted, thus is not considered verified.
                {
                    ItemsForVerification.Add(item);
                }
                else if(item.CountedQuantity == item.ServerQuantity) //if verified quantity matched server quantity
                {
                    NumOfItemsVerified++;
                }
                else //if verified quantity didn't match server quantity, the item is saved to later be represented in the UI.
                {
                    VerifiedItems.Add(item);
                    NumOfItemsVerified++;
                }
            }
        }

        public void CheckPartition(Partition partition)
        {
            foreach(Location location in partition.Locations)
            {
                Cycle.Log.AddMessage(new LocationLogMessage(DateTime.Now, partition.AssignedUser, location.ID, ItemPlusCountList(location)));
            }
            
            if (partition.IsMultiLocationItemPartition)
            {
                CheckMultiLocationPartition(partition);
            }
            else
            {
                CheckSingleLocationPartition(partition);
            }
        }

        private List<(string itemId, string countedQuantity)> ItemPlusCountList(Location location)
        {
            //Used for creating log messages. Take all items in a location, and turns them into a list of items ID's and their counted quantity

            List<(string itemId, string countedQuantity)> ItemPlusCountList = new List<(string itemId, string countedQuantity)>();

            foreach(Item item in location.Items)
            {
                ItemPlusCountList.Add((item.ID, item.CountedQuantity.ToString()));
            }

            return ItemPlusCountList;
        }

        private void CheckMultiLocationPartition(Partition partition)
        {
            List<Location> UncountedLocations = new List<Location>();
            List<string> ItemIDsSeen = new List<string>();
            Partition NewPartition;

            foreach (Location location in partition.Locations)
            {
                if (location.Visited)
                {
                    foreach (Item item in location.Items)
                    {
                        if (item.HasMultiLocation && !(ItemIDsSeen.Exists(x => x == item.ID))) //checks if the items hasn't already been seen in a differen't location
                        {
                            ItemIDsSeen.Add(item.ID);

                            if (item.AllLocationsVisited && IsEverythingTrue(WhichItemLocationsVisitedInPartition(item, partition))) //checks if all locations has been visited, in current partition
                            {
                                if (item.ServerQuantity == item.CountedQuantity)
                                {
                                    NumOfItemsVerified++;
                                }
                                else
                                {
                                    ItemsForVerification.Add(item);
                                }
                            }
                            else //if all locations wasn't visited
                            {
                                bool[] LocationsVisitedInPartition = WhichItemLocationsVisitedInPartition(item, partition);
                                int PartiallyCountedItemIndex = PartiallyCountedItems.FindIndex(x => x.Item1.ID == item.ID); //checks if current location has an entry in PartiallyCountedItems

                                if (PartiallyCountedItemIndex >= 0) //If item did exist in Partially counted items
                                {
                                    LocationsVisitedInPartition = CombineBoolArray(LocationsVisitedInPartition, PartiallyCountedItems[PartiallyCountedItemIndex].Item2);
                                    item.CountedQuantity += PartiallyCountedItems[PartiallyCountedItemIndex].Item1.CountedQuantity;
                                    PartiallyCountedItems.RemoveAt(PartiallyCountedItemIndex);

                                    if (IsEverythingTrue(LocationsVisitedInPartition)) //checks if all items locations has been visited now, after combineing with its matching counter part in PartiallyCountedItems
                                    {
                                        if (item.CountedQuantity == item.ServerQuantity)
                                        {
                                            NumOfItemsVerified++;
                                        }
                                        else
                                        {
                                            ItemsForVerification.Add(item);
                                        }
                                    }
                                    else //if the items still hasn't had all its locations visited
                                    {
                                        PartiallyCountedItems.Add(new Tuple<Item, bool[]>(item, LocationsVisitedInPartition));
                                    }
                                }
                                else //if item didn't exist in partially counted items
                                {
                                    PartiallyCountedItems.Add(new Tuple<Item, bool[]>(item, LocationsVisitedInPartition));
                                }
                            }
                        }
                    }
                }
                else //if the location being tested wasn't visited
                {
                    UncountedLocations.Add(location);
                }
            }

            if (UncountedLocations.Count != 0) // if there was any locations that wasn't visited
            {
                bool AllLocationsVisited = true;

                foreach (Location location in UncountedLocations) //tests if all the items-locations is contained withing List<Location> UncountedLocations, and sets bool for later referance
                {
                    foreach (Item item in location.Items)
                    {
                        foreach (Location ItemsLocation in item.Locations)
                        {
                            if (!(UncountedLocations.Exists(x => x.ID == ItemsLocation.ID)))
                            {
                                AllLocationsVisited = false;
                            }

                            if (!AllLocationsVisited)
                            {
                                break;
                            }
                        }

                        if (!AllLocationsVisited)
                        {
                            break;
                        }
                    }

                    if (!AllLocationsVisited)
                    {
                        break;
                    }
                }

                bool LocationsAddedToPartition = false;

                for (int x = 0; x < MultiLocationPartitions.Count; x++)
                {
                    if ((MultiLocationPartitions[x].Count == 1 && !AllLocationsVisited) || AllLocationsVisited) //If all item locations wasn't contained within UncountedLocations, the partition chain in MultiLocationPartitions can't be larger then 1
                    {
                        for (int y = 0; y < MultiLocationPartitions[x].Count; y++)
                        {
                            if (MultiLocationPartitions[x][y].Locations.Count + UncountedLocations.Count <= MaxSizeForPartitions && PathsIsCombinable(UncountedLocations, MultiLocationPartitions[x][y].Locations))
                                //Checks if the partiton and uncounted locations can be combined
                            {
                                foreach (Location location in UncountedLocations)
                                {
                                    MultiLocationPartitions[x][y].AddLocation(location);
                                }

                                FillAllPaths(new List<List<List<Location>>> { new List<List<Location>> { MultiLocationPartitions[x][y].Locations } });

                                LocationsAddedToPartition = true;

                                if (!AllLocationsVisited) // if all locations wasn't visited the partition the locations was added to is moved to priority partitions
                                {
                                    PriorityPartitions.Add(MultiLocationPartitions[x][y]);

                                    MultiLocationPartitions.RemoveAt(x);
                                }

                                break;
                            }
                        }
                    }

                    if (LocationsAddedToPartition) // breaks the loop if uncounted locations was added to a partition
                    {
                        break;
                    }
                }

                if (!LocationsAddedToPartition) //if UncountedLocations coundn't be added to an existing partitions, a new is created
                {
                    if (UncountedLocations.Exists(x => x.HasMultilocationItem))
                    {
                        NewPartition = new Partition(true);
                    }
                    else
                    {
                        NewPartition = new Partition(false);
                    }

                    foreach (Location location in UncountedLocations)
                    {
                        NewPartition.AddLocation(location);
                    }

                    FillAllPaths(new List<List<List<Location>>> { new List<List<Location>> { NewPartition.Locations } }); //adds single locations to the new partition, if contained within the same shelves already in the new partition.

                    //Adds the new partition to the appopriate place
                    if (!AllLocationsVisited)
                    {
                        PriorityPartitions.Add(NewPartition);
                    }
                    else if (NewPartition.IsMultiLocationItemPartition)
                    {
                        MultiLocationPartitions.Add(new List<Partition> { NewPartition });
                    }
                    else
                    {
                        AddPartitionToShelfs(NewPartition);
                    }
                }
            }
        }

        private void CheckSingleLocationPartition(Partition partition)
        {
            List<Location> UncountedLocations = new List<Location>();
            Partition NewPartition;

            foreach (Location location in partition.Locations) //sorts into Correctly counted, in need of verification and unvisited locations
            {
                if (location.Visited)
                {
                    foreach (Item item in location.Items)
                    {
                        if (item.CountedQuantity == item.ServerQuantity)
                        {
                            NumOfItemsVerified++;
                        }
                        else
                        {
                            ItemsForVerification.Add(item);
                        }
                    }
                }
                else
                {
                    UncountedLocations.Add(location);
                }
            }

            if (UncountedLocations.Count != 0) //unvisited locations is put into a new partition and added to appopriate shelf
            {
                NewPartition = new Partition(false);

                foreach (Location location in UncountedLocations)
                {
                    NewPartition.AddLocation(location);
                }

                AddPartitionToShelfs(NewPartition);
            }
        }

        private bool[] WhichItemLocationsVisitedInPartition(Item item, Partition partition) //checks if a multilocation item has had its locations visited.
        {
            bool[] LocationsVisited = new bool[item.Locations.Count];

            for(int n = 0; n < item.Locations.Count; n++)
            {
                LocationsVisited[n] = partition.Locations.Exists(x => x.ID == item.Locations[n].ID && x.Visited);
            }

            return LocationsVisited;
        }

        private void AddPartitionToShelfs(Partition partition) //adds partition to appopriate shelf
        {
            int Index = -1;
            bool WasAdded = false;

            Index = AvailebleShelfs.FindIndex(x => x.ShelfIndex == partition.Span.Shelf);

            if(Index >= 0)
            {
                AvailebleShelfs[Index].AddPartition(partition);
                WasAdded = true;
            }

            if (!WasAdded)
            {
                Index = OccopiedShelfs.FindIndex(x => x.ShelfIndex == partition.Span.Shelf);

                if (Index >= 0)
                {
                    OccopiedShelfs[Index].AddPartition(partition);
                    WasAdded = true;
                }
            }

            if (!WasAdded)
            {
                AvailebleShelfs.Add(new Shelf(partition.Span.Shelf));
                AvailebleShelfs.Sort();
            }
        }

        /*
        public void OldCheckPartition(Partition partition)
        //Checks an entire partitions. Going through all items to see if they've been counted, either adds items to normal rotation, verification items, partially counted item or removes it and considers it verified
        {
            List<Item> ItemsInPartition = ConvertLocationListToItemList(partition.Locations);
            bool AllItemLocationsWasVisited;
            bool[] LocationsVisitedInPartition;
            int Index;

            if (partition.IsMultiLocationItemPartition)
            {
                foreach (Item item in ItemsInPartition)
                {
                    if (item.CountedQuantity >= 0) //the items was counted by the client
                    {
                        AllItemLocationsWasVisited = true;
                        LocationsVisitedInPartition = new bool[item.Locations.Count];

                        for (int n = 0; n < item.Locations.Count; n++)
                        {
                            LocationsVisitedInPartition[n] = false;
                        }

                        for (int n = 0; n < item.Locations.Count; n++) //test if all the items locations was visited in partition
                        {
                            if (!(partition.Locations.Exists(x => x.ID == item.Locations[n].ID)))
                            {
                                AllItemLocationsWasVisited = false;
                            }
                            else
                            {
                                LocationsVisitedInPartition[n] = true;
                            }
                        }

                        if (AllItemLocationsWasVisited)
                        {
                            CheckCountedItem(item);
                        }
                        else //If all the items locations wasn't visited
                        {
                            Index = PartiallyCountedItems.FindIndex(x => x.Item1.ID == item.ID);

                            if (Index >= 0) //If the Item already exist in PartitallyCountedItems
                            {
                                LocationsVisitedInPartition = CombineBoolArray(LocationsVisitedInPartition, PartiallyCountedItems[Index].Item2);
                                item.CountedQuantity += PartiallyCountedItems[Index].Item1.CountedQuantity; //item is combined with its matching item from partiallyCountedItems

                                if (IsEverythingTrue(LocationsVisitedInPartition)) //If all the items was visited after the combinations
                                {
                                    PartiallyCountedItems.RemoveAt(Index);
                                    CheckCountedItem(item);
                                }
                                else //If all the items still wasn't visited after the combinations
                                {
                                    PartiallyCountedItems.RemoveAt(Index);
                                    PartiallyCountedItems.Add(new Tuple<Item, bool[]>(item, LocationsVisitedInPartition));
                                }
                            }
                            else //if the item didn't exist in PartitallyCountedItems
                            {
                                PartiallyCountedItems.Add(new Tuple<Item, bool[]>(item, LocationsVisitedInPartition));
                            }
                        }
                    }
                    else //If the item was never counted
                    {
                        AllItemLocationsWasVisited = true;
                        LocationsVisitedInPartition = new bool[item.Locations.Count];

                        for (int n = 0; n < item.Locations.Count; n++)
                        {
                            LocationsVisitedInPartition[n] = false;
                        }

                        for (int n = 0; n < item.Locations.Count; n++) //tests if all locations was visited
                        {
                            if(!(partition.Locations.Exists(x => x.ID == item.Locations[n].ID)))
                            {
                                AllItemLocationsWasVisited = false;
                            }
                            else
                            {
                                LocationsVisitedInPartition[n] = true;
                            }
                        }

                        if (AllItemLocationsWasVisited) //if all items was visited
                        {
                            CheckCountedItem(item);
                        }
                        else //if all items wasn't visited
                        {
                            List<List<Location>> LocationLists = new List<List<Location>>();

                            LocationLists.Add(new List<Location>());

                            for (int n = 0; n < item.Locations.Count; n++) //adds visited locations to LocationLists
                            {
                                if (LocationsVisitedInPartition[n])
                                {
                                    LocationLists[0].Add(item.Locations[n]);
                                }
                            }

                            if(LocationLists[0].Count > MaxSizeForPartitions) //Splits Locations into multiple lists if larger then MaxSizeForParitions
                            {
                                List<List<List<Location>>> path = new List<List<List<Location>>>();
                                path.Add(LocationLists);
                                DivideLargerPaths(path);
                            }

                            foreach(Partition priorityPartition in PriorityPartitions) //tries to combine the locations with already existing partitions in PriorityPartition
                            {
                                for(int x = 0; x < LocationLists.Count; x++)
                                {
                                    if(priorityPartition.Locations.Count + LocationLists[x].Count <= MaxSizeForPartitions && PathsIsCombinable(priorityPartition.Locations, LocationLists[x]))
                                    {
                                        foreach(Location location in LocationLists[x])
                                        {
                                            priorityPartition.AddLocation(location);
                                        }

                                        LocationLists.RemoveAt(x);
                                        break;
                                    }
                                }

                                if(LocationLists.Count == 0)
                                {
                                    break;
                                }
                            }

                            if(LocationLists.Count != 0) //Any LocationLists Unable to add to existing PriorityPartitions, turned into new partitons and added.
                            {
                                foreach(List<Location> locationList in LocationLists)
                                {
                                    Partition NewPartition = new Partition(true);

                                    foreach(Location location in locationList)
                                    {
                                        NewPartition.AddLocation(location);
                                    }

                                    PriorityPartitions.Add(NewPartition);
                                }
                            }
                        }
                    }
                }
            }
            else //if Partition wasn't a MultiLocationPartition
            {
                foreach(Item item in ItemsInPartition)
                {
                    CheckCountedItem(item);
                }
            }
        }
        */

        private bool IsEverythingTrue(bool[] BoolArray) //checks if all statements in a bool array is true
        {
            bool IsAllPosistionsTrue = true;

            for(int x = 0; x < BoolArray.Length; x++)
            {
                if (!BoolArray[x])
                {
                    IsAllPosistionsTrue = false;
                }
            }

            return IsAllPosistionsTrue;
        }

        private bool[] CombineBoolArray(bool[] BoolArrayA, bool[] BoolArrayB) //combines two bool arrays with and "OR" statement
        {
            if(BoolArrayA.Length == BoolArrayB.Length)
            {
                for(int x = 0; x < BoolArrayA.Length; x++)
                {
                    BoolArrayA[x] = BoolArrayA[x] || BoolArrayB[x];
                }
            }
            else
            {
                throw new Exception("Arrays must be the same size");
            }

            return BoolArrayA;
        }

        public void CheckCountedItem(Item item) //Only used in testing

        //Checks a fully counted item, if it's counted and the same as expected then the item is verified, else if it didn't match its added to verification items
        //Lastly if the item was returned but not counted its added into the normal rotation
        {
            Shelf shelf;
            List<Location> ItemsLocations;
            bool ItemsWasAddedToPartition;

            if (item.CountedQuantity < 0)
            {
                if (item.HasMultiLocation)
                {
                    //Add into MultiLocationPartitions
                    ItemsLocations = ConvertItemListToLocationList(new List<Item> { item });
                    ItemsWasAddedToPartition = false;

                    foreach(List<Partition> LinkedPartitions in MultiLocationPartitions)
                    {
                        foreach(Partition partition in LinkedPartitions)
                        {
                            if(partition.Locations.Count + ItemsLocations.Count <= MaxSizeForPartitions && PathsIsCombinable(partition.Locations, ItemsLocations))
                            {
                                foreach(Location location in ItemsLocations)
                                {
                                    partition.Locations.Add(location);
                                }

                                partition.Locations.Sort(Location_Comparer);
                                ItemsLocations.Clear();
                                ItemsWasAddedToPartition = true;
                                break;
                            }
                        }

                        if (ItemsWasAddedToPartition)
                        {
                            break;
                        }
                    }

                    if (!ItemsWasAddedToPartition)
                    {
                        if(ItemsLocations.Count > MaxSizeForPartitions)
                        {
                            List<List<List<Location>>> Path = new List<List<List<Location>>>();
                            Path.Add(new List<List<Location>>());
                            Path[0].Add(ItemsLocations);

                            DivideLargerPaths(Path);
                            MultiLocationPartitions.Add(new List<Partition>());

                            foreach(List<Location> LocationList in Path[0])
                            {
                                Partition NewPartition = new Partition(true);

                                foreach(Location location in LocationList)
                                {
                                    NewPartition.AddLocation(location);
                                }

                                MultiLocationPartitions.Last().Add(NewPartition);
                            }
                        }
                        else
                        {
                            MultiLocationPartitions.Add(new List<Partition> { new Partition(true) });

                            foreach (Location location in ItemsLocations)
                            {
                                MultiLocationPartitions.Last()[0].AddLocation(location);
                            }
                        }
                    }
                }
                else
                {
                    //add items back to normal count
                    foreach (Location location in item.Locations)
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
            }
            else if (item.ServerQuantity != item.CountedQuantity)
            {
                if (item.HasMultiLocation)
                {
                    item.CountedQuantity = -1;
                    MultiLocationItemsForVerification.Add(item);
                }
                else
                {
                    item.CountedQuantity = -1;
                    ItemsForVerification.Add(item);
                }
            }
            else
            {
                NumOfItemsVerified++;
            }
        }

        public VerificationPartition CreateVerificationPartition(User client)
        {
            VerificationPartition verificationPartition = new VerificationPartition();
            int Distance;
            int ShortestMultiLocDistance;
            int ShortestSingleLocDistance;
            int IndexOfMultiLoc;
            int IndexOfSingleLoc;

            if (!client.IsAdmin)
            {
                throw new Exception("Client requesting a verification partition isn't an admin");
            }

            //adds the first item to the new verification partition, prioritizing multi location items.
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
            //Continues to add items until the partition exceeds the size dictated by MaxSizeForPartitions, or there isn't anymore items in need of verification
            {
                ShortestMultiLocDistance = int.MaxValue;
                IndexOfMultiLoc = -1;

                for(int x = 0; x < MultiLocationItemsForVerification.Count; x++) //compares distance between multilocation items and the locations in the verification partition
                {
                    Distance = verificationPartition.CompareDistance(MultiLocationItemsForVerification[x], Location_Comparer);

                    if(Distance < ShortestMultiLocDistance)
                    {
                        ShortestMultiLocDistance = Distance;
                        IndexOfMultiLoc = x;
                    }
                }

                if(ShortestMultiLocDistance < 1000) //if a MultiLocation item can be checked without Visiting new Shelfs, its given priority over single Location items, and added immidiatly
                {
                    verificationPartition.AddItem(MultiLocationItemsForVerification[IndexOfMultiLoc]);

                    MultiLocationItemsForVerification.RemoveAt(IndexOfMultiLoc);

                    continue;
                }

                ShortestSingleLocDistance = int.MaxValue;
                IndexOfSingleLoc = -1;

                for (int x = 0; x < ItemsForVerification.Count; x++) //finds distance between a single location item and the locations in the verification partition.
                {
                    Distance = verificationPartition.CompareDistance(ItemsForVerification[x], Location_Comparer);

                    if (Distance < ShortestSingleLocDistance)
                    {
                        ShortestSingleLocDistance = Distance;
                        IndexOfSingleLoc = x;
                    }
                }

                if(ShortestMultiLocDistance / 1000 <= ShortestSingleLocDistance / 1000)
                //compares the distances to the nearest single location or multilocaiton item.
                //devideing by 1000 means that, moves between columns is ignored, only the amount of shelf needed to move is considered.
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

            client.CurrentVerificationPartition = verificationPartition;

            if (verificationPartition != null)
            {
                verificationPartition.AssignedUser = client.ID;
            }

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

        private void InitialPartitioningOfMultiLocationItems() //Creates Multilocation item Locations
        {
            List<List<List<Location>>> Paths = new List<List<List<Location>>>();
            List<Location> NewList;

            while(MultiLocationsItem_Locations.Count != 0) //All locations connected by items, is turned into a list and then added to Paths
            {
                NewList = new List<Location> {};

                CombineLocationConnectedByCommonItems(MultiLocationsItem_Locations[0], NewList);

                RemoveLocationInListA_From_ListB(NewList, MultiLocationsItem_Locations);

                NewList.Sort(Location_Comparer);

                Paths.Add(new List<List<Location>>());

                Paths.Last().Add(NewList);
            }

            DivideLargerPaths(Paths); //paths larger then MaxSizeForPartitions is devided

            CombineShorterPaths(Paths); //paths lower then MaxSizeForPartition is combined with others combinable paths. (paths are considered combinable, if all the location in both paths are contained within the same shelves)

            FillAllPaths(Paths); //Paths still lower then MaxSizeForPartition is filled with Locations that doesn't have MultiLocationItems

            foreach(List<List<Location>> ConnectedPaths in Paths) //turns List<List<List<Location>>> Paths into List<List<Partition>> MultiLocationPartitions
            {
                MultiLocationPartitions.Add(new List<Partition>());

                foreach(List<Location> Path in ConnectedPaths)
                {
                    Partition NewPartition = new Partition(true);

                    foreach(Location location in Path)
                    {
                        NewPartition.AddLocation(location);
                    }

                    MultiLocationPartitions.Last().Add(NewPartition);
                }
            }

            MultiLocationPartitions.Sort(SortListByLargestFirst);
        }

        private void DivideLargerPaths(List<List<List<Location>>> Paths)
        //Devides paths larger then MaxSizeForPartitions into multiple paths, ensureing that the fews number of item connections are cut
        {
            List<List<Location>> DevidedPath;

            int ConnectionCut;
            int LeastConnectionCut;
            int Index = -1;

            for(int x = 0; x < Paths.Count; x++)
            {
                DevidedPath = new List<List<Location>>();

                while (Paths[x][0].Count > MaxSizeForPartitions)
                {
                    LeastConnectionCut = int.MaxValue;

                    for(int y = MaxSizeForPartitions - (MaxSizeForPartitions / 4); y <= MaxSizeForPartitions; y++)
                    {
                        ConnectionCut = HowManyConnectionsSevered(Paths[x][0], y);

                        if(ConnectionCut < LeastConnectionCut)
                        {
                            Index = y;
                            LeastConnectionCut = ConnectionCut;
                        }
                    }

                    DevidedPath.Add(new List<Location>(TakeFirstXInList<Location>(Index, Paths[x][0])));
                }

                if(DevidedPath.Count > 0)
                {
                    DevidedPath.Add(Paths[x][0]);

                    Paths[x] = DevidedPath;
                }
            }
        }

        private int HowManyConnectionsSevered(List<Location> path, int Index) //Test how many items in a Connected MultiLocationList has is location before and after Index
        {
            List<string> LocationIDs = new List<string>();
            List<string> ItemIDs = new List<string>();
            int NumOfConnecteionsSevered = 0;

            for(int n = 0; n < Index; n++)
            {
                LocationIDs.Add(path[n].ID);
            }

            for (int n = 0; n < Index; n++)
            {
                foreach(Item item in path[n].Items)
                {
                    if (!(ItemIDs.Exists(x => x == item.ID)))
                    {
                        ItemIDs.Add(item.ID);

                        foreach (Location location in item.Locations)
                        {
                            if (!(LocationIDs.Exists(x => x == location.ID)))
                            {
                                NumOfConnecteionsSevered++;
                                break;
                            }
                        }
                    }
                }
            }

            return NumOfConnecteionsSevered;
        }

        private IEnumerable<T> TakeFirstXInList<T>(int X, List<T> path) //Takes Items before index and puts them into a new list
        {
            List<T> NewList = new List<T>();
            int x = X;

            while(x > 0)
            {
                NewList.Add(path[0]);

                path.RemoveAt(0);

                x--;
            }

            return NewList;
        }

        private void FillAllPaths(List<List<List<Location>>> paths)
        {
            int NumOfPaths = NumberOfPaths(paths);
            List<Location> TestPath;

            for(int x = 0; x < NumOfPaths; x++)
            {
                TestPath = FindPathAtIndex(x, paths);

                if(TestPath.Count < MaxSizeForPartitions)
                {
                    FillPath(TestPath);
                }
            }
        }

        private void FillPath(List<Location> Path)
        {
            int ClosestDistanceToShelf;
            int ShelfIndex;
            int ClosestDistanceToPartition;
            int PartitionIndex;
            int x;


            while (Path.Count < MaxSizeForPartitions)
            {
                ShelfIndex = -1;
                ClosestDistanceToShelf = int.MaxValue;

                foreach(Location location in Path)
                {
                    for(x = 0; x < AvailebleShelfs.Count; x++)
                    {
                        if(Math.Abs(location.Shelf - AvailebleShelfs[x].ShelfIndex) < ClosestDistanceToShelf)
                        {
                            ShelfIndex = x;
                            ClosestDistanceToShelf = Math.Abs(location.Shelf - AvailebleShelfs[x].ShelfIndex);

                            if(ClosestDistanceToShelf == 0)
                            {
                                break;
                            }
                        }
                    }

                    if (ClosestDistanceToShelf == 0)
                    {
                        break;
                    }
                }

                if(ClosestDistanceToShelf != 0)
                {
                    break;
                }

                PartitionIndex = -1;
                ClosestDistanceToPartition = int.MaxValue;

                for(x = 0; x < AvailebleShelfs[ShelfIndex].Partitions.Count; x++)
                {
                    foreach(Location location in Path)
                    {
                        if(Math.Abs(location.Position - AvailebleShelfs[ShelfIndex].Partitions[x].Span.Position) < ClosestDistanceToPartition)
                        {
                            ClosestDistanceToPartition = location.Position - AvailebleShelfs[ShelfIndex].Partitions[x].Span.Position;
                            PartitionIndex = x;

                            if(ClosestDistanceToPartition == 0)
                            {
                                break;
                            }
                        }
                    }

                    if (ClosestDistanceToPartition == 0)
                    {
                        break;
                    }
                }

                while(AvailebleShelfs[ShelfIndex].Partitions[PartitionIndex].Locations.Count != 0 && Path.Count < MaxSizeForPartitions)
                {
                    Path.Add(AvailebleShelfs[ShelfIndex].Partitions[PartitionIndex].Locations[0]);
                    AvailebleShelfs[ShelfIndex].Partitions[PartitionIndex].Locations.RemoveAt(0);

                    if(AvailebleShelfs[ShelfIndex].Partitions[PartitionIndex].Locations.Count == 0)
                    {
                        AvailebleShelfs[ShelfIndex].Partitions.RemoveAt(PartitionIndex);

                        if(AvailebleShelfs[ShelfIndex].Partitions.Count == 0)
                        {
                            AvailebleShelfs.RemoveAt(ShelfIndex);
                        }

                        break;
                    }
                }
            }

            Path.Sort(Location_Comparer);
        }

        private void CombineShorterPaths(List<List<List<Location>>> Paths)
        {
            int NumOfPaths = NumberOfPaths(Paths);
            List<Location> PathA;
            List<Location> PathB;

            for (int x = 0; x < NumOfPaths; x++)
            {
                PathA = FindPathAtIndex(x, Paths);

                if(PathA.Count >= MaxSizeForPartitions - 1)
                {
                    continue;
                }

                for (int y = x + 1; y < NumOfPaths; y++)
                {
                    PathB = FindPathAtIndex(y, Paths);

                    if(PathA.Count + PathB.Count <= MaxSizeForPartitions && PathsIsCombinable(PathA, PathB))
                    {
                        foreach(Location location in PathB)
                        {
                            PathA.Add(location);
                        }

                        RemovePathAtIndex(y, Paths);
                        NumOfPaths--;

                        if (PathA.Count >= MaxSizeForPartitions - 1)
                        {
                            break;
                        }

                        y--;
                    }
                }
            }
        }

        private void RemovePathAtIndex(int Index, List<List<List<Location>>> paths)
        {
            int ElementsSeen = 0;
            int ListNum = 0;

            int SizeOfPath = paths.Count;

            while (ElementsSeen < Index + 1)
            {
                if (ElementsSeen + paths[ListNum].Count >= Index + 1)
                {
                    paths[ListNum].RemoveAt(Index - ElementsSeen);

                    if (paths[ListNum].Count == 0)
                    {
                        paths.RemoveAt(ListNum);
                    }

                    break;
                }
                else
                {
                    ElementsSeen += paths[ListNum].Count;
                    ListNum++;
                }
            }
        }

        private List<Location> FindPathAtIndex(int Index, List<List<List<Location>>> paths)
        {
            int ElementsSeen = 0;
            int ListNum = 0;

            while(ElementsSeen < Index + 1)
            {
                if(ElementsSeen + paths[ListNum].Count >= Index + 1 && paths.Count < int.MaxValue)
                {
                    return paths[ListNum][Index - ElementsSeen];
                }
                else
                {
                    ElementsSeen += paths[ListNum].Count;
                    ListNum++;
                }
            }

            throw new Exception("Couldn't find Path at Index");
        }

        private int NumberOfPaths(List<List<List<Location>>> Paths)
        {
            int num = 0;

            for(int x = 0; x < Paths.Count; x++)
            {
                num += Paths[x].Count;
            }

            return num;
        }

        private bool PathsIsCombinable(List<Location> PathA, List<Location> PathB)
        //tests 2 list of locations, to test if PathA only contain locations with Shelves also contained in PathB, or vise versa
        {
            bool PathA_HasAShelfOutsideB = false;
            bool LocationAExistInB_Test;
            List<bool> LocationsInBExistInA = new List<bool>();

            for(int x = 0; x < PathB.Count; x++ )
            {
                LocationsInBExistInA.Add(false);
            }

            foreach (Location LocationInA in PathA)
            {
                LocationAExistInB_Test = false;

                for(int x = 0; x < PathB.Count; x++)
                {
                    if(LocationInA.Shelf == PathB[x].Shelf)
                    {
                        LocationAExistInB_Test = true;
                        LocationsInBExistInA[x] = true;
                    }
                }

                if (!LocationAExistInB_Test)
                {
                    PathA_HasAShelfOutsideB = true;
                }
            }

            if (!PathA_HasAShelfOutsideB || !(LocationsInBExistInA.Exists(x => x == false)))
            {
                return true;
            }

            return false;
        }

        private void CombineLocationConnectedByCommonItems(Location StartLocation, List<Location> CombinedList) //Turns Locations connected by commen MultiLocationItems into one list
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
                            CombineLocationConnectedByCommonItems(location, CombinedList);
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

        private int SortListByLargestFirst<T>(List<T> ListA, List<T> ListB)
        {
            return ListB.Count - ListA.Count;
        }

        private List<Item> ConvertLocationListToItemList(List<Location> LocationList) //Runs 3 nested loops
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

        private List<Location> ConvertItemListToLocationList(List<Item> ItemList) //Runs 3 nested loops, this is only used in CheckItem, which itself is only used for testing perposes
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
    }
}
