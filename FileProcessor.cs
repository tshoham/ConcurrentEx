using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentEx
{
    public class FileProcessor : Processor
    {
        private static readonly List<string> IGNORE_WORDS = new() { "the", "and", "she", "you", "have", "has", "does", "are", "for", "this", "was", "were", "on", "in", "had", "that", "they" ,"his","with","their","not","been","them", "all","which","from","out","there","but", "him", "other", "did", "into", "than", "every", "any", "what", "her", "never", "after", "very", "about", "even" , "our", "no", "of", "is", "we", "do", "to", "it", "he", ""};
        public ConcurrentQueue<string> _lineQueue;
        public ConcurrentDictionary<string, int> _wordCountDic;

        public FileProcessor(ConcurrentQueue<string> lineQueue, ConcurrentDictionary<string, int> concurrentDic)
        {
            _lineQueue = lineQueue;
            _wordCountDic = concurrentDic;
        }

        private string GetLineFromQueue()
        {
            string currLine;
            if (_lineQueue.TryDequeue(out currLine))
            {
                return currLine;
            }
            else return null;
        }

        protected override async Task StartProcessing()
        {
            var splitLine = SplitLine(GetLineFromQueue());
            var cleanLine = splitLine.Where(word => !IGNORE_WORDS.Contains(word)).ToList();
            cleanLine.ForEach(word => _wordCountDic.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1));
        }

        private List<string> SplitLine(string line)
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            return line.Split(/*delimeters*/delimiterChars).ToList(); //TODO: this is NOT good enough!!   Need to remove all puntuation from the words after the split.         
        }
    }
}
