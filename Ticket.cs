using System.Collections;
using System.Collections.Generic;

namespace A1_TicketingSystem
{
    class Ticket
    {
        public string TicketID { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Submitter { get; set; }
        public string Assigned { get; set; }
        public IList<string> Watching { get; set; }

        public static string HeaderLine()
        {
            return "TicketID,Summary,Status,Priority,Submitter,Assigned,Watching";
        }

        public override string ToString()
        {
            string retStr = "";
            string csv = ",";
            string watchSep = "|";
            string watching = "";

            foreach (string watcher in Watching)
            {
                if (watching.Length > 0) watching += watchSep;
                watching += watcher;
            }

            retStr = $"{TicketID}{csv}{Summary}{csv}{Status}{csv}{Priority}{csv}{Submitter}{csv}{Assigned}{csv}{watching}";

            return retStr;
        }

        public Ticket(string ticketId, string summary, string status, string priority, string submitter, string assigned, IList<string> watching)
        {
            this.TicketID = ticketId;
            this.Summary = summary;
            this.Priority = priority;
            this.Submitter = submitter;
            this.Assigned = assigned;
            this.Watching = watching;
        }

        public Ticket(string csvLine, string fieldSep = ",", string subFieldSep = "|")
        {
            string[] fields = csvLine.Split(fieldSep);
            if (fields.Length > 0) TicketID = fields[0];
            if (fields.Length > 1) Summary = fields[1];
            if (fields.Length > 2) Status = fields[2];
            if (fields.Length > 3) Priority = fields[3];
            if (fields.Length > 4) Submitter = fields[4];
            if (fields.Length > 5) Assigned = fields[5];
            if (fields.Length > 6) this.SetWatching(fields[6], subFieldSep);
        }

        public void SetWatching(string watchingLine, string fieldSep = "|")
        {
            string[] fields = watchingLine.Split(fieldSep);
            IList<string> watching = new List<string>();
            foreach (string field in fields)
            {
                watching.Add(field);
            }
            Watching = watching;
        }
    }
}