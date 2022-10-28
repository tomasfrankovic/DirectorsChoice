using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionOrderManager : MonoBehaviour
{
    /*private void Awake()
    {
        
    }*/

    private void Start()
    {
        InfiniteEnviroment.instance.Init();
        AbstractRoomLogic.instance.Init();
        AbstractRoomLogic.instance.RunRoomSimulation();
        SceneChangeManager.instance.EndTransition();
    }
}
