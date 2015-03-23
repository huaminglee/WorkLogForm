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

        public List<ChatMessage> SendChat(long recevieId,long sendId)
        {
            List<ChatMessage> loglists = new List<ChatMessage>();
            loglists = context.ChatMessages.Where(m => m.ReceiveUserId == recevieId && m.SendUserId == sendId && m.State == 0).OrderByDescending(m => m.TimeStamp).ToList();
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

        public List<ChatMessage> SendChatUnRead(long receiveId, long sendId)
        {
            List<ChatMessage> loglists = new List<ChatMessage>();
            loglists = context.ChatMessages.Where(m => m.ReceiveUserId == receiveId && m.SendUserId == sendId && m.IsRead == 0 && m.State == 1).OrderByDescending(m => m.TimeStamp).ToList();

            return loglists;
        }

        public List<ChatMessage> ChangeChatIsRead(long receiveId, long sendId)
        {
            List<ChatMessage> loglists = new List<ChatMessage>();
            loglists = context.ChatMessages.Where(m => m.ReceiveUserId == receiveId && m.SendUserId == sendId && m.IsRead == 0).ToList();
            foreach (ChatMessage lll in loglists)
            {
                lll.IsRead = 1;
                context.SaveChanges();
            }
            return loglists;
        }

        public List<ChatMessage> SearchChattingHistory(long receiveId, long sendId,DateTime etime)
        {
            List<ChatMessage> loglists = new List<ChatMessage>();
            loglists = context.ChatMessages.Where(m => ((m.ReceiveUserId == receiveId && m.SendUserId == sendId) || (m.ReceiveUserId == sendId && m.SendUserId == receiveId)) && m.IsRead == 1 && m.State == 1&&m.TimeStamp<etime.Ticks).OrderByDescending(m => m.TimeStamp).Take(5).ToList();
            return loglists;
        } 
    }
}