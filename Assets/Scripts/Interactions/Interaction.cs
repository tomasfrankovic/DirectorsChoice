using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactions<T> : MonoBehaviour
{
    public T interactObj;

    public UnityEvent interactionEvent;
}
