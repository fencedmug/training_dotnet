using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageFeatures;

// demostrating default implementations
public interface DemoInterface
{
    void RequireImplementation();

    // default implementations can be useful when designing/updating libraries
    // consumers of new version can choose to ignore implementing new methods
    string WhatIsMyName() => "Nobody";
}

public class DemoUseCase : DemoInterface
{
    public void RequireImplementation()
    {
        // need to do something
    }

    // compilable even when we omit the below
    // public string WhatIsMyName()
}

public class DemoUseCase2 : DemoInterface
{
    public void RequireImplementation()
    {
        // required
    }

    // default implementation will be ignored if we implement this method
    public string WhatIsMyName()
    {
        return "DemoUseCase2";
    }
}
