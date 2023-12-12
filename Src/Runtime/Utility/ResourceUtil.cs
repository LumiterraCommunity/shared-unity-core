/*
 * @Author: xiang huan
 * @Date: 2022-05-20 14:20:27
 * @LastEditTime: 2022-07-08 11:19:13
 * @LastEditors: Please set LastEditors
 * @Description: 资源工具类
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Thirdparty/UnityGameFramework/Scripts/Runtime/Utility/ResourceUtil.cs
 * 
 */

using System.IO;
using UnityEngine;
namespace UnityGameFramework.Runtime
{
    public enum eLoadWebAssetType
    {
        Text,       //文本
        Texture,    //图片
        Audio,      //音频
        Movie,      //视频
        Byte,       //二进制数据
    }
    public static class ResourceUtil
    {
        public static eLoadWebAssetType GetWebAssetType(string ext)
        {
            switch (ext)
            {
                case ".json":
                case ".text":
                    return eLoadWebAssetType.Text;
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".bmp":
                case ".gif":
                    return eLoadWebAssetType.Texture;
                case ".mp3":
                case ".wav":
                case ".ogg":
                    return eLoadWebAssetType.Audio;
                default:
                    return eLoadWebAssetType.Byte;
            }
        }

        public static AudioType GetAudioType(string ext)
        {
            switch (ext)
            {
                case ".mp3":
                    return AudioType.MPEG;
                case ".wav":
                    return AudioType.WAV;
                case ".ogg":
                    return AudioType.OGGVORBIS;
                default:
                    return AudioType.UNKNOWN;
            }

        }
    }
}
