using ApexCharts;

namespace AFS.ComponentLibrary.Helpers;

/// <summary>
/// Provides consistent ApexCharts styling options across all chart components.
/// Dark mode styling is handled via CSS in dark-theme.css using [data-theme="dark"] selectors.
/// </summary>
public static class ApexChartsHelper
{
    /// <summary>
    /// Gets the base chart options with consistent theming.
    /// Each call generates a new instance with a unique chart ID to prevent conflicts
    /// when multiple charts are rendered on the same page.
    /// </summary>
    public static ApexChartOptions<T> GetBaseOptions<T>() where T : class
    {
        return new ApexChartOptions<T>
        {
            // Note: ApexCharts Blazor generates unique IDs automatically per instance,
            // but we ensure each call returns a fresh options object
            Theme = new Theme
            {
                Mode = Mode.Light,
                Palette = PaletteType.Palette1
            },
            Chart = new Chart
            {
                Background = "transparent",
                FontFamily = "inherit",
                Width = "100%",
                RedrawOnParentResize = true,
                RedrawOnWindowResize = true,
                Animations = new Animations
                {
                    Enabled = true,
                    Speed = 300 // Faster animations reduce perceived layout shift
                }
            },
            Legend = new Legend
            {
                Position = LegendPosition.Bottom
            }
        };
    }

    /// <summary>
    /// Gets options configured for pie charts.
    /// </summary>
    public static ApexChartOptions<T> GetPieChartOptions<T>() where T : class
    {
        var options = GetBaseOptions<T>();

        options.DataLabels = new DataLabels
        {
            Formatter = "function(val) { return val.toFixed(1) + '%'; }",
            Style = new DataLabelsStyle
            {
                Colors = new List<string> { "#fff" }
            }
        };

        return options;
    }

    /// <summary>
    /// Gets options configured for donut charts with center label.
    /// </summary>
    public static ApexChartOptions<T> GetDonutChartOptions<T>() where T : class
    {
        var options = GetPieChartOptions<T>();

        options.PlotOptions = new PlotOptions
        {
            Pie = new PlotOptionsPie
            {
                Donut = new PlotOptionsDonut
                {
                    Labels = new DonutLabels
                    {
                        Show = true,
                        Total = new DonutLabelTotal
                        {
                            Show = true,
                            FontSize = "14px"
                        }
                    }
                }
            }
        };

        return options;
    }

    /// <summary>
    /// Gets options configured for line charts.
    /// </summary>
    public static ApexChartOptions<T> GetLineChartOptions<T>() where T : class
    {
        var options = GetBaseOptions<T>();

        options.Chart!.Toolbar = new Toolbar { Show = true };

        options.Stroke = new Stroke
        {
            Curve = Curve.Smooth,
            Width = 3
        };

        options.Markers = new Markers
        {
            Size = 5
        };

        options.Tooltip = new Tooltip
        {
            Theme = Mode.Dark
        };

        return options;
    }
}
