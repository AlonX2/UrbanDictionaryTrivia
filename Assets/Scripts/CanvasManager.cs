using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasScript current;
    public static CanvasScript other;
    public float speed;
    public Canvas MainCanvas;
    private void Start()
    {
        current = MainCanvas.transform.GetChild(0).GetComponent<CanvasScript>();
        other = MainCanvas.transform.GetChild(1).GetComponent<CanvasScript>();
        CanvasScript.Speed = speed;
        LoadFirstPage();
    }
    public static void GetReadyToLoadNewPage()
    {
        GlobalScript.Instance.LoadNewQ(GlobalScript.Instance.filter);
    }
    public static void LoadNewPage()
    {
        startNewTransition =true;
        other.gameObject.SetActive(true);
        other.GetCanvasReady();
    }
    static bool startNewTransition = false;
    public static void LoadFirstPage()
    {
        current.GetCanvasReady();
        current.TranslateToWait();
    }
    bool startOtherTransition = false;
    private void FixedUpdate()
    {
        if (startNewTransition)
        {
            if (current.transform.position.x >= CanvasScript.canvasFinalPosition.x)
            {
                OnFinishTransition1();
            }
            else
            {
                if (CanvasScript.canvasFinalPosition.x - current.transform.position.x <= Time.fixedDeltaTime * speed)
                    current.transform.position = CanvasScript.canvasFinalPosition;
                else
                    current.transform.Translate(Vector3.right * Time.fixedDeltaTime * speed);
            }
        }
        if (startOtherTransition)
        {
            if (other.transform.position.x >= CanvasScript.canvasWaitPosition.x)
            {
                OnFinishTransition2();
            }
            else
            {
                if (CanvasScript.canvasWaitPosition.x - other.transform.position.x <= Time.fixedDeltaTime * speed)
                    other.transform.position = CanvasScript.canvasWaitPosition;
                else
                    other.transform.Translate(Vector3.right * Time.fixedDeltaTime * speed);
            }
        }
    }
    void OnFinishTransition1()
    {
        startNewTransition = false;
        startOtherTransition = true;
        ExampleManager.exampleButton.ResetExampleOnNewScreen();
    }
    void OnFinishTransition2()
    {
        startOtherTransition = false;
        ButtonManager.Instance.ActivateEventSystem();
        current.TeleportToStart();
        CanvasScript a = current;
        current = other;
        other = a;
    }
}
