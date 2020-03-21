using UnityEngine;

namespace TDHeartBeat.Assets.Scripts.AStar
{
    public class Node
    {
        public Node parent;     // Parent node
        public Vector2 action;   // Action made to get here
        public State state; // State at this location
        public int cost;    // Cost of moving to this cell

        public Node(Node parent, Vector2 action, State state)
        {
            this.parent = parent;
            this.action = action;
            this.state = state;
        }

        public Node(State state)
        {
            this.parent = null;
            this.action = new Vector2(int.MinValue, int.MinValue);
            this.state = state;
            this.cost = 0;
        }

        ///<summary>
        /// Copies other node to equal this node.
        ///</summary>
        /// <param name="other">
        ///  The node that should be copied to this node
        /// </param>
        public void Copy(Node other)
        {
            this.parent = other.parent;
            this.action.x = other.action.x;
            this.action.y = other.action.y;
            this.cost = other.cost;
        }

        //*         Overrides           *//

        public static bool operator ==(Node n1, Node n2)
        {
            if(object.ReferenceEquals(n1, n2)) { return true; }
            if(object.ReferenceEquals(n1, null) || object.ReferenceEquals(n2, null)) { return false; }

            return n1.state == n2.state;
        }

        public static bool operator !=(Node n1, Node n2)
        {
            return !(n1 == n2);
        }

        // hash code is the same as the state since that is already unique for this project
        public override int GetHashCode()
        {
            return this.state.GetHashCode();
        }

        public bool Equals(Node other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Node);
        }
    }
}