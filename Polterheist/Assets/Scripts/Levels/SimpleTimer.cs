using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTimer : MonoBehaviour
{
    [SerializeField] private float initialExtraDelay = 0.0f;
    [SerializeField] private List<float> eventDelays = new List<float>();
    [SerializeField] private List<UnityEvent> events = new List<UnityEvent>();

    private float timeToNextEvent = 0.0f;
    private int nextEventIdx = 0;

    private void Start()
    {
        if (eventDelays.Count != events.Count)
        {
            Debug.LogError("Number of timed events does not match number of timer delays");
            this.enabled = false;
            return;
        }

        if (events.Count == 0)
        {
            this.enabled = false;
            return;
        }

        timeToNextEvent = eventDelays[nextEventIdx] + initialExtraDelay;
    }

    private void Update()
    {
        timeToNextEvent -= Time.deltaTime;

        if (timeToNextEvent <= 0.0f)
        {
            events[nextEventIdx].Invoke();
            nextEventIdx = (nextEventIdx + 1) % events.Count;
            timeToNextEvent = eventDelays[nextEventIdx];
        }
    }
}
