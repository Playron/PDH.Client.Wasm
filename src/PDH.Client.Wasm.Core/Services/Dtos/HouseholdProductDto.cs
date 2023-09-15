using System.Text.Json.Serialization;

namespace PDH.Client.Wasm.Core.Services.Dtos;

public class RootHouseholdProductDto
{   
    [JsonPropertyName("values")]
    public List<HouseholdProductDto>? HouseholdProducts { get; set; }
}

public class HouseholdProductDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null!;

    public string? Brand { get; set; } = null!;

    public string? Ean { get; set; } = null!;

    public string? Weight { get; set; }
    
    public WeightUnit WeightUnit { get; set; }
    
    public ProductCategory ProductGroup { get; set; }
}
public enum WeightUnit
{
    None,
    Piece,
    Milliliter,
    Deciliter,
    Litre,
    Gram,
    Kilogram
}
    
public enum ProductCategory
{
    None = 0,
    Fruit = 1,
    Vegetables = 2,
    CannedGoods = 3,
    Dairy = 4,
    Meat = 5,
    Fish = 6,
    Seafood = 7,
    Deli = 8,
    Spice = 9,
    Condiments = 10,
    Bakery = 11,
    Beverages = 12,
    Pasta = 13,
    Cereal = 14,
    BakingGoods = 15,
    FrozenFoods = 16,
    PersonalCare = 17,
    CleaningSupplies = 18,
    HealthCare = 19
}