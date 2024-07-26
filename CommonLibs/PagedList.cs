namespace CommonLibs
{
    public static class PagedList
    {
        public static List<T> ToPagedList<T>(this List<T> set, int pageNumber, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(set);
            var itemToSkips = (pageNumber - 1) * pageSize;
            return set.Skip(itemToSkips).Take(pageSize).ToList();
        }
    }
}
