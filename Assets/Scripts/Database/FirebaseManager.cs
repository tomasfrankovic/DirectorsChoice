using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if !UNITY_WEBGL
using Firebase.Database;
using Firebase.Extensions;
#endif
using System;

public class FirebaseManager : MonoBehaviour
{

    public static FirebaseManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

#if !UNITY_WEBGL
    DatabaseReference dbRef;
#endif
    public TextMeshProUGUI text;

    public void Increment()
    {
#if !UNITY_WEBGL
        if (dbRef == null)
            dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        LoadData("counter", text);
#endif
    }

    public void LoadData(string path, TextMeshProUGUI text)
    {
#if !UNITY_WEBGL
        FirebaseDatabase.DefaultInstance
          .GetReference(path)
          .GetValueAsync().ContinueWithOnMainThread(task => {
              if (task.IsFaulted)
              {
                  // Handle the error...
              }
              else if (task.IsCompleted)
              {
                  DataSnapshot snapshot = task.Result;
                  int value = int.Parse(Convert.ToString(snapshot.Value))+1;
                  text.text = "- " + value + " players completed this game";
                  

                  dbRef.Child(path).SetValueAsync(value);
              }
          });
#endif
    }

}
