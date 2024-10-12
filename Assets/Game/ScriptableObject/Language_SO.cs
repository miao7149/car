using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Language_SO", menuName = "ScriptableObjects/Language_SO", order = 1)]
public class Language_SO : ScriptableObject {
    [System.Serializable]
    public class String_String_Pair {
        public String_String_Pair(string code, string value) {
            this.code = code;
            this.value = value;
        }

        public string code;
        public string value;
    }

    [System.Serializable]
    public class String__Dictionary_Pair {
        public String__Dictionary_Pair(string key, List<String_String_Pair> values) {
            this.key = key;
            this.values = values;
        }

        public string key;
        public List<String_String_Pair> values;
    }


    [SerializeField] public List<String__Dictionary_Pair> languageDictionary = new();

    public string GetValue(string languageID, string code) {
        foreach (var pair in languageDictionary) {
            if (pair.key == languageID) {
                foreach (var subPair in pair.values) {
                    if (subPair.code == code) {
                        return subPair.value;
                    }
                }
            }
        }

        Debug.LogError("没找到对应语言的翻译:" + languageID + " " + code);

        return null; // Or handle the case where the key is not found
    }
}
