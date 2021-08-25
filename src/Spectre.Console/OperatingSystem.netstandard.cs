#if NETSTANDARD
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Spectre.Console
{
    /// <summary>
    /// Shims the System.OperatingSystem class on .NET Standard.
    /// </summary>
    /// <remarks>
    /// Adopted from dotnet/runtime.
    /// </remarks>
    internal static unsafe class OperatingSystem
    {
        private static readonly bool _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private static readonly (int Major, int Minor, int Build, int Revision) _windowsVersion = GetWindowsVersion();

        public static bool IsWindows() => _isWindows;

        public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
        {
            if (!IsWindows())
            {
                return false;
            }

            var current = _windowsVersion;

            if (current.Major != major)
            {
                return current.Major > major;
            }

            if (current.Minor != minor)
            {
                return current.Minor > minor;
            }

            if (current.Build != build)
            {
                return current.Build > build;
            }

            return current.Revision >= revision;
        }

        private static (int Major, int Minor, int Build, int Revision) GetWindowsVersion()
        {
            if (!IsWindows())
            {
                return default;
            }

            RTL_OSVERSIONINFOEX versionInfo = default;
            versionInfo.dwOSVersionInfoSize = (uint)sizeof(RTL_OSVERSIONINFOEX);
            int exitCode = RtlGetVersion(&versionInfo);

            // It is documented that the API always succeeds.
            Debug.Assert(exitCode == 0, "RtlGetVersion failed.");

            return ((int)versionInfo.dwMajorVersion, (int)versionInfo.dwMinorVersion, (int)versionInfo.dwBuildNumber, 0);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "We use the same field names as the Windows API declaration")]
        private struct RTL_OSVERSIONINFOEX
        {
            internal uint dwOSVersionInfoSize;

            internal uint dwMajorVersion;

            internal uint dwMinorVersion;

            internal uint dwBuildNumber;

            internal uint dwPlatformId;

            internal fixed char szCSDVersion[128];
        }

        [DllImport("ntdll.dll", ExactSpelling = true)]
        private static extern int RtlGetVersion(RTL_OSVERSIONINFOEX* lpVersionInformation);
    }
}
#endif
