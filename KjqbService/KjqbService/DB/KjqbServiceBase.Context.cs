﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KjqbService.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KjqbServiceEntities : DbContext
    {
        public KjqbServiceEntities()
            : base("name=KjqbServiceEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<LogMessage> LogMessages { get; set; }
        public DbSet<ScheduleMessage> ScheduleMessages { get; set; }
        public DbSet<CommentMessage> CommentMessages { get; set; }
        public DbSet<TimeArrangeForManagerMessage> TimeArrangeForManagerMessages { get; set; }
    }
}
