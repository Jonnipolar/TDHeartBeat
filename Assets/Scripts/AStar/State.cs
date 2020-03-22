using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TDHeartBeat.Assets.Scripts.AStar
{
    public class State
    {
        private int _cellPosX;
        private int _cellPosY;
        private bool _evenState;

        public State(int posX, int posY)
        {
            _cellPosX = posX;
            _cellPosY = posY;

            // Modulo is an expensive operation, only do it once
            if(_cellPosY % 2 == 0) {_evenState = true; }
            else { _evenState = false; }
        }

        ///<summary>
        /// Gets the state X position
        ///</summary>
        public int GetPosX() => _cellPosX;
        ///<summary>
        /// Gets the state Y position
        ///</summary>
        public int GetPosY() => _cellPosY;

        public List<Tuple<string, Vector2>> availableMoves(List<Vector2Int> validCells, Tilemap map)
        {
            // Creating tuple one used for execute move, other for the path returned after AStar is finished
            List<Tuple<string, Vector2>> moves = new List<Tuple<string, Vector2>>();

            // Every other cell has their diagonal increment in an another direction, hence we need to check all even and odd X's and Y's
            if (_evenState)
            {
                if(validCells.Contains(new Vector2Int(_cellPosX, _cellPosY + 1)))       { moves.Add(new Tuple<string, Vector2>(Actions.RightUp, map.GetCellCenterWorld(new Vector3Int(_cellPosX, _cellPosY + 1, 0)))); }            // Checks upper right
                if(validCells.Contains(new Vector2Int(_cellPosX - 1, _cellPosY + 1)))   { moves.Add(new Tuple<string, Vector2>(Actions.LeftUp, map.GetCellCenterWorld(new Vector3Int(_cellPosX - 1, _cellPosY + 1, 0)))); }         // Checks upper left
                if(validCells.Contains(new Vector2Int(_cellPosX - 1, _cellPosY - 1)))   { moves.Add(new Tuple<string, Vector2>(Actions.LeftDown, map.GetCellCenterWorld(new Vector3Int(_cellPosX - 1, _cellPosY - 1, 0)))); }       // Checks lower left
                if(validCells.Contains(new Vector2Int(_cellPosX, _cellPosY - 1)))       { moves.Add(new Tuple<string, Vector2>(Actions.RightDown, map.GetCellCenterWorld(new Vector3Int(_cellPosX, _cellPosY - 1, 0)))); }          // Checks lower right
            }
            else
            {
                if(validCells.Contains(new Vector2Int(_cellPosX + 1, _cellPosY + 1)))   { moves.Add(new Tuple<string, Vector2>(Actions.RightUp, map.GetCellCenterWorld(new Vector3Int(_cellPosX + 1, _cellPosY + 1, 0)))); }        // Checks upper right
                if(validCells.Contains(new Vector2Int(_cellPosX, _cellPosY + 1)))       { moves.Add(new Tuple<string, Vector2>(Actions.LeftUp, map.GetCellCenterWorld(new Vector3Int(_cellPosX, _cellPosY + 1, 0)))); }             // Checks upper left
                if(validCells.Contains(new Vector2Int(_cellPosX, _cellPosY - 1)))       { moves.Add(new Tuple<string, Vector2>(Actions.LeftDown, map.GetCellCenterWorld(new Vector3Int(_cellPosX, _cellPosY - 1, 0)))); }           // Checks lower left
                if(validCells.Contains(new Vector2Int(_cellPosX + 1, _cellPosY - 1)))   { moves.Add(new Tuple<string, Vector2>(Actions.RightDown, map.GetCellCenterWorld(new Vector3Int(_cellPosX + 1, _cellPosY - 1, 0)))); }      // Checks lower right
            }

            // left and right are always the same
            if(validCells.Contains(new Vector2Int(_cellPosX + 1, _cellPosY))) { moves.Add(new Tuple<string, Vector2>(Actions.Right, map.GetCellCenterWorld(new Vector3Int(_cellPosX + 1, _cellPosY, 0)))); }  // Checks to the right
            if(validCells.Contains(new Vector2Int(_cellPosX - 1, _cellPosY))) { moves.Add(new Tuple<string, Vector2>(Actions.Left, map.GetCellCenterWorld(new Vector3Int(_cellPosX - 1, _cellPosY, 0)))); }   // Checks to the left

            return moves;
        }

        // Excecutes the move sent in, returns the new state, if the inserted move is not a valid move it will return null
        public State executeMove(string action)
        {
            // Least expensive operations first
            if(action == Actions.Right) { return new State(_cellPosX + 1, _cellPosY); } // Moves right
            if(action == Actions.Left)  { return new State(_cellPosX - 1, _cellPosY); } // Moves left

            // Is in an even cell position y?
            if(_evenState)
            {
                if(action == Actions.RightUp)   { return new State(_cellPosX, _cellPosY + 1); }     // Moves right and up
                if(action == Actions.LeftUp)    { return new State(_cellPosX - 1, _cellPosY + 1); } // Moves up and left
                if(action == Actions.LeftDown)  { return new State(_cellPosX - 1, _cellPosY - 1); } // Moves left and down
                if(action == Actions.RightDown) { return new State(_cellPosX, _cellPosY - 1); }     // Moves right and down
            }
            else
            {
                if(action == Actions.RightUp)   { return new State(_cellPosX + 1, _cellPosY + 1); } // Moves right and up
                if(action == Actions.LeftUp)    { return new State(_cellPosX, _cellPosY + 1); }     // Moves left and up
                if(action == Actions.LeftDown)  { return new State(_cellPosX, _cellPosY - 1); }     // Moves left and down
                if(action == Actions.RightDown) { return new State(_cellPosX + 1, _cellPosY - 1); } // Moves right and down
            }

            // Return null if execute move cannot be done, something really is then wrong
            return null;
        }

        public bool isGoal(State goal)
        {
            return this == goal;
        }

        public Vector2Int GetPosition()
        {
            return new Vector2Int(_cellPosX, _cellPosY);
        }

        //*         Overrides           *//

        public static bool operator ==(State s1, State s2)
        {
            if(object.ReferenceEquals(s1, s2)) { return true; }
            if(object.ReferenceEquals(s1, null) || object.ReferenceEquals(s2, null)) { return false; }

            return s1.GetPosX() == s2.GetPosX() && s1.GetPosY() == s2.GetPosY();
        }

        public static bool operator !=(State s1, State s2)
        {
            return !(s1 == s2);
        }

        public override int GetHashCode()
        {
            var hashCode = 352033288;
            hashCode = hashCode * -1521134295 + _cellPosX.GetHashCode();
            hashCode = hashCode * -1521134295 + _cellPosY.GetHashCode();

            return hashCode;
        }

        public bool Equals(State other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as State);
        }
    }
}