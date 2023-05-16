using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCreator : MonoBehaviour
{
    public GameObject[] arrows;
    private string lastCommand = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Controller.activeCommand != "")
        {
            if(lastCommand != Controller.activeCommand)
            {
                if(lastCommand != "")
                {
                    foreach (GameObject arrow in arrows)
                    {
                        if (arrow.activeSelf)
                        {
                            arrow.SetActive(false);
                            
                        }
                    }
                }
                foreach(GameObject arrow in arrows)
                {
                    if(arrow.name == Controller.activeCommand)
                    {
                        arrow.SetActive(true);
                        lastCommand = arrow.name;
                        break;
                    }
                }
            }
        }
    }
}
