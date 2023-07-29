using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekla.Extension
{
    /// <summary>
    /// Enum for storing PROFILE_TYPE values as per https://support.tekla.com/doc/tekla-structures/2023/profile_type.
    /// </summary>
    public enum ProfileType : int
    {
        /// <summary>
        /// Corresponding API sting value is "B".
        /// </summary>
        [Description("B")]
        Plate,

        /// <summary>
        /// I - beam. Corresponding API sting value is "I".
        /// </summary>
        [Description("I")]
        Ibeam,

        /// <summary>
        /// L - shaped profile. Corresponding API sting value is "L".
        /// </summary>
        [Description("L")]
        Angle,
        
        /// <summary>
        /// [ - shaped profile. Corresponding API sting value is "U".
        /// </summary>
        [Description("U")]
        Channel,
        
        /// <summary>
        /// Round profile. Corresponding API sting value is "RU".
        /// </summary>
        [Description("RU")]
        RoundBar,
        
        /// <summary>
        /// o - shaped profile. Corresponding API sting value is "RO".
        /// </summary>
        [Description("RO")]
        RoundTube,
        
        /// <summary>
        /// ⎕ - shaped profile. Corresponding API sting value is "M".
        /// </summary>
        [Description("M")]
        RectangularTube,

        /// <summary>
        /// ∁ - shaped cold formed profile. Corresponding API sting value is "C".
        /// </summary>
        [Description("C")]
        CFChannel,
        
        /// <summary>
        /// T - shaped profile. Corresponding API sting value is "T".
        /// </summary>
        [Description("T")]
        Tbeam,
        
        /// <summary>
        /// Z and rest shaped profiles. Corresponding API sting value is "Z".
        /// </summary>
        [Description("Z")]
        Zbeam
    }
}
