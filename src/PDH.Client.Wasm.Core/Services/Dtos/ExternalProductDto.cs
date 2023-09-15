using System.Text.Json.Serialization;

namespace PDH.Client.Wasm.Core.Services.Dtos;

public class ExternalProductDto
{
    [JsonPropertyName("data")] public List<ProductInfo>? ProductInfo { get; set; }
}

public class ProductInfo
{
    public int Id { get; set; }
    
    public string? Name { get; set; }

    public string? Brand { get; set; }
    
    public string? Vendor { get; set; }
    
    public string? Ean { get; set; }
    
    public string? Url { get; set; }
    
    public string? Image { get; set; }
    
    public string? Description { get; set; }
    
    public string? Ingredients { get; set; }
    
    public double CurrentPrice { get; set; }
    
    public double? CurrentUnitPrice { get; set; }
    
    public int? Weight { get; set; }
    
    public string? WeightUnit { get; set; }
    
    public List<Label>? Labels { get; set; }
}

public class Icon
{
    public string? Svg { get; set; }
    
    public string? Png { get; set; }
}

public class Label
{
    public Icon? Icon { get; set; }
}