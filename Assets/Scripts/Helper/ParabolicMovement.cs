
using UnityEngine;

/// <summary>
/// 子弹抛物线
/// </summary>
public class ParabolicMovement : MonoBehaviour
{
    public Vector3 targetPoint; // 目标位置的Transform
    public float totalTime = 5.0f; // 总时间（秒）
    public float maxHeight = 20.0f; // 高度

    private Vector3 initialPosition;
    private float elapsedTime = 0.0f;
    
    //是否开启updata
    private bool isUpdate = false;

    public void Init(Vector3 start, Vector3 endPoint, float duration, float height = 10.0f)
    {
        // 获取初始位置和目标位置
        initialPosition = start;
        this.targetPoint = endPoint;
        this.totalTime = duration;
        this.maxHeight = height;
        this.elapsedTime = 0;
        isUpdate = true;
    }
    private void Update()
    {
        if(!isUpdate)
            return;
        
        if (elapsedTime < totalTime)
        {
            // 使用Lerp计算当前时间下子弹的位置
            float t = elapsedTime / totalTime;
            Vector3 newPosition = Vector3.Lerp(initialPosition, targetPoint, t);

            // 根据抛物线高度调整Y坐标
            float height = Mathf.Sin(Mathf.PI * t) * maxHeight;
            newPosition.y += height;

            // 更新子弹的位置
            transform.position = newPosition;

            // 更新已经经过的时间
            elapsedTime += Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        isUpdate = false;
    }
}