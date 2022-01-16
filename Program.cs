using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurrentEx
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await new PuppetMaster("C:/Users/talshoham/source/repos/ConcurrentEx/AnimalFarm.txt").Start();
        }
    }
}
