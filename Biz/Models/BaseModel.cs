using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Extensions;
using System.Collections;

namespace Biz.Models
{
    public class BaseModel :IBaseModule
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual IList SubModules
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual BaseModel ParentModule
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public virtual IList BuildModule()
        {
            throw new NotImplementedException();
        }

        public virtual IList BuildSubmodule()
        {
            throw new NotImplementedException();
        }
    }
}
