using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LanguageFeatures;

public class ExampleTaskbased
{
    public static async Task Run()
    {
        var sw = new Stopwatch();
        sw.Start();

        var logTime = () => Console.WriteLine($"Time passed: {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds}ms");

        var t1 = TaskDelay(TimeSpan.FromSeconds(1));
        var t2 = TaskDelay(TimeSpan.FromSeconds(2));
        var t3 = TaskDelay(TimeSpan.FromSeconds(3));
        // not blocked here
        logTime();

        await t1;
        await t2;
        await t3;
        // total time spent is around 6s
        // this shows t1 - t3 "ran" at the same time
        logTime();

        sw.Restart();

        t1 = DoMoreStuff("Ape", TimeSpan.FromSeconds(2), 3);
        t2 = DoMoreStuff("Cat", TimeSpan.FromSeconds(3), 4);
        t3 = DoMoreStuff("Dog", TimeSpan.FromSeconds(4), 5);
        logTime();

        // if any tasks throw exceptions, we will need to inspect AggregateException
        // this also means you can only process them when all completes
        await Task.WhenAll(t1, t2, t3);
        logTime();
    }

    public static Task TaskDelay(TimeSpan time)
    {
        // micro-optimization when we return Task.Delay
        // every await causes the compiler to create the state-machine code
        return Task.Delay(time);
    }

    public static async Task DoMoreStuff(string name, TimeSpan time, int parts)
    {
        Console.WriteLine($"DoMoreStuff with time: {time}");

        // there's an opportunity to use IAsyncEnumerable here....
        foreach (var piece in Enumerable.Range(0, parts).Select(x => time / parts))
        {
            Console.WriteLine($"{name} - waiting");
            await Task.Delay(piece);
        }
    }
}
