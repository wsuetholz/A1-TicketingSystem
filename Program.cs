using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace A1_TicketingSystem
{
    class Program
    {
        // Prompt for action..
        // Returns (0 = Quit, 1 = List Records, 2 = Add Record, 3 = Save List)
        static int Prompt()
        {
            int ret = -1;    

            while (ret == -1)
            {
                Console.WriteLine("Please Select Command (A)dd, (L)ist, (S)ave, (Q)uit:");
                string resp = Console.ReadLine().ToUpper();
                if (resp.Length > 0)
                {
                    string respCh = resp.Substring(0, 1);
                    if (respCh.Equals("Q"))
                    {
                        ret = 0;
                    }
                    else if (respCh.Equals("L"))
                    {
                        ret = 1;
                    }
                    else if (respCh.Equals("A"))
                    {
                        ret = 2;
                    }
                    else if (respCh.Equals("S"))
                    {
                        ret = 3;
                    }
                }
            }
            return ret;
        }

        static IList<Ticket> ReadList(string ticketFName)
        {
            IList<Ticket> tickets = new List<Ticket>();

            if (File.Exists(ticketFName))
            {
                bool firstLine = false;
                StreamReader sr = new StreamReader(ticketFName);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (!firstLine)
                    {
                        firstLine = true;
                        continue;
                    }
                    Ticket ticket = new Ticket(line);
                    tickets.Add(ticket);
                }
                sr.Close();
            }

            return tickets;
        }

        static void SaveList (IList<Ticket> tickets, string ticketFName)
        {
            StreamWriter sw = new StreamWriter(ticketFName);

            sw.WriteLine(Ticket.HeaderLine());

            foreach (Ticket ticket in tickets)
            {
                sw.WriteLine(ticket.ToString());
            }

            sw.Close();
        }

        static int GetMaxTicketID(IList<Ticket> tickets)
        {
            int maxTicketId = 0;

            foreach (Ticket ticket in tickets)
            {
                try
                {
                    int ticketId = Int32.Parse(ticket.TicketID);
                    if (ticketId > maxTicketId) maxTicketId = ticketId;
                }
                catch (Exception ex)
                {

                }
            }

            return maxTicketId;
        }

        static void ShowList(IList<Ticket> tickets)
        {
            Console.WriteLine("Current outstanding problem tickets are:\n");
            Console.WriteLine(Ticket.HeaderLine());
            foreach (Ticket ticket in tickets)
            {
                Console.WriteLine(ticket);
            }
        }

        static Ticket EnterTicket(int ticketId)
        {
            string sTicketId = $"{ticketId}";
            bool ok = false;
            IList<string> watchers = new List<string>();
            Ticket ticket = new Ticket(sTicketId, "", "", "", "", "", watchers);
            string resp = "";
            string[] validStatus = {"OPEN", "CLOSED", "WAITING"};
            string[] validPriority = {"CRITICAL","HIGH","LOW","INFO","INFORMATIONAL"};

            Console.WriteLine($"Enter Information for Ticket ID {ticketId}:");

            do
            {
                Console.WriteLine("Summary:");
                resp = Console.ReadLine();
                if (resp.Length > 0)
                {
                    ticket.Summary = resp;
                    ok = true;
                }
            } while (!ok);

            ok = false;
            do
            {
                Console.WriteLine("Status(Open, Closed, Waiting):");
                resp = Console.ReadLine();
                if (resp.Length > 0)
                {
                    string checkStr = resp.ToUpper();
                    foreach (string chk in validStatus)
                    {
                        if (chk.Equals(checkStr))
                        {
                            ticket.Status = resp;
                            ok = true;
                            break;
                        }
                    }
                }
            } while (!ok);

            ok = false;
            do
            {
                Console.WriteLine("Priority(Critical,High,Low,Info)");
                resp = Console.ReadLine();
                if (resp.Length > 0)
                {
                    string checkStr = resp.ToUpper();
                    foreach (string chk in validPriority)
                    {
                        if (chk.Equals(checkStr))
                        {
                            ticket.Priority = resp;
                            ok = true;
                            break;
                        }
                    }
                }
            } while (!ok);

            ok = false;
            do
            {
                Console.WriteLine("Submitter:");
                resp = Console.ReadLine();
                if (resp.Length > 0)
                {
                    ticket.Submitter = resp;
                    ok = true;
                }
            } while (!ok);

            ok = false;
            do
            {
                Console.WriteLine("Assigned:");
                resp = Console.ReadLine();
                if (resp.Length > 0)
                {
                    ticket.Assigned = resp;
                    ok = true;
                }
            } while (!ok);

            ok = false;
            do
            {
                Console.WriteLine("Watcher(Blank to Finish):");
                resp = Console.ReadLine();
                if (resp.Length > 0)
                {
                    watchers.Add(resp);
                }
                else
                {
                    ticket.Watching = watchers;
                    ok = true;
                }
            } while (!ok);

            return ticket;
        }

        static void Main(string[] args)
        {
            IList<Ticket> tickets;
            string ticketFName = "Tickets.csv";
            int lastTicketId = 0;
            int cmdCode = -1;

            Console.WriteLine("Welcome to the Acme Problem Ticket Processing System!");

            tickets = ReadList(ticketFName);
            lastTicketId = GetMaxTicketID(tickets);

            do
            {
                cmdCode = Prompt();
                switch (cmdCode) // (0 = Quit, 1 = List Records, 2 = Add Record, 3 = Save List)
                {
                    case 0:
                    case 3:
                        SaveList(tickets, ticketFName);
                        break;
                    case 1:
                        ShowList(tickets);
                        break;
                    case 2:
                        tickets.Add(EnterTicket(++lastTicketId));
                        break;
                }
            } while (cmdCode != 0);
        }
    }
}
