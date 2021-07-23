using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.AzureServiceBus.Models
{
    public class QueueInfo
    {
        public QueueInfo(string name, string status, long activeMessages, long deadletterMessages)
        {
            Name = name;
            Status = status;
            ActiveMessagesCount = activeMessages;
            DeadletterMessagesCount = deadletterMessages;
        }

        public string Name { get; set; }
        public string Status { get; set; }
        public long ActiveMessagesCount { get; set; }
        public long DeadletterMessagesCount { get; set; }
    }
}
