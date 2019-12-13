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
        public bool IsAdmin = false;
        private Partition _currentPartition;
        private VerificationPartition _CurrentVerificationPartition;
        private DateTime LastActivity;

        public Partition CurrentPartition
        {
            get
            {
                return _currentPartition;
            }
            set
            {
                LastActivity = DateTime.Now;
                _currentPartition = value;
            }
        }

        public VerificationPartition CurrentVerificationPartition
        {
            get
            {
                return _CurrentVerificationPartition;
            }
            set
            {
                LastActivity = DateTime.Now;
                _CurrentVerificationPartition = value;
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
