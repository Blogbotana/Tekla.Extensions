using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Extension class for working with model Part class in Tekla Structures
    /// </summary>
    public static class PartExtension
    {
        
        /// <summary>
        /// Checks report property PROFILE_TYPE for selected value. Usefull to recognising plates made as beams for example.
        /// </summary>
        /// <param name="part">Extended class object</param>
        /// <param name="type">String representation of Profile Type as in https://support.tekla.com/doc/tekla-structures/2023/profile_type</param>
        /// <returns>True if profile type is correct</returns>
        public static bool ProfileTypeIs(this Part part, string type)
        {
            string profType = string.Empty;
            part.GetReportProperty("PROFILE_TYPE", ref profType);
            return (type == profType);
        }
    }
}
