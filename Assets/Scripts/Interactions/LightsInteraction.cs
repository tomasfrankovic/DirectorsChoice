using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsInteraction : SyncObj
{
    public List<GameObject> lights;

    public override void Interaction()
    {
        for (int i = 0; i < lights.Count; i++)
            lights[i].SetActive(!lights[i].activeSelf);
    }

    public override void SingleAction() 
    { 
        //shrug
    }
}
