using System.Text;

public record ClassRecord 
{ 
    public int A { get; set; }
    public int B { get; set; }
}

public record ImmutableClassRecord
{
    public int A { get; init; } = default!;
    public int B { get; init; } = default!;
}

public record struct StructRecord
{
    public int X { get; set; }
    public int Y { get; set; } 
}

public readonly record struct ImmutableStructRecord
{
    public readonly int X { get; init; }
    public readonly int Y { get; init; }

    // compiler will create this method
    // we can manually create "PrintMembers" to change how fields are printed
    private bool PrintMembers(StringBuilder sb)
    {
        sb.Append($"X is {X}, ");
        sb.Append($"Y is {Y}");
        return true;
    }
}

// Alternative declaration using positional arguments
public readonly record struct ImmutablePositionalRecord(int p, double q);

public class ExampleUsageRecords
{
    public static void Run()
    {
        // records are internally classes
        // use "record struct ..." to declare a value type record

        var rec1 = new ClassRecord { A = 1, B = 2 };
        rec1.A = 4; // normal records are immutable

        var immRec = new ImmutableClassRecord { A = 3, B = 5 };
        //immRec.A = 4; // gives error when uncommented

        // demostrating how to create new record using "with"
        // this is called nondestructive mutation in docs
        var immRec2 = immRec with { A = 3 };
        var immRec3 = immRec with { B = 5 };

        // referential equality shows they are different objects
        Console.WriteLine($"Is same ref: {ReferenceEquals(immRec, immRec2)}");
        Console.WriteLine($"Is same ref: {ReferenceEquals(immRec2, immRec3)}");

        // records override == operators for structural equality (comparing fields)
        Console.WriteLine($"Is same val: {immRec == immRec2}");
        Console.WriteLine($"Is same val: {immRec2 == immRec3}");

        var immStrc = new ImmutableStructRecord { X = 1, Y = 4 };
        //immStrc.X = 12; // gives error when uncommented due to readonly

        var immaStrc2 = immStrc with { X = 1 };
        var immaStrc3 = immStrc with { Y = 2 };
        Console.WriteLine($"Is same val: {immStrc == immaStrc2}");
        Console.WriteLine($"Is same val: {immaStrc2 == immaStrc3}");

        // built-in support for printing records
        var toPrint1 = new ImmutablePositionalRecord { p = 12, q = 0.0001 };
        var toPrint2 = new ImmutableStructRecord { X = 12, Y = 23 };
        Console.WriteLine(toPrint1);
        Console.WriteLine(toPrint2);

        // records have built-in deconstruct but only with records with positional parameters        
        var (a, b) = toPrint1;
        Console.WriteLine($"Values {a}, {b}");

        // this won't work
        //var (ab, bb) = toPrint2;
    }
}