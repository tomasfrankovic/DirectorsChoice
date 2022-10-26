using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : SyncObj
{
    public override void Interaction()
    {
        gameObject.SetActive(false);
    }

    public override void SingleAction()
    {
        //add to inventory
    }
}
