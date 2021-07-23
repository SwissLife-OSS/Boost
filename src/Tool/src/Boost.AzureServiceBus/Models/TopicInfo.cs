using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.AzureServiceBus.Models
{
    public class TopicInfo
    {
        public TopicInfo(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
