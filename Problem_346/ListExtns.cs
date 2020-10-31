using System.Collections.Generic;

namespace Problem_346
{
    public static class ListExtns
    {
        public static List<InputRoutes> AddRoute(this List<InputRoutes> routes,
            string src, string dest, int price)
        {
            routes.Add(new InputRoutes(src, dest, price));
            return routes;
        }
    }
}
