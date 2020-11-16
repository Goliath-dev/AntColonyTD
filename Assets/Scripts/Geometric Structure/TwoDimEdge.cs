using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TwoDimEdge
{
    public TwoDimPoint NodeFrom { get; private set; }
    public TwoDimPoint NodeTo { get; private set; }
    
    private List<TwoDimPoint> path = new List<TwoDimPoint>(2);
    public IEnumerable<TwoDimPoint> Path
    { 
        get
        {
            return path;
        }
    }

    public TwoDimEdge(TwoDimPoint nodeFrom, TwoDimPoint nodeTo)
    {
        NodeFrom = nodeFrom;
        NodeTo = nodeTo;
        path.Add(nodeFrom); 
        path.Add(nodeTo);
    }

    public TwoDimEdge(IEnumerable<TwoDimPoint> points)
    {
        path.AddRange(points);
        NodeFrom = path.First();
        NodeTo = path.Last();
    }
    public void AddPointToPath(TwoDimPoint point)
    {
        path.Insert(path.Count - 1, point);
    }

    public void AddRangeToPath(IEnumerable<TwoDimPoint> points)
    {
        path.InsertRange(path.Count - 1, points);
    }

    public override string ToString()
    {
        return $"({path.Aggregate(string.Empty, (first, second) => first == string.Empty ? $"{second}" : $"{first}, {second}")})"; //Yeah, I am a god of one-liners.
        //Looks weird, but actually it just formats text as "((x0, y0), (x1, y1), ... (xn, yn))". 
    }
}
