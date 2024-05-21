using System.ComponentModel.DataAnnotations;
using Domain.Constants;

namespace Domain;

public class Product(int categoryId, string name) : Entity
{
    public int CategoryId { get; private set; } = categoryId;
    public string Name { get; private set; } = name;

    public virtual Category Category { get; set; }
    
    public void Update(int categoryId, string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ValidationException(ErrorMessages.ProductNameCannotBeEmptyOrNull);
        
        CategoryId = categoryId;
        Name = name;
    }
}