﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentEx
{
    public interface IFileReader
    {
        Task ReadFile(string path);
    }
}
