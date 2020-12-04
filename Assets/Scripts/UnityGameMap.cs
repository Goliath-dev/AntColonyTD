using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnityGameMap : MonoBehaviour
{
    private GameMap map;
    private TwoDimGameMap TDMap;
    public GameObject PathEl;
    public Transform GameMapRoot;

    void Start()
    {
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

        map = new GameMap(graph, nodes[0], finalNode);
        var fncs = new GraphToSpaceFunctions();
        TDMap = fncs.GameMapToSpace(map);
        CreateGameMap(TDMap, PathEl, GameMapRoot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateGameMap(TwoDimGameMap tdMap, GameObject pathElPrefab, Transform gameMapRoot)
    {
        foreach (var edge in tdMap.graph.Edges)
        {
            for (int i = 0; i < edge.Path.Count() - 1; i++) //Not a typo, we don't need the last point.
            {
                var currPoint = new Vector2(edge.Path.ElementAt(i).X, edge.Path.ElementAt(i).Y);
                var nextPoint = new Vector2(edge.Path.ElementAt(i + 1).X, edge.Path.ElementAt(i + 1).Y);
                var diff = nextPoint - currPoint;
                //TODO: Bugged a bit, gotta check the order of the first/last points appearance.
                for (int j = 0; j < diff.magnitude; j++)
                {
                    var pos = currPoint + j * diff.normalized;
                    var newEl = Instantiate(pathElPrefab, new Vector3(pos.x, pos.y), Quaternion.identity);
                    newEl.SetActive(true);
                    newEl.transform.SetParent(gameMapRoot);
                }
            }    
        }
    }
}
