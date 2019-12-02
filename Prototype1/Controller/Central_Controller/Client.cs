using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Central_Controller
{
    public class Client : IComparable
    {
        public string ID { get; }
        private Partition _CurrentPartition;
        private DateTime LastActivity;

        public Partition CurrentPartition
        {
            get
            {
                return _CurrentPartition;
            }
            set
            {
                LastActivity = DateTime.Now;
                _CurrentPartition = value;
            }
        }
        
        public Client(string _ID)
        {
            ID = _ID;
        }

        public Client() { } // Used for JSON serialization

        public bool IsAFK(TimeSpan TimeBeforeAFK)
        {
            TimeSpan TimeSinceLastActivity = DateTime.Now.Subtract(LastActivity);
            return TimeSinceLastActivity < TimeBeforeAFK ? false : true;
        }

        public int CompareTo(object obj)
        {
            return ID.CompareTo(((Client)obj).ID);
        }
    }
}
