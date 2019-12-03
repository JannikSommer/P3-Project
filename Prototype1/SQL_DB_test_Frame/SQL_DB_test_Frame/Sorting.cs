using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB_test_Frame
{
    class Sorting
    {
        public List<string>[] createCombinedList(List<string>[] list)
        {
            //Create a list to store the result
            List<string>[] combinedList = new List<string>[3];
            combinedList[0] = new List<string>();
            combinedList[1] = new List<string>();
            combinedList[2] = new List<string>();

            int longestIndex = findLargestList(list);

            //combinedList[0].Add(list[0][0]);
            //combinedList[1].Add(list[1][0]);
            //combinedList[2].Add(list[2][0]);

            bool found = false;

            for (int i = 0; i < list[0].Count && i < 10; i++)
            {
                found = false;
                int j_count = combinedList[0].Count;
                for (int j = 0; j < j_count; j++)
                {
                    if (list[0][i] == combinedList[0][j])
                    {
                        combinedList[1][j] = (Int32.Parse(combinedList[1][j]) + Int32.Parse(list[1][i])).ToString();
                        //combinedList[2][j] = /*locationCleaner(combinedList[2][j], list[2][i]);*/combinedList[2][j] + ";" + "000A00";//list[2][i];

                        found = true;
                    }
                }
                if (!found)
                {
                    combinedList[0].Add(list[0][i]);
                    combinedList[1].Add(list[1][i]);
                    combinedList[2].Add("000A00"/*list[2][i]*/);
                }
            }

            return combinedList;
        }
        private int findLargestList(List<string>[] list)
        {
            int longestIndex = 0;
            for (int i = 0; i < list.Length; i++)
            {
                if (longestIndex < list[i].Count)
                {
                    longestIndex = i;
                }
            }
            return longestIndex;
        }
        public List<string> locationStringToList(string locationString)
        {
            List<string> locationList = new List<string>();
            foreach (var location in locationString.Split(';'))
            {
                locationList.Add(location);
            }
            return locationList;
        }
        //private void locationCleaner(string org, string add)
        //{
        //    List<string> originalLocations;
        //    for (int i = 0; i < length; i++)
        //    {
        //        if (true)
        //        {

        //        } 
        //    }
        //}
    }
}
