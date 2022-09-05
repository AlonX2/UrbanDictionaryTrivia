using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public static RectTransform currentR;
    public static RectTransform otherR;
    public static CanvasScript current;
    public static CanvasScript other;
    public static float Speed;
    public static Vector2 canvasStartPosition;
    public static Vector2 canvasWaitPosition;
    public static Vector2 canvasFinalPosition;
    public static Vector3[] cRcorners = new Vector3[4];
    public void Awake()
    {
        if (canvasStartPosition != Vector2.zero)
        {
            TeleportToStart();
            return;
        }
        transform.GetComponentInParent<RectTransform>().GetWorldCorners(cRcorners);
        float width = cRcorners[2].x - cRcorners[1].x; //topRight minus topLeft
        float height = cRcorners[1].y - cRcorners[0].y; //topLeft miuns bottomLeft
        Vector2 Mid = new Vector2(cRcorners[0].x + width / 2, cRcorners[0].y + height / 2);//middle
        canvasStartPosition = new Vector2(Mid.x - width, Mid.y);
        canvasWaitPosition = Mid;
        canvasFinalPosition = new Vector2(Mid.x + width, Mid.y);
        TeleportToStart();
    }
    private void FixedUpdate()
    {
        if (startTranslateToFinal)
        {
            if (transform.position.x >= canvasFinalPosition.x)
                startTranslateToFinal = false;
            else
            {
                if (canvasFinalPosition.x - transform.position.x <= Time.fixedDeltaTime * Speed)
                    transform.position = canvasFinalPosition;
                else
                    transform.Translate(Vector3.right * Time.fixedDeltaTime * Speed);
            }
        }
        if (startTranslateToWait)
        {
            if (transform.position.x >= canvasWaitPosition.x)
                startTranslateToWait = false;
            else
            {
                if (canvasWaitPosition.x - transform.position.x <= Time.fixedDeltaTime * Speed)
                    transform.position = canvasWaitPosition;
                else
                    transform.Translate(Vector3.right * Time.fixedDeltaTime * Speed);
            }
        }
    }
    public void GetCanvasReady()
    {
        TextDisplayer.DisplayToCanvas(gameObject.GetComponent<RectTransform>());
        ButtonManager.Instance.UpdateCanvas(gameObject);
    }
    public void TeleportToStart()
    {
        transform.position = canvasStartPosition;
    }
    bool startTranslateToWait = false;
    public void TranslateToWait()
    {
        startTranslateToWait = true;
    }
    bool startTranslateToFinal = false;
    public void TranslateToFinal()
    {
        startTranslateToFinal = true;
    }
}
