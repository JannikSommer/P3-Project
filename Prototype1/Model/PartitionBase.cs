using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class PartitionBase
    {
        public PartitionState State { get; set; }
        public int TotalNrOFItems { get; set; }
        public int ItemsCounted { get; set; }
        public PartitionRequsitionState RequsitionState { get; set; }
        public string Employee { get; set; }
    }
}