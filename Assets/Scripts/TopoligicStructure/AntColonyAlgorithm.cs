using System.Collections.Generic;
using System.Linq;

public static class AntColonyAlgorithm 
{
    private const int AntCount = 100;
    private const int StepCount = 100;
    private const int AntHealth = 100;
    private static List<Ant> ants;
    public static System.Random Rand = new System.Random();

    public static List<Edge> Algorithm(GameMap map)
    {
        var result = new List<Edge>();
        var antRange = from i in Enumerable.Range(0, AntCount) select new Ant(map.StartNode, AntHealth);
        ants = new List<Ant>(antRange);
        for (int i = 0; i <= StepCount; i++)
        {
            foreach (var ant in ants)
            {
                ant.Move();
                if (ant.CurrentNode == map.FinalNode)
                {
                    result.Clear();
                    result.AddRange(ant.Path);
                    ant.CurrentNode = map.StartNode;
                    foreach (var edge in ant.Path.Distinct())
                    {
                        edge.Pheromone += 1;
                    }
                    ant.Path.Clear();
                }
            }
            if (i % 2 == 0) DryPheromones(map.UnderlyingGraph.Edges);
        }
        return result;
    }

    private static void DryPheromones(IEnumerable<Edge> edges)
    {
        foreach(var edge in edges)
        {
            if (edge.Pheromone > 1) edge.Pheromone -= 1;
        }
    }
}
