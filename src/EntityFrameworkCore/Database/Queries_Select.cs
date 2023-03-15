using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        query = ctx.Items.Where(item => item.Id > 10);
        Consolex.WriteLine(1, query);

        query = ctx.Items.Where(item => item.Name.ToUpper().Contains("HELLO WORLD"));
        Consolex.WriteLine(2, query);

        query = ctx.Items.Where(item => item.Created < DateTime.Now && item.Id == 300);
        Consolex.WriteLine(3, query);

        query = ctx.Items.Where(item => item.Created < DateTime.Now).Where(item =>item.Id == 300);
        Consolex.WriteLine(4, query);


        // this query demonstrate query that requires a custom order by
        // make sure to double check query generated
        // there used to be a bug with how the query was generated in
        // earlier versions of entity framework
        query = ctx.Items.Select(item => new
                {
                    item = item,
                    sort = 
                        item.EnumValue == "Pending" ? 0 :
                        item.EnumValue == "Accepted" ? 1 :
                        item.EnumValue == "Rejected" ? 2 : 3
                })
                .OrderBy(anon => anon.sort)
                .Select(anon => anon.item);
        Consolex.WriteLine("Custom OrderBy", query);


    }

    
}
