using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments
{
    public class MessageDeletedEventBusArgs : IEventBusArgs
    {
        public int MessageId { get; private set; }
        public int ChatId { get; private set; }

        public MessageDeletedEventBusArgs(int messageId, int chatId)
        {
            MessageId = messageId;
            ChatId = chatId;
        }
    }
}
