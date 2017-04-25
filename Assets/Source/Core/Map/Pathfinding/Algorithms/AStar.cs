using System;
using System.Collections.Generic;
using Core.Map.Pathfinding;
using System.Linq;
using Core.Map;
using UnityEngine;
using System.Diagnostics;


namespace Core.Pathfinding.Algorithms
{
    public class AStar : IPathFinder
    {
        private List<ECellType> _ignoredNodeTypes;

        #region IPathFinder implementation


        public EPathfindingAlgorithm Algorithm
        {
            get
            {
                return EPathfindingAlgorithm.AStar;
            }
        }

        public AStar()
        {
            _ignoredNodeTypes = new List<ECellType> { ECellType.Blocked, ECellType.Busy };
        }

        public Path FindPathToDestination(Vector3 currentNodeIndex, Vector3 targetNodeIndex, MapController map)
        {
        
            Node startNode = map.GetNodeByPosition(currentNodeIndex);
            Node targetNode = map.GetNodeByPosition(targetNodeIndex);
           
            Heap<Node> openSet = new Heap<Node>(map.MapDimentions.I * map.MapDimentions.J);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);
            var iterator = 0;
            while (openSet.Count > 0)
            {
                Node node = openSet.RemoveFirst();
                closedSet.Add(node);

                if (node == targetNode)
                {
                     return RetracePath(startNode, targetNode);
                }
                var neighbours = map.GetNeighbours(node);

                for (int i = 0; i < neighbours.Length; i++)
                {
                    if (neighbours[i] == null)
                    {
                        continue;
                    }
                    var ignored = _ignoredNodeTypes.Any(p => p == neighbours[i].CurrentCellType);

                    if (ignored || closedSet.Contains(neighbours[i]))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.GCost + MapController.GetDistance(node, neighbours[i]);
                    if (newCostToNeighbour < neighbours[i].GCost || !openSet.Contains(neighbours[i]))
                    {
                        neighbours[i].GCost = newCostToNeighbour;
                        neighbours[i].HCost = MapController.GetDistance(neighbours[i], targetNode);
                        neighbours[i].Parent = node;
                        iterator++;
                        if (!openSet.Contains(neighbours[i]))
                        {
                            openSet.Add(neighbours[i]);
                        }else
                        {
                            openSet.Update(neighbours[i]);
                        }
                            
                    }
                }
            }

            return new Path();
        }

        #endregion

        private Path RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;

            }
            path.Reverse();

            return new Path(path);
        }
    }
}

