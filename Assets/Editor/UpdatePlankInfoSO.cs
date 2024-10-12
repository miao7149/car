using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class UpdatePlankInfoSO {
    [MenuItem("Assets/Fill Existing GameSettings")]
    // public static void FillExistingSettings() {
    //     // 获取选中的对象
    //     ImageResoure_SO config = Selection.activeObject as ImageResoure_SO;
    //
    //
    //     if (config == null) {
    //         Debug.LogError("请选中一个 GameSettings 资产！");
    //         return;
    //     }
    //
    //     int count = 163;
    //
    //     PlankImageInfo[] plankSprites = new PlankImageInfo[count];
    //
    //     for (int i = 1; i <= count; i++) {
    //         plankSprites[i - 1] = new PlankImageInfo();
    //         Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Game/Texture/{i}.png");
    //
    //         if (sprite == null) {
    //             Debug.LogError("未能找到指定的 Sprite 资源。请检查路径。");
    //             return;
    //         }
    //
    //         plankSprites[i - 1].colliderSprites = sprite;
    //
    //         sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Game/Texture/frames_plate_frame_{i}.png");
    //
    //         if (sprite == null) {
    //             Debug.LogError("未能找到指定的 Sprite 资源。请检查路径。");
    //             return;
    //         }
    //
    //         plankSprites[i - 1].plankSprites = sprite;
    //         sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Game/Texture/plates_plate_{i}.png");
    //
    //         if (sprite == null) {
    //             Debug.LogError("未能找到指定的 Sprite 资源。请检查路径。");
    //             return;
    //         }
    //
    //         plankSprites[i - 1].colorSprites = sprite;
    //     }
    //
    //     config.plankSprites = plankSprites;
    //
    //
    //     // 标记资产为已修改
    //     EditorUtility.SetDirty(config);
    //
    //     // 保存资产更改
    //     AssetDatabase.SaveAssets();
    //
    //     Debug.Log("修改成功！");
    // }

    // [MenuItem("Assets/关卡顺序")]
    // public static void ExportLevelOrder() {
    //     // 获取选中的对象
    //     TextAsset textAsset = Selection.activeObject as TextAsset;
    //
    //     if (textAsset == null) {
    //         Debug.LogError("请选中一个语言文件！");
    //         return;
    //     }
    //
    //     LevelOrder_SO levelOrder = AssetDatabase.LoadAssetAtPath<LevelOrder_SO>("Assets/Game/ScriptableObject/LevelOrder.asset");
    //
    //     if (levelOrder == null) {
    //         Debug.LogError("未能找到 Language 资产！");
    //         return;
    //     }
    //
    //     levelOrder.levelIDs.Clear();
    //
    //     var lines = Regex.Split(textAsset.text, LINE_SPLIT_RE);
    //     string[] languageKeys = new string[0];
    //     List<List<string>> data = new List<List<string>>();
    //     for (var i = 0; i < lines.Length; i++) {
    //         if (lines[i] == "") break;
    //         var values = int.Parse(lines[i]);
    //         levelOrder.levelIDs.Add(values);
    //     }
    //
    //
    //     // 标记资产为已修改
    //     EditorUtility.SetDirty(levelOrder);
    //
    //     // 保存资产更改
    //     AssetDatabase.SaveAssets();
    //
    //     Debug.Log("导出LevelOrder成功！");
    // }
    [MenuItem("Assets/Export language")]
    public static void ExportLanguage() {
        // 获取选中的对象
        TextAsset textAsset = Selection.activeObject as TextAsset;

        if (textAsset == null) {
            Debug.LogError("请选中一个语言文件！");
            return;
        }

        Language_SO language = AssetDatabase.LoadAssetAtPath<Language_SO>("Assets/Game/ScriptableObject/Language.asset");

        if (language == null) {
            Debug.LogError("未能找到 Language 资产！");
            return;
        }

        language.languageDictionary.Clear();

        // 读取语言文件内容
        var lines = Regex.Split(textAsset.text, LINE_SPLIT_RE);
        string[] languageKeys = new string[0];
        List<List<string>> data = new List<List<string>>();
        for (var i = 0; i < lines.Length; i++) {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            if (i == 0) {
                languageKeys = new string[values.Length - 1];
                for (int j = 1; j < values.Length; j++) {
                    languageKeys[j - 1] = values[j];
                }

                //language.languageDictionary.Add(new Language_SO.String__Dictionary_Pair());
            }
            else {
                data.Add(new List<string>());
                for (int j = 0; j < values.Length; j++) {
                    data[i - 1].Add(values[j]);
                }
            }
        }

        Debug.Log(data.Count);

        // 填充语言数据
        for (int i = 0; i < languageKeys.Length; i++) {
            var list = new List<Language_SO.String_String_Pair>();


            for (int j = 0; j < data.Count; j++) {
                list.Add(new Language_SO.String_String_Pair(data[j][0], data[j][i + 1]));
            }

            language.languageDictionary.Add(new Language_SO.String__Dictionary_Pair(languageKeys[i], list));
        }


        //language.language = textAsset.text;s

        // 标记资产为已修改
        EditorUtility.SetDirty(language);

        // 保存资产更改
        AssetDatabase.SaveAssets();

        Debug.Log("导出语言成功！");
    }

    private static readonly string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    //读取csv字符串
    public static void ReadLanguage(string content) {
        //var csv = new CsvObject();

        var lines = Regex.Split(content, LINE_SPLIT_RE);

        for (var i = 0; i < lines.Length; i++) {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;


            //var arr = new string[values.Length - 1];
            //Array.Copy(values, 1, arr, 0, values.Length - 1);
            // csv.AddDictionary(values[0], arr);
        }

        // Application.systemLanguage = SystemLanguage.English;

        //return csv;
    }


    // [MenuItem("Assets/测试坐标转换")]
    // // 计算旋转后的点
    // public static void RotatePoint() {
    //     var a = RotatePoint(1.6f, new Vector2(119, 218), 135, new Vector2(153, 155));
    //     Debug.Log(a);
    // }
    //
    // // 计算旋转后的点，角度以度为单位
    // public static Vector2 RotatePoint(float a, Vector2 point, float angleInDegrees, Vector2 parentSize) {
    //     //angleInDegrees = angleInDegrees;
    //     point = point / a;
    //
    //     point = point - parentSize / 2;
    //
    //     // 将角度转换为弧度
    //     float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
    //
    //     // 计算余弦和正弦值
    //     float cosAngle = Mathf.Cos(angleInRadians);
    //     float sinAngle = Mathf.Sin(angleInRadians);
    //
    //     // 使用旋转矩阵公式计算新的坐标
    //     float x = point.x * cosAngle - point.y * sinAngle;
    //     float y = point.x * sinAngle + point.y * cosAngle;
    //
    //     return new Vector2(x, y) + parentSize / 2;
    // }
}
