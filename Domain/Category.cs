using System.ComponentModel.DataAnnotations;
using Domain.Constants;

namespace Domain;

public class Category : Entity
{
    public string Name { get; set; }

    public virtual ICollection<Product> Products { get; set; }

    public Category()
    {
        Products = new HashSet<Product>();
    }

    public Category(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public void Update(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ValidationException(ErrorMessages.CategoryNameCannotBeEmptyOrNull);
        
        Name = name;
    } 
}