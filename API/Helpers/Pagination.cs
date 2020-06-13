using System.Collections.Generic;

namespace API.Helpers
{
    public class Pagination<T> where T : class
    {
        public Pagination(int _pageSize, int _pageIndex, int _count, IReadOnlyList<T> _data)
        {
            pageSize = _pageSize;
            pageIndex = _pageIndex;
            Count = _count;
            Data = _data;
        }

        public int pageSize { get; set; }
        public int pageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}