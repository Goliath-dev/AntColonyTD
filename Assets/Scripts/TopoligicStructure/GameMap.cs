public class GameMap
{
    public Graph UnderlyingGraph { get; private set; }
    public Node StartNode { get; private set; }
    public Node FinalNode { get; private set; }

    public GameMap(Graph graph, Node aStartNode, Node aFinalNode)
    {
        UnderlyingGraph = graph;
        StartNode = aStartNode;
        FinalNode = aFinalNode;
    }
}
