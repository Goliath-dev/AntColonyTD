using System;
using System.Collections.Generic;

public class Edge: IEquatable<Edge>
{
    public Node NodeFrom { get; private set; }
    public Node NodeTo { get; private set; }
    public int Weight { get; set; }
    public int Pheromone { get; set; }

    public Edge(Node nodeFrom, Node nodeTo, int weight, int pheromone)
    {
        this.NodeFrom = nodeFrom;
        this.NodeTo = nodeTo;
        this.Weight = weight;
        this.Pheromone = pheromone;
    }

    //public override bool Equals(object obj)
    //{
    //    return obj is Edge edge &&
    //           (NodeFrom.Equals(edge.NodeFrom) &&
    //           NodeTo.Equals(edge.NodeTo) ||
    //           NodeFrom.Equals(edge.NodeTo) &&
    //           NodeTo.Equals(edge.NodeFrom)) &&
    //           Weight == edge.Weight;
    //}

    public override bool Equals(object obj)
    {
        return obj is Edge edge &&
               NodeFrom.Equals(edge.NodeFrom) &&
               NodeTo.Equals(edge.NodeTo) &&
               Weight == edge.Weight;
    }

    //public override int GetHashCode()
    //{
    //    int hashCode = -1700587230;
    //    hashCode = hashCode * -1521134295 + EqualityComparer<Node>.Default.GetHashCode(NodeFrom)
    //        + EqualityComparer<Node>.Default.GetHashCode(NodeTo);
    //    hashCode = hashCode * -1521134295 + Weight.GetHashCode();
    //    return hashCode;
    //}

    public override int GetHashCode()
    {
        int hashCode = -1700587230;
        hashCode = hashCode * -1521134295 + EqualityComparer<Node>.Default.GetHashCode(NodeFrom);
        hashCode = hashCode * -1521134295 + EqualityComparer<Node>.Default.GetHashCode(NodeTo);
        hashCode = hashCode * -1521134295 + Weight.GetHashCode();
        return hashCode;
    }

    //public bool Equals(Edge other)
    //{
    //    return
    //   (NodeFrom.Equals(other.NodeFrom) &&
    //   NodeTo.Equals(other.NodeTo) ||
    //   NodeFrom.Equals(other.NodeTo) &&
    //   NodeTo.Equals(other.NodeFrom)) &&
    //   Weight == other.Weight;
    //}

    public bool Equals(Edge other)
    {
        return
        !(other is null) &&
        NodeFrom.Equals(other.NodeFrom) &&
        NodeTo.Equals(other.NodeTo) &&
        Weight == other.Weight;
    }

    public static bool operator ==(Edge first, Edge second) => (first is null && second is null) || first.Equals(second);
    public static bool operator !=(Edge first, Edge second) => !(first == second); //TODO: Fix that!

    public override string ToString()
    {
        return NodeFrom.label + " " + NodeTo.label;
    }

}
