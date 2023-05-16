using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SideScanner : MonoBehaviour
{
    private int maxSide = 6;
    private int statusSide = 0;
    private string[] statusIndicator = { "green", "orange", "blue", "red", "yellow", "white" };
    private string[] statusIndicatorChar = { "g", "o", "b", "r", "y", "w" };
    public string cubeStr = "";
    public TMP_Text status;
    public TMP_Text statusScan;

    // Update is called once per frame
    void Update()
    {
        statusScan.text = "Scanned: " + cubeStr;
        if (statusSide < maxSide)
        {
            status.text = "Scan " + statusIndicator[statusSide] + " side";
        }
    }

    public void saveScan()
    {
        if(CubeSide.cube != null && CubeSide.cube.colors != null)
        {
            for(int i = 0; i < 9; ++i)
            {
                if (i == 4)
                {
                    cubeStr += statusIndicatorChar[statusSide];
                }
                else
                {
                    cubeStr += CubeSide.cube.colors[i];
                }
            }
            Debug.Log(cubeStr);
        }
        statusSide += 1;

    }

    public void deleteLastScan()
    {
        if (statusSide > 0)
        {
            statusSide -= 1;

            if (statusSide != 0)
            {
                cubeStr = cubeStr.Substring(0, statusSide * 9);
            }
            else
            {
                cubeStr = "";
            }
        }
    }

    public void saveAll()
    {
        Controller.sideScan = true;
        status.text = "Solving";
        Controller.status = 1;
        Controller.startCube = cubeStr;
    }

    public void scanSide()
    {
        Controller.sideScan = true;
    }
}
