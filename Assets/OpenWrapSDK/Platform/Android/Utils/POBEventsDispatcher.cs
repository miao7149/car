#if UNITY_ANDROID
/*
* PubMatic Inc. ("PubMatic") CONFIDENTIAL
* Unpublished Copyright (c) 2006-2022 PubMatic, All Rights Reserved.
*
* NOTICE:  All information contained herein is, and remains the property of PubMatic. The intellectual and technical concepts contained
* herein are proprietary to PubMatic and may be covered by U.S. and Foreign Patents, patents in process, and are protected by trade secret or copyright law.
* Dissemination of this information or reproduction of this material is strictly forbidden unless prior written permission is obtained
* from PubMatic.  Access to the source code contained herein is hereby forbidden to anyone except current PubMatic employees, managers or contractors who have executed
* Confidentiality and Non-disclosure agreements explicitly covering such access or to such other persons whom are directly authorized by PubMatic to access the source code and are subject to confidentiality and nondisclosure obligations with respect to the source code.
*
* The copyright notice above does not evidence any actual or intended publication or disclosure  of  this source code, which includes
* information that is confidential and/or proprietary, and is a trade secret, of  PubMatic.   ANY REPRODUCTION, MODIFICATION, DISTRIBUTION, PUBLIC  PERFORMANCE,
* OR PUBLIC DISPLAY OF OR THROUGH USE  OF THIS  SOURCE CODE  WITHOUT  THE EXPRESS WRITTEN CONSENT OF PUBMATIC IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE
* LAWS AND INTERNATIONAL TREATIES.  THE RECEIPT OR POSSESSION OF  THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT  MAY DESCRIBE, IN WHOLE OR IN PART.
*/
using System;
using System.Collections.Generic;
using OpenWrapSDK.Common;
using UnityEngine;

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Class for dispatching callback events to main thread
    /// Initialize this class once to start dispatching events on main thread
    /// Use ScheduleInUpdate method to pass the callback events from ad format classes 
    /// </summary>
    internal class POBEventsDispatcher : MonoBehaviour
    {

        private static POBEventsDispatcher instance = null;

        private readonly string TAG = "POBEventDispatcher";

        // Queue For Events
        private static readonly Queue<Action> owEventsQueue = new Queue<Action>();


        /// <summary>
        /// Method to dispatch callback events to main thread
        /// <param name="action">event/action to be dispatched on main thread</param>
        /// </summary>
        internal static void ScheduleInUpdate(Action action)
        {
            lock (owEventsQueue)
            {
                owEventsQueue.Enqueue(action);
            }
        }

        void Update()
        {
            // dispatch events on the main thread when the queue is bigger than 0
            while (owEventsQueue.Count > 0)
            {
                Action owDequedAction = null;
                try
                {
                    lock (owEventsQueue)
                    {
                        owDequedAction = owEventsQueue.Dequeue();
                    }
                    if (owDequedAction != null)
                    {
                        owDequedAction.Invoke();
                    }
                }
                catch (Exception e)
                {
                    POBLog.Info(TAG, e.ToString());
                }
            }
        }

        /// <summary>
        /// Method to Initiize the Event Dispatcher
        /// </summary>
        internal static void Initialize()
        {
            if (IsCreated())
            {
                return;
            }

            // Add an invisible game object to the scene which will call this script
            GameObject owDispatcherObject = new GameObject("POBEventsDispatcher");
            owDispatcherObject.hideFlags = HideFlags.HideAndDontSave;
            //To keep gameObject when new scene loads 
            DontDestroyOnLoad(owDispatcherObject);
            instance = owDispatcherObject.AddComponent<POBEventsDispatcher>();
        }

        /// <summary>
        /// Method to check if eventDispatcher instance is created or not
        /// </summary>
        internal static bool IsCreated()
        {
            return instance != null;
        }

        /// <summary>
        /// Method calls when an active GameObject that contains the script is initialized when a Scene loads
        /// </summary>
        public void Awake()
        {
            // To keep the gameObject when scene loads
            DontDestroyOnLoad(gameObject);
        }
        /// <summary>
        /// Method calls when an gameobject becomes disable
        /// </summary>
        public void OnDisable()
        {
            instance = null;
        }

    }
}
#endif