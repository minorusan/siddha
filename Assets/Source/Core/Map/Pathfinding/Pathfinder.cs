using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Map;
using Core.Pathfinding.Algorithms;


namespace Core.Map.Pathfinding
{
    public class Pathfinder
    {
        private static Dictionary<EPathfindingAlgorithm, IPathFinder> _currentAlgorithms;

        static Pathfinder()
        {
            _currentAlgorithms = new Dictionary<EPathfindingAlgorithm, IPathFinder>();
            _currentAlgorithms.Add(EPathfindingAlgorithm.AStar, new AStar());
        }


        public static Path FindPathToDestination(MapController map, Vector3 currentNodeIndex, Vector3 targetNodeIndex,
                                                 EPathfindingAlgorithm algorithm = EPathfindingAlgorithm.AStar)
        {
            var pathfinderToUse = _currentAlgorithms[algorithm];
            return pathfinderToUse.FindPathToDestination(currentNodeIndex, targetNodeIndex, map);
        }
    }
}

