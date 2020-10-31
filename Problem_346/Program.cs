using System;
using System.Collections.Generic;

namespace Problem_346
{
    public class InputRoutes
    {
        public string Source;
        public string Destination;
        public int Price;

        public InputRoutes(string src, string dest, int cost)
        {
            Source = src;
            Destination = dest;
            Price = cost;
        }

        string GetSource() => Source;
        string GetDestination() => Destination;
        int GetCost() => Price;
    }

    public class RouteNode
    {
        public string Name;
        public Dictionary<RouteNode, int> Destinations = null;

        public RouteNode(string name)
        {
            Name = name;
        }

        public void AddDestination(string destinationName, int travelCost)
        {
            if (destinationName == Name)
                return;

            if (Destinations == null)
                Destinations = new Dictionary<RouteNode, int>();

            Destinations.Add(new RouteNode(destinationName), travelCost);
        }
    }

    class Program
    {
        static List<InputRoutes> inputData = new List<InputRoutes>();
        static Dictionary<string, RouteNode> AvailableRoutes = new Dictionary<string, RouteNode>();
        static string shortestRoute = "";
        static int? cheapestPrice = null;

        static void Main(string[] args)
        {
            // fluent initialization
            inputData.AddRoute("JFK", "ATL", 150)
                .AddRoute("ATL", "SFO", 400)
                .AddRoute("ORD", "LAX", 200)
                .AddRoute("LAX", "DFW", 80)
                .AddRoute("JFK", "HKG", 800)
                .AddRoute("ATL", "ORD", 90)
                .AddRoute("JFK", "LAX", 500)
                .AddRoute("JFK", "PTL", 10)
                .AddRoute("SFO", "HKG", 20)
                .AddRoute("SFO", "DFW", 75)
                .AddRoute("DFW", "PTL", 20)
                .AddRoute("PTL", "ABC", 5)
                .AddRoute("ABC", "LAX", 10);

            bool isNewNode = false;
            foreach (var route in inputData)
            {
                RouteNode node = null;
                if (!AvailableRoutes.TryGetValue(route.Source, out node))
                {
                    node = new RouteNode(route.Source);
                    isNewNode = true;
                }
                node.AddDestination(route.Destination, route.Price);

                if (isNewNode)
                    AvailableRoutes.Add(route.Source, node);

                isNewNode = false;
            }

            FindCheapestRoute("JFK", "LAX", "JFK", 0, 3);
            if (string.IsNullOrEmpty(shortestRoute))
            {
                Console.WriteLine("No connecting flights between JFK and LAX");
            }
            else
            {
                Console.WriteLine("Cheapest Path: " + shortestRoute);
                Console.WriteLine("Price: " + cheapestPrice);
            }

            Console.ReadLine();
        }

        static bool FindCheapestRoute(string source, string destination, string route, int price, int kHopsLeft)
        {
            if (source == destination)
            {
                Console.WriteLine("Source and destination cannot be same");
                return false;
            }

            if (kHopsLeft <= 0)
            {
                return false;
            }

            RouteNode alldestinations = null;
            if (!AvailableRoutes.TryGetValue(source, out alldestinations))
            {                
                return false;
            }

            foreach (KeyValuePair<RouteNode, int> connectingRoute in alldestinations.Destinations)
            {
                route += "->" + connectingRoute.Key.Name;
                price += connectingRoute.Value;

                if (connectingRoute.Key.Name != destination) // reached destination
                {
                    if (!FindCheapestRoute(connectingRoute.Key.Name, destination, route, price, kHopsLeft - 1))
                    {
                        // revert route changes in case of error
                        route = route.Replace("->" + connectingRoute.Key.Name, "");
                        price -= connectingRoute.Value;
                    }
                }
                else
                {
                    if (cheapestPrice == null || cheapestPrice > price)
                    {
                        cheapestPrice = price;
                        shortestRoute = route;
                    }
                }

                if (route.Contains("->" + connectingRoute.Key.Name))
                {
                    route = route.Replace("->" + connectingRoute.Key.Name, "");
                    price -= connectingRoute.Value;
                }
            }

            return true;
        }
    }
}
