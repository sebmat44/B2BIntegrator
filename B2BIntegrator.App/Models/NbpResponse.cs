using System.Text.Json.Serialization;

namespace B2BIntegrator.App.Models;

public class NbpResponse
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("rates")]
    public List<NbpRate> Rates { get; set; } = new();
}

public class NbpRate
{
    [JsonPropertyName("effectiveDate")]
    public string EffectiveDate { get; set; } = string.Empty;

    [JsonPropertyName("mid")]
    public decimal Mid { get; set; }
}