using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioChat
{
    public class ChatGPTMessage
    {
        public ChatGPTMessageContext content { get; set; }
    }

    public class ChatGPTMessageContext
    {
        public string content_type { get; set; }
        public List<string> parts { get; set; }
    }
}
