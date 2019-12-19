using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Central_Controller.Central_Controller {
    public partial class Controller {
        private class Shelf : IComparable {
            public Shelf(int shelfIndex) {
                ShelfIndex = shelfIndex;
            }

            public int ShelfIndex { get; set; }
            public int NumberOfClients { 
                get { 
                    return Users.Count; 
                }
            }
            public List<Partition> Partitions { get; private set; } = new List<Partition>();

            private List<UserInfo> Users = new List<UserInfo>();


            public int CompareTo(object Other) {
                Shelf OtherShelf = (Shelf)Other;
                return ShelfIndex - OtherShelf.ShelfIndex;
            }

            public void AddLocation(Location location) {
                bool PositionExistsInPartitions = false;

                if(location.Shelf == ShelfIndex) {
                    foreach(Partition partition in Partitions) {
                        if(partition.Span.Position == location.Position) {
                            PositionExistsInPartitions = true;
                            partition.AddLocation(location);
                            partition.Locations.Sort((x,y) => x.Row.CompareTo(y.Row));
                            break;
                        }
                    }

                    if(!PositionExistsInPartitions) {
                        Partitions.Add(new Partition(location));
                        Partitions.Sort();
                    }
                } else {
                    throw new Exception("can't add Location to shelf because shelf indexes doesn't match");
                }
            }

            public bool HasUsers(User client) {
                return Users.Exists(x => x.Client.ID == client.ID);
            }

            public void RemoveUser(User client) {
                int ClientIndex = Users.FindIndex(x => x.Client.ID == client.ID);
                Users.RemoveAt(ClientIndex);
            }

            public void AddUser(User client) {
                List<int> LargestGap;

                if(!HasUsers(client)) {
                    if(Users.Count == 0) {
                        Users.Add(new UserInfo(client, 0, false));
                    } else if(Users.Count == 1) {
                        LargestGap = FindLargestGapInPartitions_Positions();

                        if(LargestGap[1] == int.MaxValue) {
                            Users.Add(new UserInfo(client, Partitions.Count - 1, true));
                        } else {
                            Users.Add(new UserInfo(client, 0, false));
                        }  
                    } else {
                        LargestGap = FindLargestGapInPartitions_Positions();

                        if(LargestGap[0] == -1) {
                            Users.Add(new UserInfo(client, 0, false));
                        } else if(LargestGap[1] == int.MaxValue) {
                            Users.Add(new UserInfo(client, Partitions.Count - 1, true));
                        } else {  
                            if(Users[LargestGap[0]].ReverseCount && Users[LargestGap[1]].ReverseCount) { //LargestGap[0] count direction: <-      LargestGap[1] count direction: <- 
                                Users.Add(new UserInfo(client, Users[LargestGap[0]].PartitionIndex + 1, false));
                            } else if(!Users[LargestGap[0]].ReverseCount && Users[LargestGap[1]].ReverseCount) { //LargestGap[0] count direction: ->      LargestGap[1] count direction: <-
                                // 1/3 of the amounts of partition between the 2 largest gap partitions + the the start point of the first one
                                int PartitionIndex = (Users[LargestGap[1]].PartitionIndex - Users[LargestGap[0]].PartitionIndex) / 3 + Users[LargestGap[0]].PartitionIndex;

                                Users.Add(new UserInfo(client, PartitionIndex, false));
                            } else if(Users[LargestGap[0]].ReverseCount && !Users[LargestGap[1]].ReverseCount) { //LargestGap[0] count direction: <-      LargestGap[1] count direction: ->
                                Users.Add(new UserInfo(client, Users[LargestGap[0]].PartitionIndex + 1, false));
                            } else { //LargestGap[0] count direction: ->      LargestGap[1] count direction: ->
                                Users.Add(new UserInfo(client, Users[LargestGap[1]].PartitionIndex - 1, true));
                            }
                        }
                    }
                } else {
                    throw new Exception("Shelf already contains client");
                }

                Users.Sort();
            }

            private List<int> FindLargestGapInPartitions_Positions() {
                int BiggestGapSize = -1;
                List<int> LargestGapPositions = new List<int>();

                if(Users.Count > 0 && Users[0].PartitionIndex != 0) {
                    BiggestGapSize = Users[0].PartitionIndex;
                    LargestGapPositions = new List<int> { -1, 0 };
                }

                if(Users.Count >= 2) {
                    for(int x = 1; x < Users.Count; x++) {
                        if(Users[x].PartitionIndex - Users[x - 1].PartitionIndex > BiggestGapSize) {
                            if(!Users[x - 1].ReverseCount && Users[x].ReverseCount) {
                                if((Users[x].PartitionIndex - Users[x - 1].PartitionIndex) / 3 > BiggestGapSize) {
                                    BiggestGapSize = (Users[x].PartitionIndex - Users[x - 1].PartitionIndex) / 3;
                                    LargestGapPositions = new List<int> { x - 1, x };
                                }
                            } else {
                                BiggestGapSize = Users[x].PartitionIndex - Users[x - 1].PartitionIndex;
                                LargestGapPositions = new List<int> { x - 1, x };
                            }
                        }
                    }
                }

                if(Users.Count > 0 && Users.Last().PartitionIndex != Partitions.Count) {
                    if(Partitions.Count - 1 - Users.Last().PartitionIndex > BiggestGapSize) {
                        LargestGapPositions = new List<int> { Users.Count - 1, int.MaxValue };
                    }
                }

                return LargestGapPositions;
            }

            public int FindLargestGapInPartitions_size() {
                int BiggestGapSize = -1;

                if(Users.Count > 0 && Users[0].PartitionIndex != 0) {
                    BiggestGapSize = Users[0].PartitionIndex;
                }

                if(Users.Count >= 2) {
                    for(int x = 1; x < Users.Count; x++) {
                        if(Users[x].PartitionIndex - Users[x - 1].PartitionIndex > BiggestGapSize) {
                            if(!Users[x - 1].ReverseCount && Users[x].ReverseCount) {
                                if((Users[x].PartitionIndex - Users[x - 1].PartitionIndex) / 3 > BiggestGapSize) {
                                    BiggestGapSize = (Users[x].PartitionIndex - Users[x - 1].PartitionIndex) / 3;
                                }
                            } else {
                                BiggestGapSize = Users[x].PartitionIndex - Users[x - 1].PartitionIndex;
                            }
                        }
                    }
                }

                if(Users.Count > 0 && Users.Last().PartitionIndex != Partitions.Count - 1) {
                    if(Partitions.Count - 1 - Users.Last().PartitionIndex > BiggestGapSize) {
                        BiggestGapSize = Partitions.Count - 1 - Users.Last().PartitionIndex;
                    }
                }

                return BiggestGapSize;
            }

            public Partition UsersNextPartition(User client) {
                int ClientsIndex = Users.FindIndex(x => x.Client.ID == client.ID);
                Partition partition;

                if(ClientsIndex < 0) {
                    throw new Exception("Shelf doesn't have client with that ID assigned");
                }

                if(Users[ClientsIndex].ReverseCount) {
                    if(ClientsIndex != 0 && Users[ClientsIndex].PartitionIndex == Users[ClientsIndex - 1].PartitionIndex) { //Should never look outside of index because of short circuiting
                        //give next partition and remove _Clients[ClientsIndex] and _Clients[ClientsIndex - 1]
                        partition = ExtractUsersNextPartition(ClientsIndex);

                        RemoveUsers(new List<int> { ClientsIndex, ClientsIndex - 1 });
                    } else {
                        //give partition normally
                        partition = ExtractUsersNextPartition(ClientsIndex);
                    }
                } else {
                    if(ClientsIndex != Users.Count - 1 && Users[ClientsIndex].PartitionIndex == Users[ClientsIndex + 1].PartitionIndex) { //Should never look outside of index because of short circuiting
                        //give next partition and remove _Clients[ClientsIndex] and _Clients[ClientsIndex + 1]
                        partition = ExtractUsersNextPartition(ClientsIndex);

                        RemoveUsers(new List<int> { ClientsIndex, ClientsIndex + 1 });
                    } else {
                        //give partition normally
                        partition = ExtractUsersNextPartition(ClientsIndex);
                    }
                }

                client.CurrentPartition = partition;
                return partition;
            }

            public void RemoveInactiveUsers(User client) {
                int InactiveClientIndex = Users.FindIndex(x => x.Client.ID == client.ID);
                int IndexOfItem = int.MaxValue;

                if(InactiveClientIndex < 0) {
                    throw new Exception("Client isn't assigned to shelf");
                }

                Partitions.Add(Users[InactiveClientIndex].Client.CurrentPartition);
                Partitions.Sort();

                //IndexOfItem = Partitions.FindIndex(x => x.CompareTo(client.CurrentPartition) == 0);

                for(int x = 0; x < Partitions.Count; x++) {
                    if(Partitions[x].Span.Shelf == Users[InactiveClientIndex].Client.CurrentPartition.Span.Shelf
                        && Partitions[x].Span.Position == Users[InactiveClientIndex].Client.CurrentPartition.Span.Position) {
                        IndexOfItem = x;
                    }
                }

                foreach(UserInfo _client in Users) {
                    if(_client.PartitionIndex >= IndexOfItem) {
                        _client.PartitionIndex++;
                    }
                }

                Users.RemoveAt(InactiveClientIndex);
            }

            private void RemoveUsers(List<int> IndexList) {
                IndexList.Sort();

                for(int x = IndexList.Count - 1; x >= 0; x--) {
                    Users.RemoveAt(x);
                }
            }

            private Partition ExtractUsersNextPartition(int Index) {
                Partition partition = Partitions[Users[Index].PartitionIndex];
                Partitions.RemoveAt(Users[Index].PartitionIndex);
                Users[Index].IncrementIndex();

                int Count = Index + 1;

                while(Count < Users.Count) { //Decrements PartitionIndex of all Users with PartitionIndex high then said client
                    Users[Count].PartitionIndex--;
                    Count++;
                }

                return partition;
            }

            public void AddPartition(Partition partition) {
                int index = Partitions.Count / 2;
                int increment = index / 2;

                while(!(Partitions[index].Span.Position > partition.Span.Position && Partitions[index - 1].Span.Position < partition.Span.Position)) {
                    if(partition.Span.Shelf > Partitions[index].Span.Shelf) {
                        index += increment;
                        increment /= 2;
                    } else {
                        index -= increment;
                        increment /= 2;
                    }
                }

                Partitions.Insert(index, partition);

                foreach(UserInfo client in Users) {
                    if(client.PartitionIndex >= index) {
                        client.PartitionIndex++;
                    }
                }
            }
        }
    }
}
