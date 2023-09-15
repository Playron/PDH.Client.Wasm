using PDH.Client.Wasm.Core.PDH.Services.Dtos;

namespace PDH.Client.Wasm.Core.PDH.Services;

public static class Extensions
{
    public static string ProductCategoryMapper(this ProductCategory category) => category switch
    {
        ProductCategory.None => "Ingen",
        ProductCategory.Fruit => "Frukt",
        ProductCategory.Vegetables => "Grønnsaker",
        ProductCategory.CannedGoods => "Hermetikk",
        ProductCategory.Dairy => "Bakst",
        ProductCategory.Meat => "Kjøtt",
        ProductCategory.Fish => "Fisk",
        ProductCategory.Seafood => "Sjømat",
        ProductCategory.Deli => "Deli",
        ProductCategory.Spice => "Krydder",
        ProductCategory.Condiments => "Tilbehør",
        ProductCategory.Bakery => "Bakst",
        ProductCategory.Beverages => "Drikkevarer",
        ProductCategory.Pasta => "Pasta",
        ProductCategory.Cereal => "Frokostblanding",
        ProductCategory.BakingGoods => "Bakevarer",
        ProductCategory.FrozenFoods => "Frossenmat",
        ProductCategory.PersonalCare => "Personlig hygene",
        ProductCategory.CleaningSupplies => "Vaskeutstyr",
        ProductCategory.HealthCare => "Helse",
        _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
    };

    public static string WeightTypeMapper(this WeightUnit unit) => unit switch
    {
        WeightUnit.None => "Ingen",
        WeightUnit.Piece => "stk",
        WeightUnit.Milliliter => "ml",
        WeightUnit.Deciliter => "dl",
        WeightUnit.Litre => "l",
        WeightUnit.Gram => "g",
        WeightUnit.Kilogram => "kg",
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
}