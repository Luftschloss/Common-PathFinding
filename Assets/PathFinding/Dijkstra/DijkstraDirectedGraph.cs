using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Dijkstra算法有向图应用场景
/// </summary>
public class DijkstraDirectedGraph : MonoBehaviour
{
    class NodeGameObject : IEquatable<NodeGameObject>
    {
        public GameObject root;

        public int side1;
        public int side2;

        public string text 
        {
            set { root.GetComponentInChildren<TextMesh>().text = value; }
        }

        public void Show(Vector3 start, Vector3 end)
        {
            var lr = root.GetComponentInChildren<LineRenderer>();
            if (lr == null)
                lr = root.AddComponent<LineRenderer>();
            lr.startColor = Color.blue;
            lr.endColor = Color.blue;
            lr.widthMultiplier = 0.3f;
            lr.positionCount = 2;
            lr.SetPositions(new Vector3[2] { start, end });
            root.GetComponentInChildren<TextMesh>().transform.position = (start + end) / 2 + new Vector3(0f, 0f, -0.1f);
        }

        public bool Equals(NodeGameObject obj)
        {
            return (this.side1 == obj.side1 && this.side2 == obj.side2) || (this.side2 == obj.side1 && this.side1 == obj.side2);
        }
    }
    public GameObject PathNodeGo;
    public GameObject DistanceGo;

    public int PathNodeCount = 5;
    float[,] distanceMap;
    Vector3[] pathNode;
    NodeGameObject[] PathNodeGoes;
    List<NodeGameObject> DistanceGoes;

    private void Start()
    {
        AutoGeneratePathNode();
    }

    public void AutoGeneratePathNode()
    {
        ClearPathNode();
        pathNode = new Vector3[PathNodeCount];
        PathNodeGoes = new NodeGameObject[PathNodeCount];
        DistanceGoes = new List<NodeGameObject>();
        for (int i = 0; i < PathNodeCount; i++)
        {
            pathNode[i] = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
            PathNodeGoes[i] = new NodeGameObject() { root = Instantiate<GameObject>(PathNodeGo), text = i.ToString() };
            PathNodeGoes[i].root.transform.position = pathNode[i];
        }
        distanceMap = new float[PathNodeCount, PathNodeCount];
        for (int x = 0; x < PathNodeCount; x++)
        {
            for (int y = 0; y < PathNodeCount; y++)
            {
                if (x == y)
                    continue;
                bool connect = Random.Range(0, 2) == 0;
                if(connect)
                {
                    float distance = Vector3.Distance(pathNode[x], pathNode[y]);
                    NodeGameObject nodeGo = new NodeGameObject() { root = Instantiate<GameObject>(DistanceGo), 
                        text = distance.ToString("f1"), side1 = x, side2 = y };
                    if (!DistanceGoes.Contains(nodeGo))
                    {
                        DistanceGoes.Add(nodeGo);
                        nodeGo.Show(pathNode[x], pathNode[y]);
                    }
                    else
                        Destroy(nodeGo.root);
                    distanceMap[x, y] = distance;
                }
                else
                {
                    distanceMap[x, y] = float.MaxValue;
                }
            }
        }
    }

    void ClearPathNode()
    {
        distanceMap = null;
        pathNode = null;
        if(PathNodeGoes != null)
        {
            for (int i = 0; i < PathNodeGoes.Length; i++)
            {
                Destroy(PathNodeGoes[i].root);
            }
            PathNodeGoes = null;
        }
        if(DistanceGoes != null)
        {
            for (int i = 0; i < DistanceGoes.Count; i++)
            {
                Destroy(DistanceGoes[i].root);
            }
            DistanceGoes.Clear();
        }
    }
}
