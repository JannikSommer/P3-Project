using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Cycle
    {
        bool DataFormatted { get; set; }
        bool CycleState { get; set; }
        List<Partition> Partitions { get; set; }

        public Cycle()
        {

        }

        void DownloadFromServer()
        {
            // Download data from the StreetAmmo server
            throw new NotImplementedException();
        }

        void UploadToServer()
        {
            // Uploads data to the StreetAmmo server
            throw new NotImplementedException();
        }

        Partition CreatePartition()
        {
            Partition _newPartition = new Partition();

            //Algoritme som uddeler items.

            return _newPartition;
        }

        VerificationPartition CreateVerificationPartition()
        {
            VerificationPartition output = new VerificationPartition();

            return output;
        }
    }
}
