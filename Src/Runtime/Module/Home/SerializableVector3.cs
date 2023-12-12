using UnityEngine;

/// <summary>
/// 专门用于序列化Vector3的结构体 只保留两位小数 为了节省序列化存储空间
/// </summary>
[System.Serializable]
public struct SerializableVector3
{
    public float X;
    public float Y;
    public float Z;

    public SerializableVector3(Vector3 vector) : this(vector.x, vector.y, vector.z)
    {
    }

    public SerializableVector3(float x, float y, float z)
    {
        X = Mathf.Round(x * 100) / 100;//保留两位小数
        Y = Mathf.Round(y * 100) / 100;
        Z = Mathf.Round(z * 100) / 100;
    }

    public readonly Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }
}