using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RubikSolver;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FridrichSolver Solver = new FridrichSolver("oooggwggwybboooooorrrybbybbggwrrrrrryygyygyygwwwwwwbbb"); //The Superflip!

        Solver.Solve();

        if (Solver.IsSolved)
        {
            Debug.Log(Solver.Solution);
        }
        InterpretSolution inter = new InterpretSolution(Solver.Solution, "oooggwggwybboooooorrrybbybbggwrrrrrryygyygyygwwwwwwbbb");
        RubikCubeState[] states = inter.Interpret();
        Debug.Log(states.Length);
        for (int i = 0; i < states.Length; ++i)
        {
            Debug.Log("test");
            Debug.Log(states[i].lastMove);
            Debug.Log(string.Join(" ", states[i].currentCube));
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
