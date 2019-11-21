using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SAScanApp
{
   
    public enum PartitionState { NotCounted, Counted, Verified }
    public enum PartitionRequsitionState { Requested, Uploaded, Recieved }

    public class Partition
    {
        public PartitionState State { get; set; }
        public int TotalNrOFItems { get; set; }
        public int ItemsCounted { get; set; }
        public PartitionRequsitionState RequsitionState { get; set; }
        public List<Model.Location> Locations { get; set; }

        public Partition()
        {
            State = PartitionState.NotCounted;
            RequsitionState = PartitionRequsitionState.Requested;
            TotalNrOFItems = 0;
            ItemsCounted = 0;
            Locations = new List<Model.Location>();
        }

        public void AddLocation(Model.Location _Location)
        {
            Locations.Add(_Location);
            TotalNrOFItems += _Location.Items.Count;
        }
    }
}

