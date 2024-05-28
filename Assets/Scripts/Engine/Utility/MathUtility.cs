using UnityEngine;
using System.Collections.Generic;
using System;

public enum ECompareType
{
    ECT_GREATER = 0,
    ECT_GREATER_EQUAL,
    ECT_EQUAL,
    ECT_LESS,
    ECT_LESS_EQUAL,
    ECT_NOT_EUQAL,
}

public static class MathUtility
{
    public static void Rotate(ref float x, ref float z, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        float locationX = sin * z + cos * x;
        float locationZ = cos * z - sin * x;
        x = locationX;
        z = locationZ;
    }

    public static float DistanceSqr(Vector3 p0, Vector3 p1)
    {
        float x = p0.x - p1.x, z = p0.z - p1.z;
        return x * x + z * z;
    }

    public static Vector3 ArrToV3(double[] ds)
    {
        if (ds.Length <= 0)
            return Vector3.zero;
        return new Vector3((float)ds[0], ds.Length > 1 ? (float)ds[1] : 0, ds.Length > 2 ? (float)ds[2] : 0);
    }

    public static Vector3 ArrToV3(float[] ds)
    {
        if (ds.Length <= 0)
            return Vector3.zero;
        return new Vector3(ds[0], ds.Length > 1 ? ds[1] : 0, ds.Length > 2 ? ds[2] : 0);
    }

    public static Vector3 ArrToV3(int[] ds)
    {
        if (ds.Length <= 0)
            return Vector3.zero;
        return new Vector3(ds[0], ds.Length > 1 ? ds[1] : 0, ds.Length > 2 ? ds[2] : 0);
    }

    public static float DistanceMax(float x1, float y1, float x2, float y2)
    {
        return Mathf.Max(Mathf.Abs(x1 - x2), Mathf.Abs(y1 - y2));
    }

    public static float DistanceMax(Vector3 p0, Vector3 p1)
    {
        return Mathf.Max(Mathf.Abs(p0.x - p1.x), Mathf.Abs(p0.z - p1.z));
    }

    public static float WorldPosToCameraScale(Vector3 worldPos)
    {
        if (Camera.main == null || Camera.main.transform == null)
            return 0;
        return WorldPosToCameraScale(Vector3.Distance(worldPos, Camera.main.transform.position));
    }

    public static float WorldPosToCameraScale(float length)
    {
        return Mathf.Clamp(Mathf.Abs(1.0f - length / 30f), 0.3f, 1.0f);
    }


    public static float CrossProduct(float x0, float y0, float x1, float y1) { return x0 * y1 - y0 * x1; }

    //---------------------------------------------------------------------------
    public static bool RectangleHitDefineCollision(
        Vector3 HitDefPos, float HitDefOrientation,
        Vector3 HitDef,
        Vector3 AttackeePos, float AttackeeOrientation,
        Vector3 AttackeeBounding)
    {
        //排除高度影响，以XZ平面坐标作为判定基准
        if (HitDefPos.y > AttackeePos.y + AttackeeBounding.y ||
            AttackeePos.y > HitDefPos.y + HitDef.y)
        {
            return false;
        }

        // 计算出第一个四边形的四个定点
        float x0 = -HitDef.x * 0.5f, z0 = -HitDef.z * 0.5f;
        float x1 = -HitDef.x * 0.5f, z1 = HitDef.z * 0.5f;
        Rotate(ref x0, ref z0, HitDefOrientation);
        Rotate(ref x1, ref z1, HitDefOrientation);
        Vector2 maxHit = new Vector2(Mathf.Max(Mathf.Abs(x0), Mathf.Abs(x1)), Mathf.Max(Mathf.Abs(z0), Mathf.Abs(z1)));
        float[] HitDefPointX = new float[4] {
            HitDefPos.x - x0,
            HitDefPos.x - x1,
            HitDefPos.x + x0,
            HitDefPos.x + x1};
        float[] HitDefPointZ = new float[4] {
            HitDefPos.z - z0,
            HitDefPos.z - z1,
            HitDefPos.z + z0,
            HitDefPos.z + z1};

        // 计算出第二个四边形的四个顶点
        x0 = -AttackeeBounding.x * 0.5f;
        z0 = -AttackeeBounding.z * 0.5f;
        x1 = -AttackeeBounding.x * 0.5f;
        z1 = AttackeeBounding.z * 0.5f;
        Rotate(ref x0, ref z0, AttackeeOrientation);
        Rotate(ref x1, ref z1, AttackeeOrientation);
        Vector2 maxAtk = new Vector2(Mathf.Max(Mathf.Abs(x0), Mathf.Abs(x1)), Mathf.Max(Mathf.Abs(z0), Mathf.Abs(z1)));
        float[] AttackeePointX = new float[4] {
            AttackeePos.x - x0,
            AttackeePos.x - x1,
            AttackeePos.x + x0,
            AttackeePos.x + x1};
        float[] AttackeePointZ = new float[4] {
            AttackeePos.z - z0,
            AttackeePos.z - z1,
            AttackeePos.z + z0,
            AttackeePos.z + z1};

        if (HitDefPos.x > AttackeePos.x + maxHit[0] + maxAtk[0] ||
            HitDefPos.x < AttackeePos.x - maxHit[0] - maxAtk[0] ||
            HitDefPos.z > AttackeePos.z + maxHit[1] + maxAtk[1] ||
            HitDefPos.z < AttackeePos.z - maxHit[1] - maxAtk[1])
            return false;

        // 拿四边形的四个顶点判断，是否在另外一个四边形的四条边的一侧
        for (int i = 0; i < 4; i++)
        {
            x0 = HitDefPointX[i];
            x1 = HitDefPointX[(i + 1) % 4];
            z0 = HitDefPointZ[i];
            z1 = HitDefPointZ[(i + 1) % 4];

            bool hasSameSidePoint = false;
            for (int j = 0; j < 4; j++)
            {
                float v = CrossProduct(x1 - x0, z1 - z0, AttackeePointX[j] - x0, AttackeePointZ[j] - z0);
                if (v < 0)
                {
                    hasSameSidePoint = true;
                    break;
                }
            }

            // 如果4个定点都在其中一条边的另外一侧，说明没有交点
            if (!hasSameSidePoint)
                return false;
        }

        // 所有边可以分割另外一个四边形，说明有焦点。
        return true;
    }

    //---------------------------------------------------------------------------
    public static bool CylinderHitDefineCollision(
        Vector3 HitDefPos, float HitDefOrientation,
        float HitRadius, float HitDefHeight,
        Vector3 AttackeePos, float AttackeeOrientation,
        Vector3 AttackeeBounding)
    {
        //排除高度影响，以XZ平面坐标作为判定基准
        if (HitDefPos.y > AttackeePos.y + AttackeeBounding.y ||
            AttackeePos.y > HitDefPos.y + HitDefHeight)
            return false;

        float vectz = HitDefPos.z - AttackeePos.z;
        float vectx = HitDefPos.x - AttackeePos.x;
        if (vectx != 0 || vectz != 0)
            Rotate(ref vectx, ref vectz, -AttackeeOrientation);

        if ((Mathf.Abs(vectx) > (HitRadius + AttackeeBounding.z)) || (Mathf.Abs(vectz) > (HitRadius + AttackeeBounding.x)))
            return false;

        return true;
    }


