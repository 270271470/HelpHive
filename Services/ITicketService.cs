﻿using HelpHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Services
{
    public interface ITicketService
    {
        TicketModel CurrentTicket { get; }
        void Ticket(TicketModel ticket);
    }
}