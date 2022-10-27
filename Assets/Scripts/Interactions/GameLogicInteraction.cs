using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicInteraction : SyncObj
{
    public override void Interaction()
    {
        //throw new System.NotImplementedException();
    }

    public override void SingleAction()
    {
        AbstractRoomLogic.instance.InteractionHappened(uniqueId);
    }
}
