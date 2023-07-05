# Tekla.Extensions
The best **unofficial** library for Tekla Structures [Open API](https://developer.tekla.com/)

You can easily change you coding in Tekla Structures with this library


## Benefits
1. Easy to use
2. Saving time for code
3. More classes for showing your data

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