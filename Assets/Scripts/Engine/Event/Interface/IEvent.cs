using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    void AddEventListener();
    void RemoveEventListener();
}
