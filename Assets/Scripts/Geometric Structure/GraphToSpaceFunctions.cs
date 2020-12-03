using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphToSpaceFunctions 
{
    private Dictionary<Edge, TwoDimEdge> edgeDict = new Dictionary<Edge, TwoDimEdge>();
    private TwoDimGraph graph = new TwoDimGraph(0, 0);
    private TwoDimGameMap twoDimGameMap;

    public TwoDimEdge EdgeToSpace(Edge edge)
    {
        return edgeDict[edge];
    }

    public TwoDimGameMap GameMapToSpace(GameMap gameMap)
    {
        if (twoDimGameMap == null)
        {
            TwoDimPoint finalNode = null;
            var startNode = new TwoDimPoint(0, 0); //Start node is always at (0, 0). 
            graph.AddTwoDimNode(startNode);
            var startNeighbourCount = gameMap.StartNode.neighbouringEdges.Count();
            var quadrants = Enum.GetValues(typeof(Quadrant)).Cast<Quadrant>().ToArray(); //Adviced in StackOverflow, but looks so weird.
            for (int i = 0; i < startNeighbourCount; i++)
            {
                var currEdge = gameMap.StartNode.neighbouringEdges.ElementAt(i);
                var nextEdge = i + 1 < startNeighbourCount ?
                    gameMap.StartNode.neighbouringEdges.ElementAt(i + 1) :
                    gameMap.StartNode.neighbouringEdges.ElementAt(0);
                var currTwoDimNode = TurnPointToQuadrant(new TwoDimPoint(currEdge.Weight, 0), quadrants[i]);
                var nextTwoDimNode = TurnPointToQuadrant(new TwoDimPoint(0, nextEdge.Weight), quadrants[i]);
                var currTwoDimEdge = new TwoDimEdge(startNode, currTwoDimNode);
                var nextTwoDimEdge = new TwoDimEdge(startNode, nextTwoDimNode);
                graph.AddTwoDimNode(currTwoDimNode); graph.AddTwoDimNode(nextTwoDimNode);
                AssociateAndAddToGraph(currEdge, currTwoDimEdge);
                var (firstMidEdge, secondMidEdge) = GetCommonEdges(currEdge, nextEdge);
                var firstMidTwoDimEdge = TurnEdgeToQuadrant(CreateMidEdges(currEdge, nextEdge), quadrants[i]);
                var secondTwoDimMidEdge = ReflectOverYAxis(TurnEdgeToQuadrant(CreateMidEdges(nextEdge, currEdge), Quadrant.Second));
                secondTwoDimMidEdge = TurnEdgeToQuadrant(secondTwoDimMidEdge, quadrants[i]);
                var midTwoDimNode = firstMidTwoDimEdge.Path.Last();
                graph.AddTwoDimNode(midTwoDimNode);
                var midNode = from first in currEdge.NodeTo.neighbouringEdges
                              from second in nextEdge.NodeTo.neighbouringEdges
                              where first.NodeTo == second.NodeTo
                              select first.NodeTo; //Looks weird, need to rewrite. 
                if (midNode.FirstOrDefault() == gameMap.FinalNode) finalNode = midTwoDimNode;
                //graph.AddTwoDimEdge(firstMidTwoDimEdge); graph.AddTwoDimEdge(secondTwoDimMidEdge);
                AssociateAndAddToGraph(firstMidEdge, firstMidTwoDimEdge); AssociateAndAddToGraph(secondMidEdge, secondTwoDimMidEdge);
            }
            twoDimGameMap = new TwoDimGameMap(startNode, finalNode, graph);
        }
        return twoDimGameMap;
    }

    private void AssociateAndAddToGraph(Edge edge, TwoDimEdge tdEdge)
    {
        graph.AddTwoDimEdge(tdEdge);
        edgeDict[edge] = tdEdge;
    }

    private (Edge first, Edge second) GetCommonEdges(Edge firstEdge, Edge secondEdge)
    {
        return (from first in firstEdge.NodeTo.neighbouringEdges
               from second in secondEdge.NodeTo.neighbouringEdges
               where first.NodeTo == second.NodeTo
               select (first, second)).FirstOrDefault();
    }
    private TwoDimEdge CreateMidEdges(Edge firstEdge, Edge secondEdge)
    {
        var firstTwoDimNode = new TwoDimPoint(firstEdge.Weight, 0);
        var secondTwoDimNode = new TwoDimPoint(0, secondEdge.Weight);
        //EdgesSharingCommonNode are the edges that connect the neighbours of the start node via one node in between. 
        //Something like this.
        //1
        //|\\
        //| \\
        //|  \\
        //|   3
        //|    \\
        //0-----2
        //Here zero is a start node, 1 and 2 are its neighbours, firstTwoDimNode and secondTwoDimNode, respectively, 
        //3 is a common node 'shared' by 1 and 2, lines are edges and double-lines are EdgesSharingCommonNode.
        var EdgesSharingCommonNode = from first in firstEdge.NodeTo.neighbouringEdges
                                     from second in secondEdge.NodeTo.neighbouringEdges
                                     where first.NodeTo == second.NodeTo
                                     select (first, second);
        var edges = EdgesSharingCommonNode.FirstOrDefault(); //Suppose there's just one such pair or not a single one at all. 
        if (edges != default(ValueTuple<Edge, Edge>)) //In case such a pair exists we'll do the magic.
        {
            if ((edges.first.Weight + edges.second.Weight > firstEdge.Weight * secondEdge.Weight) ||
                (edges.first.Weight + edges.second.Weight < firstEdge.Weight + secondEdge.Weight))
            {
                throw new ArgumentException($"Edges {edges.first} and {edges.second} are inconsistent.");
            }
            //Calculate the position of the node shared by EdgesSharingCommonNode. 
            var firstEdgeRatio = (float)(edges.first.Weight) / (edges.first.Weight + edges.second.Weight);
            var secondEdgeRatio = (float)(edges.second.Weight) / (edges.first.Weight + edges.second.Weight);
            var midPointCoord1 = Convert.ToInt32(firstEdgeRatio * firstEdge.Weight);
            var midPointCoord2 = Convert.ToInt32(secondEdgeRatio * secondEdge.Weight);
            var commonNode = new TwoDimPoint(firstEdge.Weight - midPointCoord1, secondEdge.Weight - midPointCoord2);
            var commonEdge = new TwoDimEdge(firstTwoDimNode, commonNode);
            //The edges is supposed to be 'folded' in the specific location, so calculate the number of creases. 
            var creaseCount = (edges.first.Weight - 2 * (secondEdge.Weight - midPointCoord2)) /
            (3 * (secondEdge.Weight - midPointCoord2 - 2) + 2); //Wrong formula! 
            //Geometrically, an edge is a polygonal chain consisted of three parts: Г-shape part at the beginning, 
            //several creases in the middle and ⌋-shape part at the end. 
            //The first part.
            var point1 = firstTwoDimNode.ShiftPointTo((commonNode.Y - 1) * TwoDimPoint.Up);
            var point2 = point1.ShiftPointTo(TwoDimPoint.Left);
            commonEdge.AddPointToPath(point1); commonEdge.AddPointToPath(point2);
            //The second part.
            var startPoint = point2;
            for (int i = 0; i < creaseCount; i++)
            {
                var point3 = startPoint.ShiftPointTo((commonNode.Y - 2) * TwoDimPoint.Down);
                var point4 = point3.ShiftPointTo(TwoDimPoint.Left);
                var point5 = point4.ShiftPointTo((commonNode.Y - 2) * TwoDimPoint.Up);
                var point6 = point5.ShiftPointTo(TwoDimPoint.Left);
                commonEdge.AddRangeToPath(new TwoDimPoint[4] { point3, point4, point5, point6 }); //Shorter to code; not necessary, though. 
                startPoint = point6;
            }
            //The third part.
            var point7 = startPoint.ShiftPointTo(Math.Abs(commonNode.X - startPoint.X) * TwoDimPoint.Left);
            commonEdge.AddPointToPath(point7);
            return commonEdge;
        }
        else
        {
            return null;
        }
    }

        private TwoDimPoint TurnPointBy(TwoDimPoint point, double phi)
        {
            return new TwoDimPoint(Convert.ToInt32(point.X * Math.Cos(phi) - point.Y * Math.Sin(phi)),
                Convert.ToInt32(point.X * Math.Sin(phi) + point.Y * Math.Cos(phi)));
        }
        private TwoDimEdge TurnEdgeBy(TwoDimEdge edge, double phi)
        {
            return new TwoDimEdge(from point in edge.Path select TurnPointBy(point, phi));
        }

        private TwoDimPoint TurnPointToQuadrant(TwoDimPoint point, Quadrant quadrant)
        {
            switch (quadrant)
            {
                case Quadrant.First:
                    return point; //Every edge is supposed to be placed in the first quadrant, so no change needed. 
                                  //NB: This might be a trouble, as behaviour is different for different cases (returning the point itself here 
                                  //and returning new points below, so the reference inequality violates).
                case Quadrant.Second:
                    return new TwoDimPoint(-point.Y, point.X);
                case Quadrant.Third:
                    return new TwoDimPoint(-point.X, -point.Y);
                case Quadrant.Fourth:
                    return new TwoDimPoint(point.Y, -point.X);
                default:
                    throw new ArgumentException("Four quadrants is not enough somehow.");
            }
        }

        //Mathematically equivalent to TurnEdgeBy(edge, PI/2) (or PI, or 3*PI/2), but cheaper and needs not rounding. 
        private TwoDimEdge TurnEdgeToQuadrant(TwoDimEdge edge, Quadrant quadrant)
        {
            return new TwoDimEdge(from point in edge.Path select TurnPointToQuadrant(point, quadrant));
        }

        //Previously named ChangeRightToLeft. Not sure about perfectly clear name, though. 
        //Mathematically, it changes the right-hand coordinate system to the left-hand one, but this naming seems more comprehensible.
        private TwoDimEdge ReflectOverYAxis(TwoDimEdge edge)
        {
            return new TwoDimEdge(from point in edge.Path select new TwoDimPoint(-point.X, point.Y));
        }

    private enum Quadrant
    {
        First = 0,
        Second = 1,
        Third = 2,
        Fourth = 3
    }
}
