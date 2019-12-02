namespace SAScanApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
       
    class CheckSumCalc
    {
        

        public ulong CheckSumValidation(string _Barcode)
        {
            ulong SumOfOdds = 0;
            ulong SumOfEvens = 0;
            ulong CheckSum = 0;
            int CheckDigit = 0;
            int CheckSumCalcation = Convert.ToInt32(_Barcode);
            int count = _Barcode.Length;

           

            if (_Barcode.Length != 12)
            {
                Console.WriteLine("Invalid barcode");
            }
            else
            {
                CheckDigit = _Barcode.Last();

                for (int i = 2; i < count; i++)
                {

                    /* 0 is an even number, but since we'd usually start from 1,
                    then the evens represent odds and vice versa */
                    SumOfOdds += ;
                    SumOfEvens += ;

                    if (i % 2 == 0)
                    {
                        SumOfOdds += ;
                    }
                    else
                    {
                        SumOfEvens += ;
                    }
                }
            }

            CheckSum = ((3 * SumOfOdds) + SumOfEvens);

            ulong NearestMultipleOf10 = (((10 - (CheckSum % 10)) + CheckSum) - CheckSum);

            // Finds nearest multiple of 10
            if (NearestMultipleOf10 == (ulong)CheckDigit){
                return CheckSum;
            }

            return 0;

          


        }
    }




}
