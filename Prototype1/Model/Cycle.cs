using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Cycle
    {
        public bool DataFormatted { get; set; }
        public bool CycleState { get; set; }
        public List<PartitionBase> UncountedPartitions { get; set; }
        public List<PartitionBase> CountedPartitions { get; set; }
        public List<PartitionBase> VerifiedPartitions { get; set; }

        public Partition GetPartitionForClient()
        {
            Partition sendPartition = new Partition();
            foreach (Partition partition in UncountedPartitions)
            {
                if (partition.State == PartitionState.NotCounted)
                {
                    UncountedPartitions.Remove(partition); // should only be removed when the client uploads
                    sendPartition = partition;
                    break;
                }
            }
            return sendPartition;
        }

        public void ReceicePartitionUpload(Partition partition)
        {
            if (partition.State == PartitionState.NotCounted)
            {
                partition.State = PartitionState.Counted;
                CountedPartitions.Add(partition);
            }
            else if (partition.State == PartitionState.Counted)
            {
                partition.State = PartitionState.Verified;
                VerifiedPartitions.Add(partition);
            }
        }

        public void DownloadFromServer()
        {
            // Download data from the StreetAmmo server
            throw new NotImplementedException();
        }

        public void UploadToServer()
        {
            // Uploads data to the StreetAmmo server
            throw new NotImplementedException();
        }

        public Partition CreatePartition()
        {
            Partition _newPartition = new Partition();

            //Algoritme som uddeler items.

            return _newPartition;
        }

        public VerificationPartition CreateVerificationPartition()
        {
            VerificationPartition output = new VerificationPartition();

            return output;
        }
    }
}
