using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentEx
{
    public abstract class Processor
    {
        private readonly int _processId;
        private static int _lastProcessId = 1;
        protected bool _isRunning; // Use State machine : 0 - ready , 1 - running , 2- Finished , 3 - canceled , 4- Error 

        public Processor()
        {
            _processId = _lastProcessId++;
            _isRunning = false;
        }

        public Task Start()
        {
            if (!_isRunning)
            {
                Console.WriteLine($"Starting Process {_processId}.....");
                _isRunning = true;
                return Task.Run(() => StartProcessing());
            }
            return Task.CompletedTask;
        }

        public void Stop()
        {
            if (_isRunning)
            {
                Console.WriteLine("Stopping.....");
                _isRunning = false;
            }
        }

        protected abstract Task StartProcessing();
    }
}
