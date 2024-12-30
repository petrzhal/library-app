namespace Library.Domain.Models
{
    public class PageInfo
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public PageInfo(int pageIndex, int pageSize) 
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
