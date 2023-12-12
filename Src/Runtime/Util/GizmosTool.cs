using UnityEngine;

public static class GizmosTools
{
    /// <summary>
    /// 绘制半圆
    /// </summary>
    public static void DrawWireSemicircle(Vector3 origin, Vector3 direction, float radius, float angle, Color color)
    {
        DrawWireSemicircle(origin, direction, radius, angle, Vector3.up, color);
    }
    public static void DrawWireSemicircle(Vector3 origin, Vector3 direction, float radius, float angle, Vector3 axis, Color color)
    {
        Gizmos.color = color;
        Vector3 leftDir = Quaternion.AngleAxis(-angle / 2, axis) * direction;
        Vector3 rightDir = Quaternion.AngleAxis(angle / 2, axis) * direction;

        Vector3 currentP = origin + (leftDir * radius);
        Vector3 oldP;
        if (angle != 360)
        {
            Gizmos.DrawLine(origin, currentP);
        }
        for (int i = 0; i < angle / 10; i++)
        {
            Vector3 dir = Quaternion.AngleAxis(10 * i, axis) * leftDir;
            oldP = currentP;
            currentP = origin + (dir * radius);
            Gizmos.DrawLine(oldP, currentP);
        }
        oldP = currentP;
        currentP = origin + (rightDir * radius);
        Gizmos.DrawLine(oldP, currentP);
        if (angle != 360)
        {
            Gizmos.DrawLine(currentP, origin);
        }
    }

    // public static Mesh SemicircleMesh(float radius, int angle, Vector3 axis)
    // {
    //     Vector3 leftDir = Quaternion.AngleAxis(-angle / 2, axis) * Vector3.forward;
    //     Vector3 rightDir = Quaternion.AngleAxis(angle / 2, axis) * Vector3.forward;
    //     int pCount = angle / 10;
    //     //顶点
    //     Vector3[] vertexArr = new Vector3[3 + pCount];
    //     vertexArr[0] = Vector3.zero;
    //     int index = 1;
    //     vertexArr[index] = leftDir * radius;
    //     index++;
    //     for (int i = 0; i < pCount; i++)
    //     {
    //         Vector3 dir = Quaternion.AngleAxis(10 * i, axis) * leftDir;
    //         vertexArr[index] = dir * radius;
    //         index++;
    //     }
    //     vertexArr[index] = rightDir * radius;
    //     //三角面
    //     int[] triangles = new int[3 * (1 + pCount)];
    //     for (int i = 0; i < 1 + pCount; i++)
    //     {
    //         triangles[3 * i] = 0;
    //         triangles[3 * i + 1] = i + 1;
    //         triangles[3 * i + 2] = i + 2;
    //     }

    //     Mesh mesh = new Mesh();
    //     mesh.vertices = vertexArr;
    //     mesh.triangles = triangles;
    //     mesh.RecalculateNormals();
    //     return mesh;
    // }
}