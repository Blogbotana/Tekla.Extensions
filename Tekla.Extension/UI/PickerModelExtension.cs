using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;

namespace Tekla.Extension.UI
{
    /// <summary>
    /// Class for working with <see cref="Picker"/> in Tekla Structures
    /// </summary>
    public static class PickerModelExtension
    {
        public static IEnumerable<T> PickObjects<T>(this Picker picker, Picker.PickObjectsEnum _enum, string prompt = "") where T : ModelObject
        {
            return picker.PickObjects(_enum, prompt).ToIEnumerable<T>();
        }
        public static IEnumerable<Point> PickPointsEnumerable(this Picker picker, Picker.PickPointEnum _enum, string prompt = "")
        {
            return picker.PickPoints(_enum, prompt).Cast<Point>();
        }
    }
}
