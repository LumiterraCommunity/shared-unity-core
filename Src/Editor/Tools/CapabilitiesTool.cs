using System;

/// <summary>
/// 
/// PlatformID:
/// https://docs.microsoft.com/en-us/dotnet/api/system.platformid?view=net-6.0
/// 
/// </summary>
public static class CapabilitiesTool
{
    public static bool IsWindow()
    {
        OperatingSystem osInfo = Environment.OSVersion;
        return osInfo.Platform is PlatformID.Win32S
        or PlatformID.Win32Windows
        or PlatformID.Win32NT
        or PlatformID.WinCE;
    }

    public static bool IsMac()
    {
        OperatingSystem osInfo = Environment.OSVersion;
        return osInfo.Platform == PlatformID.Unix;
    }

}