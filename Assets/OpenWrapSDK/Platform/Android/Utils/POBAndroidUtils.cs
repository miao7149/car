#if UNITY_ANDROID
/*
*PubMatic Inc. ("PubMatic") CONFIDENTIAL
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
using UnityEngine;
using OpenWrapSDK.Common;

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Android specific Util class.
    /// </summary>
    internal static class POBAndroidUtils {

        #region Private variables
        private static readonly string Tag = "POBAndroidUtils";
        #endregion

        /// <summary>
        /// Returs the unity player activity.
        /// </summary>
        /// <returns>Unity player activity</returns>
        internal static AndroidJavaObject getActivity() {
            AndroidJavaClass player = new AndroidJavaClass(POBConstants.UnityPlayerClassName);
            AndroidJavaObject activityContext = player.GetStatic<AndroidJavaObject>("currentActivity");
            return activityContext;
        }

        /// <summary>
        /// Converts native POBError class to POBErrorEventArgs
        /// </summary>
        /// <param name="_error"> native error object</param>
        /// <returns>POBErrorEventArgs instance</returns>
        internal static POBErrorEventArgs ConvertToPOBErrorEventArgs(AndroidJavaObject _error)
        {
            POBErrorEventArgs _errorEventArgs = null;
            if (_error != null)
            {
                _errorEventArgs = new POBErrorEventArgs();
                _errorEventArgs.ErrorCode = _error.Call<int>("getErrorCode");
                _errorEventArgs.Message = _error.Call<string>("getErrorMessage");
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.ConvertToPOBErrorEventArgsLog);
            }
            return _errorEventArgs;
        }

        /// <summary>
        /// Converts native POBRewardClient class to POBRewardEventArgs
        /// </summary>
        /// <param name="reward"></param>
        /// <returns></returns>
        internal static POBRewardEventArgs GetPOBRewardEventArgs(AndroidJavaObject reward)
        {
            POBRewardEventArgs rewardEventArgs = null;
            if(reward != null)
            {
                POBRewardClient rewardClient = new POBRewardClient(reward);
                rewardEventArgs = new POBRewardEventArgs(rewardClient);
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.GetPOBRewardEventArgsLog);
            }
            return rewardEventArgs;
        }

        /// <summary>
        /// Converts C# POBAdSize class to native POBAdSize
        /// </summary>
        /// <param name="adSize">ad size</param>
        /// <returns>native POBAdSize</returns>
        internal static AndroidJavaObject ConvertToPOBAdSize(POBAdSize adSize)
        {
            if(adSize != null)
            {
                return new AndroidJavaObject(POBConstants.POBBannerAdSizeClassName, adSize.GetWidth(), adSize.GetHeight());
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.ConvertToPOBAdSizeLog);
                return null;
            }
        }

        /// <summary>
        /// Converts POBGender to respective JAVA enum instance
        /// </summary>
        /// <param name="gender"> gender instance</param>
        /// <returns></returns>
        internal static AndroidJavaObject ConvertGenderToJavaObject(POBGender gender)
        {
            AndroidJavaClass POBGenderEnum = new AndroidJavaClass(POBConstants.POBGenderClassName);
            AndroidJavaObject enumObject = null;
            switch(gender)
            {
                case POBGender.Male:
                    enumObject = POBGenderEnum.GetStatic<AndroidJavaObject>("MALE");
                    break;
                case POBGender.Female:
                    enumObject = POBGenderEnum.GetStatic<AndroidJavaObject>("FEMALE");
                    break;
                case POBGender.Other:
                    enumObject = POBGenderEnum.GetStatic<AndroidJavaObject>("OTHER");
                    break;
                default:
                    POBLog.Info(Tag, POBLogStrings.ConvertGenderToJavaObjectLog);
                    break;
            }
            return enumObject;
        }

        /// <summary>
        /// Converts JAVA enum instance to respective POBGender enum.
        /// </summary>
        /// <param name="javaObject">JAVA enum instance</param>
        /// <returns>POBGender type</returns>
        internal static POBGender GetPOBGender(AndroidJavaObject javaObject)
        {
            POBGender gender;
            if(javaObject != null)
            {
                string value = javaObject.Call<string>("getValue");
                switch (value)
                {
                    case "M":
                        gender = POBGender.Male;
                        break;
                    case "F":
                        gender = POBGender.Female;
                        break;
                    case "O":
                        gender = POBGender.Other;
                        break;
                    default:
                        gender = POBGender.None;
                        break;
                }
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.GetPOBGenderLog);
                return POBGender.None;
            }
            return gender;
        }

        /// <summary>
        /// Converts POBLocSource to respective JAVA enum instance
        /// </summary>
        /// <param name="locSource"> location source instance</param>
        /// <returns>POBLocSource type</returns>
        internal static AndroidJavaObject ConvertLocationSourceToJavaObject(POBLocSource locSource)
        {
            AndroidJavaClass POBLocSourceEnum = new AndroidJavaClass(POBConstants.POBLocationSourceClassName);
            AndroidJavaObject enumObject;
            switch (locSource)
            {
                case POBLocSource.GPS:
                    enumObject = POBLocSourceEnum.GetStatic<AndroidJavaObject>("GPS");
                    break;
                case POBLocSource.IPAddress:
                    enumObject = POBLocSourceEnum.GetStatic<AndroidJavaObject>("IP_ADDRESS");
                    break;
                case POBLocSource.UserProvided:
                    enumObject = POBLocSourceEnum.GetStatic<AndroidJavaObject>("USER");
                    break;
                default:
                    enumObject = null;
                    break;
            }
            return enumObject;
        }

        /// <summary>
        /// Converts POBSDKLogLevel to respective JAVA enum instance
        /// </summary>
        /// <param name="logLevel"> log level instance</param>
        /// <returns>POBSDKLogLevel type</returns>
        internal static AndroidJavaObject ConvertLogLevelToJavaObject(POBSDKLogLevel logLevel)
        {
            AndroidJavaClass POBLogLevelEnum = new AndroidJavaClass(POBConstants.POBLogLevelClassName);
            AndroidJavaObject enumObject;
            switch (logLevel)
            {
                case POBSDKLogLevel.All:
                    enumObject = POBLogLevelEnum.GetStatic<AndroidJavaObject>("All");
                    break;
                case POBSDKLogLevel.Verbose:
                    enumObject = POBLogLevelEnum.GetStatic<AndroidJavaObject>("Verbose");
                    break;
                case POBSDKLogLevel.Debug:
                    enumObject = POBLogLevelEnum.GetStatic<AndroidJavaObject>("Debug");
                    break;
                case POBSDKLogLevel.Info:
                    enumObject = POBLogLevelEnum.GetStatic<AndroidJavaObject>("Info");
                    break;
                case POBSDKLogLevel.Warning:
                    enumObject = POBLogLevelEnum.GetStatic<AndroidJavaObject>("Warn");
                    break;
                case POBSDKLogLevel.Error:
                    enumObject = POBLogLevelEnum.GetStatic<AndroidJavaObject>("Error");
                    break;
                case POBSDKLogLevel.Off:
                    enumObject = POBLogLevelEnum.GetStatic<AndroidJavaObject>("Off");
                    break;
                default:
                    enumObject = null;
                    break;
            }
            return enumObject;
        }

        /// <summary>
        /// Generic method to convert c# dictionary to java map
        /// </summary>
        /// <param name="dict"> instance of dictionary</param>
        /// <returns>AndroidJavaObject of type map</returns>
        internal static AndroidJavaObject ConvertDictionaryToJavaMap(Dictionary<string, AndroidJavaObject> dictionary)
        {

            AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");

            //Get the map's put method using reflection
            IntPtr putMethod = AndroidJNIHelper.GetMethodID(map.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

            //iterate the dictionary and put every entry in java hash map
            foreach (var entry in dictionary)
            {
                AndroidJNI.CallObjectMethod(
                    map.GetRawObject(),
                    putMethod,
                    AndroidJNIHelper.CreateJNIArgArray(new object[] { entry.Key, entry.Value })
                );
            }
            return map;
        }

        /// <summary>
        /// Generic method to convert c# list to java list
        /// </summary>
        /// <param name="list"> instance of List</param>
        /// <returns>AndroidJavaObject of type arrayList</returns>
        internal static AndroidJavaObject ConvertListToJavaList<T>(List<T> list)
        {
            AndroidJavaObject javaList = new AndroidJavaObject("java.util.ArrayList");
            if(list != null)
            {
                //Iterate the list and add each entry of c# list to java arrayList
                foreach (T obj in list)
                {
                    javaList.Call<bool>("add", obj);
                }
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.ConvertListToJavaListLog);
            }
            return javaList;
        }

        /// <summary>
        ///  Method to convert java map object to Dictionary
        /// </summary>
        /// <param name="map"> AndroidJavaObject of map</param>
        /// <returns>Dictionary of given map</returns>
        internal static Dictionary<K, V> ConvertJavaMapToDictionary<K, V>(AndroidJavaObject map)
        {
            var dict = new Dictionary<K, V>();
            if (map != null)
            {
                var iterator = map.Call<AndroidJavaObject>("keySet").Call<AndroidJavaObject>("iterator");
                while (iterator.Call<bool>("hasNext"))
                {
                    K key = iterator.Call<K>("next");
                    V value = map.Call<V>("get", key);
                    dict.Add(key, value);
                }
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.ConvertJavaMapToDictionaryLog);
            }
            return dict;
        }

        /// <summary>
        ///  Method to convert native adPosition object to POBAdPosition
        /// </summary>
        /// <param name="value"> value of adPosition enum</param>
        /// <returns>instance of POBAdPosition</returns>
        internal static POBAdPosition convertJavaAdPostionToPOBAdPosition(int value)
        {
            POBAdPosition adPosition = POBAdPosition.Unknown;

            switch (value)
            {
                case 1:
                    adPosition = POBAdPosition.AboveTheFold;
                    break;
                case 3:
                    adPosition = POBAdPosition.BelowTheFold;
                    break;
                case 4:
                    adPosition = POBAdPosition.Footer;
                    break;
                case 5:
                    adPosition = POBAdPosition.Fullscreen;
                    break;
                case 6:
                    adPosition = POBAdPosition.Header;
                    break;
                case 7:
                    adPosition = POBAdPosition.Sidebar;
                    break;
                default:
                    adPosition = POBAdPosition.Unknown;
                    break;

            }

            return adPosition;

        }

        /// <summary>
        ///  Method to convert  POBAdPosition object to native adPosition instance
        /// </summary>
        /// <param name="value"> instance of POBAdPosition</param>
        /// <returns>string representation of AdPosition</returns>
        internal static string convertPOBAdPostionToAdPosition(POBAdPosition adPosition)
        {
            string adPositionString;
            switch ((int)adPosition)
            { 
                case 1:
                    adPositionString = "ABOVE_THE_FOLD";
                    break;
                case 3:
                    adPositionString = "BELOW_THE_FOLD";
                    break;
                case 4:
                    adPositionString = "HEADER";
                    break;
                case 5:
                    adPositionString = "FOOTER";
                    break;
                case 6:
                    adPositionString = "SIDEBAR";
                    break;
                case 7:
                    adPositionString = "FULL_SCREEN";
                    break;
                default:
                    adPositionString = "UNKNOWN";
                    break;
            }

            return adPositionString;

        }

        /// <summary>
        /// Converts c# dictionary<string,string> to java JSONObject
        /// </summary>
        /// <param name="dictionary"> instance of dictionary </param>
        /// <returns>AndroidJavaObject of type JSONObject</returns>
        internal static AndroidJavaObject ConvertDictionaryToJavaJSONObject(Dictionary<string, string> dictionary)
        {
            AndroidJavaObject extensionJsonObject = null;

            if(dictionary != null && dictionary.Count > 0)
            {
                // Initialise the AndroidJavaObject only when dictionary has some values in it.
                extensionJsonObject = new AndroidJavaObject("org.json.JSONObject");
                //Get the JSONObjects's put method using reflection
                IntPtr putMethod = AndroidJNIHelper.GetMethodID(extensionJsonObject.GetRawClass(), "put", "(Ljava/lang/String;Ljava/lang/Object;)Lorg.json/JSONObject;");

                //iterate the dictionary and put every entry in java Json object

                foreach (KeyValuePair<string, string> entry in dictionary)
                {
                    AndroidJNI.CallObjectMethod(
                        extensionJsonObject.GetRawObject(),
                        putMethod,
                        AndroidJNIHelper.CreateJNIArgArray(new object[] { entry.Key, entry.Value })
                    );
                }
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.ConvertDictionaryToJavaJSONObjectLog);
            }
            return extensionJsonObject;
        }

        /// <summary>
        /// Converts java JSONObject to c# dictionary<string,string> 
        /// </summary>
        /// <param name="androidJavaObject"> instance of AndroidJavaObject of java JSONObject </param>
        /// <returns>AndroidJavaObject of type JSONObject</returns>
        internal static Dictionary<string, string> ConvertJavaJSONObjectToDictionary(AndroidJavaObject androidJavaObject)
        {
            Dictionary<string, string> extensionMap = null;
            if (androidJavaObject != null && androidJavaObject.Call<int>("length") > 0)
            {
                // Initialise the dictionary only when Java JSONObject has some values in it.
                extensionMap = new Dictionary<string, string>();
                // Iterate JSONObject and put its key and values to Dictionary of <string, string>
                AndroidJavaObject iteratorObject = androidJavaObject.Call<AndroidJavaObject>("keys");
                while (iteratorObject.Call<bool>("hasNext"))
                {
                    string key = iteratorObject.Call<string>("next");
                    string value = androidJavaObject.Call<string>("getString", key);
                    extensionMap.Add(key, value);
                }
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.ConvertJavaJSONObjectToDictionaryLog);
            }
            return extensionMap;
        }
    }
}
#endif
