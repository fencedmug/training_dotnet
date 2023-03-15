using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Database;

public static class Queries_Joins
{
    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("Examples: Join Queries");


        using var ctx = new ItemContext();
        IQueryable<Item> query = ctx.Items;

        var joinQuery = from item in ctx.Items
                        join tag in ctx.Tags
                        on item.Name equals tag.Name into tagged
                        from t in tagged.DefaultIfEmpty()
                        select new { item.Name, t.Value };

        Consolex.WriteLine("Join #1", joinQuery);
    }
}
