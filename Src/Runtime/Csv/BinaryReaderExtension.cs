/*
 * @Author: xiang huan
 * @Date: 2022-05-20 10:05:57
 * @LastEditTime: 2022-07-06 16:42:01
 * @LastEditors: Please set LastEditors
 * @Description: 
 * @FilePath: /lumiterra-unity/Assets/Src/Csv/BinaryReaderExtension.cs
 * 
 */
using System;
using System.IO;
using UnityEngine;


public static class BinaryReaderExtension
{
    public static Color32 ReadColor32(this BinaryReader binaryReader)
    {
        return new Color32(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
    }

    public static Color ReadColor(this BinaryReader binaryReader)
    {
        return new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
    }

    public static DateTime ReadDateTime(this BinaryReader binaryReader)
    {
        return new DateTime(binaryReader.ReadInt64());
    }

    public static Quaternion ReadQuaternion(this BinaryReader binaryReader)
    {
        return new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
    }

    public static Rect ReadRect(this BinaryReader binaryReader)
    {
        return new Rect(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
    }

    public static Vector2 ReadVector2(this BinaryReader binaryReader)
    {
        return new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
    }

    public static Vector3 ReadVector3(this BinaryReader binaryReader)
    {
        return new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
    }

    public static Vector4 ReadVector4(this BinaryReader binaryReader)
    {
        return new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
    }

    public static T[] ReadArray<T>(this BinaryReader binaryReader)
    {
        string arrayStr = binaryReader.ReadString();
        T[] values = DataTableParseUtil.ParseArray<T>(arrayStr);
        return values;
    }

    public static T[][] ReadArrayList<T>(this BinaryReader binaryReader)
    {
        string arrayStr = binaryReader.ReadString();
        T[][] values = DataTableParseUtil.ParseArrayList<T>(arrayStr);
        return values;
    }
}

