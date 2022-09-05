using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;
public class LoadingScript : MonoBehaviour
{
    public int loadingTime;
    public GameObject loadingPanel;
    public static LoadingScript currentInstance;

    Stopwatch stopwatch = new Stopwatch();
    private void Start()
    {
        currentInstance = this;
    }
    private void Update()
    {
        if (stopwatch.ElapsedMilliseconds >= loadingTime)
        {
            stopwatch.Reset();
            currentInstance.loadingPanel.SetActive(false);
        }
    }
    public static async Task LoadWhileTask(Task task)
    {
        currentInstance.loadingPanel.SetActive(true);
        await task;
        currentInstance.loadingPanel.SetActive(false);

    }

    public static void ActivateLoadingScreen()
    {
        currentInstance.loadingPanel.SetActive(true);
    }
    public static void DeActivateLoadingScreen()
    {
        currentInstance.stopwatch.Start();
    }
}