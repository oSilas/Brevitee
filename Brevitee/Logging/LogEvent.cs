using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Brevitee.Logging
{
    [Serializable]
    public class LogEvent
    {
        string source;
        string category;
        int eventid;
        string user;
        DateTime timeOccurred;
        string message;
        string computer;
        LogEventType type;

        public string Source
        {
            get { return this.source; }
            set { this.source = value; }
        }

        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        public string Computer
        {
            get { return this.computer; }
            set { this.computer = value; }
        }

        public LogEventType Severity
        {
            get { return type; }
            set { type = value; }
        }
        
        public string Category
        {
            get { return this.category; }
            set { this.category = value; }
        }

        public int EventID
        {
            get { return this.eventid; }
            set { this.eventid = value; }
        }

        public string User
        {
            get { return this.user; }
            set { this.user = value; }
        }

        public DateTime Time
        {
            get { return this.timeOccurred; }
            set { this.timeOccurred = value; }
        }

        [XmlIgnore]
        public string MessageSignature { get; set; }

        [XmlIgnore]
        public string[] MessageVariableValues { get; set; }

        public string StackTrace { get; set; }
    }
}
