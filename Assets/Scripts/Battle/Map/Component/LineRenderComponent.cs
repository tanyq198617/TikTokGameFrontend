using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderComponent : MonoBehaviour
{
    public event Action onMouseDown;
    public event Action onMouse;
    public event Action onMouseUp;

    private bool isDrawing = false;

    [SerializeField] public bool isCanDraw = false;

    private void Update()
    {
        if (isCanDraw)
        {
            if (isDrawing)
            {
                if (onMouseUp != null) onMouseUp();
                isDrawing = false;
            }

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            if (onMouseDown != null) 
                onMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            isDrawing = true;
            if (onMouse != null) 
                onMouse();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (onMouseUp != null) 
                onMouseUp();
            isDrawing = false;
        }
    }
}
