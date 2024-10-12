using System.Collections;
using MM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common {
    public class I18N : MonoBehaviour {
        public string code;

        private void Awake() {
            InitText();
        }

        private void InitText() {
            var label = gameObject.GetComponent<Text>();
            if (label) label.text = LanguageManager.Instance.language.GetValue(LanguageManager.Instance.languageID, code);

            var label1 = gameObject.GetComponent<TMP_Text>();
            if (label1) label1.text = LanguageManager.Instance.language.GetValue(LanguageManager.Instance.languageID, code);
        }
    }
}
