using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Database;

public static class Consolex
{
    /// <summary>
    /// Helper function to print a block of query
    /// Making use of ToQueryString to print out
    /// generated SQL statements.
    /// 
    /// Be aware that it does not show the eventual
    /// SQL statements used for SQL Server. Take note
    /// the statements are not parameterized in this
    /// case.
    /// </summary>
    private static void InnerWriteLine(string? val, IQueryable query)
    {
        Console.WriteLine();
        Console.WriteLine($"Query {val}:");
        Console.WriteLine(query.ToQueryString());
        Console.WriteLine();
    }

    public static void WriteLine(string? val, IQueryable query)
    {
        val = val != null ? $"\"{val}\"" : val;
        InnerWriteLine(val, query);
    }

    public static void WriteLine(int val, IQueryable query)
    {
        InnerWriteLine(val.ToString(), query);
    }

    public static void WriteLine(IQueryable query)
    {
        InnerWriteLine(null, query);
    }
}
