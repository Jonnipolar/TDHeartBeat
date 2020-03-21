using System.Collections.Generic;

namespace TDHeartBeat.Assets.Scripts.AStar
{
    public class FrontierComparer : IComparer<Node> 
    { 
        public int Compare(Node x, Node y) 
        { 
            // if (x.cost == 0 || y.cost == 0) 
            // { 
            //     return 0; 
            // } 
            
            // CompareTo() method 
            return x.cost.CompareTo(y.cost); 
            
        } 
    } 
}