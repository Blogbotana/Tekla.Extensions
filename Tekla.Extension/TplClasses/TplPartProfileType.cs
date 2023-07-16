using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for storing PROFILE_TYPE values as per https://support.tekla.com/doc/tekla-structures/2023/profile_type.
    /// </summary>
    public class TplPartProfileType
    {
        /// <summary>
        /// String value Tekla API working with.
        /// </summary>
        public string Value { get; private set; }

        #region ValuesForPROFILE_TYPE
        /// <summary>
        /// String representation of I - shaped beams profile type in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType IforIbeam = new TplPartProfileType("I");

        /// <summary>
        /// String representation of L - shaped beams profile type in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType LforAngle = new TplPartProfileType("L");

        /// <summary>
        /// String representation of [ - shaped beams profile type in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType UforChannel = new TplPartProfileType("U");

        /// <summary>
        /// String representation of plates profile type for beams and contour plates in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType BforPlates = new TplPartProfileType("B");

        /// <summary>
        /// String representation of round shaped beams profile type in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType RUforRoundBar = new TplPartProfileType("RU");

        /// <summary>
        /// String representation of round tube beams profile type in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType ROforRoundTube = new TplPartProfileType("RO");

        /// <summary>
        /// String representation of rectangular tube shaped beams profile type in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType MforRectangularTube = new TplPartProfileType("M");

        /// <summary>
        /// String representation of C - shaped beams profile (with bended flanges) type in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType CforChannel = new TplPartProfileType("C");

        /// <summary>
        /// String representation of T - shaped beams profile type in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType TforTbeam = new TplPartProfileType("T");

        /// <summary>
        /// String representation of Z and all the rest beams profile type in Tekla API.
        /// </summary>
        public static readonly TplPartProfileType ZforZbeamsAndUnknown = new TplPartProfileType("Z");
        #endregion

        public TplPartProfileType(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Implicit operator that allows class object to be converted to a strings by the runtime.
        /// </summary>
        /// <param name="instance">Current class instance to be converted to string.</param>
        public static implicit operator string(TplPartProfileType instance)
        {
            return instance.Value;
        }
    }
}
