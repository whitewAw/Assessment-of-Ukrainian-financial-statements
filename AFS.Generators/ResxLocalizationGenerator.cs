using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AFS.Generators;

/// <summary>
/// Source generator that creates AOT-safe localization dictionaries from .resx files.
/// Generates a ChartLabels class with FrozenDictionary for O(1) lookup without reflection.
/// </summary>
[Generator]
public class ResxLocalizationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find all .resx files
        var resxFiles = context.AdditionalTextsProvider
            .Where(file => file.Path.EndsWith(".resx", StringComparison.OrdinalIgnoreCase));

        // Combine with compilation
        var compilationAndFiles = context.CompilationProvider.Combine(resxFiles.Collect());

        // Generate source
        context.RegisterSourceOutput(compilationAndFiles, (spc, source) =>
        {
            var (compilation, files) = source;
            GenerateLocalization(spc, compilation, files);
        });
    }

    private void GenerateLocalization(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<AdditionalText> resxFiles)
    {
        if (resxFiles.IsDefaultOrEmpty)
            return;

        // Group files by base name (Resource.resx, Resource.uk-UA.resx, etc.)
        var fileGroups = resxFiles
            .Where(f => Path.GetFileName(f.Path).StartsWith("Resource", StringComparison.OrdinalIgnoreCase))
            .GroupBy(f => GetBaseName(f.Path))
            .ToList();

        if (!fileGroups.Any())
            return;

        // Parse the main Resource.resx to get all keys
        var mainResx = resxFiles.FirstOrDefault(f => 
            Path.GetFileName(f.Path).Equals("Resource.resx", StringComparison.OrdinalIgnoreCase));

        if (mainResx == null)
            return;

        var allKeys = ParseResxKeys(mainResx);
        if (!allKeys.Any())
            return;

        // Parse all culture-specific files
        var cultureData = new Dictionary<string, Dictionary<string, string>>();
        
        foreach (var file in resxFiles.Where(f => 
            Path.GetFileName(f.Path).StartsWith("Resource", StringComparison.OrdinalIgnoreCase)))
        {
            var culture = GetCultureFromFileName(file.Path);
            var entries = ParseResxEntries(file);
            
            if (entries.Any())
            {
                cultureData[culture] = entries;
            }
        }

        // Generate the source code
        var source = GenerateChartLabelsClass(allKeys, cultureData);
        context.AddSource("ChartLabels.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    private static string GetBaseName(string path)
    {
        var fileName = Path.GetFileNameWithoutExtension(path);
        var dotIndex = fileName.IndexOf('.');
        return dotIndex > 0 ? fileName.Substring(0, dotIndex) : fileName;
    }

    private static string GetCultureFromFileName(string path)
    {
        var fileName = Path.GetFileNameWithoutExtension(path);
        var parts = fileName.Split('.');
        
        // Resource.resx -> "" (invariant)
        // Resource.uk-UA.resx -> "uk-UA"
        // Resource.en.resx -> "en"
        return parts.Length > 1 ? parts[1] : "";
    }

    private static List<string> ParseResxKeys(AdditionalText file)
    {
        var keys = new List<string>();
        
        try
        {
            var text = file.GetText()?.ToString();
            if (string.IsNullOrEmpty(text))
                return keys;

            var doc = XDocument.Parse(text);
            var dataElements = doc.Descendants("data")
                .Where(e => e.Attribute("name") != null);

            foreach (var element in dataElements)
            {
                var name = element.Attribute("name")?.Value;
                if (!string.IsNullOrEmpty(name))
                {
                    keys.Add(name);
                }
            }
        }
        catch
        {
            // Ignore parse errors
        }

        return keys;
    }

    private static Dictionary<string, string> ParseResxEntries(AdditionalText file)
    {
        var entries = new Dictionary<string, string>();
        
        try
        {
            var text = file.GetText()?.ToString();
            if (string.IsNullOrEmpty(text))
                return entries;

            var doc = XDocument.Parse(text);
            var dataElements = doc.Descendants("data")
                .Where(e => e.Attribute("name") != null);

            foreach (var element in dataElements)
            {
                var name = element.Attribute("name")?.Value;
                var value = element.Element("value")?.Value;
                
                if (!string.IsNullOrEmpty(name) && value != null)
                {
                    entries[name] = value;
                }
            }
        }
        catch
        {
            // Ignore parse errors
        }

        return entries;
    }

    private static string GenerateChartLabelsClass(
        List<string> allKeys,
        Dictionary<string, Dictionary<string, string>> cultureData)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("// This file is generated by AFS.Generators.ResxLocalizationGenerator");
        sb.AppendLine("// Do not modify this file directly.");
        sb.AppendLine();
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Frozen;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Globalization;");
        sb.AppendLine();
        sb.AppendLine("namespace AFS.ComponentLibrary.Resources;");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// AOT-safe localization generated from .resx files.");
        sb.AppendLine("/// Uses FrozenDictionary for O(1) lookup without reflection.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static class ChartLabels");
        sb.AppendLine("{");
        
        // Generate culture dictionaries
        foreach (var culture in cultureData.Keys.OrderBy(c => c))
        {
            var cultureName = string.IsNullOrEmpty(culture) ? "Invariant" : culture.Replace("-", "_");
            var entries = cultureData[culture];
            
            sb.AppendLine($"    private static readonly FrozenDictionary<string, string> _{cultureName} = new Dictionary<string, string>");
            sb.AppendLine("    {");
            
            foreach (var entry in entries.OrderBy(e => e.Key))
            {
                var escapedValue = EscapeString(entry.Value);
                sb.AppendLine($"        [\"{entry.Key}\"] = \"{escapedValue}\",");
            }
            
            sb.AppendLine("    }.ToFrozenDictionary(StringComparer.Ordinal);");
            sb.AppendLine();
        }
        
        // Generate culture lookup dictionary
        sb.AppendLine("    private static readonly FrozenDictionary<string, FrozenDictionary<string, string>> _cultures = new Dictionary<string, FrozenDictionary<string, string>>");
        sb.AppendLine("    {");
        
        foreach (var culture in cultureData.Keys.OrderBy(c => c))
        {
            var cultureName = string.IsNullOrEmpty(culture) ? "Invariant" : culture.Replace("-", "_");
            var cultureKey = string.IsNullOrEmpty(culture) ? "" : culture;
            sb.AppendLine($"        [\"{cultureKey}\"] = _{cultureName},");
        }
        
        sb.AppendLine("    }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);");
        sb.AppendLine();
        
        // Generate Get method
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Gets a localized string for the current UI culture. AOT-safe.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static string Get(string? key)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (string.IsNullOrEmpty(key))");
        sb.AppendLine("            return \"Unknown\";");
        sb.AppendLine();
        sb.AppendLine("        var culture = CultureInfo.CurrentUICulture;");
        sb.AppendLine("        ");
        sb.AppendLine("        // Try exact culture (e.g., \"uk-UA\")");
        sb.AppendLine("        if (_cultures.TryGetValue(culture.Name, out var exactDict) && exactDict.TryGetValue(key, out var exactValue))");
        sb.AppendLine("            return exactValue;");
        sb.AppendLine();
        sb.AppendLine("        // Try parent culture (e.g., \"uk\")");
        sb.AppendLine("        if (_cultures.TryGetValue(culture.TwoLetterISOLanguageName, out var parentDict) && parentDict.TryGetValue(key, out var parentValue))");
        sb.AppendLine("            return parentValue;");
        sb.AppendLine();
        sb.AppendLine("        // Fallback to invariant culture");
        sb.AppendLine("        if (_cultures.TryGetValue(\"\", out var invariantDict) && invariantDict.TryGetValue(key, out var invariantValue))");
        sb.AppendLine("            return invariantValue;");
        sb.AppendLine();
        sb.AppendLine("        // Return key as fallback");
        sb.AppendLine("        return key;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        // Generate GetOrKey method for charts
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Gets a localized string with key as fallback. AOT-safe.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static string GetOrKey(string? key) => Get(key);");
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }

    private static string EscapeString(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }
}
