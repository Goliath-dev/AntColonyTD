using System.Collections.Generic;

public class Graph 
{
    private List<Node> nodes;

    private List<Edge> inEdges = new List<Edge>();
    public IEnumerable<Edge> Edges 
    {
        get
        {
            return inEdges;
        }
    }
    public Graph(List<Node> aNodes)
    {
        nodes = aNodes;
        foreach(var node in nodes)
        {
            inEdges.AddRange(node.neighbouringEdges);
        }
    }
}
