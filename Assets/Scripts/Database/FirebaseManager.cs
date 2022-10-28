using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class FirebaseManager : MonoBehaviour
{
    DatabaseReference dbRef;
    public TextMeshProUGUI text;

    public void Increment()
    {
        if (dbRef == null)
            dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        LoadData("counter", text);
    }

    public void LoadData(string path, TextMeshProUGUI text)
    {
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

                  text.text = snapshot.Value.ToString();
                  int value = int.Parse(Convert.ToString(snapshot.Value));

                  dbRef.Child(path).SetValueAsync(value + 1);
              }
          });
    }

}
