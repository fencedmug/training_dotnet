public class DemoFunctionals
{
    // Code from Simon Painter's talk - https://www.youtube.com/watch?v=ETX9sch7zAs
    public static void Run()
    {
        // usage of Map/Bind like function
        var text = "hello every one";
        text.Split().InvokeEach(x => Console.WriteLine("Printing - " + x));
        Console.WriteLine($"Celsius = {FahrenheitToCelsius(120)}");

        // demostrate how Just/Maybe/Bind saves writing much if/else code
        int? id = null;
        string finalStr = id.ToMaybe()
            .Bind(x => new { fname = "first name", lname = "last name"})
            .Bind(x => $"{x.fname} {x.lname}")
            .Bind(x => x.Replace("b", "11"));
        Console.WriteLine($"String from id: {finalStr}");
    }

    public static double FahrenheitToCelsius(double celsius)
    {
        return celsius
            .Map(x => x - 32)
            .Map(x => x * 5.0)
            .Map(x => x / 9.0)
            .Map(x => Math.Round(x, 2));
    }
}

public static class FunctionalExtensions
{
    public static TTo Map<TFrom, TTo>(this TFrom @this, Func<TFrom, TTo> func) => func(@this);

    public static void InvokeEach<T>(this IEnumerable<T> @this, Action<T> invoke) 
    {
        foreach (var item in @this)
        {
            invoke(item);
        }
    }

    public static Maybe<T> ToMaybe<T>(this T value) => new Just<T>(value);

    public static Maybe<TTo> Bind<TFrom, TTo>(this Maybe<TFrom> @this, Func<TFrom, TTo> func)
    {
        switch (@this)
        {
            case Just<TFrom> sth when !EqualityComparer<TFrom>.Default.Equals(sth.Value, default(TFrom)):
                try
                {
                    return func(sth).ToMaybe();
                }
                catch
                {
                    return new Nothing<TTo>();
                }
            
            default: return new Nothing<TTo>();
        }
    }
}

public abstract class Maybe<T>
{
    public abstract T Value { get; }
    public static implicit operator T(Maybe<T> @this) => @this.Value;
}

public class Just<T> : Maybe<T>
{
    public override T Value { get; }

    public Just(T value)
    {
        Value = value;
    }
}

public class Nothing<T> : Maybe<T>
{
    public override T Value => default(T)!;
}