using AFS.Core.Models;
using System.Text.Json.Serialization;

namespace AFS.Core.Json;

[JsonSerializable(typeof(AFSModel))]
[JsonSerializable(typeof(Form1))]
[JsonSerializable(typeof(Form2))]
[JsonSerializable(typeof(AdditionalCompanyInfo))]
[JsonSerializable(typeof(FixedAssetsInfo))]
[JsonSerializable(typeof(BeginEnd))]
[JsonSerializable(typeof(CurrentPrevious))]
[JsonSerializable(typeof(string))]
[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNameCaseInsensitive = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    GenerationMode = JsonSourceGenerationMode.Default)]
public partial class AFSJsonSerializerContext : JsonSerializerContext
{
}
