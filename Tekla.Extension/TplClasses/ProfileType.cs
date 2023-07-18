using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekla.Extension.TplClasses
{
    public enum ProfileType
    {
        [StringValue("B")]
        Plate,
        [StringValue("I")]
        IBeam,
    }
}
