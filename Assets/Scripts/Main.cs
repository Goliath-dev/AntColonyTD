using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VR;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //var nodes = (from i in Enumerable.Range(0, 4) select new Node(i.ToString())).ToList();

        //foreach(Node node in nodes)
        //{
        //    foreach(var neighbour in nodes.Except(new List<Node>() { node }))
        //    {
        //        node.AddNeighbour(neighbour, 1, 1);
        //    }
        //}
        //var finalNode = new Node("4");
        //finalNode.AddNeighbour(nodes.Last(), 1, 1);
        //nodes.Last().AddNeighbour(finalNode, 1, 1);
        //nodes.Add(finalNode);
        //nodes[0].neighbouringEdges.First().Weight = 10;

        var node0 = new Node("0");
        var node1 = new Node("1");
        var node2 = new Node("2");
        var node3 = new Node("3");
        var node4 = new Node("4");
        var node5 = new Node("5");
        var node6 = new Node("6");
        var node7 = new Node("7");
        var node8 = new Node("8");

        node0.AddNeighbour(node1, 10, 1);
        node0.AddNeighbour(node2, 10, 1);
        node1.AddNeighbour(node3, 10, 1);
        node2.AddNeighbour(node3, 16, 1);

        node0.AddNeighbour(node4, 10, 1);
        node2.AddNeighbour(node5, 10, 1);
        node4.AddNeighbour(node5, 16, 1);

        node0.AddNeighbour(node6, 10, 1);
        node4.AddNeighbour(node7, 10, 1);
        node6.AddNeighbour(node7, 16, 1);

        node6.AddNeighbour(node8, 10, 1);
        node1.AddNeighbour(node8, 16, 1);

        var nodes = new List<Node>(8) { node0, node1, node2, node3, node4, node5, node6, node7 };
        var finalNode = node3;

        var graph = new Graph(nodes);

        GameMap map = new GameMap(graph, nodes[0], finalNode);

        var fncs = new GraphToSpaceFunctions();
        TwoDimGameMap map2 = fncs.GameMapToSpace(map);
        //foreach (var edge in map2.graph.Edges) UnityEngine.Debug.Log(edge);
        foreach (var edge in graph.Edges) UnityEngine.Debug.Log(fncs.EdgeToSpace(edge));
        //var path = AntColonyAlgorithm.Algorithm(map);
        ////map.UnderlyingGraph.Edges.ToList().ForEach(ed => print(ed + " " + ed.Pheromone));
        //for (int i = 0; i < path.Count; i++)
        //{
        //    print("Edge is " + path[i] + ". Pheromone is " + path[i].Pheromone + ".");
        //}
        //path.ForEach(edge => print(edge.NodeFrom.label + ", " + edge.NodeTo.label));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
