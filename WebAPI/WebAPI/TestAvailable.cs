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
    
    public partial class TestAvailable
    {
        public int TestID { get; set; }
        public int GroupID { get; set; }
        public bool IsTestAvailable { get; set; }
        public int TestAvailableID { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual Test Test { get; set; }
    }
}