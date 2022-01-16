using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConcurrentEx
{
    public class PuppetMaster : Processor
    {
        private const int NUM_THREADS = 5;
        private ConcurrentQueue<string> _sentenceQueue;
        private ConcurrentDictionary<string, int> _wordCountDictionary;
        private string _pathToFile;

        public PuppetMaster(string pathToFile)
        {
            _sentenceQueue = new ConcurrentQueue<string>();
            _wordCountDictionary = new ConcurrentDictionary<string, int>();
            _pathToFile = pathToFile;
        }

        protected override async Task StartProcessing()
        {
            List<Processor> processors = new List<Processor>();
            List<Task> runningProcessors = new List<Task>();

            processors.Add(new FileReader(_sentenceQueue, _pathToFile));
            runningProcessors.Add(processors[0].Start());

            for (int i = 0; i < NUM_THREADS; ++i)
            {
                processors.Add(new FileProcessor(_sentenceQueue, _wordCountDictionary));
                runningProcessors.Add(processors[i+1].Start());
            }
            await Task.WhenAny(runningProcessors);

            if (runningProcessors[0].IsCompleted)
            {
                processors.ForEach(processor => processor.Stop());
            }

            await Task.WhenAll(runningProcessors);

            Console.WriteLine(JsonConvert.SerializeObject(_wordCountDictionary));
        }
    }
}
