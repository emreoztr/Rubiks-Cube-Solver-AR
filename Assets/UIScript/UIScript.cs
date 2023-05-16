using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject curvedUICam;
    public GameObject noGlassesRenderer;
    public GameObject rightEye;
    public GameObject leftEye;
    public GameObject curvedScreen;
    public GameObject normalScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleARGlasses()
    {
        rightEye.SetActive(!rightEye.activeSelf);
        leftEye.SetActive(!leftEye.activeSelf);
        noGlassesRenderer.SetActive(!noGlassesRenderer.activeSelf);
        curvedScreen.SetActive(!curvedScreen.activeSelf);
        normalScreen.SetActive(!normalScreen.activeSelf);
    }

    public void toggleJapaneseSet()
    {
        Controller.japaneseSet = !Controller.japaneseSet;
    }

    public void Restart()
    {

    }
}
