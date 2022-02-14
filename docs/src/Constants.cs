namespace Docs
{
    public static class Constants
    {
        public const string NoLink = nameof(NoLink);
        public const string EditLink = nameof(EditLink);
        public const string Description = nameof(Description);
        public const string Hidden = nameof(Hidden);

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

        public const string ExampleSourceFiles = nameof(ExampleSourceFiles);

        public const string ApiReference = "Reference";

        public static class Emojis
        {
            public const string Root = "EMOJIS_ROOT";
        }

        public static class Colors
        {
            public const string Url = "https://raw.githubusercontent.com/spectreconsole/spectre.console/main/resources/scripts/Generator/Data/colors.json";
            public const string Root = "COLORS_ROOT";
        }

        public static class Site
        {
            public const string Owner = "SITE_OWNER";
            public const string Repository = "SITE_REPOSITORY";
            public const string Branch = "SITE_BRANCH";
        }

        public static class Deployment
        {
            public const string NetlifySiteId = "NETLIFY_SITE_ID";
            public const string NetlifyAccessToken = "NETLIFY_ACCESS_TOKEN";
        }
    }
}
