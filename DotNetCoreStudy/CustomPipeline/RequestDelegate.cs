using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomPipeline
{
    public delegate Task RequestDelegate(RequestContext context);
}
