using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Html.Common;
public static class ComponentId
{
    public static string Next() => 
        "comp-" + Guid.NewGuid().ToString()
        .Replace("-","");

}
