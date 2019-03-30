using System;
using System.Collections.Generic;

namespace DbContext
{
    public  class BindInfo
    {
        public Type DestType { get; set; }
        
        public IEnumerable<BindMemberInfo> BindMemberInfos { get; set; }
    }
}