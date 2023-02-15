using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LanguageFeatures;

public class FloatingPoints
{
    [ModuleInitializer]
    public static void Run()
    {
        // demo floating point values
        // might help you to decide which type to use

        var counts = 100;

        float floatVal = Enumerable.Range(0, counts).Select(x => 0.000_000_000_000_1f).Sum();
        Console.WriteLine($"Float: {floatVal:F30}");

        double dblVal = Enumerable.Range(0, counts).Select(x => 0.000_000_000_000_1).Sum();
        Console.WriteLine($"Double: {dblVal:F30}");

        // be aware that decimal calculations can be 10 times slower than double
        decimal dmVal = Enumerable.Range(0, counts).Select(x => 0.000_000_000_000_1m).Sum();
        Console.WriteLine($"Decimal: {dmVal}");

        Console.WriteLine($"Float: {1.0f / 3.3f:F30}");
        Console.WriteLine($"Double: {1.0 / 3.3:F30}");
        Console.WriteLine($"Decimal: {1.0m / 3.3m}");
    }
}
