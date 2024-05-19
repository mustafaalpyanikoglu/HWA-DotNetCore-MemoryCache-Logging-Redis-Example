using Core.Persistence.Repositories;

namespace Domain;

public class Category(string name) : Entity
{
    public string Name { get; private set; } = name;

    public List<Product>? Products { get; set; }

    public void Update(string name)
    {
        Name = name;
    } 
}
