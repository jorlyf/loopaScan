using System;
using System.Collections.Generic;
using System.Linq;

namespace loopaScan.Infrastructure
{
    static class Generic
    {
        public static List<List<string>> SplitStringList(List<string> source, int nChunks)
        {
            int totalLength = source.Count;
            int chunkLength = (int)Math.Ceiling(totalLength / (double)nChunks);
            var parts = Enumerable.Range(0, nChunks)
                                  .Select(i => source.Skip(i * chunkLength)
                                         .Take(chunkLength)
                                         .ToList())
                      .ToList();

            return parts;
        }
    }
}
