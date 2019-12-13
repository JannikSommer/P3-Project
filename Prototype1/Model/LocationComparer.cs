using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class LocationComparer : IComparer<Location>
    {
        public int[] ShelfHierarchy { get; private set; }
        private int HighestValueShelf;

        public LocationComparer(int highestShelfNumber)
        {
            ShelfHierarchy = new int[highestShelfNumber + 1];
            HighestValueShelf = highestShelfNumber;

            for(int x = 0; x <= highestShelfNumber; x++)
            {
                ShelfHierarchy[x] = x;
            }
        }

        public LocationComparer(int[] ShelfHierachyList)
        {
            ConvertHierachyList(ShelfHierachyList);
        }

        int IComparer<Location>.Compare(Location a, Location b)
        {
            
            int ShelfComparison = ShelfHierarchy[a.Shelf] - ShelfHierarchy[b.Shelf];
            int x;

            if (a.Shelf > HighestValueShelf || ShelfHierarchy[a.Shelf] < 0)
            {
                throw new Exception("Shelf " + a.Shelf + " doesn't currently exist in the shelf hierachy");
            }

            if (b.Shelf > HighestValueShelf || ShelfHierarchy[b.Shelf] < 0)
            {
                throw new Exception("Shelf " + b.Shelf + " doesn't currently exist in the shelf hierachy");
            }

            if (ShelfComparison != 0)
            {
                return ShelfComparison;
            }
            else
            {
                x = a.Position.CompareTo(b.Position);

                if (x == 0)
                {
                    return a.Row.CompareTo(b.Row);
                }

                return x;
            }
        }

        private void ConvertHierachyList(int[] HierachyListArray)
        {
            HighestValueShelf = -1;

            for(int x = 0; x < HierachyListArray.Length; x++)
            {
                if(HierachyListArray[x] > HighestValueShelf)
                {
                    HighestValueShelf = HierachyListArray[x];
                }
            }

            ShelfHierarchy = new int[HighestValueShelf + 1];

            for(int x = 0; x < ShelfHierarchy.Length; x++)
            {
                ShelfHierarchy[x] = -1;
            }

            for(int x = 0; x < HierachyListArray.Length; x++)
            {
                ShelfHierarchy[HierachyListArray[x]] = x;
            }
        }

        public void IncreasePriority(int PositionOfElement)
        {
            int temp;

            if (ShelfHierarchy[PositionOfElement] < HighestValueShelf)
                temp = ++ShelfHierarchy[PositionOfElement];
            else
                throw new Exception("Can't increase priority further");

            for(int x = 0; x <= HighestValueShelf; x++)
            {
                if (ShelfHierarchy[x] == temp)
                {
                    if (x != PositionOfElement)
                    {
                        ShelfHierarchy[x]--;
                        break;
                    }
                }
            }
        }

        public void DecreasePriority(int PositionOfElement)
        {
            int temp;

            if (ShelfHierarchy[PositionOfElement] > 0)
                temp = --ShelfHierarchy[PositionOfElement];
            else
                throw new Exception("Can't decrease priority further");

            for (int x = 0; x <= HighestValueShelf; x++)
            {
                if (ShelfHierarchy[x] == temp)
                {
                    if (x != PositionOfElement)
                    {
                        ShelfHierarchy[x]++;
                        break;
                    }
                }
            }
        }

        public int ShelfHierarchyOf(int shelfIndex)
        {
            return ShelfHierarchy[shelfIndex];
        }

        /*int IComparer<Location>.Compare(Location a, Location b)
        {
            for(int x = 0; x < HighestValueShelf; x++)
            {
                if(ShelfHierakyi[x] == a.Shelf & ShelfHierakyi[x] != b.Shelf)
                {
                    return -1;
                }
                else if (ShelfHierakyi[x] != a.Shelf & ShelfHierakyi[x] == b.Shelf)
                {
                    return 1;
                }
                else if (ShelfHierakyi[x] == a.Shelf & ShelfHierakyi[x] == b.Shelf)
                {
                    x = a.Row.CompareTo(b.Row);

                    if(x == 0)
                    {
                        return a.Position.CompareTo(b.Position);
                    }

                    return x;
                }
            }

            throw new Exception("Shelfs doesn't exist in hierarki array");
        }*/
    }
}
