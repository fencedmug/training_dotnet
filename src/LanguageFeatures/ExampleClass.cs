namespace LanguageFeatures;

public class ExampleClass
{
    // this tells dotnet to run this method when application starts
    // can be useful if we need something to run before main executes
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void WillInvokeAtRuntime()
    {
        Console.WriteLine($"Method '{nameof(WillInvokeAtRuntime)}' invoked!");
    }

    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void AutoRun()
    {
        Console.WriteLine($"Method '{nameof(AutoRun)}' invoked!");
    }
}
