using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Model;
using Central_Controller;
using Networking;
using System.Threading;
using SQL_DB_test_Frame;
using PrestaSharpAPI;

namespace TESTPIS2
{
    class Program2
    {
        static void Main(string[] args)
        {
            DB_connector connector = new DB_connector();
            Stopwatch timer = new Stopwatch();
            Sorting sorter = new Sorting();
            Item item = new Item();
            Location location = new Location();
            Controller controller = new Controller();
            ProductAPI presta = new ProductAPI();
            //Server server = new Server();
            
            timer.Start();
            //string test_name, test2;

            //console.writeline("give table string");
            //test_name = console.readline();
            //console.writeline("give value string");
            //test2 = console.readline();
            //connector.insert(test_name,test2);
            //Console.WriteLine("Items: "+connector.Count());

            //timer.Stop();
            //Console.WriteLine("Time: " + timer.ElapsedMilliseconds);
            //Console.ReadKey();


            //Console.WriteLine("give value string");
            //test_name = Console.ReadLine();
            //Console.WriteLine("give table string");
            //test2 = Console.ReadLine();
            //List<string>[] testList = new List<string>[3];
            //testList = connector.Select("select * from ps_cent_control_data");
            //int highest = 0;
            //foreach (var str in testList[0])
            //{
            //    if (highest < Convert.ToInt32(str))
            //    {
            //        highest = Convert.ToInt32(str);
            //    }
            //}
            //Console.WriteLine("highest: " + highest);
            //Console.ReadKey();
            //for (int index = 0; index < testList[2].Count; index++)
            //{
            //    Console.WriteLine(testList[0][index] + " | " + testList[1][index] + " | " + testList[2][index]);
            //}

            //Console.WriteLine("L1: " + testList[0].Count);
            //List<string>[] testList2 = new List<string>[3];
            //testList2 = sorter.createCombinedList(testList);
            //Console.ReadKey();
            //int highest2 = 0;
            //foreach (var str in testList2[0])
            //{
            //    if (highest2 < Convert.ToInt32(str))
            //    {
            //        highest2 = Convert.ToInt32(str);
            //    }
            //}
            //Console.WriteLine("highest: " + highest2);
            //Console.ReadKey();
            //Console.WriteLine("Done1!");
            //Console.ReadKey();
            //Console.WriteLine("index: "+ testList2[2].Count);
            //Console.WriteLine("LÆNGDE! " + testList2[2].Count);



            //for (int index = 0; index < testList2[0].Count; index++)
            //{
            //    Console.WriteLine(testList2[0][index] + " | " + testList2[1][index] + " | " + testList2[2][index]);
            //}



            //Console.WriteLine("L2: " + testList2[0].Count);
            //Console.WriteLine("Done2!");
            //Console.ReadKey();
            //string temp = "test";
            //for (int index = 0; index < testList2[2].Count - 1; index++)
            //{
            //    Console.WriteLine(testList2[0][index] + " | " + testList2[2].Count);

            //    if (testList2[0][index] == temp)
            //    {
            //        Console.ReadKey();
            //    }
            //    temp = testList2[0][index];
            //}
            //Console.WriteLine("Done3!");
            //Console.ReadKey();

            //for (int i = 0; i < testList2[2].Count; i++)
            //{
            //    controller.InitialAddItem(new Item(testList2[0][i]), sorter.locationStringToList(testList2[2][i]));
            //}

            //Console.WriteLine("Count items: " + controller.UnPartitionedLocations["000A00"].Items.Count);
            //Console.WriteLine("Count locations: " + controller.UnPartitionedLocations.Count);
            //foreach (Location itemss in controller.UnPartitionedLocations.Values)
            //{
            //    Console.WriteLine("name locations: " + itemss.ID);
            //}
            //int x = 0;
            //foreach (string stringthing in testList2[2])
            //{
            //    if (stringthing != "000A00")
            //    {
            //        Console.WriteLine("fejl: " + stringthing + " | " + testList2[0][x]);
            //    }
            //    x++;
            //}
            List<Item> items = new List<Item>();
            items = presta.GetAllItems();
            int maxquan = 0;
            int totalquan = 0;
            int emptylocations = 0;
            int nonemptylocations = 0;
            int negativequan = 0;
            string itemidlast = string.Empty;
            foreach (var itemmm in items)
            {
                totalquan += itemmm.ServerQuantity;
                Console.WriteLine("ID: " + itemmm.ID + " | QA: " + itemmm.ServerQuantity + " | CO: " + itemmm.Color + " | SI: " + itemmm.Size + " | LO: " + itemmm.Locations[0].ID + " | IM: " + itemmm.ImageUrl);
                if (maxquan < itemmm.ServerQuantity)
                {
                    maxquan = itemmm.ServerQuantity;
                }
                if (itemmm.Locations.Count == 0)
                {
                    emptylocations++;
                }
                if (itemmm.ServerQuantity > 0)
                {
                    nonemptylocations++;
                }
                if (itemmm.ServerQuantity < 0)
                {
                    //Console.ReadKey();
                    negativequan++;
                }
                if (itemmm.Locations.Count > 1)
                {
                    Console.ReadKey();
                }
                if (itemidlast == itemmm.ID)
                {
                    Console.ReadKey();
                }
                itemidlast = itemmm.ID;
            }
            Console.WriteLine("maxquan: " + maxquan);
            Console.WriteLine("total: " + totalquan);
            Console.WriteLine("empty: " + emptylocations);
            Console.WriteLine("!empty: " + nonemptylocations);
            Console.WriteLine("Negative quan: " + negativequan);
            Console.WriteLine("Done1!");

            controller.InitialPartitionUnpartitionedLocations();

            Central_Controller.Client client = new Central_Controller.Client("01");
            Console.WriteLine("Done2!");
            controller.AddClient(client);

            //Console.WriteLine("TestPartition Count: " + TestPartition.Locations[0].Items.Count);
            Console.WriteLine("Done3!");
            Server server = new Server(controller);
            //Thread NetworkingThread = new Thread(new ThreadStart(server.StartServer));
            //NetworkingThread.Start();

            List<string> temp = new List<string>();
            foreach (Item _item in items)
            {

                foreach (Location _location in _item.Locations)
                {
                    temp.Add(_location.ID);
                }
                controller.InitialAddItem(_item, temp);
                temp.Clear();
                _item.Locations.Clear();
            }
            controller.InitialPartitionUnpartitionedLocations();


            server.StartServer();
            Console.WriteLine("Done4!");





            //server.SendPartition(TestPartion);




            timer.Stop();
            Console.WriteLine("Time: " + timer.ElapsedMilliseconds);
            Console.WriteLine("Done5!");
            Console.ReadKey();
        }
    }
}
