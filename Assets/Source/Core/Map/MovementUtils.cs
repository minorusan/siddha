using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;


namespace Core.Map
{
    public enum EMovableObjectState
    {
        Walking,
        Standing
    }

    public class Path
    {
        public bool Empty
        {
            get
            {
                return _nodes.Count <= 0;
            }

        }

        public Path()
        {
            _nodes = new List<Node>();
        }

        private readonly List<Node> _nodes;

        public List<Node> Nodes
        {
            get
            {
                return _nodes;
            }
        }

        public Path(List<Node> nodes)
        {
            _nodes = nodes;
        }
    }

    public enum ECellType
    {
        Walkable,
        Blocked,
        Busy
    }

    public class Node
    {
        public int GCost;
        public int HCost;

        public int FCost
        {
            get
            {
                return GCost + HCost;
            }
        }

        public MapController Map { get; set; }

        public Node Parent;
        public ECellType CurrentCellType;
        public Vector3 Position;
        public IJ GridPosition;
        public bool Target;
    }

    [Serializable]
    public class IJ
    {
        public int I;
        public int J;

        public IJ(int i, int j)
        {
            I = i;
            J = j;
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return I * 1000 + J;
        }
    }
}

