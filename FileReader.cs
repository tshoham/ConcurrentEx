using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentEx
{
    class FileReader : IFileReader
    {
        public static ConcurrentQueue<string> _lineQueue;

        public FileReader(ConcurrentQueue<string> sentenceQueue)
        {
            _lineQueue = sentenceQueue;
        }

        public async Task ReadFile(string path)
        {
            using StreamReader reader = File.OpenText(path);
            string line;

            while ((line = await reader.ReadLineAsync()) is not null)
            {
                _lineQueue.Enqueue(line);
            }
        }
}
