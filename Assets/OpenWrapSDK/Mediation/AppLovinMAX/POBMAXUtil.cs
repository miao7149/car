#if UNITY_IOS || UNITY_ANDROID
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
#if UNITY_IOS
using System.Runtime.InteropServices;
#else
using UnityEngine;
#endif

namespace OpenWrapSDK.Mediation.AppLovinMAX
{
    /// <summary>
    /// Class for the data type conversion for passing local extras values to AppLovin MAX
    /// </summary>
    public static class POBMAXUtil
    {
#if UNITY_IOS
        #region Plugin methods
        [DllImport("__Internal")]
        internal static extern IntPtr POBCreateStringIntPtr(string adUnit, string value);

        [DllImport("__Internal")]
        internal static extern IntPtr POBCreateIntegerIntPtr(string adUnit, int value);

        [DllImport("__Internal")]
        internal static extern IntPtr POBCreateBoolIntPtr(string adUnit, bool value);

        [DllImport("__Internal")]
        internal static extern void POBClearLocalExtrasForAdUnit(string adUnit);
        #endregion

        #region Public methods
        /// <summary>
        /// Method to get the IntPtr from any generic object
        /// </summary>
        /// <param name="adUnit">AppLovin MAX ad unit</param>
        /// <param name="obj">Generic Object</param>
        /// <returns>IntPtr of the object</returns>
        public static IntPtr GetIntPtr(string adUnit, object obj)
        {
            if (adUnit != null && obj != null)
            {
                if (obj.GetType() == typeof(string))
                {
                    return POBCreateStringIntPtr(adUnit, (string)obj);
                }
                if (obj.GetType() == typeof(int))
                {
                    return POBCreateIntegerIntPtr(adUnit, (int)obj);
                }
                if (obj.GetType() == typeof(bool))
                {
                    return POBCreateBoolIntPtr(adUnit, (bool)obj);
                }
            }
            
            return IntPtr.Zero;
        }

        /// <summary>
        /// To retain the IntPtr in memory, they are saved in a local data structure against the ad unit. It needs to be cleaned up while destroying the ads.
        /// </summary>
        /// <param name="adUnit">AppLovin MAX ad unit</param>
        public static void ClearLocalExtrasForAdUnit(string adUnit)
        {
            if (adUnit != null)
            {
                POBClearLocalExtrasForAdUnit(adUnit);
            }
        }
        #endregion

#else
        /// <summary>
        /// Method to get AndroidJavaObject from any generic object
        /// </summary>
        /// <param name="adUnit">AppLovin MAX ad unit</param>
        /// <param name="obj">Generic Object</param>
        /// <returns>AndroidJavaObject of the given object</returns>
        public static AndroidJavaObject GetAndroidJavaObject(object obj)
        {
            if (obj.GetType() == typeof(string))
            {
                return new AndroidJavaObject("java.lang.String", obj);
            }
            if (obj.GetType() == typeof(int))
            {
                return new AndroidJavaObject("java.lang.Integer", Convert.ToInt32(obj));
            }
            if (obj.GetType() == typeof(bool))
            {
                return new AndroidJavaObject("java.lang.Boolean", Convert.ToBoolean(obj));
            }
            return null;
        }
#endif
    }
}
#endif