//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KoollaExample
{
    using System;
    using System.Collections.Generic;
    
    public partial class customer
    {
        public customer()
        {
            this.deal = new HashSet<deal>();
        }
    
        public int cid { get; set; }
        public int uid { get; set; }
        public string name { get; set; }
        public string company { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int channel { get; set; }
        public int product { get; set; }
        public int money { get; set; }
    
        public virtual ICollection<deal> deal { get; set; }
        public virtual users users { get; set; }
    }
}