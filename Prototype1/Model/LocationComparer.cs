using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class LocationComparer : IComparer<Location>
    {
        public int[] ShelfHierakyi { get; private set; }
        private int HighestValueShelf;

        public LocationComparer(int highestShelfNumber)
        {
            ShelfHierakyi = new int[highestShelfNumber + 1];
            HighestValueShelf = highestShelfNumber;

            for(int x = 0; x <= highestShelfNumber; x++)
            {
                ShelfHierakyi[x] = x;
            }
        }

        int IComparer<Location>.Compare(Location a, Location b)
        {
            int ShelfComparison = ShelfHierakyi[a.Shelf] - ShelfHierakyi[b.Shelf];
            int x;

            if(ShelfComparison != 0)
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

        public void IncreasePriority(int PosistionOfElement)
        {
            int temp;

            if (ShelfHierakyi[PosistionOfElement] < HighestValueShelf)
                temp = ++ShelfHierakyi[PosistionOfElement];
            else
                throw new Exception("Can't increase priority further");

            for(int x = 0; x < HighestValueShelf; x++)
            {
                if (ShelfHierakyi[x] == temp)
                {
                    if (x == PosistionOfElement) { }
                    else
                    {
                        ShelfHierakyi[x]--;
                        break;
                    }
                }
            }
        }

        public void DecreasePriority(int PosistionOfElement)
        {
            int temp;

            if (ShelfHierakyi[PosistionOfElement] > 0)
                temp = --ShelfHierakyi[PosistionOfElement];
            else
                throw new Exception("Can't decrease priority further");

            for (int x = 0; x < HighestValueShelf; x++)
            {
                if (ShelfHierakyi[x] == temp)
                {
                    if (x == PosistionOfElement) { }
                    else
                    {
                        ShelfHierakyi[x]++;
                        break;
                    }
                }
            }
        }

        public int ShelfHierakyOf(int shelfIndex)
        {
            return ShelfHierakyi[shelfIndex];
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
                        return a.Posistion.CompareTo(b.Posistion);
                    }

                    return x;
                }
            }

            throw new Exception("Shelfs doesn't exist in hierarki array");
        }*/

        /*public void IncreasePriority(int PosistionOfElement)
        {
            int temp;

            if (PosistionOfElement > 0)
            {
                temp = ShelfHierakyi[PosistionOfElement];
                ShelfHierakyi[PosistionOfElement] = ShelfHierakyi[PosistionOfElement - 1];
                ShelfHierakyi[PosistionOfElement - 1] = temp;
            }
            else
            {
                throw new Exception("Can't increase priority further");
            }
        }*/

        /*public void DecreasePriority(int PosistionOfElement)
        {
            int temp;

            if (PosistionOfElement < HighestValueShelf)
            {
                temp = ShelfHierakyi[PosistionOfElement];
                ShelfHierakyi[PosistionOfElement] = ShelfHierakyi[PosistionOfElement + 1];
                ShelfHierakyi[PosistionOfElement + 1] = temp;
            }
            else
            {
                throw new Exception("Can't decrease priority further");
            }
        }*/
    }
}
