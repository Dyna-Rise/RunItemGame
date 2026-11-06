using System;
using System.Collections.Generic;
using UnityEngine;

// 1つの会話データを表すクラス（インスペクターに表示するため [System.Serializable] が必須）
[Serializable]
public class SingleTalk
{ 
    [Header("名前・セリフ・画像")]
    public string name;

    [TextArea(2, 5)] // インスペクター上で複数行入力できるようにする属性（最小2行、最大5行）
    public string message;

    public Sprite faceImage;
}

// 会話データのリストを保持するScriptableObject
[CreateAssetMenu(fileName = "NewTalkData", menuName = "ScriptableObjects/TalkData", order = 1)]
public class TalkData : ScriptableObject
{
    public List<SingleTalk> talks = new List<SingleTalk>();
}