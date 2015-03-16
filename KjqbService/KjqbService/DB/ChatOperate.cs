using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService.DB
{
    public class ChatOperate:DBoperate
    {
        public long InsertIntoEntity(ChatInService l)
        {
            ChatMessage lm = new ChatMessage();

            lm.ChatContent = l.ChatContent;
            lm.SendUserId = l.SendUserId;
            lm.ReceiveUserId = l.ReceiveUserId;
            lm.TimeStamp = DateTime.Now.Ticks;
            lm.State = 0;
            lm.IsRead = 0;
            context.ChatMessages.Add(lm);
            context.SaveChanges();
            return (long)lm.Id;
        }
        public List<ChatMessage> SendChat(long l)
        {
            List<ChatMessage> loglists = new List<ChatMessage>();
            loglists = context.ChatMessages.Where(m => m.ReceiveUserId == l && m.State == 0).OrderByDescending(m => m.TimeStamp).ToList();
            foreach (ChatMessage lll in loglists)
            {
                lll.State = 1;
                context.SaveChanges();
            }
            return loglists;
        }

        public List<ChatMessage> SendChatUnRead(long l)
        {
            List<ChatMessage> loglists = new List<ChatMessage>();
            loglists = context.ChatMessages.Where(m => m.ReceiveUserId == l && m.IsRead == 0 && m.State == 1).OrderByDescending(m => m.TimeStamp).ToList();

            return loglists;
        }

        public List<ChatMessage> ChangeChatIsRead(long l)
        {
            List<ChatMessage> loglists = new List<ChatMessage>();
            loglists = context.ChatMessages.Where(m => m.ReceiveUserId == l && m.IsRead == 0).ToList();
            foreach (ChatMessage lll in loglists)
            {
                lll.IsRead = 1;
                context.SaveChanges();
            }
            return loglists;
        }

    }
}