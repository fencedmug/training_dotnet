using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Database;

public static class Queries_Select
{
    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("Examples: Select Queries");


        using var ctx = new ItemContext();
        IQueryable<Item> query = ctx.Items;

        // as long as we do not call ToList or FirstOrDefault
        // the following LINQ will not attempt to establish
        // connection to database defined in connection string

        var itemId = 10;
        query = ctx.Items.Where(item => item.Id > itemId);
        Consolex.WriteLine(1, query);

        var nameToMatch = "HELL WORLD";
        query = ctx.Items.Where(item => item.Name.ToUpper().Contains(nameToMatch));
        Consolex.WriteLine(2, query);

        query = ctx.Items.Where(item => item.Created < DateTime.Now && item.Id == itemId);
        Consolex.WriteLine(3, query);

        // break up Where clauses -> doesn't seem to affect SQL generated
        query = ctx.Items.Where(item => item.Created < DateTime.Now).Where(item => item.Id == 300);
        Consolex.WriteLine(4, query);

        Run_Distinct_OrderBy_Queries(ctx);
        Run_Custom_OrderBy_Queries(ctx);
    }

    private static void Run_Distinct_OrderBy_Queries(ItemContext ctx)
    {
        // if we have logging framework added, entity framework
        // will complain about the calling order of Distinct + OrderBy
        // the generated query will not show "ORDER BY"
        var wrongQuery = ctx.Items
            .Select(item => new { item.Name, item.Created })
            .OrderBy(it => it.Created)
            .Distinct();

        Consolex.WriteLine("Wrong ORDER BY + DISTINCT", wrongQuery);

        // the SQL generated will show "ORDER BY" but
        // as of Mar 2023, the generated SQL doesn't seem to be "right"
        // the following would look more "right"
        // SELECT DISTINCT ...
        // FROM TABLE
        // ORDER BY Created
        var correctQuery = ctx.Items
            .Select(item => new { item.Name, item.Created })
            .Distinct()
            .OrderBy(it => it.Created);

        Consolex.WriteLine("Correct DISTINCT + ORDER BY", correctQuery);
    }
    private static void Run_Custom_OrderBy_Queries(ItemContext ctx)
    {
        // this query demonstrate query that requires a custom order by
        // make sure to double check query generated
        // there used to be a bug with how the query was generated in
        // earlier versions of entity framework
        var query = ctx.Items
            .Select(item => new
            {
                item,
                sortIndex =
                    item.EnumValue == "Pending" ? 0 :
                    item.EnumValue == "Accepted" ? 1 :
                    item.EnumValue == "Rejected" ? 2 : 3
            })
            .OrderBy(anon => anon.sortIndex)
            .Select(anon => anon.item);

        Consolex.WriteLine("Custom OrderBy", query);
    }
}
