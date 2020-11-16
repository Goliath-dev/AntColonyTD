using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimGraph 
{
    private List<TwoDimPoint> nodes;
    public IEnumerable<TwoDimPoint> Nodes => nodes; 
    
    private List<TwoDimEdge> edges;
    public IEnumerable<TwoDimEdge> Edges => edges;

    public TwoDimGraph(int nodeCount, int edgeCount)
    {
        nodes = new List<TwoDimPoint>(nodeCount);
        edges = new List<TwoDimEdge>(edgeCount);
    }

    public void AddTwoDimNode(TwoDimPoint node)
    {
        nodes.Add(node);
    }

    public void AddTwoDimEdge(TwoDimEdge edge)
    {
        edges.Add(edge);
    }
}
