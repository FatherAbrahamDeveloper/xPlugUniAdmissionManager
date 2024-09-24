using Microsoft.AspNetCore.Mvc.Razor;

namespace xPlugUniAdmissionManager;

public class AppViewConfig
{
    private static string[] _customSharedDirectory = ["Components", "Layouts", "ScriptRefs", "Widgets", "SideBars"];
    private static string[] _customViewDirectory = ["Registration"];
    private static string[] _customViewSubDirectory = ["Instructions", "Modules", "ReadViews"];

    public static List<string> CustomSharedDirectories()
    {
        List<string> locations = [];
        try
        {
            locations.Add($"/Views/Shared/{{0}}{RazorViewEngine.ViewExtension}");

            if (_customViewDirectory != null && _customViewDirectory.Length > 0)
            {
                _customViewDirectory.ForEachx(m =>
                {
                    if (_customViewSubDirectory != null && _customViewSubDirectory.Any())
                    {
                        _customViewSubDirectory.ForEachx(p =>
                        {
                            locations.Add($"/Views/{m.Trim()}/{p.Trim()}/{{0}}{RazorViewEngine.ViewExtension}");
                        });
                    }
                    else
                    {
                        locations.Add($"/Views/{m.Trim()}/{{0}}{RazorViewEngine.ViewExtension}");
                    }
                });
            }


            if (_customSharedDirectory != null && _customSharedDirectory.Any())
            {
                _customSharedDirectory.ForEachx(m =>
                {
                    locations.Add($"/Views/Shared/{m.Trim()}/{{0}}{RazorViewEngine.ViewExtension}");
                });
            }

            return locations;
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return locations;
        }
    }
}
