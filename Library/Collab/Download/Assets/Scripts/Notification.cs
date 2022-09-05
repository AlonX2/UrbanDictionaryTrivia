/*
 * type 1 : Notifications of wait time of locked shit
 * type 2 : Notifications of didnt play for a while
 * ...
 * 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification
{
    #region NotificationDefenition
    public int type;
    public string labelText;
    public string contentText;
    public int timeToActivate;
    #endregion
    #region front
    public static List<Notification> Notifications = new List<Notification>();
    public static void GetNotificationsWritenInFile()
    {
        TextAsset notifications = Resources.Load<TextAsset>(@"Notifications");
        List<string> list = new List<string>(System.Text.RegularExpressions.Regex.Split(notifications.text, System.Environment.NewLine));
        Notifications.Clear();
        foreach (string line in list)
        {
            Notifications.Add(JsonUtility.FromJson<Notification>(line));
        }
    }
    public static void ActivateNotificationOfType(int type)
    {
        desiredType = type;
        List<Notification> notificationsOfType = Notifications.FindAll(IsOfType);
        ActivateNotification(notificationsOfType[Random.Range(0,notificationsOfType.Count)]);
    }
    #endregion
    #region backend
    static int desiredType;
    private static bool IsOfType(Notification notif)
    {
        if (notif == null)
            return false;
        if (notif.type == desiredType)
            return true;
        return false;
          
    }
    #endregion
    private static void ActivateNotification(Notification notif)
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            //scheduale android notification using notif... maybe use function to make it cleaner, not necesary 
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //scheduale IOS notification using notif... maybe use function to make it cleaner, not necesary 
        }
        //will continue working on it once getting home from camping sry bruh I have no time
    }
}
