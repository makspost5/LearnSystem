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
    
    public partial class SectionBlockResult
    {
        public int SectionBlockResultID { get; set; }
        public System.DateTime FinishTime { get; set; }
        public int SectionBlockID { get; set; }
        public int SubjectSectionResultID { get; set; }
    
        public virtual SectionBlock SectionBlock { get; set; }
        public virtual SubjectSectionResult SubjectSectionResult { get; set; }
    }
}