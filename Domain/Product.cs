using Core.Persistence.Repositories;

namespace Domain;

public class Product(int categoryId, string name) : Entity
{
    public int CategoryId { get; private set; } = categoryId;
    public string Name { get; private set; } = name;

    public Category? Category { get; set; }

    public void Update(int categoryId, string name)
    {
        CategoryId = categoryId;
        Name = name;
    }
}