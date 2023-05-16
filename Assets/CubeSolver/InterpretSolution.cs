using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubikSolver
{
    public class InterpretSolution
    {
        private string[] solution;
        private FridrichSolver cube;
        public RubikCubeState[] states;



        public InterpretSolution(string solution, string cube)
        {
            this.solution = solution.Split(' ');

            this.cube = new FridrichSolver(cube);
        }

        public RubikCubeState[] Interpret(){
            if(this.solution.Length == 0)
                return null;
            
            this.states = new RubikCubeState[this.solution.Length];
            Debug.Log(this.solution);
            for (int i = 0; i < this.solution.Length; ++i)
            {
                Debug.Log("tesst");
                this.states[i] = new RubikCubeState(this.solution[i], this.cube);
            }

            return this.states;
        }


    }
}