    //---------------------------------------------------------------------------
    public static bool RingHitDefineCollision(
        Vector3 HitDefPos, float HitDefOrientation,
        float HitInnerRadius, float HitDefHeight, float HitOutRadius,
        Vector3 AttackeePos, float AttackeeOrientation,
        Vector3 AttackeeBounding)
    {
        //排除高度影响，以XZ平面坐标作为判定基准
        if (HitDefPos.y > AttackeePos.y + AttackeeBounding.y ||
            AttackeePos.y > HitDefPos.y + HitDefHeight)
            return false;

        float radius = Mathf.Min(AttackeeBounding.x, AttackeeBounding.z);
        float distance = (AttackeePos - HitDefPos).magnitude;
        if (distance + radius < HitInnerRadius || distance - radius > HitOutRadius)
            return false;

        return true;
    }
    //---------------------------------------------------------------------------
    public static bool FanDefineCollision(
                Vector3 HitDefPos, float HitDefOrientation,
                float HitRadius, float HitDefHeight, float StartAngle, float EndAngle,
                Vector3 AttackeePos, float AttackeeOrientation,
                Vector3 AttackeeBounding)
    {
        //排除高度影响，以XZ平面坐标作为判定基准
        if (HitDefPos.y > AttackeePos.y + AttackeeBounding.y ||
            AttackeePos.y > HitDefPos.y + HitDefHeight)
            return false;

        //圆心的坐标转化到被攻击者的坐标系去
        float vectz = AttackeePos.z - HitDefPos.z;
        float vectx = AttackeePos.x - HitDefPos.x;
        Rotate(ref vectz, ref vectx, HitDefOrientation);

        float hitRadius = HitRadius;

        float attackCenter_x = vectz;
        float attackCenter_y = vectx;

        float attackRadius = AttackeeBounding.x > AttackeeBounding.z ? AttackeeBounding.z : AttackeeBounding.x;

        float centerDis = Distance(0, 0, attackCenter_x, attackCenter_y);
        if (centerDis > (hitRadius + attackRadius)) //相离
            return false;

        if ((centerDis <= attackRadius))
            return true;

        float start_rad = StartAngle * Mathf.Deg2Rad;
        float end_rad = EndAngle * Mathf.Deg2Rad;


        float center_angle = 0.5f * (start_rad + end_rad);
        float axis_x = Mathf.Cos(center_angle);
        float axis_y = Mathf.Sin(center_angle);

        float len = Mathf.Sqrt(attackCenter_x * attackCenter_x + attackCenter_y * attackCenter_y);
        float temp_x = attackCenter_x / len;
        float temp_y = attackCenter_y / len;

        float dot = axis_x * temp_x + temp_y * axis_y;

        float value = Mathf.Cos(0.5f * (end_rad - start_rad));
        if (dot >= value)
            return true;

        float dis1 = PointToLineSegmentDistance(attackCenter_x, attackCenter_y, 0, 0,
            hitRadius * Mathf.Cos(start_rad), hitRadius * Mathf.Sin(start_rad));
        float dis2 = PointToLineSegmentDistance(attackCenter_x, attackCenter_y, 0, 0,
            hitRadius * Mathf.Cos(end_rad), hitRadius * Mathf.Sin(end_rad));

        if ((dis1 <= attackRadius) || (dis2 <= attackRadius))
            return true;

        return false;
    }

    /// <summary>
    /// 获取点在线段上垂足
    /// </summary>
    /// <param name="pt">点</param>
    /// <param name="begin">线段起始点</param>
    /// <param name="end">线段结束点</param>
    /// <returns></returns>
    public static Vector3 GetFootOfPerpendicular(Vector3 pt, Vector3 begin, Vector3 end)   // 直线结束点
    {
        Vector3 retVal = new Vector3();
        float dx = begin.x - end.x;
        float dy = begin.y - end.y;
        float dz = begin.z - end.z;

        if (Mathf.Abs(dx) < 0.00000001f && Mathf.Abs(dy) < 0.00000001f && Mathf.Abs(dz) < 0.00000001)
        {
            retVal = begin;
            return retVal;
        }

        float u = (pt.x - begin.x) * (begin.x - end.x) +
                  (pt.y - begin.y) * (begin.y - end.y) +
                  (pt.z - begin.z) * (begin.z - end.z);

        u = u / ((dx * dx) + (dy * dy) + (dz * dz));
        retVal.x = begin.x + u * dx;
        retVal.y = begin.y + u * dy;
        retVal.z = begin.z + u * dz;
        return retVal;
    }

    /// <summary>
    /// 点到线段距离
    /// </summary>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    /// <param name="x1">线段1X坐标</param>
    /// <param name="y1">线段1Y坐标</param>
    /// <param name="x2">线段2X坐标</param>
    /// <param name="y2">线段2Y坐标</param>
    /// <returns></returns>
    public static float PointToLineSegmentDistance(float x, float y, float x1, float y1, float x2, float y2)
    {
        float v1_x = x2 - x1;
        float v1_y = y2 - y1;

        //at + b = 0
        float a = (x2 - x1) * v1_x + (y2 - y1) * v1_y;
        float b = v1_x * (x1 - x) + v1_y * (y1 - y);

        if (a == 0.0f)
            return 0;

        float t = -b / a;

        //因为是线段,不是直线,处理极端的情况
        if (t < 0)
            t = 0;
        if (t > 1)
            t = 1;

        float point_x = x1 + t * (x2 - x1);
        float point_y = y1 + t * (y2 - y1);

        return Distance(point_x, point_y, x, y);
    }

    static float Distance(float x1, float y1, float x2, float y2)
    {
        float deletax = x1 - x2;
        float deletay = y1 - y2;
        return Mathf.Sqrt(deletax * deletax + deletay * deletay);
    }

