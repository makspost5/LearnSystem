//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class AnswerResult
    {
        public int AnswerResultID { get; set; }
        public int AnswerID { get; set; }
        public bool IsSelected { get; set; }
        public int QuestionResultID { get; set; }
    
        public virtual Answer Answer { get; set; }
        public virtual QuestionResult QuestionResult { get; set; }
    }
}