using Microsoft.EntityFrameworkCore;

namespace CRUD_asp.netMVC.Models.Product
{
    public class PaginatedList<T> : List<T>
    {
        public int PageCurrent { get; set; }
        public int TotalPage { get; set; }

        public PaginatedList(List<T> items, int count, int pageCurrent, int pageSize)
        {
            PageCurrent = pageCurrent;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public bool PreviousPage => PageCurrent > 1;
        public bool NextPage => PageCurrent < TotalPage;

        public static async Task<PaginatedList<T>> CreatePagAsync(IQueryable<T> source, int pageCurrent, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageCurrent, pageSize);
        }
    }
}
