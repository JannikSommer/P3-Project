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

namespace SQL_DB_test_Frame
{
    class Program
    {
        static void Main(string[] args)
        {
            DB_connector connector = new DB_connector();
            Stopwatch timer = new Stopwatch();
            Sorting sorter = new Sorting();
            Item item = new Item();
            Location location = new Location();
            Controller controller = new Controller();
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
            List<string>[] testList = new List<string>[3];
            testList = connector.Select("*", "ps_cent_control_data");
            for (int index = 0; index < testList[2].Count - 1; index++)
            {
                Console.WriteLine(testList[0][index] + " | " + testList[1][index] + " | " + testList[2][index]);
                //Console.ReadKey();
            }


            List<string>[] testList2 = new List<string>[3];
            testList2 = sorter.createCombinedList(testList);
            Console.ReadKey();
            //Console.WriteLine("Done1!");
            //Console.ReadKey();
            //Console.WriteLine("index: "+ testList2[2].Count);
            //for (int index = 0; index < testList2[2].Count - 1; index++)
            //{
            //    Console.WriteLine(testList2[0][index] + " | " + testList2[1][index] + " | " + testList2[2][index]);
            //}
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

            for (int i = 0; i < testList2[2].Count; i++)
            {
                controller.InitialAddItem(new Item(testList2[0][i]), sorter.locationStringToList(testList2[2][i]));
            }

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
            Console.WriteLine("Done1!");
            controller.InitialPartitionUnpartitionedLocations();

            Central_Controller.Client client = new Central_Controller.Client("01");
            Console.WriteLine("Done2!");
            controller.AddClient(client);

            //Console.WriteLine("TestPartition Count: " + TestPartition.Locations[0].Items.Count);
            Console.WriteLine("Done3!");
            Server server = new Server(controller);
            Thread NetworkingThread = new Thread(new ThreadStart(server.StartServer));
            NetworkingThread.Start();
            // server.StartServer();
            // Console.WriteLine("Done4!");
            // server.SendPartition(TestPartition);



            timer.Stop();
            Console.WriteLine("Time: " + timer.ElapsedMilliseconds);
            Console.WriteLine("Done5!");
            Console.ReadKey();
        }
    }
}
