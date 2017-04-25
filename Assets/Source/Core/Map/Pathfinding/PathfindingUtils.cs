using System;
using Core.Map;
using UnityEngine;

namespace Core.Map.Pathfinding
{
    internal interface IPathFinder
    {
        EPathfindingAlgorithm Algorithm
        {
            get;
        }

        Path FindPathToDestination(Vector3 currentNodeIndex, Vector3 targetNodeIndex, MapController map);
    }

    public enum EPathfindingAlgorithm
    {
        Deikstra,
        AStar,
        DepthFirstSearch,
        BreadthFirst
    }
}

