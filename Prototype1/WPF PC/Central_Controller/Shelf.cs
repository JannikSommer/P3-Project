using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace WPF_PC.Central_Controller
{
    public partial class Controller
    {
        private class Shelf : IComparable
        {
            public int ShelfIndex;
            public List<Partition> Partitions { get; private set; } = new List<Partition>();
            // public Client BeingCountedByUser = null; //needed for first implimentation of send next partition
            private List<ClientInfo> Clients = new List<ClientInfo>();

            public Shelf(int shelfIndex)
            {
                ShelfIndex = shelfIndex;
            }

            public int CompareTo(object Other)
            {
                Shelf OtherShelf = (Shelf)Other;
                return ShelfIndex - OtherShelf.ShelfIndex;
            }

            public void AddLocation(Location location)
            {
                bool PosistionExistsInPartitions = false;

                if (location.Shelf == ShelfIndex)
                {
                    foreach (Partition partition in Partitions)
                    {
                        if (partition.Span.Posistion == location.Posistion)
                        {
                            PosistionExistsInPartitions = true;
                            partition.AddLocation(location);
                            partition.Locations.Sort();
                            break;
                        }
                    }

                    if (!PosistionExistsInPartitions)
                    {
                        Partitions.Add(new Partition(location));
                        Partitions.Sort();
                    }
                }
                else
                {
                    throw new Exception("can't add Location to shelf because shelf indexes doesn't match");
                }
            }

            public bool HasClient(Client client)
            {
                return Clients.Exists(x => x.Client.ID == client.ID);
            }

            public void AddClient(Client client)
            {
                List<int> LargestGap;
                
                if (!HasClient(client))
                {
                    if(Clients.Count == 0)
                    {
                        Clients.Add(new ClientInfo(client, 0, false));
                    }
                    else if(Clients.Count == 1)
                    {
                        Clients.Add(new ClientInfo(client, Partitions.Count - 1, true));
                    }
                    else
                    {
                        LargestGap = FindLargestGapInPartitions_Posistions();

                        if(Clients[LargestGap[0]].ReverseCount && Clients[LargestGap[1]].ReverseCount) //LargestGap[0] count direction: <-      LargestGap[1] count direction: <-   
                        {
                            Clients.Add(new ClientInfo(client, Clients[LargestGap[0]].PartitionIndex + 1, false)); 
                        }
                        else if (!Clients[LargestGap[0]].ReverseCount && Clients[LargestGap[1]].ReverseCount) //LargestGap[0] count direction: ->      LargestGap[1] count direction: <-
                        {
                            // 1/3 of the amounts of partition between the 2 largest gap partitions + the the start point of the first one
                            int PartitionIndex = (Clients[LargestGap[1]].PartitionIndex - Clients[LargestGap[0]].PartitionIndex) / 3 + Clients[LargestGap[0]].PartitionIndex;

                            Clients.Add(new ClientInfo(client, PartitionIndex, false));
                        }
                        else if (Clients[LargestGap[0]].ReverseCount && !Clients[LargestGap[1]].ReverseCount) //LargestGap[0] count direction: <-      LargestGap[1] count direction: ->
                        {
                            Clients.Add(new ClientInfo(client, Clients[LargestGap[0]].PartitionIndex + 1, false));
                        }
                        else //LargestGap[0] count direction: ->      LargestGap[1] count direction: ->
                        {
                            Clients.Add(new ClientInfo(client, Clients[LargestGap[1]].PartitionIndex - 1, true));
                        }
                    }
                }
                else
                {
                    throw new Exception("Shelf already contains client");
                }

                Clients.Sort();
            }

            private List<int> FindLargestGapInPartitions_Posistions()
            {
                int BiggestGapSize = -1;
                List<int> LargestGapPosistions = new List<int>();

                if(Clients.Count >= 2)
                {
                    for(int x = 1; x < Clients.Count; x++)
                    {
                        if(Clients[x].PartitionIndex - Clients[x - 1].PartitionIndex > BiggestGapSize)
                        {
                            BiggestGapSize = Clients[x].PartitionIndex - Clients[x - 1].PartitionIndex;
                            LargestGapPosistions = new List<int> { x - 1, x };
                        }
                    }
                }

                return LargestGapPosistions;
            }

            public int FindLargestGapInPartitions_size()
            {
                int BiggestGapSize = -1;
                List<int> LargestGapPosistions = new List<int>();

                if (Clients.Count >= 2)
                {
                    for (int x = 1; x < Clients.Count; x++)
                    {
                        if (Clients[x].PartitionIndex - Clients[x - 1].PartitionIndex > BiggestGapSize)
                        {
                            BiggestGapSize = Clients[x].PartitionIndex - Clients[x - 1].PartitionIndex;
                            LargestGapPosistions = new List<int> { x - 1, x };
                        }
                    }
                }

                return BiggestGapSize;
            }

            public Partition ClientsNextPartition(Client client)
            {
                int ClientsIndex = Clients.FindIndex(x => x.Client.ID == client.ID);
                Partition partition;

                if(ClientsIndex < 0)
                {
                    throw new Exception("Shelf doesn't have client with that ID assigned");
                }

                if (Clients[ClientsIndex].ReverseCount)
                {
                    if(ClientsIndex != 0 && Clients[ClientsIndex].PartitionIndex == Clients[ClientsIndex - 1].PartitionIndex) //Should never look outside of index because of short circuiting 
                    {
                        //give next partition and remove _Clients[ClientsIndex] and _Clients[ClientsIndex - 1]
                        partition = ExtractClintsNextPartition(ClientsIndex);

                        RemoveUsers(new List<int> { ClientsIndex, ClientsIndex - 1 });
                    }
                    else
                    {
                        //give partition normally
                        partition = ExtractClintsNextPartition(ClientsIndex);
                    }
                }
                else
                {
                    if(ClientsIndex != Clients.Count - 1 && Clients[ClientsIndex].PartitionIndex == Clients[ClientsIndex + 1].PartitionIndex) //Should never look outside of index because of short circuiting 
                    {
                        //give next partition and remove _Clients[ClientsIndex] and _Clients[ClientsIndex + 1]
                        partition = ExtractClintsNextPartition(ClientsIndex);

                        RemoveUsers(new List<int> { ClientsIndex, ClientsIndex + 1 });
                    }
                    else
                    {
                        //give partition normally
                        partition = ExtractClintsNextPartition(ClientsIndex);
                    }
                }

                return partition;
            }

            private void RemoveUsers(List<int> IndexList)
            {
                IndexList.Sort();

                for(int x = IndexList.Count - 1; x >= 0; x--)
                {
                    Clients.RemoveAt(x);
                }
            }

            private Partition ExtractClintsNextPartition(int Index)
            {
                Partition partition = Partitions[Clients[Index].PartitionIndex];
                Partitions.RemoveAt(Clients[Index].PartitionIndex);
                Clients[Index].IncrementIndex();

                int Count = Index + 1;

                while(Count < Clients.Count) //Decrements PartitionIndex of all Clients with PartitionIndex high then said client
                {
                    Clients[Count].PartitionIndex--;
                    Count++;
                }

                return partition;
            }

            private class ClientInfo : IComparable
            {
                public Client Client;
                public int PartitionIndex;
                public bool ReverseCount;

                public ClientInfo(Client client, int partitionIndex, bool reverseCount)
                {
                    Client = client;
                    PartitionIndex = partitionIndex;
                    ReverseCount = reverseCount;
                }

                public int CompareTo(object obj)
                {
                    return PartitionIndex.CompareTo(((ClientInfo)obj).PartitionIndex);
                }

                public void IncrementIndex()
                {
                    if (ReverseCount)
                    {
                        PartitionIndex--;
                    }
                }
            }
        }
    }
}
