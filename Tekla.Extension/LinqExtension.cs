using System;
using System.Collections;
using System.Collections.Generic;
using Tekla.Structures.Model;

namespace Tekla.Extension;

/// <summary>
/// Extension class for queries in model
/// </summary>
public static class LinqExtension
{
    /// <summary>
    /// Convet enumerator to enumerable for LINQ queries
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator enumerator) where T : class
    {
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            T @object = null;
            try
            {
                @object = enumerator.Current as T;//Only this way working with broken models in Tekla
            }
            catch (Exception)
            { }

            if (@object is not null)
            {
                yield return @object;
            }
        }
    }
    /// <summary>
    /// Get all objects with specified type
    /// </summary>
    /// <typeparam name="T">Type of specified objects in the model</typeparam>
    /// <param name="model">Your current Tekla model</param>
    /// <param name="isQuick">Indicates that the instance Select() is called when the 'Current' item is asked from the enumerator. The user can set this to 'false' if no members are ever asked from the instance. This is the case when, for example, asking only a report property from the identifier. Warning: normally the user should not change this value. </param>
    /// <returns></returns>
    public static IEnumerable<T> GetAllObjectsWithType<T>(this Model model, bool isQuick = false) where T : ModelObject
    {
        var selector = model.GetModelObjectSelector();
        if (isQuick)
            selector.GetEnumerator().SelectInstances = isQuick;

        return selector.GetAllObjectsWithType(new Type[] { typeof(T) })
            .ToIEnumerable<T>();
    }
}
