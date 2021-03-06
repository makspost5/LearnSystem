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
    
    public partial class Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test()
        {
            this.Question = new HashSet<Question>();
            this.Result = new HashSet<Result>();
            this.TestAvailable = new HashSet<TestAvailable>();
        }
    
        public int TestID { get; set; }
        public string Name { get; set; }
        public int SubjectID { get; set; }
        public int TypeTestID { get; set; }
        public Nullable<int> PassageMinutes { get; set; }
        public string image { get; set; }
        public Nullable<int> HierarchyLevel { get; set; }
        public int TeacherID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Question { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Result> Result { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual TestType TestType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestAvailable> TestAvailable { get; set; }
    }
}
