using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class AnsiConsoleFacade : IAnsiConsole
    {
        private readonly object _renderLock;
        private readonly AnsiConsoleBackend _ansiBackend;
        private readonly LegacyConsoleBackend _legacyBackend;

        public Profile Profile { get; }
        public IAnsiConsoleCursor Cursor => GetBackend().Cursor;
        public IAnsiConsoleInput Input { get; }
        public RenderPipeline Pipeline { get; }

        public AnsiConsoleFacade(Profile profile)
        {
            _renderLock = new object();
            _ansiBackend = new AnsiConsoleBackend(profile);
            _legacyBackend = new LegacyConsoleBackend(profile);

            Profile = profile ?? throw new ArgumentNullException(nameof(profile));
            Input = new DefaultInput(Profile);
            Pipeline = new RenderPipeline();
        }

        public void Clear(bool home)
        {
            lock (_renderLock)
            {
                GetBackend().Clear(home);
            }
        }

        public void Write(IEnumerable<Segment> segments)
        {
            lock (_renderLock)
            {
                GetBackend().Render(segments);
            }
        }

        private IAnsiConsoleBackend GetBackend()
        {
            if (Profile.Capabilities.Ansi)
            {
                return _ansiBackend;
            }

            return _legacyBackend;
        }
    }
}
