using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TDHeartBeat.Assets.Scripts.AStar
{
    public class AStarAgent
    {
        //* Private variables
        private Node root;
        private Node endNode;
        private State goalState;
        private List<Vector2Int> availableCells;

        private Stack<Vector2> move;

        /// <summary>
        ///  Creates an A Star agent
        /// </summary>
        /// <param name="goalLocation">
        ///  Contains the position of the goal
        /// </param>
        public AStarAgent(Vector2Int goalLocation)
        {
            goalState = new State(goalLocation.x, goalLocation.y);
        }

        /// <summary>
        ///  Uses the A Star algorithm to find the moves needed to reach the goal.
        /// </summary>
        /// <param name="availableCells">
        ///  Must contain all cells available at this moment
        /// </param>
        /// <param name="agentPosition">
        ///  The position of the agent right now
        /// </param>
        /// <param name="map">
        ///  Tilemap on the map the algorithm is working on
        /// </param>
        /// <returns>
        ///  Returns a Stack of vector2 with the moves needed to reach the goal.
        /// 
        ///  Returns null if no path is found. 
        /// </returns>
        //* Traverse the tree via the parent nodes and add the actions to the stack,
        //* Available cells should be stored once in some controller class and updated there
        public Stack<Vector2> getMoves(List<Vector2Int> availableCells, Vector2Int agentPosition, Tilemap map)
        {
            endNode = null;
            move = new Stack<Vector2>();
            root = new Node(new State(agentPosition.x, agentPosition.y));
            this.availableCells = availableCells;

            endNode = Search(map); // perform the search

            if(endNode == null) { return null; }    // no goal found, return null

            // Get all the node action be traversing the tree backwards from the end node
            Node currNode = endNode;
            while(currNode.parent != null)
            {
                move.Push(currNode.action);
                currNode = currNode.parent;
            }

            return move;
        }

        private Node Search(Tilemap map)
        {
            // The Dictionary data structure is equivalent to a hashtable
            Dictionary<int, bool> exploredSet = new Dictionary<int, bool>();    // Used to quickly check if a node has been explored
            Dictionary<int, int> extraFrontier = new Dictionary<int, int>();    // Lookup table for the frontier to very quickly find if a node is within the frontier, as well as seeing the node cost
            
            // Frontier and comparer
            List<Node> frontier = new List<Node>(); // The frontier holds all the unexpanded leafs
            FrontierComparer comparer = new FrontierComparer(); // Used to compare nodes for the sorting algorithm

            // Checks if the root is the goal state
            if(root.state.isGoal(goalState)) { return root; }
            else { frontier.Add(root); }    // add the root to the frontier

            while(frontier.Count > 0)
            {
                Node node = frontier[0];    // get the first node
                frontier.RemoveAt(0);       // removes that same node
                extraFrontier.Remove(node.GetHashCode());   // removes the same node from extra frontier, doesn't throw exception, returns false if not found

                // Adds cost to the new node
                if(node.parent != null) { node.cost = node.parent.cost + 1; }

                // Return this node if it is the goal state
                if(node.state.isGoal(goalState)) { return node; }

                // Add node to explored set
                exploredSet.Add(node.GetHashCode(), true);

                // Get all available actions from this node and the transition (as tuples)
                IEnumerable<Tuple<string, Vector2>> actions = node.state.availableMoves(availableCells, map);

                // Loop through all the actions and create leaf nodes with heuristic cost, only if not expanded before
                foreach (var action in actions)
                {
                    // Create child note, where Node.action is the position moved to in the action while execute move is the string representation of the movement in the action
                    Node child = new Node(node, action.Item2, node.state.executeMove(action.Item1));
                    child.cost += heuristicSearch(child);   // Check Heuristic cost
                    bool inFrontier = extraFrontier.ContainsKey(child.GetHashCode());

                    // Only add the child to the frontier if not there already and has not been explored
                    // if it is in the frontier update cost if it's less expencive
                    if(!inFrontier && !exploredSet.ContainsKey(child.GetHashCode()))
                    {
                        frontier.Add(child);                                // Add new child to frontier
                        frontier.Sort(comparer);                            // Sort the fronter after adding
                        extraFrontier.Add(child.GetHashCode(), child.cost); // Add the node and it's cost to extra frontier
                    }
                    else if(inFrontier)
                    {
                        if(extraFrontier[child.GetHashCode()] > child.cost)
                        {
                            frontier[0].Copy(child);                            // copy child node info
                            frontier.Sort(comparer);                            // Sort the frontier by node cost
                            extraFrontier[child.GetHashCode()] = child.cost;    // update the cost of the node in the extra frontier
                        }
                    }
                }
            }

            // Return null if no goal is found, enemies are then probably blocked off and should wait
            return null;
        }

        private int heuristicSearch(Node node)
        {
            //TODO: maybe check performance with euclidian distance
            // Returns the manhattan distance to the goal (delta x + delta y)
            return Mathf.Abs(node.state.GetPosX() - goalState.GetPosX()) + Mathf.Abs(node.state.GetPosY() - goalState.GetPosY());
        }
    }
}