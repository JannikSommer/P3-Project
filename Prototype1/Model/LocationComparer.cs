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
            ShelfHierakyi = new int[highestShelfNumber];
            HighestValueShelf = highestShelfNumber;

            for(int x = 0; x <= highestShelfNumber + 1; x++)
            {
                ShelfHierakyi[x - 1] = x;
            }
        }

        int IComparer<Location>.Compare(Location a, Location b)
        {
            int ShelfHierakyi_a = ShelfHierakyi[a.Shelf];
            int ShelfHierakyi_b = ShelfHierakyi[b.Shelf];
            int x;

            if(ShelfHierakyi_a > ShelfHierakyi_b)
            {
                return -1;
            }
            else if(ShelfHierakyi_a < ShelfHierakyi_b)
            {
                return 1;
            }
            else
            {
                x = a.Row.CompareTo(b.Row);

                if (x == 0)
                {
                    return a.Posistion.CompareTo(b.Posistion);
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
