using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentEx
{
    public class FileReader : Processor
    {
        private ConcurrentQueue<string> _sentenceQueue;
        private string _pathToFile;


        public FileReader(ConcurrentQueue<string> sentenceQueue, string pathToFile)
        {
            _sentenceQueue = sentenceQueue;
            _pathToFile = pathToFile;
        }

        protected override async Task StartProcessing()
        {
            using StreamReader reader = File.OpenText(_pathToFile);
            string line;

            while ((line = await reader.ReadLineAsync()) is not null)
            {
                _sentenceQueue.Enqueue(line);
            }

            Console.WriteLine("DONE reading file");
        }
    }
}
