using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace WPF_PC.Central_Controller
{
    class Controller
    {
        private List<Client> Clients;

        public Controller()
        {
            Clients = new List<Client>();
        }

        public void SendNextPartition()
        {
            throw new NotImplementedException();
        }
    }
}
