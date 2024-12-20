﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI {
    [AddComponentMenu("UI/Extensions/Extensions Toggle Group")]
    [DisallowMultipleComponent]
    public class ExtensionsToggleGroup : UIBehaviour {
        [SerializeField] private bool m_AllowSwitchOff;

        public ToggleGroupEvent onToggleGroupChanged = new();
        public ToggleGroupEvent onToggleGroupToggleChanged = new();

        public List<ExtensionsToggle> m_Toggles = new();


        protected ExtensionsToggleGroup() { }

        public bool AllowSwitchOff {
            get => m_AllowSwitchOff;
            set => m_AllowSwitchOff = value;
        }

        public ExtensionsToggle SelectedToggle { get; private set; }

        private void ValidateToggleIsInGroup(ExtensionsToggle toggle) {
            if (toggle == null || !m_Toggles.Contains(toggle))
                throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[] { toggle, this }));
        }

        public void NotifyToggleOn(ExtensionsToggle toggle) {
            ValidateToggleIsInGroup(toggle);

            // disable all toggles in the group
            for (var i = 0; i < m_Toggles.Count; i++) {
                if (m_Toggles[i] == toggle) {
                    SelectedToggle = toggle;
                    continue;
                }

                m_Toggles[i].IsOn = false;
            }

            onToggleGroupChanged.Invoke(AnyTogglesOn());
        }

        public void QuickNotifyToggleOn(ExtensionsToggle toggle) {
            // disable all toggles in the group
            for (var i = 0; i < m_Toggles.Count; i++) {
                if (m_Toggles[i] == toggle) {
                    SelectedToggle = toggle;
                    continue;
                }

                m_Toggles[i].QuickSet(false);
            }

            onToggleGroupChanged.Invoke(AnyTogglesOn());
        }


        public void UnregisterToggle(ExtensionsToggle toggle) {
            if (m_Toggles.Contains(toggle)) {
                m_Toggles.Remove(toggle);
                toggle.onValueChanged.RemoveListener(NotifyToggleChanged);
            }
        }

        private void NotifyToggleChanged(bool isOn) {
            onToggleGroupToggleChanged.Invoke(isOn);
        }

        public void RegisterToggle(ExtensionsToggle toggle) {
            if (!m_Toggles.Contains(toggle)) {
                m_Toggles.Add(toggle);
                toggle.onValueChanged.AddListener(NotifyToggleChanged);
            }
        }

        public bool AnyTogglesOn() {
            return m_Toggles.Find(x => x.IsOn) != null;
        }

        public IEnumerable<ExtensionsToggle> ActiveToggles() {
            return m_Toggles.Where(x => x.IsOn);
        }

        public void SetAllTogglesOff() {
            var oldAllowSwitchOff = m_AllowSwitchOff;
            m_AllowSwitchOff = true;

            for (var i = 0; i < m_Toggles.Count; i++)
                m_Toggles[i].IsOn = false;

            m_AllowSwitchOff = oldAllowSwitchOff;
        }

        public void HasTheGroupToggle(bool value) {
            Debug.Log("Testing, the group has toggled [" + value + "]");
        }

        public void HasAToggleFlipped(bool value) {
            Debug.Log("Testing, a toggle has toggled [" + value + "]");
        }

        public void SelectToggleByID(int id) {
            if (m_Toggles.Count > id) m_Toggles.Find(x => x.UniqueID == id).QuickSet(true); // = true;
        }

        public int CurIndex() {
            return m_Toggles.Find(x => x.IsOn).UniqueID;
        }

        [Serializable]
        public class ToggleGroupEvent : UnityEvent<bool> { }
    }
}
