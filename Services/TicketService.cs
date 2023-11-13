using HelpHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Services
{
    public class TicketService : ITicketService
    {
        public TicketModel CurrentTicket { get; private set; }
        public void Ticket(TicketModel ticket)
        {
            CurrentTicket = ticket;
        }
    }
}