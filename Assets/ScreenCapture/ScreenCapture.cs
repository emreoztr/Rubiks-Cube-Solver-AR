using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using RubikSolver;

public class ScreenCapture : MonoBehaviour
{
    private GameObject cubeSideIndicator = null;
    public int imgPixel = 350;
    public TMP_Text cube00;
    public TMP_Text cube01;
    public TMP_Text cube02;
    public TMP_Text cube10;
    public TMP_Text cube11;
    public TMP_Text cube12;
    public TMP_Text cube20;
    public TMP_Text cube21;
    public TMP_Text cube22; 

    public RubikCubeState[] states;
    public string[] solutionSteps;
    private GameObject next;
    private bool solved = false;

    public RenderTexture nonVrTexture;
    public Camera uiCamera;

    public string enabledArrow = "";

    int stateIndex = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(captureScreenshot());
    }

    // Update is called once per frame
    void Update()
    {
        //enableArrow("R");
        if (solved && states.Length > stateIndex)
        {
            next = GameObject.Find("next");
            TMP_Text nextText = next.GetComponent<TextMeshPro>();
            nextText.text = solutionSteps[stateIndex];
            Controller.activeCommand = solutionSteps[stateIndex];
            //enableArrow("R'");
        }
        if(Controller.status == 1 && !solved)
        {
            Debug.Log("SOLVING");
            solved = true;
            FridrichSolver Solver = new FridrichSolver(Controller.startCube); //The Superflip!

            Solver.Solve();

            if (Solver.IsSolved)
            {
                Debug.Log(Solver.Solution);
            }
            solutionSteps = Solver.Solution.Split(' ');
            InterpretSolution inter = new InterpretSolution(Solver.Solution, Controller.startCube);
            states = inter.Interpret();
            Debug.Log(states.Length);
            for (int i = 0; i < states.Length; ++i)
            {
                Debug.Log("test");
                Debug.Log(states[i].lastMove);
                Debug.Log(string.Join(" ", states[i].currentCube));
            }
        }
    }

    IEnumerator captureScreenshot()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject cubeSide = GameObject.FindGameObjectWithTag("Indicator");
            Debug.Log(System.DateTime.Now.ToLongTimeString());

            Vector3 cubePos;
            if (cubeSide != null && (Controller.sideScan || Controller.status == 1))
            {
                Debug.Log(cubeSide.ToString());
                Vector3 screenPos = Camera.main.WorldToScreenPoint(cubeSide.transform.position);
                if (screenPos.x >= imgPixel && screenPos.y >= imgPixel && screenPos.x <= Screen.width - imgPixel && screenPos.y <= Screen.height - imgPixel && screenPos.z >= 0)
                {

                    /*Texture2D screenImage = new Texture2D(2*imgPixel, 2 * imgPixel);
                    //Get Image from screen
                    RenderTexture.active = nonVrTexture;
                    Camera.main.Render();
                    screenImage.ReadPixels(new Rect(screenPos.x - imgPixel, screenPos.y - imgPixel, screenPos.x + imgPixel, screenPos.y + imgPixel), 0, 0);
                    screenImage.Apply();

                    RenderTexture.active = null;
                    //Color[] pix = nonVrTexture.((int)screenPos.x - imgPixel, (int)screenPos.y - imgPixel, 2*imgPixel, 2 * imgPixel);
                    
                    //screenImage.SetPixels(pix);
                    //screenImage.ReadPixels(new Rect(screenPos.x - imgPixel, screenPos.y - imgPixel, screenPos.x + imgPixel, screenPos.y + imgPixel), 0, 0);
                    //screenImage.Apply();
                    //Convert to png
                    */
                    Texture2D miniTexture = extractMiniImage(screenPos);
                    byte[] imageBytes = miniTexture.EncodeToPNG();
                    string stringData = Convert.ToBase64String(imageBytes);

                    WWWForm form = new WWWForm();
                    form.AddField("b64", stringData);
                    Debug.Log(stringData);

                    using (UnityWebRequest www = UnityWebRequest.Post("http://3.70.47.41:8080/side_mini", form))
                    {
                        yield return www.SendWebRequest();

                        if (www.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log(www.error);
                           
                        }
                        else
                        {
                            Debug.Log("Form upload complete!");
                            var data = www.downloadHandler.text;
                            Debug.Log(data);
                            GameObject cube00_go = GameObject.Find("cube0-0");
                            GameObject cube01_go = GameObject.Find("cube0-1");
                            GameObject cube02_go = GameObject.Find("cube0-2");
                            GameObject cube10_go = GameObject.Find("cube1-0");
                            GameObject cube11_go = GameObject.Find("cube1-1");
                            GameObject cube12_go = GameObject.Find("cube1-2");
                            GameObject cube20_go = GameObject.Find("cube2-0");
                            GameObject cube21_go = GameObject.Find("cube2-1");
                            GameObject cube22_go = GameObject.Find("cube2-2");

                            cube00 = cube00_go.GetComponent<TextMeshPro>();
                            cube01 = cube01_go.GetComponent<TextMeshPro>();
                            cube02 = cube02_go.GetComponent<TextMeshPro>();
                            cube10 = cube10_go.GetComponent<TextMeshPro>();
                            cube11 = cube11_go.GetComponent<TextMeshPro>();
                            cube12 = cube12_go.GetComponent<TextMeshPro>();
                            cube20 = cube20_go.GetComponent<TextMeshPro>();
                            cube21 = cube21_go.GetComponent<TextMeshPro>();
                            cube22 = cube22_go.GetComponent<TextMeshPro>();

                            CubeSide.cube = JsonUtility.FromJson<CubeSide.Cube>(data);
                            cube00.text = CubeSide.cube.colors[0];
                            
                            cube01.text = CubeSide.cube.colors[1];
                            cube02.text = CubeSide.cube.colors[2];
                            cube10.text = CubeSide.cube.colors[3];
                            cube11.text = CubeSide.cube.colors[4];
                            cube12.text = CubeSide.cube.colors[5];
                            cube20.text = CubeSide.cube.colors[6];
                            cube21.text = CubeSide.cube.colors[7];
                            cube22.text = CubeSide.cube.colors[8];

                            if (solved)
                            {
                                char sideWanted = 'g';
                                CubeSide.cube.colors[4] = "green";
                                if (solutionSteps[stateIndex] == "B" || solutionSteps[stateIndex] == "B2" || solutionSteps[stateIndex] == "B'" || solutionSteps[stateIndex] == "B'2")
                                {
                                    sideWanted = 'b';
                                    CubeSide.cube.colors[4] = "blue";
                                }
                                

                                if (stateIndex < states.Length && sideEqual(CubeSide.cube.colors, states[stateIndex].getSide(sideWanted)))
                                {
                                    switch (solutionSteps[stateIndex])
                                    {
                                        case "F":
                                            Controller.frontRotationCount -= 1;
                                            break;
                                        case "F'":
                                            Controller.frontRotationCount += 1;
                                            break;
                                        case "F2":
                                            Controller.frontRotationCount -= 2;
                                            break;
                                        case "F'2":
                                            Controller.frontRotationCount += 2;
                                            break;


                                        case "B":
                                            Controller.rearRotationCount -= 1;
                                            break;
                                        case "B'":
                                            Controller.rearRotationCount += 1;
                                            break;
                                        case "B2":
                                            Controller.rearRotationCount -= 2;
                                            break;
                                        case "B'2":
                                            Controller.rearRotationCount += 2;
                                            break;

                                    }
                                    stateIndex++;
                                    
                                }
                            }
                            Controller.sideScan = false;
                        }
                    }

                }
                else
                {
                    Debug.Log("no object seen");
                }
            }
            else
            {
                Debug.Log("null");
            }
        }
    }

    bool sideEqual(string[] detectedSides, string actualSide)
    {
        string[] actualSideArray = actualSide.Split(' ');
        for(int i = 0; i < 9; ++i)
        {
            Debug.Log(detectedSides[i] + actualSideArray[i]);
            if(actualSideArray[i][0] != detectedSides[i][0])
            {

                return false;
            }
        }
        return true;
    }

    bool isObjectSeen(Vector3 pos)
    {
        if (pos == null)
            return false;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        Debug.Log("screenpos" + screenPos.ToString());
        return screenPos.x >= 0 && screenPos.y >= 0 && screenPos.x <= 1 && screenPos.y <= 1 && screenPos.z >= 0;
    }

    void enableArrow(string command)
    {
        if(command != null && enabledArrow != command && command != "")
        {
            if(enabledArrow != "")
            {
                GameObject oldArrow = GameObject.Find(enabledArrow);
                oldArrow.SetActive(false);
            }
            GameObject newArrow = GameObject.Find(command);
            if(newArrow == null)
            {
                Debug.Log("NULLLL");
                Debug.Log(command);
            }
            newArrow.SetActive(true);
            enabledArrow = command;
        }
    }

    Texture2D extractMiniImage(Vector3 screenPos)
    {
        Texture2D cubeImage = new Texture2D(2 * imgPixel, 2 * imgPixel);
        Texture2D miniImage = new Texture2D(3,3);
        //Get Image from screen
        RenderTexture.active = nonVrTexture;
        Camera.main.Render();
        cubeImage.ReadPixels(new Rect(screenPos.x - imgPixel, screenPos.y - imgPixel, screenPos.x + imgPixel, screenPos.y + imgPixel), 0, 0);
        RenderTexture.active = null;

        byte[] imageBytes = cubeImage.EncodeToPNG();
        string stringData = Convert.ToBase64String(imageBytes);
        //Debug.Log(stringData);

        int imgPixelHalf = imgPixel / 2;

        for(int i = 0; i < 3; ++i)
        {
            for(int j = 0; j < 3; ++j)
            {
                miniImage.SetPixel(j, i, cubeImage.GetPixel((j+1)*imgPixelHalf, (i+1)*imgPixelHalf));
            }
        }


        return miniImage;

    }

}
