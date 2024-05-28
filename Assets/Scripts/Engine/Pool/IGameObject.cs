using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameObject
{
    GameObject GetGameObject();
    void setObj(GameObject gameObject);
    void OnDestroy();
    void OnDisable();
    void OnEnable();
    void Attach(Transform root);
}
