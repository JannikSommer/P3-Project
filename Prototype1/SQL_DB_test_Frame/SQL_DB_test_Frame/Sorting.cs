using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace SQL_DB_test_Frame
{
    public class Sorting
    {
        public List<string>[] createCombinedList(List<string>[] list)
        {
            //Create a list to store the result
            List<string>[] combinedList = new List<string>[3];
            combinedList[0] = new List<string>();
            combinedList[1] = new List<string>();
            combinedList[2] = new List<string>();

            int longestIndex = findLargestList(list);

            bool found = false;

            for (int i = 0; i < list[0].Count; i++)
            {
                found = false;
                int j_count = combinedList[0].Count;
                for (int j = 0; j < j_count; j++)
                {
                    if (list[0][i] == combinedList[0][j])
                    {
                        combinedList[1][j] = (Int32.Parse(combinedList[1][j]) + Int32.Parse(list[1][i])).ToString();
                        combinedList[2][j] = locationCombiner(combinedList[2][j], list[2][i]);

                        found = true;
                    }
                }
                if (!found)
                {
                    combinedList[0].Add(list[0][i]);
                    combinedList[1].Add(list[1][i]);
                    combinedList[2].Add(locationChecker(list[2][i]));
                }
            }
            for (int i = 0; i < combinedList[2].Count; i++)
            {
                string LocationString = combinedList[2][i];
                combinedList[2][i] = locationCleaner(LocationString);
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
        public List<Location> locationStringToList(string locationString)
        {
            List<Location> locationList = new List<Location>();
            foreach (var location in locationString.Split(';'))
            {
                locationList.Add(new Location (location));
            }
            return locationList;
        }
        private string locationCombiner(string org, string add)
        {
            org += ";" + locationChecker(add);
            return org;
        }
        public string locationChecker(string location)
        {
            if (location == " ")
            {
                return "999Z99";
            }
            if (location == "")
            {
                return "999Z99";
            }
            if (location == null)
            {
                return "999Z99";
            }
            if (location.Length == 6)
            {
                return location;
            }
            if (location.Length < 6)
            {
                return "999Z99";
            }
            if (location.Length > 6)
            {
                bool semiFound = false;
                for (int i = 0; i < location.Length; i++)
                {
                    if (location[i] == ';')
                    {
                        semiFound = true;
                    }
                }
                if (semiFound)
                {
                    List<string> locationList = new List<string>();
                    string Final = string.Empty;
                    foreach (var difLocations in location.Split(';'))
                    {
                        locationList.Add(difLocations);
                    }
                    foreach (var difLocation in locationList)
                    {
                        Final = locationCombiner(Final,locationChecker(difLocation));
                    } 
                }
                return "999Z99";
            }
            throw new Exception("Error! Unable to handle location:" + location);
        }
        public string locationCleaner(string locationString)
        {
            List<string> locationList = new List<string>();
            List<string> returnLocationList = new List<string>();
            foreach (var Location in locationString.Split(';'))
            {
                locationList.Add(Location);
            }
            bool dubFound = false;
            for (int i = 0; i < locationList.Count; i++)
            {
                for (int j = 0; j < returnLocationList.Count; j++)
                {
                    if (locationList[i] == returnLocationList[j])
                    {
                        dubFound = true;
                    }
                }
                if (!dubFound)
                {
                    returnLocationList.Add(locationList[i]);
                }
            }
            string returnString = returnLocationList[0];
            for (int i = 1; i < returnLocationList.Count; i++)
            {
                returnString = locationCombiner(returnString, returnLocationList[i]); 
            }
            return returnString;
        }
    }
}