    /// <summary>
    /// 任意点到直线(任意两点的连线)的距离
    /// </summary>
    /// <param name="point">任意点</param>
    /// <param name="linePoint1">线段组成点A</param>
    /// <param name="linePoint2">线段组成点B</param>
    /// <returns></returns>
    public static float distancePoint2Line(Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
    {
        float fProj = Vector3.Dot(point - linePoint1, (linePoint1 - linePoint2).normalized);
        return Mathf.Sqrt((point - linePoint1).sqrMagnitude - fProj * fProj);
    }

    /// <summary>
    /// 在区间里面
    /// </summary>
    /// <param name="data"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static bool IsInIntervalToInt(int data, int min, int max)
    {
        return data >= min && data < max;
    }

    public static bool Compare(ECompareType com, object a, object v)
    {
        if (a is int)
        {
            switch (com)
            {
                case ECompareType.ECT_EQUAL: return (int)a == (int)v;
                case ECompareType.ECT_NOT_EUQAL: return (int)a != (int)v;
                case ECompareType.ECT_GREATER: return (int)a > (int)v;
                case ECompareType.ECT_LESS: return (int)a < (int)v;
                case ECompareType.ECT_GREATER_EQUAL: return (int)a >= (int)v;
                case ECompareType.ECT_LESS_EQUAL: return (int)a <= (int)v;
                default: return false;
            }
        }
        else if (a is uint)
        {
            switch (com)
            {
                case ECompareType.ECT_EQUAL: return (uint)a == (uint)v;
                case ECompareType.ECT_NOT_EUQAL: return (uint)a != (uint)v;
                case ECompareType.ECT_GREATER: return (uint)a > (uint)v;
                case ECompareType.ECT_LESS: return (uint)a < (uint)v;
                case ECompareType.ECT_GREATER_EQUAL: return (uint)a >= (uint)v;
                case ECompareType.ECT_LESS_EQUAL: return (uint)a <= (uint)v;
                default: return false;
            }
        }
        else if (a is float)
        {
            switch (com)
            {
                case ECompareType.ECT_EQUAL: return (float)a == (float)v;
                case ECompareType.ECT_NOT_EUQAL: return (float)a != (float)v;
                case ECompareType.ECT_GREATER: return (float)a > (float)v;
                case ECompareType.ECT_LESS: return (float)a < (float)v;
                case ECompareType.ECT_GREATER_EQUAL: return (float)a >= (float)v;
                case ECompareType.ECT_LESS_EQUAL: return (float)a <= (float)v;
                default: return false;
            }
        }
        else if (a is long)
        {
            switch (com)
            {
                case ECompareType.ECT_EQUAL: return (long)a == (long)v;
                case ECompareType.ECT_NOT_EUQAL: return (long)a != (long)v;
                case ECompareType.ECT_GREATER: return (long)a > (long)v;
                case ECompareType.ECT_LESS: return (long)a < (long)v;
                case ECompareType.ECT_GREATER_EQUAL: return (long)a >= (long)v;
                case ECompareType.ECT_LESS_EQUAL: return (long)a <= (long)v;
                default: return false;
            }
        }
        else if (a is ulong)
        {
            switch (com)
            {
                case ECompareType.ECT_EQUAL: return (ulong)a == (ulong)v;
                case ECompareType.ECT_NOT_EUQAL: return (ulong)a != (ulong)v;
                case ECompareType.ECT_GREATER: return (ulong)a > (ulong)v;
                case ECompareType.ECT_LESS: return (ulong)a < (ulong)v;
                case ECompareType.ECT_GREATER_EQUAL: return (ulong)a >= (ulong)v;
                case ECompareType.ECT_LESS_EQUAL: return (ulong)a <= (ulong)v;
                default: return false;
            }
        }
        else
        {
            Debug.Log(a.GetType() + "当前类型未加入Compare函数");
            return false;
        }
    }

    public static float PlusRate(object src, float rate)
    {
        if (src is int) return (int)(src) * rate;
        else if (src is uint) return (uint)(src) * rate;
        else if (src is float) return (float)(src) * rate;
        else if (src is long) return (long)(src) * rate;
        else if (src is ulong) return (ulong)(src) * rate;
        else return 0f;
    }

    public static object Plus(object src, float rate)
    {
        if (src is int) return (int)((int)(src) * rate);
        else if (src is uint) return (uint)((uint)(src) * rate);
        else if (src is float) return (float)(src) * rate;
        else if (src is long) return (long)((long)(src) * rate);
        else if (src is ulong) return (ulong)((ulong)(src) * rate);
        else return 0;
    }

    /// <summary>
    /// 生成圆形网格
    /// </summary>
    /// <param name="grid_x"></param>
    /// <param name="grid_z"></param>
    /// <param name="inner_radius"></param>
    /// <param name="outer_radius"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Mesh GenerateCircleMesh(int grid_x, int grid_z, float inner_radius, float outer_radius, float angle)
    {
        Mesh mesh = new Mesh();

        int vertex_count_x = grid_x + 1;
        int vertex_count_z = grid_z + 1;

        Vector3[] vertices = new Vector3[vertex_count_x * vertex_count_z];
        Vector2[] uvs = new Vector2[vertex_count_x * vertex_count_z];

        float per_angle = angle / 360.0f;
        float per_angle_radians = per_angle * Mathf.PI * 2.0f / grid_x;
        float start_angle_radians = -per_angle * Mathf.PI;

        float per_radius = (outer_radius - inner_radius) / grid_z;
        float curr_radius = outer_radius;

        for (int l = 0; l < vertex_count_z; ++l)
        {
            float v = 1.0f - (float)l / grid_z;
            for (int i = 0; i < vertex_count_x; ++i)
            {
                float angle_radian = per_angle_radians * i + start_angle_radians;
                vertices[l * vertex_count_x + i] = new Vector3(Mathf.Sin(angle_radian) * curr_radius, 0.0f, Mathf.Cos(angle_radian) * curr_radius);

                uvs[l * vertex_count_x + i] = new Vector2((float)i / grid_x, v);
            }

            curr_radius -= per_radius;
        }


        mesh.vertices = vertices;
        mesh.uv = uvs;

        int[] indices = new int[grid_x * grid_z * 6];

        int n = 0;

        for (int iz = 0; iz < grid_z; ++iz)
        {
            for (int ix = 0; ix < grid_x; ++ix)
            {
                int nLocIndex = iz * vertex_count_x + ix;
                indices[n++] = (nLocIndex);
                indices[n++] = (nLocIndex + 1);
                indices[n++] = (nLocIndex + vertex_count_x);

                indices[n++] = (nLocIndex + vertex_count_x);
                indices[n++] = (nLocIndex + 1);
                indices[n++] = (nLocIndex + vertex_count_x + 1);
            }
        }

        mesh.triangles = indices;
        return mesh;
    }

    public static Vector3 Vec2ToVec3(Vector2 vec2)
    {
        return new Vector3(vec2.x, 0, vec2.y);
    }

    public static Vector3 Vec3ToVec2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.z);
    }

    /// <summary>
    /// 生成方形网格
    /// </summary>
    /// <param name="width"></param>
    /// <param name="length"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Mesh GenerateSquareMesh(float width, float length, float angle)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] indices = new int[6];

        float half_w = width * 0.5f;

        float angle_radian = angle / 360.0f * Mathf.PI * 2.0f;
        float sin = Mathf.Sin(angle_radian);
        float cos = Mathf.Cos(angle_radian);

        vertices[0] = new Vector3(-half_w * cos, 0.0f, half_w * sin);
        vertices[1] = new Vector3(-half_w * cos + length * sin, 0.0f, length * cos + half_w * sin);
        vertices[2] = new Vector3(half_w * cos, 0.0f, -half_w * sin);
        vertices[3] = new Vector3(half_w * cos + length * sin, 0.0f, length * cos - half_w * sin);

        uvs[0] = new Vector2(0.0f, 0.0f);
        uvs[1] = new Vector2(0.0f, 1.0f);
        uvs[2] = new Vector2(1.0f, 0.0f);
        uvs[3] = new Vector2(1.0f, 1.0f);

        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;
        indices[3] = 2;
        indices[4] = 1;
        indices[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = indices;

        return mesh;
    }

    public static Mesh GeneratePlaneMesh(float width, float length, int grid_x, int grid_z)
    {
        Mesh mesh = new Mesh();

        int vertex_count_x = grid_x + 1;
        int vertex_count_z = grid_z + 1;

        Vector3[] vertices = new Vector3[vertex_count_x * vertex_count_z];
        Vector2[] uvs = new Vector2[vertex_count_x * vertex_count_z];


        float start_x = -width * 0.5f;
        float start_z = length * 0.5f;

        float per_w = width / grid_x;
        float per_l = length / grid_z;

        for (int l = 0; l < vertex_count_z; ++l)
        {
            float z = start_z - per_l * l;
            float v = 1.0f - (float)l / grid_z;
            for (int i = 0; i < vertex_count_x; ++i)
            {
                vertices[l * vertex_count_x + i] = new Vector3(start_x + per_w * i, 0.0f, z);

                uvs[l * vertex_count_x + i] = new Vector2((float)i / grid_x, v);
            }
        }


        mesh.vertices = vertices;
        mesh.uv = uvs;

        int[] indices = new int[grid_x * grid_z * 6];
        //int triangles_count = grid_x * grid_z;

        int n = 0;

        for (int iz = 0; iz < grid_z; ++iz)
        {
            for (int ix = 0; ix < grid_x; ++ix)
            {
                int nLocIndex = iz * vertex_count_x + ix;
                indices[n++] = (nLocIndex);
                indices[n++] = (nLocIndex + 1);
                indices[n++] = (nLocIndex + vertex_count_x);

                indices[n++] = (nLocIndex + vertex_count_x);
                indices[n++] = (nLocIndex + 1);
                indices[n++] = (nLocIndex + vertex_count_x + 1);
            }
        }

        mesh.triangles = indices;
        return mesh;
    }

    /// <summary>
    /// 求2D线段交点
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static bool SegmentsIntr2D(Vector2 a, Vector2 b, Vector2 c, Vector2 d, ref Vector3 point)
    {
        // 三角形abc 面积的2倍  
        var area_abc = (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);

        // 三角形abd 面积的2倍  
        var area_abd = (a.x - d.x) * (b.y - d.y) - (a.y - d.y) * (b.x - d.x);

        // 面积符号相同则两点在线段同侧,不相交 (对点在线段上的情况,本例当作不相交处理);  
        if (area_abc * area_abd >= 0)
        {
            return false;
        }

        // 三角形cda 面积的2倍
        var area_cda = (c.x - a.x) * (d.y - a.y) - (c.y - a.y) * (d.x - a.x);
        // 三角形cdb 面积的2倍
        // 注意: 这里有一个小优化.不需要再用公式计算面积,而是通过已知的三个面积加减得出.
        var area_cdb = area_cda + area_abc - area_abd;
        if (area_cda * area_cdb >= 0)
        {
            return false;
        }

        //计算交点坐标
        var t = area_cda / (area_abd - area_abc);
        var dx = t * (b.x - a.x);
        var dy = t * (b.y - a.y);
        point.Set(a.x + dx, a.y + dy, 0.0f);
        return true;
    }

    private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
    {
        return 0.5f *
               ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i +
                (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
    }

    public static Rect CatMaxRectInCir(Vector3 pos, Vector3 forward, float radiu)
    {
        Vector3 curMin = pos + Quaternion.Euler(Vector3.up * -45f) * forward * radiu;
        Vector3 curMax = pos + Quaternion.Euler(Vector3.up * 135f) * forward * radiu;
        return new Rect(curMin.x, curMin.z, curMax.x - curMin.x, curMax.z - curMin.z);
    }

    public static void CalcBounds(Transform top, Transform node, ref Bounds bounds)
    {
        MeshFilter mesh_filter = node.GetComponent<MeshFilter>();
        SkinnedMeshRenderer skinned_renderer = node.GetComponent<SkinnedMeshRenderer>();

        if (null != mesh_filter || null != skinned_renderer)
        {
            Vector3 local_scale = Vector3.one;
            Quaternion local_rotation = Quaternion.identity;
            Vector3 local_position = Vector3.zero;

            if (node != top)
            {
                local_scale = new Vector3(node.lossyScale.x / top.lossyScale.x, node.lossyScale.y / top.lossyScale.y, node.lossyScale.z / top.lossyScale.z);
                local_rotation = node.localRotation;
                Transform parent = node.parent;
                while (parent && parent != top)
                {
                    local_rotation = local_rotation * parent.localRotation;
                    parent = parent.parent;
                }
                local_position = node.position - top.position;
            }

            Bounds local_bounds = new Bounds(Vector3.zero, Vector3.zero);
            if (mesh_filter && mesh_filter.mesh)
            {
                local_bounds.Encapsulate(mesh_filter.mesh.bounds);
            }
            if (skinned_renderer)
            {
                local_bounds.Encapsulate(skinned_renderer.localBounds);
            }

            Vector3 max = local_bounds.max;
            Vector3 min = local_bounds.min;

            bounds.Encapsulate(TransformPoint(local_scale, local_rotation, local_position, new Vector3(max.x, max.y, max.z)));
            bounds.Encapsulate(TransformPoint(local_scale, local_rotation, local_position, new Vector3(min.x, max.y, max.z)));
            bounds.Encapsulate(TransformPoint(local_scale, local_rotation, local_position, new Vector3(max.x, max.y, min.z)));
            bounds.Encapsulate(TransformPoint(local_scale, local_rotation, local_position, new Vector3(min.x, max.y, min.z)));
            bounds.Encapsulate(TransformPoint(local_scale, local_rotation, local_position, new Vector3(min.x, min.y, min.z)));
            bounds.Encapsulate(TransformPoint(local_scale, local_rotation, local_position, new Vector3(max.x, min.y, min.z)));
            bounds.Encapsulate(TransformPoint(local_scale, local_rotation, local_position, new Vector3(min.x, min.y, max.z)));
            bounds.Encapsulate(TransformPoint(local_scale, local_rotation, local_position, new Vector3(max.x, min.y, max.z)));
        }

        for (int i = 0; i < node.childCount; ++i)
        {
            CalcBounds(top, node.GetChild(i), ref bounds);
        }
    }

    public static Vector3 TransformPoint(Vector3 scale, Quaternion rotation, Vector3 translate, Vector3 point)
    {
        point = new Vector3(point.x * scale.x, point.y * scale.y, point.z * scale.z);
        point = rotation * point + translate;
        return point;
    }

    public static string TickToHMS(int tick)
    {
        int H = 0, M = 0, S = 0;
        H = tick / 3600;
        M = (tick % 3600) / 60;
        S = tick % 60;

        if (H > 0)
        {
            return StringUtility.AppendFormat("{0}时{1}分{2}秒", H, M, S);
        }

        if (M > 0)
        {
            return StringUtility.AppendFormat("{1}分{2}秒", H, M, S);
        }

        return StringUtility.AppendFormat("{2}秒", H, M, S);
    }

    public static string TickToTime(int tick)
    {
        int H = 0, M = 0, S = 0;
        H = tick / 3600;
        M = (tick % 3600) / 60;
        S = tick % 60;

        if (H > 0)
        {
            return StringUtility.AppendFormat("{0}:{1}:{2}", H.ToString("#00"), M.ToString("#00"), S.ToString("#00"));
        }

        if (M > 0)
        {
            return StringUtility.AppendFormat("{1}:{2}", H.ToString("#00"), M.ToString("#00"), S.ToString("#00"));
        }

        return StringUtility.AppendFormat("{1}:{2}", H, M.ToString("#00"), S.ToString("#00"));
    }

    public static bool IsInBox(Vector3 position, BoxCollider collider)
    {
        Vector3 local_position = collider.transform.InverseTransformPoint(position) - collider.center;

        if (Mathf.Abs(local_position.x) > collider.size.x * 0.5f ||
            Mathf.Abs(local_position.y) > collider.size.y * 0.5f ||
            Mathf.Abs(local_position.z) > collider.size.z * 0.5f)
        {
            return false;
        }

        return true;
    }

    public static bool IsOutTime(ref float time, float maxTime, bool isSys = false, float rate = 1.0f)
    {
        float curTime = isSys ? Time.realtimeSinceStartup : TimeMgr.RealTime;
        //float curTime = Time.realtimeSinceStartup;
        if (time == 0) time = curTime;
        return (curTime - time) * rate >= maxTime;
    }

    public static float GetPassTime(ref float time, float maxTime, bool isSys = false, float rate = 1.0f)
    {
        float curTime = isSys ? Time.realtimeSinceStartup : TimeMgr.RealTime;
        if (time == 0) time = curTime;
        return Mathf.Min((curTime - time) * rate, maxTime);
    }

    public static float GetrogressTime(ref float time, float maxTime, bool isSys = false, float rate = 1.0f)
    {
        float curTime = isSys ? Time.realtimeSinceStartup : TimeMgr.RealTime;
        //float curTime = Time.realtimeSinceStartup;
        if (time == 0) time = curTime;
        return Mathf.Clamp01((curTime - time) * rate / maxTime);
    }

    public static float GetLastTime(ref float time, float maxTime, bool isSys = false, float rate = 1.0f)
    {
        float curTime = isSys ? Time.realtimeSinceStartup : TimeMgr.RealTime;
        //float curTime = Time.realtimeSinceStartup;
        if (time == 0) time = curTime;
        return Mathf.Max(0, maxTime - (curTime - time) * rate);
    }

    #region 计算从一个点到一个线段的距离并填充信息沿线段的最接近选择的点，和段投影。
    public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1,
    ref Vector3 chosenPoint,
    ref float segmentProjection)
    {
        var normal = ep1 - ep0;
        var length = normal.magnitude;
        normal *= 1 / length;

        return PointToSegmentDistance(point, ep0, ep1, normal, length,
            ref chosenPoint, ref segmentProjection);
    }

    public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1,
    Vector3 segmentNormal, float segmentLength,
    ref float segmentProjection)
    {
        var cp = Vector3.zero;
        return PointToSegmentDistance(point, ep0, ep1, segmentNormal, segmentLength,
            ref cp, ref segmentProjection);
    }

    public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1,
        Vector3 segmentNormal, float segmentLength,
        ref Vector3 chosenPoint)
    {
        float sp = 0;
        return PointToSegmentDistance(point, ep0, ep1, segmentNormal, segmentLength,
            ref chosenPoint, ref sp);
    }

    public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1,
        Vector3 segmentNormal, float segmentLength,
        ref Vector3 chosenPoint,
        ref float segmentProjection)
    {
        var local = point - ep0;
        segmentProjection = Vector3.Dot(segmentNormal, local);

        if (segmentProjection < 0)
        {
            chosenPoint = ep0;
            segmentProjection = 0;
            return (point - ep0).magnitude;
        }

        if (segmentProjection > segmentLength)
        {
            chosenPoint = ep1;
            segmentProjection = segmentLength;
            return (point - ep1).magnitude;
        }

        chosenPoint = segmentNormal * segmentProjection;
        chosenPoint += ep0;
        return Vector3.Distance(point, chosenPoint);
    }
    #endregion

    #region 范围检测数据工具类
    /// <summary>
    ///在扇形里
    /// </summary>
    /// <param name="tagetPos">检测目标点</param>
    /// <param name="forward">圆的前方向</param>
    /// <param name="pos">圆心点</param>
    /// <param name="radius">半径</param>
    /// <param name="angle">扇形半角度数</param>
    /// <returns></returns>
    public static bool IsInSector(Vector3 tagetPos, Vector3 forward, Vector3 pos, float radius, float angle)
    {
        tagetPos.y = pos.y;
        float distance = Vector3.Distance(tagetPos, pos);
        if (distance > radius) return false;

        Vector3 v = tagetPos - pos;
        float bHalfAngle = angle * 0.5f;
        float sAngle = Vector3.Angle(forward, v);
        if (sAngle > bHalfAngle) return false;
        return true;
    }

    /// <summary>
    /// 在三角形里
    /// </summary>
    /// <param name="tagetPos">检测目标点</param>
    /// <param name="forward">圆的前方向</param>
    /// <param name="pos">圆心点</param>
    /// <param name="Length">边长</param>
    /// <returns></returns>
    public static bool IsInTriangle(Vector3 tagetPos, Vector3 forward, Vector3 pos, float Length)
    {
        Vector3 cross = Vector3.Cross(forward, Vector3.up).normalized;
        Vector3[] curVector = new Vector3[3];
        curVector[0] = pos + forward * Length * Mathf.Tan(Mathf.PI / 3.0f) * 0.5f;
        curVector[1] = pos + cross * Length * 0.5f;
        curVector[2] = pos - cross * Length * 0.5f;
        return IsInRectangle(tagetPos, curVector);
    }


    private static Vector3[] curVector = new Vector3[4];
    /// <summary>
    /// 在矩形
    /// </summary>
    /// <param name="tagetPos">检测目标点</param>
    /// <param name="forward">圆的前方向</param>
    /// <param name="pos">圆心点</param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <returns></returns>
    public static bool IsInRectangle(Vector3 tagetPos, Vector3 forward, Vector3 pos, float width, float height)
    {
        Vector3 cross = Vector3.Cross(forward, Vector3.forward).normalized;
        curVector[0] = pos - forward * width * 0.5f - cross * height * 0.5f;
        curVector[1] = curVector[0] + forward * width;
        curVector[3] = curVector[0] + cross * height;
        curVector[2] = curVector[0] + forward * width + cross * height;
        return IsInRectangle(tagetPos, curVector);
    }

    public static bool IsInRectangle2(Vector3 tagetPos, Vector3 forward, Vector3 pos, float height, float width)
    {
        Vector3 cross = Vector3.Cross(forward, Vector3.forward).normalized;
        curVector[0] = pos - forward * width * 0.5f - cross * height * 0.5f;
        curVector[1] = curVector[0] + forward * width;
        curVector[3] = curVector[0] + cross * height;
        curVector[2] = curVector[0] + forward * width + cross * height;
        return IsInRectangle(tagetPos, curVector);
    }

    /// <summary> 在矩形范围内 (矩形在自身前方) </summary>
    public static bool IsNewRectangleByForward(Vector3 tagetPos, Vector3 forward, Vector3 pos, float height, float width)
    {
        Vector3 cross = Vector3.Cross(forward, Vector3.up).normalized;
        curVector[0] = pos - cross * width * 0.5f;
        curVector[1] = curVector[0] + cross * width;
        curVector[3] = curVector[0] + forward * height;
        curVector[2] = curVector[0] + cross * width + forward * height;
        return IsInRectangleIgnoreY(tagetPos, curVector);
    }

    /// <summary> 在矩形范围内 (矩形在自身中心) </summary>
    public static bool IsNewRectangleByCenter(Vector3 tagetPos, Vector3 forward, Vector3 pos, float height, float width)
    {
        Vector3 cross = Vector3.Cross(forward, Vector3.up).normalized;
        curVector[0] = pos - cross * width * 0.5f - forward * height * 0.5f;
        curVector[1] = curVector[0] + cross * width;
        curVector[3] = curVector[0] + forward * height;
        curVector[2] = curVector[0] + cross * width + forward * height;
        return IsInRectangleIgnoreY(tagetPos, curVector);
    }

    public static bool IsInRectangleByEdge(Vector3 tagetPos, Vector3 forward, Vector3 pos, float height, float width)
    {
        tagetPos.y = pos.y;
        Vector3 cross = GetVerticalDir(forward);
        curVector[0] = pos + cross * width * 0.5f;
        curVector[1] = pos - cross * width * 0.5f;
        curVector[3] = curVector[0] + forward * height;
        curVector[2] = curVector[1] + forward * height;
        return IsPointInRectangle(tagetPos, curVector);
    }

    public static float Cross(this Vector3 a, Vector3 b)
    {
        return a.x * b.z - b.x * a.z;
    }

    public static bool IsPointInRectangle(Vector3 P, Vector3[] rectCorners)
    {
        return IsPointInRectangle(P, rectCorners[0], rectCorners[1], rectCorners[2], rectCorners[3]);
    }

    public static bool IsPointInRectangle(Vector3 P, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        Vector3 AB = A - B;
        Vector3 AP = A - P;
        Vector3 CD = C - D;
        Vector3 CP = C - P;

        Vector3 DA = D - A;
        Vector3 DP = D - P;
        Vector3 BC = B - C;
        Vector3 BP = B - P;

        bool isBetweenAB_CD = AB.Cross(AP) * CD.Cross(CP) > 0;
        bool isBetweenDA_BC = DA.Cross(DP) * BC.Cross(BP) > 0;
        return isBetweenAB_CD && isBetweenDA_BC;
    }

    /// <summary>
    /// 获取某向量的垂直向量
    /// </summary>
    public static Vector3 GetVerticalDir(Vector3 _dir)
    {
        if (_dir.z == 0)
        {
            return new Vector3(0, 0, -1);
        }
        else
        {
            return new Vector3(-_dir.z / _dir.x, 0, 1).normalized;
        }
    }

    /// <summary>
    /// 判断多边形里
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public static bool IsInRectangle(Vector3 pos, Vector3[] rectangle)
    {
        if (rectangle == null || rectangle.Length == 0) return false;
        /*
        for (int i = 0; i < rectangle.Length; i++)
        {
            Debug.DrawLine(rectangle[i], rectangle[(i + 1) % rectangle.Length], Color.red);
        }*/

        bool inside = false;
        int count = rectangle.Length - 1;
        Vector3 vec_i;
        Vector3 vec_c;

        for (int i = 0; i < rectangle.Length; i++)
        {
            vec_i = rectangle[i];
            vec_c = rectangle[count];
            if ((vec_i.y < pos.y && vec_c.y >= pos.y ||
                vec_c.y < pos.y && vec_i.y >= pos.y) &&
                (vec_i.x <= pos.x || vec_c.x <= pos.x))
            {
                inside ^= (vec_i.x + (pos.y - vec_i.y) / (vec_c.y - vec_i.y) * (vec_c.x - vec_i.x) < pos.x);
            }
            count = i;
        }
        return inside;
    }

    public static bool IsInRectangleIgnoreY(Vector3 pos, Vector3[] rectangle)
    {
        if (rectangle == null || rectangle.Length == 0) return false;

        bool inside = false;
        int count = rectangle.Length - 1;
        Vector3 vec_i;
        Vector3 vec_c;

        for (int i = 0; i < rectangle.Length; i++)
        {
            vec_i = rectangle[i];
            vec_c = rectangle[count];
            if ((vec_i.z < pos.z && vec_c.z >= pos.z ||
                vec_c.z < pos.z && vec_i.z >= pos.z) &&
                (vec_i.x <= pos.x || vec_c.x <= pos.x))
            {
                inside ^= (vec_i.x + (pos.z - vec_i.z) / (vec_c.z - vec_i.z) * (vec_c.x - vec_i.x) < pos.x);
            }
            count = i;
        }
        return inside;
    }

    public static bool IsInProbabilityByRate(float rate)
    {
        float randNum = UnityEngine.Random.Range(0, 1.0f);
        return randNum < rate;
    }

    /// <summary>
    /// 检查是否在概率中
    /// 0 = 不是(false)， 1= 是(true)
    /// </summary>
    /// <param name="rate">0-100的数</param>
    /// <returns></returns>
    public static int CheckRandom(float rate, int randomRate = 1)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());//500-10000
        int randNum = random.Next(0, randomRate);
        //float randNum = UnityEngine.Random.Range(0f, 1.0f * randomRate);
        return randNum <= rate ? 1 : 0;
    }

    public static int CheckRandom(double rate, int randomRate = 1)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
        int randNum = random.Next(0, 100 * randomRate);
        //int randNum = UnityEngine.Random.Range(0, 100 * randomRate);
        return randNum <= rate * randomRate ? 1 : 0;
    }

    /// <summary>
    /// 是否在概率中
    /// </summary>
    /// <param name="rate">参考概率</param>
    /// <returns></returns>
    public static bool IsInProbability(float rate, int seed)
    {
        //System.Random random = new System.Random(seed);
        //float randNum = random.Next(0, 100) * 0.01f;
        float randNum = UnityEngine.Random.Range(0, 101) * 0.01f;
        return randNum <= rate;
    }
    public static bool IsInProbability(float value)
    {
        float randomNum = UnityEngine.Random.Range(0, 101);
        if (randomNum < value)
            return true;
        return false;
    }

    public static bool IsInProbabilitySysSeek(float rate)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
        int randNum = random.Next(0, 101);
        return randNum <= rate;
    }

    public static int GetRandomNum(int count, int seed)
    {
        System.Random random = new System.Random(seed);
        return random.Next(0, count);
    }

    public static int GetRandomNumSysSeek(int count)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
        return random.Next(0, count);
    }

    public static int GetRandomNum(int start, int end, int seed)
    {
        System.Random random = new System.Random(seed);
        return random.Next(start, end + 1);
    }

    public static int GetRandomNumSysSeek(int start, int end)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
        return random.Next(start, end + 1);
    }

    public static int GetRandomNumUnity(int start, int end)
    {
        return UnityEngine.Random.Range(start, end + 1);
    }

    public static int GetRandomByWeight(List<int> list)
    {
        return GetRandomByWeight(list.ToArray());
    }

    public static int GetRandomByWeight(int[] list)
    {
        if (list.IsNullOrNoCount())
            return 0;

        int weight = 0;
        int[] orders = new int[list.Length];
        for (int i = 0; i < list.Length; i++)
        {
            weight += list[i];
            orders[i] = weight;
        }

        int num = GetRandomNum(weight, Guid.NewGuid().GetHashCode());
        int index = 0;

        for (int i = 0; i < orders.Length; i++)
        {
            if (orders[i] == 0)
                continue;

            if (num / orders[i] == 0)
            {
                index = i;
                return index;
            }
        }
        return index;
    }

    /// <summary>
    /// 通过一个值，获得大小区间
    /// </summary>
    /// <param name="steps">升序步近</param>
    /// <param name="value">比对的值</param>
    /// <returns></returns>
    public static int GetIndexByStep(List<int> steps, int value)
    {
        if (steps.IsNullOrNoCount())
            return 0;

        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i] == 0)
            {
                if (value == 0)
                    return i;
                else
                    continue;
            }

            if (value == steps[i])
                return i;

            if (value / steps[i] == 0)
                return i;
        }
        return steps.Count - 1;
    }

    //[10,20] 11
    public static int GetIndexByStep(int[] steps, int value)
    {
        if (steps.IsNullOrNoCount())
            return 0;

        if (steps.Length < 1 && value <= steps[0])
            return 0;

        for (int i = 0; i < steps.Length; i++)
        {
            if (steps[i] == 0)
            {
                if (value == 0)
                    return i;
                else
                    continue;
            }

            if (value == steps[i])
                return i;

            if (value / steps[i] == 0)
                return i;
        }
        return steps.Length - 1;
    }

    /// <summary>
    /// rate 概率百分比
    /// </summary>
    /// <param name="rate"></param>
    /// <returns></returns>
    public static bool RandomInPercent(float rate)
    {
        float randomNum = UnityEngine.Random.Range(0, 100);
        if (randomNum < rate)
            return true;
        return false;
    }

    public static long Clamp(long value, long min, long max)
    {
        return Math.Min(Math.Max(value, min), max);
    }

    public static float Clamp(float value, float min, float max)
    {
        return Math.Min(Math.Max(value, min), max);
    }

    public static ulong Clamp(ulong value, ulong min, ulong max)
    {
        return Math.Min(Math.Max(value, min), max);
    }

    public static int Clamp(int value, int min, int max)
    {
        return Math.Min(Math.Max(value, min), max);
    }


    public static void GetVector(Vector3 Contemt, Vector3 oneNorDir, ref Vector3 oneDir, ref Vector3 twoDir)
    {
        oneDir = Vector3.Dot(Contemt, oneNorDir) * oneNorDir;
        twoDir = Contemt - oneDir;
    }
    #endregion

    public static bool IntToBool(int num) { return num > 0 ? true : false; }

    public static bool StrToTool(string str) { if (string.IsNullOrEmpty(str)) return false; else return str.Equals("1"); }

    public static int BoolToInt(bool num) { return num ? 1 : -1; }

    public static bool IsNullOrEmpty<T>(ref List<T> list) { return list == null || list.Count == 0; }

    public static T StrToEnum<T>(string strNum)
    {
        if (string.IsNullOrEmpty(strNum)) return default(T);
        int i = 0;
        if (int.TryParse(strNum, out i))
            return IntToEnum<T>(i);
        return default(T);
    }

    public static T IntToEnum<T>(int num)
    {
        return (T)Enum.ToObject(typeof(T), num);
    }

    public static T ParseStrToEnum<T>(string enumStr)
    {
        return (T)Enum.Parse(typeof(T), enumStr);
    }

    public static float RandomFloat(this Vector2 vec) { return UnityEngine.Random.Range(vec.x, vec.y); }

    public static float RandomFloat(this Vector2 vec, float cell)
    {
        if (vec.x >= vec.y) return vec.x;
        int num = (int)((vec.y - vec.x) / cell);
        return vec.x + UnityEngine.Random.Range(0, num) * cell;
    }

    public static List<T> GetRandomList<T>(List<T> srcList, int num)
    {
        if (srcList == null || srcList.Count == 0) return null;
        if (srcList.Count <= num) return new List<T>(srcList);
        List<T> curList = new List<T>();
        for (int i = 0; i < num; i++)
        {
            int index = UnityEngine.Random.Range(0, srcList.Count);
            curList.Add(srcList[index]);
            srcList.RemoveAt(index);
        }
        return curList;
    }

    public static int GetWorldLevelData(int rank, string rangeCmd)
    {
        if (rangeCmd.IsNullOrEmpty()) return 0;
        string[] cmdArr = rangeCmd.Split(GameKeyName.Serialsuffix);
        if (cmdArr.Length != 2) return 0;
        int m, n = 0;
        if (!int.TryParse(cmdArr[0], out m)) return 0;
        if (!int.TryParse(cmdArr[1], out n)) return 0;
        return GetWorldLevelData(rank, m, n);
    }

    public static int GetWorldLevelData(int rank, int m, int n)
    {
        return m + rank * n;
    }

    public static Vector3 RemoveDirY(this Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }

    //根据半径随机，忽略Y轴
    public static Vector3 RandomIgnoreY(this Vector3 vec, float distance)
    {
        //float x = UnityEngine.Random.Range(vec.x, vec.x + distance);
        //float y = ignoreY ? vec.y : UnityEngine.Random.Range(vec.y, vec.y + distance);
        //float z = UnityEngine.Random.Range(vec.z, vec.z + distance);
        //return new Vector3(x, y, z);

        //Random.insideUnitCircle 返回半径为1的圆内的一个随机点
        var insPos = UnityEngine.Random.insideUnitCircle * distance;
        float x = insPos.x + vec.x;
        float z = insPos.y + vec.z;
        return new Vector3(x, vec.y, z);
    }

    public static object GetTrueValue(string type, string value)
    {
        string typeTolower = type.ToLower();
        if (typeTolower.Equals("int")) return Int32.Parse(value);
        else if (typeTolower.Equals("uint")) return UInt32.Parse(value);
        else if (typeTolower.Equals("float")) return float.Parse(value);
        else if (typeTolower.Equals("long")) return Int64.Parse(value);
        else if (typeTolower.Equals("ulong")) return UInt64.Parse(value);
        else if (typeTolower.Equals("double")) return Double.Parse(value);
        else if (typeTolower.Equals("vector3")) return StringUtility.StrToVec3(value);
        else return value;
    }

    public static string RectToStr(Rect rect) { return StringUtility.AppendFormat("{0},{1},{2},{3}", rect.x, rect.y, rect.width, rect.height); }


    public static Rect StrToRect(string str)
    {
        if (string.IsNullOrEmpty(str)) return Rect.zero;
        string[] strArr = str.Split(',');
        if (strArr == null || strArr.Length != 4) return Rect.zero;
        return new Rect(float.Parse(strArr[0]),
                        float.Parse(strArr[1]),
                        float.Parse(strArr[2]),
                        float.Parse(strArr[3]));
    }

    public static bool IsIgnoreDistance(Vector3 src, Vector3 dis)
    {
        return Vector3.Distance(src, dis) <= 0.001f;
    }

    public static bool IsWithInDistance(Vector3 src, Vector3 dis, float distance)
    {
        return (src - dis).sqrMagnitude <= distance * distance + 0.001f;
    }

    public static float WithInDistance(Vector3 src, Vector3 dis) { return (src - dis).sqrMagnitude; }

    public static bool IsIgnoreValue(float src, float dis, float standard) { return Math.Abs(src - dis) <= standard; }

    public static bool IsIgnoreValue(float src, float dis) { return IsIgnoreValue(src - dis); }

    public static bool IsIgnoreValue(float distance) { return Math.Abs(distance) <= 0.001f; }

    public static int GetID(int x, int y) { return x << 16 | y; }

    public static long GetHashCode(object x, object y) { return ((long)x.GetHashCode() << 32) + y.GetHashCode(); }

    public static Vector3 GetNormalizeNoY(Vector3 dec) { Vector3 curVec = Vector3.Normalize(dec); return new Vector3(curVec.x, 0, curVec.z); }


    public static bool IsInRectangleXY(Vector3 pos, Vector3[] rectangle)
    {
        if (rectangle == null || rectangle.Length == 0) return false;
        /*
        for (int i = 0; i < rectangle.Length; i++)
        {
            Debug.DrawLine(rectangle[i], rectangle[(i + 1) % rectangle.Length], Color.red);
        }*/

        bool inside = false;
        int count = rectangle.Length - 1;
        Vector3 vec_i;
        Vector3 vec_c;

        for (int i = 0; i < rectangle.Length; i++)
        {
            vec_i = rectangle[i];
            vec_c = rectangle[count];
            if ((vec_i.y < pos.y && vec_c.y >= pos.y ||
                vec_c.y < pos.y && vec_i.y >= pos.y) &&
                (vec_i.x <= pos.x || vec_c.x <= pos.x))
            {
                inside ^= (vec_i.x + (pos.y - vec_i.y) / (vec_c.y - vec_i.y) * (vec_c.x - vec_i.x) < pos.x);
            }
            count = i;
        }
        return inside;
    }


    /// <summary>
    /// 获取b点相对于a点的角度，也就是说a点加上这角度就会指向b点。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float GetAngle(Vector3 a, Vector3 b)
    {
        return GetAngle(a.x, a.z, b.x, b.z);
    }
    public static float GetAngle(float ax, float az, float bx, float bz)
    {
        bx -= ax;
        bz -= az;

        float deltaAngle = 0;
        if (bx == 0 && bz == 0)
        {
            return 0;
        }
        else if (bx > 0 && bz > 0)
        {
            deltaAngle = 0;
        }
        else if (bx > 0 && bz == 0)
        {
            return 90;
        }
        else if (bx > 0 && bz < 0)
        {
            deltaAngle = 180;
        }
        else if (bx == 0 && bz < 0)
        {
            return 180;
        }
        else if (bx < 0 && bz < 0)
        {
            deltaAngle = -180;
        }
        else if (bx < 0 && bz == 0)
        {
            return -90;
        }
        else if (bx < 0 && bz > 0)
        {
            deltaAngle = 0;
        }

        float angle = Mathf.Atan(bx / bz) * Mathf.Rad2Deg + deltaAngle;
        return angle;
    }

    public static Vector3 CheckZeroDir(Vector3 dir)
    {
        return dir == Vector3.zero ? Vector3.forward : dir;
    }

    public static Vector3 GetBoundPos(Bounds bounds, float x, float y, float z)
    {
        if (bounds == null) return Vector3.zero;
        return bounds.center + new Vector3(bounds.size.x * x, bounds.size.y * y, bounds.size.z * z);
    }

    public static Vector3 GetBoundSideCenter(Bounds bounds, BoundSideType type)
    {
        Vector3 vec_0 = Vector3.zero;
        Vector3 vec_1 = Vector3.zero;
        if (type == BoundSideType.Top)
        {
            vec_0 = GetBoundPos(bounds, -0.5f, 0.5f, 0.5f);
            vec_1 = GetBoundPos(bounds, 0.5f, 0.5f, -0.5f);
        }
        else if (type == BoundSideType.Buttom)
        {
            vec_0 = GetBoundPos(bounds, -0.5f, -0.5f, 0.5f);
            vec_1 = GetBoundPos(bounds, 0.5f, -0.5f, -0.5f);
        }
        else if (type == BoundSideType.Left)
        {
            vec_0 = GetBoundPos(bounds, -0.5f, 0.5f, -0.5f);
            vec_1 = GetBoundPos(bounds, -0.5f, -0.5f, 0.5f);
        }
        else if (type == BoundSideType.Forward)
        {
            vec_0 = GetBoundPos(bounds, -0.5f, 0.5f, 0.5f);
            vec_1 = GetBoundPos(bounds, 0.5f, -0.5f, 0.5f);
        }
        else if (type == BoundSideType.Right)
        {
            vec_0 = GetBoundPos(bounds, 0.5f, 0.5f, -0.5f);
            vec_1 = GetBoundPos(bounds, 0.5f, -0.5f, 0.5f);
        }
        else if (type == BoundSideType.Back)
        {
            vec_0 = GetBoundPos(bounds, -0.5f, 0.5f, -0.5f);
            vec_1 = GetBoundPos(bounds, 0.5f, -0.5f, -0.5f);
        }
        return (vec_0 + vec_1) * 0.5f;
    }

    public static Vector3[] GetBoundCorners(Bounds bounds, BoundSideType type)
    {
        Vector3[] vecArr = new Vector3[4];
        if (type == BoundSideType.Top)
        {
            vecArr[0] = GetBoundPos(bounds, -0.5f, 0.5f, 0.5f);
            vecArr[1] = GetBoundPos(bounds, 0.5f, 0.5f, 0.5f);
            vecArr[2] = GetBoundPos(bounds, 0.5f, 0.5f, -0.5f);
            vecArr[3] = GetBoundPos(bounds, -0.5f, 0.5f, -0.5f);
        }
        else if (type == BoundSideType.Buttom)
        {
            vecArr[0] = GetBoundPos(bounds, -0.5f, -0.5f, 0.5f);
            vecArr[1] = GetBoundPos(bounds, 0.5f, -0.5f, 0.5f);
            vecArr[2] = GetBoundPos(bounds, 0.5f, -0.5f, -0.5f);
            vecArr[3] = GetBoundPos(bounds, -0.5f, -0.5f, -0.5f);
        }
        else if (type == BoundSideType.Left)
        {
            vecArr[0] = GetBoundPos(bounds, -0.5f, 0.5f, -0.5f);
            vecArr[1] = GetBoundPos(bounds, -0.5f, 0.5f, 0.5f);
            vecArr[2] = GetBoundPos(bounds, -0.5f, -0.5f, 0.5f);
            vecArr[3] = GetBoundPos(bounds, -0.5f, -0.5f, -0.5f);
        }
        else if (type == BoundSideType.Forward)
        {
            vecArr[0] = GetBoundPos(bounds, -0.5f, 0.5f, 0.5f);
            vecArr[1] = GetBoundPos(bounds, 0.5f, 0.5f, 0.5f);
            vecArr[2] = GetBoundPos(bounds, 0.5f, -0.5f, 0.5f);
            vecArr[3] = GetBoundPos(bounds, -0.5f, -0.5f, 0.5f);
        }
        else if (type == BoundSideType.Right)
        {
            vecArr[0] = GetBoundPos(bounds, 0.5f, 0.5f, -0.5f);
            vecArr[1] = GetBoundPos(bounds, 0.5f, 0.5f, 0.5f);
            vecArr[2] = GetBoundPos(bounds, 0.5f, -0.5f, 0.5f);
            vecArr[3] = GetBoundPos(bounds, 0.5f, -0.5f, -0.5f);
        }
        else if (type == BoundSideType.Back)
        {
            vecArr[0] = GetBoundPos(bounds, -0.5f, 0.5f, -0.5f);
            vecArr[1] = GetBoundPos(bounds, 0.5f, 0.5f, -0.5f);
            vecArr[2] = GetBoundPos(bounds, 0.5f, -0.5f, -0.5f);
            vecArr[3] = GetBoundPos(bounds, -0.5f, -0.5f, -0.5f);
        }
        return vecArr;
    }

    public static Vector3 GetCenterPos(Vector3[] corners)
    {
        if (corners == null || corners.Length <= 0) return Vector3.zero;
        Vector3 totalPos = Vector3.zero;
        for (int i = 0; i < corners.Length; i++) totalPos += corners[i];
        return totalPos / corners.Length;
    }

    /// <summary>
    /// 某个向量围绕某个轴旋转多少度
    /// </summary>
    /// <param name="dir">初始向量,正前方</param>
    /// <param name="axis">围绕哪个轴旋转</param>
    /// <param name="angle">旋转度数</param>
    /// <returns></returns>
    public static Vector3 GetRoundByEuler(Vector3 dir, Vector3 axis, float angle)
    {
        Vector3 newVec = Quaternion.AngleAxis(angle, axis) * dir;
        return newVec;
    }

    /// <summary>
    /// 围绕某点旋转指定角度
    /// </summary>
    /// <param name="position">自身坐标</param>
    /// <param name="center">旋转中心</param>
    /// <param name="axis">围绕旋转轴</param>
    /// <param name="angle">旋转角度</param>
    /// <returns></returns>
    public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
    {
        return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
    }

    public static bool IsSameMathfSign(float sign_0, float sign_1)
    {
        if (sign_0 >= 0 && sign_1 < 0) return false;
        else if (sign_0 < 0 && sign_1 >= 0) return false;
        else return true;
    }


    public static bool IsEquals(this Vector3 v1, Vector3 v2)
    {
        float x = v1.x / v2.x;
        float y = v1.y / v2.y;
        float z = v1.z / v2.z;
        if (x > 0.99f && x <= 1 &&
            y > 0.99f && y <= 1 &&
            z > 0.99f && z <= 1)
        {
            return true;
        }
        return false;
    }

    public static float ToSecond(this int millisecond)
    {
        return millisecond * 0.001f;
    }

    public static float ToPercentage(this int number)
    {
        return number * 0.01f;
    }

    public static float ToMillisecond(this int second)
    {
        return second * 1000f;
    }

    public static float Angle180To360(this float angle)
    {
        if (angle >= 0 && angle <= 180)
            return angle;
        else
            return 360 + angle;
    }

    public static float Float(this double value)
    {
        //return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        return (float)value;
    }
}
