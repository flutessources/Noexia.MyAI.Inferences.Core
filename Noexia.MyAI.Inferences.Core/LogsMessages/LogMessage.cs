using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noexia.MyAI.Inferences.Core.Logs
{
    public struct LogMessage
    {
        public string Title { get; private set; }
        public string Message { get; private set; }
        public ELogMessage Type { get; private set; }

        public LogMessage(string title, string message, ELogMessage type)
        {
            Title = title;
            Message = message;
            Type = type;
        }

        public LogMessage(string title, string message) : this(title, message, ELogMessage.Default)
        {
        }

        public LogMessage(string message, ELogMessage type) : this(string.Empty, message, type)
        {
        }

        public LogMessage(string message) : this(string.Empty, message, ELogMessage.Default)
        {
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return string.Format("{0} : {1}", Type.ToString(), Message);
            }
            else
            {
                return string.Format("{0} : {1}", Title, Message);
            }
        }
    }
}
