using System.Collections.Generic;

public class Node 
{
    public string label;

    private List<Edge> neiEdges = new List<Edge>();
    public IEnumerable<Edge> neighbouringEdges
    {
        get
        {
            return neiEdges;
        }
    }

    public Node(string aLabel)
    {
        label = aLabel;
    }

    public void AddNeighbour(Node neighbour, int edgeWeight, int pheromone)
    {
        neiEdges.Add(new Edge(this, neighbour, edgeWeight, pheromone));
    }
    public override string ToString()
    {
        return label;
    }
}
