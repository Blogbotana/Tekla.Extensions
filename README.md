# Tekla.Extensions
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
using Tekla.Extensions;
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


## Licence

The Tekla.Extensions library is made available under  [The 2-Clause BSD License](LICENCE.md).

## Third Party Licences
The Tekla.Extensions library makes use of the 3rd party software package, under his associated licences
*  'Tekla Structures open API' : https://www.tekla.com/terms-and-conditions/eula

## Support & Help

Create the ussie if you have any question or problems using the library. Our contributors will try to help you

###Tasks to do
- [ ] Make nuget package
- [ ] Add Drawer class
- [ ] Add Assebmy and Part class
- [ ] Add more exambles for readme file
- [ ] Make Tests 
- [ ] Add code conventions
- [ ] Add wiki