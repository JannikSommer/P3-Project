using System;
using Model;

namespace Central_Controller.Central_Controller
{
    public class User : IComparable {
        public User() { } // Used for JSON serialization
        public User(string id) {
            ID = id;
        }

        public string ID { get; }
        public bool IsAdmin { get; set; } = false;
        public Partition CurrentPartition {
            get {
                return _currentPartition;
            }
            set {
                LastActivity = DateTime.Now;
                _currentPartition = value;
            }
        }
        public VerificationPartition CurrentVerificationPartition {
            get {
                return _CurrentVerificationPartition;
            }
            set {
                LastActivity = DateTime.Now;
                _CurrentVerificationPartition = value;
            }
        }
        
        private Partition _currentPartition;
        private VerificationPartition _CurrentVerificationPartition;
        private DateTime LastActivity;


        public bool IsAFK(TimeSpan TimeBeforeAFK) {
            TimeSpan TimeSinceLastActivity = DateTime.Now.Subtract(LastActivity);
            return TimeSinceLastActivity < TimeBeforeAFK ? false : true;
        }

        public int CompareTo(object obj) {
            return ID.CompareTo(((User)obj).ID);
        }
    }
}
