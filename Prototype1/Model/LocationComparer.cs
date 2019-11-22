using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class LocationComparer : IComparer<Location>
    {
        public int[] ShelfHieraky { get; private set; }
        private int HighestValueShelf;

        public LocationComparer(int highestShelfNumber)
        {
            ShelfHieraky = new int[highestShelfNumber + 1];
            HighestValueShelf = highestShelfNumber;

            for(int x = 0; x <= highestShelfNumber; x++)
            {
                ShelfHieraky[x] = x;
            }
        }

        public int ShelfHierakyOf(int ShelfIndex)
        {
            return ShelfHieraky[ShelfIndex];
        }

        int IComparer<Location>.Compare(Location a, Location b)
        {
            int ShelfHierakyi_a = ShelfHieraky[a.Shelf];
            int ShelfHierakyi_b = ShelfHieraky[b.Shelf];
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
                x = a.Posistion.CompareTo(b.Posistion);

                if (x == 0)
                {
                    return a.Row.CompareTo(b.Row);
                }

                return x;
            }
        }

        public void IncreasePriority(int ShelfIndex)
        {
            int temp;

            if (ShelfHieraky[ShelfIndex] < HighestValueShelf)
                temp = ++ShelfHieraky[ShelfIndex];
            else
                throw new Exception("Can't increase priority further");

            for(int x = 0; x < HighestValueShelf; x++)
            {
                if (ShelfHieraky[x] == temp)
                {
                    if (x == ShelfIndex) { }
                    else
                    {
                        ShelfHieraky[x]--;
                        break;
                    }
                }
            }
        }

        public void DecreasePriority(int PosistionOfElement)
        {
            int temp;

            if (ShelfHieraky[PosistionOfElement] > 0)
                temp = --ShelfHieraky[PosistionOfElement];
            else
                throw new Exception("Can't decrease priority further");

            for (int x = 0; x < HighestValueShelf; x++)
            {
                if (ShelfHieraky[x] == temp)
                {
                    if (x == PosistionOfElement) { }
                    else
                    {
                        ShelfHieraky[x]++;
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
