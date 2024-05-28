using UnityEngine;

public enum BoundSideType
{
    Top = 0,
    Buttom,
    Left,
    Forward,
    Right,
    Back,
}


public class ShowRangeMono : MonoBehaviour
{
    public Collider m_collider = null;
    public BoundSideType m_type = BoundSideType.Buttom;
    private Vector3 _startColliderPos;
    private Vector3 _startSelfPos;
    private float m_offest = 0.001f;

    void Start()
    {
        if (m_collider != null)
            ShowRange(m_collider, m_type);
    }

    public void ShowRange(Collider m_collider, BoundSideType type)
    {
        if (m_collider == null) return;
        _startColliderPos = m_collider.transform.position;
        Vector3 pos_0 = Vector3.zero; //MathUtility.GetBoundPos(m_collider.bounds, -0.5f, -0.5f, -0.5f);
        Vector3 pos_1 = Vector3.zero; //= MathUtility.GetBoundPos(m_collider.bounds, 0.5f, -0.5f, 0.5f);
        GetBoundSide(m_collider, type, ref pos_0, ref pos_1);
        Vector3 offest = pos_1 - pos_0;
        Vector3 halfPos = (pos_0 + pos_1) * 0.5f;
        _startSelfPos = halfPos;
        transform.position = halfPos;
        transform.localScale = CheckScale(offest);
    }

    public void UpdateCollider(Collider m_collider)
    {
        transform.position = m_collider.transform.position - _startColliderPos + _startSelfPos;
    }

    private Vector3 CheckScale(Vector3 pos)
    {
        float x = Mathf.Abs(pos.x);
        float y = Mathf.Abs(pos.y);
        float z = Mathf.Abs(pos.z);
        return new Vector3(x == 0 ? 0.0001f : x, y == 0 ? 0.0001f : y, z == 0 ? 0.0001f : z);
    }

    private void GetBoundSide(Collider m_collider, BoundSideType type, ref Vector3 pos_1, ref Vector3 pos_2)
    {
        if (type == BoundSideType.Buttom)
        {
            pos_1 = MathUtility.GetBoundPos(m_collider.bounds, -0.5f, -0.5f, -0.5f) + Vector3.down * m_offest;
            pos_2 = MathUtility.GetBoundPos(m_collider.bounds, 0.5f, -0.5f, 0.5f) + Vector3.down * m_offest;
        }
        else if (type == BoundSideType.Top)
        {
            pos_1 = MathUtility.GetBoundPos(m_collider.bounds, -0.5f, 0.5f, 0.5f) + Vector3.up * m_offest;
            pos_2 = MathUtility.GetBoundPos(m_collider.bounds, 0.5f, 0.5f, -0.5f) + Vector3.up * m_offest;
        }
        else if (type == BoundSideType.Left)
        {
            pos_1 = MathUtility.GetBoundPos(m_collider.bounds, -0.5f, 0.5f, -0.5f) + Vector3.left * m_offest;
            pos_2 = MathUtility.GetBoundPos(m_collider.bounds, -0.5f, -0.5f, 0.5f) + Vector3.left * m_offest;
        }
        else if (type == BoundSideType.Forward)
        {
            pos_1 = MathUtility.GetBoundPos(m_collider.bounds, -0.5f, -0.5f, 0.5f) + Vector3.forward * m_offest;
            pos_2 = MathUtility.GetBoundPos(m_collider.bounds, 0.5f, 0.5f, 0.5f) + Vector3.forward * m_offest;
        }
        else if (type == BoundSideType.Right)
        {
            pos_1 = MathUtility.GetBoundPos(m_collider.bounds, 0.5f, 0.5f, -0.5f) + Vector3.right * m_offest;
            pos_2 = MathUtility.GetBoundPos(m_collider.bounds, 0.5f, -0.5f, 0.5f) + Vector3.right * m_offest;
        }
        else if (type == BoundSideType.Back)
        {
            pos_1 = MathUtility.GetBoundPos(m_collider.bounds, -0.5f, 0.5f, -0.5f) + Vector3.back * m_offest;
            pos_2 = MathUtility.GetBoundPos(m_collider.bounds, 0.5f, -0.5f, -0.5f) + Vector3.back * m_offest;
        }
    }
}
