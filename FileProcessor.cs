using Newtonsoft.Json;
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
        private static readonly List<string> IGNORE_WORDS = new() { "", "the", "and", "she", "you", "have", "has", "does", "are", "for", "this", "was", "were", "on", "in", "had", "that", "they" ,"his","with","their","not","been","them", "all","which","from","out","there","but", "him", "other", "did", "into", "than", "every", "any", "what", "her", "never", "after", "very", "about", "even" , "our", "no", "of", "is", "we", "do", "to", "it", "he", ""};
        public ConcurrentQueue<string> _sentenceQueue;
        public ConcurrentDictionary<string, int> _wordCountDic;
        public bool _stillReadingFile;

        public FileProcessor(ConcurrentQueue<string> sentenceQueue, ConcurrentDictionary<string, int> wordCountDic)
        {
            _sentenceQueue = sentenceQueue;
            _wordCountDic = wordCountDic;
            _stillReadingFile = true;
        }

        private async Task<string> GetSentenceFromQueue()
        {
            //add some retry policy for dequing s so that is will only come back empty when the file is all read
            string currLine;
            while (_sentenceQueue.TryDequeue(out currLine) == true|| _isRunning)
            {
                if (currLine is not null)
                {
                    return currLine;
                }
            }

            return null;
        }

        protected override async Task StartProcessing()
        {
            string sentence;
            while ((sentence = await GetSentenceFromQueue()) is not null)
            {
                var splitSentence = SplitSentence(sentence);
                var cleanSentence = splitSentence.Where(word => !IGNORE_WORDS.Contains(word)).ToList();
                cleanSentence.ForEach(word => _wordCountDic.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1));
            }           
        }

        private List<string> SplitSentence(string sentence)
        {
            var punctuation = sentence.Where(Char.IsPunctuation).Distinct().ToArray();
            return sentence.Split().Select(x => x.Trim(punctuation)).ToList();
        }
    }
}
