using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityEvent<Vector2> OnMouseClickRayCast = new UnityEvent<Vector2>();

    public static UnityEvent<float> OnCamDampChanged = new UnityEvent<float>();
}
