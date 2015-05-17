using System.Collections.Generic;

namespace Client
{
    public interface IFooServiceProxy
    {
        BarResult Get(int id);
        IEnumerable<BarResult> Search(BarQuery query);
    }

    public class BarQuery
    {
        public string Name { get; set; }
    }

    public class BarResult
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}