using System;
using System.Threading;

namespace Spectre.Console
{
    internal sealed class ProgressRefreshThread : IDisposable
    {
        private readonly ProgressContext _context;
        private readonly TimeSpan _refreshRate;
        private readonly ManualResetEvent _running;
        private readonly ManualResetEvent _stopped;
        private readonly Thread? _thread;

        public ProgressRefreshThread(ProgressContext context, TimeSpan refreshRate)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _refreshRate = refreshRate;
            _running = new ManualResetEvent(false);
            _stopped = new ManualResetEvent(false);

            _thread = new Thread(Run);
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Dispose()
        {
            if (_thread == null || !_running.WaitOne(0))
            {
                return;
            }

            _stopped.Set();
            _thread.Join();

            _stopped.Dispose();
            _running.Dispose();
        }

        private void Run()
        {
            _running.Set();

            try
            {
                while (!_stopped.WaitOne(_refreshRate))
                {
                    _context.Refresh();
                }
            }
            finally
            {
                _stopped.Reset();
                _running.Reset();
            }
        }
    }
}
