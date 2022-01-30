using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace ConcurrentEx
{
    public class FileProcessor : Processor
    {
        private static readonly List<string> IGNORE_WORDS = new() { "", "the", "and", "she", "you", "have", "has", "does", "are", "for", "this", "was", "were", "on", "in", "had", "that", "they" ,"his","with","their","not","been","them", "all","which","from","out","there","but", "him", "other", "did", "into", "than", "every", "any", "what", "her", "never", "after", "very", "about", "even" , "our", "no", "of", "is", "we", "do", "to", "it", "he", ""};
        public BlockingCollection<string> _sentenceQueue;
        public ConcurrentDictionary<string, int> _wordCountDic;
        public bool _stillReadingFile;

        public FileProcessor(BlockingCollection<string> sentenceQueue, ConcurrentDictionary<string, int> wordCountDic)
        {
            _sentenceQueue = sentenceQueue;
            _wordCountDic = wordCountDic;
            _stillReadingFile = true;
        }


        protected override async Task StartProcessing()
        {
            foreach (var sentence in _sentenceQueue.GetConsumingEnumerable())
            {
                var splitSentence = SplitSentence(sentence);
                var cleanSentence = splitSentence.Where(word => !IGNORE_WORDS.Contains(word)).ToList();
                cleanSentence.ForEach(word =>
                {
                    _wordCountDic.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1);
                });
            }
        }

        private static List<string> SplitSentence(string sentence) => Regex.Split(sentence, @"\W+").ToList();
        
    }
}
