using System.Reflection;
using System.Text.RegularExpressions;

public static class AssemblyExtensions
{
    public static string GetApplicationRoot(this Assembly assembly)
    {
        string assemblyPath = assembly.Location!;
        string assemblyDirectory = Path.GetDirectoryName(assemblyPath)!;
        DirectoryInfo directoryInfo = new(assemblyDirectory);

        while (directoryInfo != null && !directoryInfo.EnumerateFiles("*.sln", SearchOption.TopDirectoryOnly).Any())
        {
            directoryInfo = directoryInfo.Parent!;
        }
        return directoryInfo?.FullName!;
    }
}