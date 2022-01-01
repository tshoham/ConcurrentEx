using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentEx
{
    class PuppetMaster
    {
        private const int NUM_THREADS = 5;
        private static ConcurrentQueue<string> _lineQueue;
        private static ConcurrentDictionary<string, int> _wordCountDic;
        private IFileReader _reader;
        private IFileProcessor _processor;

        public PuppetMaster()
        {
            _lineQueue = new ConcurrentQueue<string>();
            _wordCountDic = new ConcurrentDictionary<string, int>();

            _reader = new FileReader(_lineQueue);
            _processor = new FileProcessor(_lineQueue, _wordCountDic);
        }

        public async Task ReadAndProcessFile (string pathToFile)
        {
            var t1 = _reader.ReadFile(pathToFile);

            //todo: need to create some 
            var tasks = Enumerable.Repeat(_processor.ProcessLine(), NUM_THREADS - 1).ToArray();
        }
    }
}
