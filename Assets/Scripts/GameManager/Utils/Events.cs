using UnityEngine;
using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }
    [System.Serializable] public class EventMobDeath : UnityEvent<MobyType, Vector3 > {}
    [System.Serializable] public class EventIntegerEvent : UnityEvent<int>{ }
}
