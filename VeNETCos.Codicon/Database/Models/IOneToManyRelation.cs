﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeNETCos.Codicon.Database.Models;
public interface IOneToManyRelation<TOne, TMany>
    where TMany : class
    where TOne : class
{
    ICollection<TMany> Many { get; }
    TOne One { get; }
}
