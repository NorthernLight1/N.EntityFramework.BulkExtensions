﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N.EntityFramework.Extensions
{
    public class FetchOptions
    {
        //public Expression<Func<T, object>> SelectColumns { get; set; }
        public int BatchSize { get; set; }
    }
}
