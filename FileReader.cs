using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentEx
{
    public class FileReader : IFileReader
    {
        private ConcurrentQueue<string> _lineQueue;
        
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
            //todo: add some return value to mark done, so that in the processing threads- we create a loop based onthis. its not enough to check if the queeue is empty- becaseu it may be empty when more threads have been dequed than queued- which doesnt mean the file has been read completely.
            //on the other hand can add in the processor some loop that keeps checking if can dequed- and not check only once

        }
    }
}
