using Microsoft.EntityFrameworkCore;
using System;

namespace EntityFrameworkCore.Database;

public class ItemContext : DbContext
{
	public DbSet<Item> Items { get; set; }
	public DbSet<Tag> Tags { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SomeDbName;Trusted_Connection=True");
	}
}

public class Item
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Status { get; set; } = string.Empty;
	public int SomeId { get; set; }
	public DateTimeOffset Created { get; set; }
	public string EnumValue { get; set; } = string.Empty;
}

public class Tag
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Value { get; set; } = string.Empty;
}