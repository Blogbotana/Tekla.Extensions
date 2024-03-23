# Tekla.Extensions
[![NuGet](https://img.shields.io/nuget/v/Tekla.Extension.svg)](https://www.nuget.org/packages/Tekla.Extension/)
[![Release](https://img.shields.io/github/release/Blogbotana/Tekla.Extensions.svg)](https://github.com/Blogbotana/Tekla.Extensions/releases/latest)
[![License](https://img.shields.io/github/license/Blogbotana/Tekla.Extensions.svg)](https://github.com/Blogbotana/Tekla.Extensions/blob/main/LICENSE.md) 

The best **unofficial** library for Tekla Structures [Open API](https://developer.tekla.com/)

You can easily change you coding in Tekla Structures with this library

## Benefits
1. Easy to use
2. Saving time for code
3. More classes for showing your data
4. Makes your code clean and readable

## Code Examples

This code examples show you why you need to use this library. 

### How did you code before
```csharp
using TSMUI = Tekla.Structures.Model.UI;
using Tekla.Structures.Model;
    public double GetWeight()
    {
            //Summing all weight of all parts in Assembly
            TSMUI.ModelObjectSelector selector = new();
            double allWeight = 0;
            ModelObjectEnumerator enumerator = selector.GetSelectedObjects();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is Assembly assembly)
                {
                    ModelObject mainPart = assembly.GetMainPart();
                    _ = mainPart.GetReportProperty("WEIGHT", ref allWeight);
                    foreach (object item in assembly.GetSecondaries())
                    {
                        ModelObject modelObject = item as ModelObject;
                        double tempWeight = 0;
                        modelObject.GetReportProperty("WEIGHT", ref tempWeight);
                        allWeight += tempWeight;
                    }
                }
            }
            return allWeight;
    }
```

### How you write with LINQ queries
```csharp
using TSMUI = Tekla.Structures.Model.UI;
using Tekla.Structures.Model;
using Tekla.Extension;
        public double GetWeight()
        {
            //Summing all weight of all parts in Assembly
            TSMUI.ModelObjectSelector selector = new();

            return selector.GetSelectedObjects()
                    .ToIEnumerable<Assembly>()
                    .FirstOrDefault()
                    .GetAllPartsOfAssembly()
                    .Select(a => a.GetWeight())
                    .Sum();
        }
```
### How did you code before

```csharp
public class AssemblyPrefixNumberer
{
    public static void NumberSelected()
    {
        Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
        var objects = selector.GetSelectedObjects();

        List<Assembly> assemblies = new List<Assembly>();
        Assembly tempAssembly;
        var enumerator = objects.GetEnumerator();
        while (enumerator.MoveNext())
        {
            try
            {
                tempAssembly = enumerator.Current as Assembly;
                if (tempAssembly != null) assemblies.Add(tempAssembly);
            }
            catch { }
        }

        int index = 1;
        Part tempPart;
        foreach (Assembly assembly in assemblies)
        {
            var parts = assembly.GetSecondaries();
            parts.Add(assembly.GetMainPart());
            foreach (object partObj in parts)
            {
                tempPart = partObj as Part;
                if (tempPart != null)
                {
                    // Изначально хотел заполнить поле префикс сборки для детали, но это не работает.
                    tempPart.AssemblyNumber.Prefix = $"{index}";
                    tempPart.Modify();
                }
            }
            index++;
        }
        var result = new Model().CommitChanges();
    }
}

```
### The same code after using Tekla.Extension

```csharp
public class AssemblyPrefixNumberer
{
    public static void NumberSelected()
    {
        int index = 1;
        new Tekla.Structures.Model.UI.ModelObjectSelector().GetSelectedObjects()
            .ToIEnumerable<Assembly>()
            .SelectMany(a => a.GetAllPartsOfAssembly())
            .ToList()
            .ForEach(p =>
            {
                p.PartNumber.Prefix = index.ToString("0");
                p.Modify();
                index++;
            });

         new Model().CommitChanges();
     }
}
```

### Non case sensetive report property
```csharp
using TSMUI = Tekla.Structures.Model.UI;
using Tekla.Structures.Model;
using Tekla.Extensions;


        var part = new TSMUI.ModelObjectSelector()
            .GetSelectedObjects()
            .ToIEnumerable<Part>()
            .FirstOrDefault();

        double netWeight = part.GetReportProperty<double>(" weight net ");
```

## Licence

The Tekla.Extensions library is made available under  [The 2-Clause BSD License](LICENSE.md).

## Third Party Licences
The Tekla.Extensions library makes use of the 3rd party software package, under his associated licences
*  'Tekla Structures open API' : https://www.tekla.com/terms-and-conditions/eula

## Support & Help

Create the ussie if you have any question or problems using the library. Our contributors will try to help you

### Tasks to do
- [x] Make nuget package
- [x] Add Assebmy and Part class
- [ ] Add more examples for readme file
- [ ] Add example project
- [ ] Make Tests 
- [ ] Add Forms Dialogs
- [ ] Add Intersections
- [ ] Add Geometry core 3d
