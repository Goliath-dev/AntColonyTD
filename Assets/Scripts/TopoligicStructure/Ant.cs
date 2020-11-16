using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
public class AntDiedArgs 
{
    public Ant AntArg { get; }

    public AntDiedArgs(Ant ant)
    {
        AntArg = ant;
    }
}
public class Ant 
{
    public Node CurrentNode { get; set; }
    public Node PrevNode { get; set; }

    private List<Edge> path = new List<Edge>();
    public IEnumerable<Edge> Path
    {
        get
        {
            return path;
        }
    }

    private AntDiedArgs antArgs;
    public event EventHandler<AntDiedArgs> AntDied;

    private int health = 0;
    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            if (value <= 0)
            {
                Die();
            }
            else
            {
                health = value;
            }
        }
    }
    private int CurrentPosOnEdge { get; set; } = 1;
    private Edge CurrentEdge { get; set; }
    public Ant(Node currentNode, int healthPoint)
    {
        antArgs = new AntDiedArgs(this);
        CurrentNode = currentNode;
        Health = healthPoint;
    }


    /// <summary>
    /// If in the node, chooses the edge to go onto and moves; if in the edge already, just moves along it.
    /// </summary>
    public void Move()
    {
        //While in node, choose a random edge among neighbouring ones except the edge we came here onto.
        if (CurrentPosOnEdge == 1)
        {
            CurrentEdge = ChooseRandomEdge(from edge in CurrentNode.neighbouringEdges where edge.NodeTo != PrevNode select edge);
        }
        MoveAlong(CurrentEdge);
    }

    public void ClearPath()
    {
        path.Clear();
    }
    private void MoveAlong(Edge edge)
    {
        if (CurrentPosOnEdge != edge.Weight)
        {
            CurrentPosOnEdge += 1;
        }
        else
        {
            PrevNode = CurrentNode;
            CurrentNode = edge.NodeTo;
            path.Add(edge);
            CurrentPosOnEdge = 1;
        }
    }

    private Edge ChooseRandomEdge(IEnumerable<Edge> edges)
    {
        var accumulatedPheromones = new List<int>(edges.Count());
        var sum = 0;
        foreach (var edge in edges)
        {
            sum += edge.Pheromone;
            accumulatedPheromones.Add(sum);
        }
        var rand = AntColonyAlgorithm.Rand.Next(0, accumulatedPheromones.Last());
        var edgesAndPheromones = Enumerable.Zip(edges, accumulatedPheromones, (edge, ph) => new Tuple<Edge, int>(edge, ph));
        return (from edgePh in edgesAndPheromones where edgePh.Item2 > rand select edgePh.Item1).FirstOrDefault();
    }

    private void Die()
    {
        AntDied?.Invoke(this, antArgs);
    }
}
