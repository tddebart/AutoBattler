
using System;
using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list, Random r = null) 
    {
        r ??= new Random((int)DateTime.Now.Ticks);
        
        var shuffledList = list.Select(x => new { Number = r.Next(), Item = x }).OrderBy(x => x.Number).Select(x => x.Item);
        return shuffledList.ToList();
    }
}
