using Library.Domain.Models;

namespace Library.Application.DTOs
{
    public class Pagination<T>(List<T> items, int count, PageInfo pageInfo)
    {
        public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageInfo.PageSize);
        public int TotalCount { get; private set; } = count;
        public List<T> Items { get; private set; } = items;

        public static Pagination<T> ToPagedList(IEnumerable<T> source, PageInfo pageInfo)
        {
            var count = source.Count();
            var items = source.Skip((pageInfo.PageIndex - 1) * pageInfo.PageSize).Take(pageInfo.PageSize).ToList();
            return new Pagination<T>(items, count, pageInfo);
        }
    }
}
