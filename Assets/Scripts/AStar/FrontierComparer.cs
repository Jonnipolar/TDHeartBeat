using System.Collections.Generic;

namespace TDHeartBeat.Assets.Scripts.AStar
{
    public class FrontierComparer : IComparer<Node> 
    { 
        public int Compare(Node x, Node y) 
        {
            return x.cost.CompareTo(y.cost); 
        } 
    } 
}