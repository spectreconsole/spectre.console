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

        public static class Emojis
        {
            public const string Root = "EMOJIS_ROOT";
        }

        public static class Colors
        {
            public const string Url = "https://raw.githubusercontent.com/spectreconsole/spectre.console/main/resources/scripts/Generator/Data/colors.json";
            public const string Root = "COLORS_ROOT";
        }

        public static class Snippets
        {
            public const string ExampleProject = @"../examples/Examples.sln";
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
