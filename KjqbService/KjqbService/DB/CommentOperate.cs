using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService.DB
{
    public class CommentOperate : DBoperate
    {
        public int InsertIntoCommentEntity(CommentInService l)
        {
            CommentMessage lm = new CommentMessage();

            lm.LogId = l.LogId;
            lm.CommentUserName = l.CommentUserName;
            lm.UserId = l.LogUserId;
            lm.TimeStamp = l.TimeStamp;
            lm.State = 0;
            lm.IsRead = 0;
            context.CommentMessages.Add(lm);
            context.SaveChanges();
            return lm.Id;
        }
        public List<CommentMessage> SendComment(long l)
        {
            List<CommentMessage> loglists = new List<CommentMessage>();
            loglists = context.CommentMessages.Where(m => m.UserId == l && m.State == 0).ToList();
            foreach (CommentMessage lll in loglists)
            {
                lll.State = 1;
                context.SaveChanges();
            }
            return loglists;
        }

        public List<CommentMessage> SendCommentLogUnRead(long l)
        {
            List<CommentMessage> loglists = new List<CommentMessage>();
            loglists = context.CommentMessages.Where(m => m.UserId == l && m.IsRead == 0 && m.State == 1).ToList();

            return loglists;
        }

        public List<CommentMessage> ChangeCommnentLogIsRead(long l)
        {
            List<CommentMessage> loglists = new List<CommentMessage>();
            loglists = context.CommentMessages.Where(m => m.UserId == l && m.IsRead == 0).ToList();
            foreach (CommentMessage lll in loglists)
            {
                lll.IsRead = 1;
                context.SaveChanges();
            }
            return loglists;
        }


    }
}