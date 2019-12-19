namespace SAScanApp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
       
    public class CheckSum
    {
        private string _barcode { get; set; } = "xxxxxxxxxx";
        public bool CheckSumValidation(string barcode)
        {
            ulong SumOfOdds = 0;
            ulong SumOfEvens = 0;
            int checkSum = 0;
            int CheckDigit = 0;
            if(_barcode != "xxxxxxxxxx") { _barcode = barcode; }          
            List<ulong> CheckSumCalculation = new List<ulong>();
            
            foreach(char c in _barcode)
            {
                CheckSumCalculation.Add((ulong)c);
            }

            int count = CheckSumCalculation.Count;
            
            if (count != 13)
            {
                Console.WriteLine("Invalid barcode");
            }
            else
            {
                SumOfOdds += CheckSumCalculation[0];
                SumOfEvens += CheckSumCalculation[1];

                CheckDigit = _barcode.Last();
                
                for (int i = 2; i < count; i++)
                {

                    /* 0 is an even number, but since we'd usually start from 1,
                    then the evens represent odds and vice versa */
                    if (i % 2 == 0)
                    {
                        SumOfOdds += CheckSumCalculation[i];
                    }
                    else
                    {
                        SumOfEvens += CheckSumCalculation[i];
                    }
                }
            }

            checkSum = Convert.ToInt32(((3 * SumOfOdds) + SumOfEvens));

            int NearestMultipleOf10 = (((10 - (checkSum % 10)) + checkSum));

            // Finds nearest multiple of 10
            if ((NearestMultipleOf10 - checkSum) == CheckDigit){
                return true;
            }

            return false;

        }
    }

    // Der mangler kode for at tjekke om koden er UPC-A, måske bare være dovne og læse ind fra ZXing
}
