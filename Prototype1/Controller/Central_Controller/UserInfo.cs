using System;

namespace Central_Controller.Central_Controller {
    internal class UserInfo : IComparable {
        public int PartitionIndex;
        public bool ReverseCount;
        public User Client;

        public UserInfo(User client, int partitionIndex, bool reverseCount) {
            Client = client;
            PartitionIndex = partitionIndex;
            ReverseCount = reverseCount;
        }

        public int CompareTo(object obj) {
            return PartitionIndex.CompareTo(((UserInfo)obj).PartitionIndex);
        }

        public void IncrementIndex() {
            if(ReverseCount) {
                PartitionIndex--;
            }
        }
    }
}
