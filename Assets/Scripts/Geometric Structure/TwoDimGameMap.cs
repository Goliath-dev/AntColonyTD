public class TwoDimGameMap 
{
    public TwoDimPoint startNode { get; }
    public TwoDimPoint finalNode { get; }
    public TwoDimGraph graph { get; }

    public TwoDimGameMap(TwoDimPoint startNode, TwoDimPoint finalNode, TwoDimGraph graph)
    {
        this.startNode = startNode;
        this.finalNode = finalNode;
        this.graph = graph;
    }
}
