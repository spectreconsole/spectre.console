namespace Docs
{
    public static class Constants
    {
        public const string NoContainer = nameof(NoContainer);
        public const string NoSidebar = nameof(NoSidebar);
        public const string NoLink = nameof(NoLink);
        public const string Topic = nameof(Topic);
        public const string EditLink = nameof(EditLink);
        public const string Description = nameof(Description);
        public const string Hidden = nameof(Hidden);
        public const string XmlDocs = "XmlDocs";
        public const string XmlDocsType = "XmlDocsType";

        /// <summary>
        /// Indicates where to locate source files for the API documentation.
        /// By default the globbing pattern "src/**/{!bin,!obj,!packages,!*.Tests,}/**/*.cs"
        /// is used which searches for all "*.cs" files at any depth under a "src" folder
        /// but not under "bin", "obj", "packages" or "Tests" folders. You can specify
        /// your own globbing pattern (or more than one globbing pattern) if your source
        /// files are found elsewhere.
        /// </summary>
        /// <type><see cref="string"/> or <c>IEnumerable&lt;string&gt;</c></type>
        public const string SourceFiles = nameof(SourceFiles);

        /// <summary>
        /// Indicates where to locate solution files for the API documentation.
        /// </summary>
        /// <type><see cref="string"/> or <c>IEnumerable&lt;string&gt;</c></type>
        public const string SolutionFiles = nameof(SolutionFiles);

        public static class Emojis
        {
            public const string Root = "EMOJIS_ROOT";
        }

        public static class Colors
        {
            public const string Url = "https://raw.githubusercontent.com/spectreconsole/spectre.console/main/resources/scripts/Generator/Data/colors.json";
            public const string Root = "COLORS_ROOT";
        }

        public static class CodeProvider
        {
            public const string Solution = @"../examples/Examples.sln";
        }

        public static class Site
        {
            public const string Owner = "SITE_OWNER";
            public const string Repository = "SITE_REPOSITORY";
            public const string Branch = "SITE_BRANCH";
        }

        public static class Deployment
        {
            public const string GitHubToken = "GITHUB_TOKEN";
            public const string TargetBranch = "DEPLOYMENT_TARGET_BRANCH";
        }

        public static class Sections
        {
            public const string Splash = nameof(Splash);
            public const string Sidebar = nameof(Sidebar);
            public const string Subtitle = nameof(Subtitle);
        }
    }
}
