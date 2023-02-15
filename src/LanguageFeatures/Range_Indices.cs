using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LanguageFeatures;

public class ExampleUsageRangeAndIndices
{
    [ModuleInitializer]
    public static void Run()
    {
        // using Range type
        Range r1 = 0..; // from 0th to end
        Range r2 = ..3; // from start to 3rd
        Range r3 = ^2..; // from 2nd last to end

        // arrays accept Range; List<T> currently does not
        var values = new int[] { 1, 2, 3, 4, 5 };
        
        Console.WriteLine($"First range: {string.Join(", ", values[r1])}");
        Console.WriteLine($"Second range: {string.Join(", ", values[r2])}");
        Console.WriteLine($"Third range: {string.Join(", ", values[r3])}");

        // example to show out of range
        HandleEx(() => Console.WriteLine($"Range: {string.Join(", ", values[8..])}"));

        // using Index type
        Index ii1 = 1; 
        Index ii3 = ^3; // 3rd last item
        Console.WriteLine($"Index: {values[ii1]}");
        Console.WriteLine($"Index: {values[ii3]}");
        Console.WriteLine($"Index: {values[^3]}"); // same as above

        var list = values.ToList();
        Console.WriteLine($"Index: {list[^1]}");
        Console.WriteLine($"Index: {list[^2]}");

        // index cannot be negative
        HandleEx(() => { Index ii2 = -2; });

        // index cannot be ^0; must be at least ^1 (last item)
        HandleEx(() => Console.WriteLine($"Index: {list[^0]}"));
    }

    private static void HandleEx(Action action)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}
