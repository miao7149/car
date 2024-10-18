using System.Linq;
using UnityEngine;

namespace MM {
    public class LanguageManager : MonoBehaviour {
        public static LanguageManager Instance;
        public Language_SO language;

        public string languageID;
        string[] languages = { "ChineseSimplified", "English", "Japanese", "Portuguese", "Spanish", "German", "French", "Korean", "Arabic", "Russian" };

        private void Awake() {
            languageID = Application.systemLanguage.ToString();


            if (!languages.Contains(languageID)) {
                languageID = "English";
            }

            if (languageID == "ChineseSimplified") {
                languageID = "Chinese";
            }

            //languageID = "English";

            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        public string GetStringByCode(string code) {
            return language.GetValue(languageID, code);
        }

        public string GetStringByCode(string code, string arg) {
            var a = language.GetValue(languageID, code);
            if (a.Contains("XX"))
                return a.Replace("XX", arg);

            if (a.Contains("xx"))
                return a.Replace("xx", arg);
            return a;
        }
    }
}
