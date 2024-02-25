using Tekla.Structures;
using Tekla.Structures.Model;

namespace Tekla.Extension;
/// <summary>
/// Helps to work with components.
/// </summary>
public static class ComponentHelper
{
    /// <summary>
    /// Delete all cache objects of the component.
    /// </summary>
    /// <param name="identifier"></param>
    public static void DeleteChildren(Identifier identifier)
    {
        Component component = new()
        {
            Identifier = identifier
        };

        if (!component.Select())
        {
            return;
        }

        foreach (ModelObject child in component.GetChildren().ToIEnumerable<ModelObject>())
        {
            if (child is not null && (child.IsConnectionObject() || child.IsAssociativeObject()))
            {
                _ = child.Delete();
            }
        }
    }
}
