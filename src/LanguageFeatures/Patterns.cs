using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LanguageFeatures;

public class ExampleUsagePatterns
{
    [ModuleInitializer] // make use of attribute to auto run
    public static void Run()
    {
        var list = new List<object>();
        var obj = list;

        // is { } => is not null
        // important to remember this pattern since it's starting to pop up in stackoverflow
        if (obj is { } someObj)
        {
            Console.WriteLine($"{nameof(someObj)}is not null");
        }

        ComparingNullChecksWithPatterns();
        UsingPatternsToCheckList();
        UsingPatternsInSwitch();
    }

    private static void ComparingNullChecksWithPatterns()
    {
        int? nullableInt = 5;
        // using nullable class's methods to check for null
        if (nullableInt.HasValue)
        {
            Console.WriteLine($"Int val: {nullableInt.Value}");
        }

        // inspect 'notNullInt' -> it's of type int
        // micro-optimization -> doesn't need to call .Value where an additional null check is used
        if (nullableInt is { } notNullInt)
        {
            Console.WriteLine($"Not null val: {notNullInt}");
        }
    }

    private static void UsingPatternsToCheckList()
    {
        var list = new List<int> { 1, 2, 3, 4 };

        // checks if first 3 elements are 1, 2, 3
        if (list is [1, 2, 3, ..])
        {
            Console.WriteLine("First three elements are 1, 2, 3");
        }

        // using ".." in list patterns are restricted to beginning or end of array
        // the var patterns can be used to extract the elements in those positions
        if (list is [.., 2, var a, var b])
        {
            Console.WriteLine($"Last 2 elements are {a} and {b}");
        }

        // using property pattern
        if (list is { Count: >2 })
        {
            Console.WriteLine("List has more than 2 items");
        }
    }

    private static void UsingPatternsInSwitch()
    {
        Item item = new() { Name = "FirstItem", Length = 6, Values = new List<int> { 1, 2, 3, 4 } };

        // local static function -> static helps to prevent capture of local variables
        static void getText(Item item)
        {
            // the item in this scope has nothing to do with item in outer scope
            // but it is confusing if we keep using same names
            var text = item switch
            {
                // perform checks using the var pattern
                // this helps to assign to new object name
                var it when it.Length > 5 => $"{it.Name}'s length is more than 2",

                // using property pattern for the inner list property
                { Values.Count : 4} => $"{item.Name} has 4 elements in values",

                // checks Length property
                { Length: 4 } => $"{item.Name}'s length is exactly 4!",

                // match by object type
                ValueType => "well, it's a value type!",

                // we cannot use discards here because compiler is smart enough
                // to understand that Item is value type and above ValueType check
                // already handles all possible cases
            };
            Console.WriteLine(text);
        };
        
        getText(item);
        getText(item with { Name = "VeryShort", Length = 1});
        getText(item with { Name = "SetToFour!", Length = 4, Values = new List<int>()});
    }

    public readonly record struct Item(string Name, int Length, List<int> Values);
}