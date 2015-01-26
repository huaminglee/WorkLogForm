using System;
using System.Collections;

namespace ClassLibrary
{
    /// <summary>
    ///功能描述    :    
    ///开发者      :    
    ///建立时间    :    2012-2-8 10:40:30
    ///修订描述    :    
    ///进度描述    :    
    ///版本号      :    1.0
    ///最后修改时间:    2012-2-8 10:40:30
    ///
    ///Function Description :    
    ///Developer                :    
    ///Builded Date:    2012-2-8 10:40:30
    ///Revision Description :    
    ///Progress Description :    
    ///Version Number        :    1.0
    ///Last Modify Date     :    2012-2-8 10:40:30
    /// </summary>
    public class Course : IEntity
    {
        #region 构造函数
        public Course()
        { }

        public Course(int CourseID, string CourseName, DateTime DataCreated)
        {
            //this.id = Id;
            this.courseID = CourseID;
            this.courseName = CourseName;
            this.dataCreated = DataCreated;
        }
        #endregion

        #region 成员
        //private int id;
        private int courseID;
        private string courseName;
        private DateTime dataCreated;
        #endregion


        #region 属性
        //public virtual int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        public virtual int CourseID
        {
            get { return courseID; }
            set { courseID = value; }
        }

        public virtual string CourseName
        {
            get { return courseName; }
            set { courseName = value; }
        }

        public virtual DateTime DataCreated
        {
            get { return dataCreated; }
            set { dataCreated = value; }
        }

        #endregion

    }
}
