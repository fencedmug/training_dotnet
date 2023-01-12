using System.ComponentModel.DataAnnotations;

// From code magazine
// https://codemag.com/Article/2301031/The-Rich-Set-of-Data-Annotation-and-Validation-Attributes-in-.NET?utm_source=DeveloperWeek1.11.2023&utm_medium=newsletter&utm_campaign=sm-articles
public class DemoValidations
{
    public static void Run()
    {
        Console.WriteLine("Example of using Microsoft's DataAnnotations and Validators");

        Product prod = new();
        
        ValidationContext ctx = new(prod);
        List<ValidationResult> results = new();
        if (!Validator.TryValidateObject(prod, ctx, results, true))
        {
            foreach (var result in results)
            {
                if (result.MemberNames.Any())
                {
                    Console.WriteLine($"({result.MemberNames.First()}) {result.ErrorMessage}");
                }
            }
        }
    }
}

// Annotation references: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-6.0
public class Product
{
    [CannotBeZero, Display(Name = "||The Product ID||")]
    public int ProductID { get; set; }

    [CannotBeZero]
    public int? CompanyID { get; set; }

    [Display(Name = "Product Name")]
    [Required(ErrorMessage = "{0} cannot be empty.")]
    public string? Name { get; set; }

    [Display(Name = "|Product Origin|"), MaxLength(20)]
    public string Origin { get; set; } = "Very long name for origin origin origin origin";

    [Range(typeof(DateTime), "20/5/2011", "21/6/2023", ErrorMessage = "{0} must be between {1:d} and {2:d}")]
    public DateTime StartDate { get; set; }

    [Compare(nameof(Name))]
    public string SameName => "Not the same!";
}

public class CannotBeZeroAttribute : ValidationAttribute
{
    // Custom validation logic
    protected override ValidationResult? IsValid(object? value, ValidationContext ctx) =>
        value switch 
        {
            null => Bad("null", ctx),
            0    => Bad("zero", ctx),
            _    => ValidationResult.Success,
        };

    private static ValidationResult Bad(string valueType, ValidationContext ctx) =>
        new ValidationResult($"{ctx.DisplayName} cannot be {valueType}", new string[] { ctx.MemberName! });
}


// ==================================
// Example usage of validation logic
public class ApiController
{
    // Style 1 -- log each error and return an error code
    public object ApiCall(IncomingRequest request)
    {
        List<string> errors = request.Validate();
        if (errors.Any())
        {
            Console.WriteLine($"here are the errors ... ");
            return 423; //return invalid entity
        }

        return 200;
    }

    // Style 2 -- throw on validation error
    // unhandled exception will show up as 500 error
    // handling error means return a pre-defined error code for validation errors
    public object ApiCall2(IncomingRequest request)
    {
        try
        {
            request.EnsureValid();
            // proceed with rest of business logic

            return 200;
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
            return 423;
        }
    }
}

public class IncomingRequest : BaseValidation
{
    [EmailAddress]
    public string? EmailAddress { get; set; }

    [MinLength(1)]
    public string? PersonName { get; set; }

    [Range(0, 1000)]
    public int PersonID { get; set; }    
}

// This base class can be inherited to provide validation logic`
public abstract class BaseValidation
{
    // return list of error msgs if any
    public virtual List<string> Validate()
    {
        ValidationContext ctx = new(this);
        List<ValidationResult> results = new();
        if (Validator.TryValidateObject(this, ctx, results))
            return new List<string>();

        return results
        .Select(item => $"({item.MemberNames.FirstOrDefault() ?? "?"}) {item.ErrorMessage}")
        .ToList();
    }

    // throw validation exception on any validation error
    public virtual void EnsureValid()
    {
        List<string> errors = Validate();
        if (errors.Any())
        {
            throw new ValidationException(string.Join("; ", errors));
        }
    }
}