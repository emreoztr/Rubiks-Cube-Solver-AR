using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RubikSolver
{
    public class RubikCubeState
    {
        public string lastMove;
        public FridrichSolver cube;
        public char[] currentCube;
        public char faceSide;

        public RubikCubeState(string lastMove, FridrichSolver cube)
        {
            this.lastMove = lastMove;
            this.cube = cube;
            applyMove();
        }

        private void applyMove()
        {
            Tools.RotateCube(this.cube, 123, 123, this.lastMove);
            char[] newCube = new char[cube.Cube.Length];

            for (int i = 0; i < cube.Cube.Length; ++i)
            {
                
                newCube[i] = cube.Cube[i];
            }
            Debug.Log(string.Join(" ", cube.Cube));
            currentCube = newCube;
        }

        public string getSide(char side)
        {
            for(int i = 4; i < currentCube.Length; i += 9)
            {
                if(currentCube[i] == side)
                {
                    char[] sideChoice = new char[9];
                    for(int j = i - 4; j <= i + 4; ++j)
                    {
                        sideChoice[j - (i - 4)] = currentCube[j];
                    }
                    return string.Join(" ", sideChoice);
                }
            }
            return "";
        }
    }
}