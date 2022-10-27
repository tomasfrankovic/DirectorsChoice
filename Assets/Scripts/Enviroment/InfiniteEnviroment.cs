using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteEnviroment : MonoBehaviour
{
    public static InfiniteEnviroment instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public GameObject envToCopy;

    public Transform objToWatch;

    public Transform mainEnviroment;
    public Transform secondaryEnviroment;

    public float moveAmmount = 20f;

    public void Init()
    {
        InitInfinity();
    }

    private void Update()
    {
        UpdatePositions();
    }

    public void InitInfinity()
    {
        mainEnviroment = envToCopy.transform;
        secondaryEnviroment = Instantiate(mainEnviroment);
        secondaryEnviroment.position = mainEnviroment.position + (Vector3.right * moveAmmount);
        mainEnviroment.GetComponent<SyncController>().AddToManager();
        secondaryEnviroment.GetComponent<SyncController>().AddToManager();
    }

    void UpdatePositions()
    {
        if(Vector2.Distance(objToWatch.position, mainEnviroment.position) >= Vector2.Distance(objToWatch.position, secondaryEnviroment.position))
        {
            Transform temp = mainEnviroment;
            mainEnviroment = secondaryEnviroment;
            secondaryEnviroment = temp;
        }

        float side = mainEnviroment.position.x - objToWatch.position.x;
        if (side >=0 && mainEnviroment.position.x < secondaryEnviroment.position.x)
            secondaryEnviroment.position = new Vector2(mainEnviroment.position.x - moveAmmount, mainEnviroment.position.y);
        else if (side < 0 && mainEnviroment.position.x > secondaryEnviroment.position.x)
            secondaryEnviroment.position = new Vector2(mainEnviroment.position.x + moveAmmount, mainEnviroment.position.y);


    }

}
