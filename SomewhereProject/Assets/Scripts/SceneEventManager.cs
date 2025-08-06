using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class EventMapping
{
    public string eventName;
    public UnityEvent onEventTriggered;
}

public class SceneEventManager : MonoBehaviour
{
    public List<EventMapping> eventMappings;

    void Start()
    {
        foreach (var mapping in eventMappings)
        {
            EventMapping currentMapping = mapping;
            EventManager.Instance.StartListening(currentMapping.eventName, () =>
            {
                currentMapping.onEventTriggered.Invoke();
            });
        }
    }
}