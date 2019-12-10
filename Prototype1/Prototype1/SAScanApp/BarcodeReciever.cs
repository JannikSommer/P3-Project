using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SAScanApp
{
   public class BarcodeReciever
    {
        private string barc { get; set; }
        public ObservableCollection<string> barcodes { get; set; } = new ObservableCollection<string>();

        public bool RecieveBarcode(Partition partition)
        {
            char[] input = new char[13];
            string tempBc = string.Empty;
            
            for(int i = 0; i < 13; i++)
            {
                if(Console.Read() != '\n')
                {
                    input[i] = Console.ReadKey().KeyChar;
                }
            }

            foreach(var element in input)
            {
                 tempBc += element;
            }

            if(tempBc.Length == 13)
            {
                barc = tempBc;
            }

            /*MessagingCenter.Subscribe<Object, string> (this, "Barcode", (a, s) =>
            {

                barc = a.ToString();
                barcodes.Add(a.ToString());                           
                
            }); */

            var Checker = new CheckSum(); 

            if (Checker.CheckSumValidation(barc) == true)
            {

                List<Item> currentItems = new List<Item>();
                int j = 0;

                for (int i = 0; i < partition.Locations.Count; i++)
                {
                    for(j = 0; j < partition.Locations[i].Items.Count; j++)
                    {
                        if (partition.Locations[i].Items[j].ID == barc)
                        {
                            partition.Locations[i].Items[j].CountedQuantity++;

                            return true;
                        }
                    }

                    
                }
            }

            return false;
        }
    }
}
    