using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class Cycle
    {
        bool DataFormatted { get; set; }
        List<Partition> Partitions { get; set; }
        bool CycleState { get; set; }

        public Cycle()
        {

        }

        void DownloadFromServer()
        {
            throw new NotImplementedException();
        }
        void UploadToServer()
        {
            throw new NotImplementedException();
        }
        Partition CreatePartition()
        {
            Partition _newPartition = new Partition();

            //Algoritme som uddeler items.

            return _newPartition;
            //.
        }
        VerificationPartition CreateVerificationPartition()
        {
            VerificationPartition output = new VerificationPartition();

            return output;

            //.
        }




    }
}
