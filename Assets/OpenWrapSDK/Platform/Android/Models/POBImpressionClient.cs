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

using System.Collections.Generic;
using UnityEngine;

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Android impression client to set or get parameters on impression object.
    /// </summary>
    internal class POBImpressionClient : IPOBImpression
    {
        // Reference to Android's POBImpression
        private readonly AndroidJavaObject ImpressionObject;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="impression">AndroidJavaObject of POBImpression</param>
        internal POBImpressionClient(AndroidJavaObject javaObject)
        {
            ImpressionObject = javaObject;
        }

        /// <summary>
        /// Getter for Ad Position on POBImpression object
        /// </summary>
        /// <returns>POBAdPosition</returns>
        public POBAdPosition GetAdPosition()
        {

            var adPosition = ImpressionObject.Call<AndroidJavaObject>("getAdPosition");
            int adPositionValue = adPosition.Call<int>("getValue");

            return POBAndroidUtils.convertJavaAdPostionToPOBAdPosition(adPositionValue);
        }

        /// <summary>
        /// Getter for test creative id on POBImpression object
        /// </summary>
        /// <returns>test creative id as string</returns>
        public string GetTestCreativeId()
        {
            return ImpressionObject.Call<string>("getTestCreativeId");
        }

        /// <summary>
        /// Getter for zone id on POBImpression object
        /// </summary>
        /// <returns>zone id as string</returns>
        public string GetZoneId()
        {
            return ImpressionObject.Call<string>("getPMZoneId");
        }

        /// <summary>
        /// Setter for ad position on POBImpression object
        /// </summary>
        /// <param name="position">Instance of POBAdPosition</param>
        public void SetAdPosition(POBAdPosition position)
        {
            //Convert a POBAdPosition to native POBAdPosition
            AndroidJavaClass adPositionClass = new AndroidJavaClass("com.pubmatic.sdk.openwrap.core.POBRequest$AdPosition");
            AndroidJavaObject adPosition = adPositionClass.GetStatic<AndroidJavaObject>(POBAndroidUtils.convertPOBAdPostionToAdPosition(position));

            //set the adpostion to impression objet
            ImpressionObject.Call("setAdPosition", adPosition);
        }

        /// <summary>
        /// Setter for Custom parameters on POBImpression object
        /// </summary>
        /// <param name="position">Instance of POBAdPosition</param>
        public void SetCustomParams(Dictionary<string, List<string>> customParams)
        {
            //Convert a value lists to AndroidJavaObject of List
            Dictionary<string, AndroidJavaObject> customParamDict = new Dictionary<string, AndroidJavaObject>();
            foreach (KeyValuePair<string, List<string>> entry in customParams)
            {
                customParamDict.Add(entry.Key, POBAndroidUtils.ConvertListToJavaList(entry.Value));
            }

            //Convert c# dictionary to java hashMap
            AndroidJavaObject customParamMap = POBAndroidUtils.ConvertDictionaryToJavaMap(customParamDict);

            ImpressionObject.Call("setCustomParam", customParamMap);
        }

        /// <summary>
        /// Setter for Test creative id on POBImpression object
        /// </summary>
        /// <param name="creativeId">Instance of string</param>
        public void SetTestCreativeId(string creativeId)
        {
            ImpressionObject.Call("setTestCreativeId", creativeId);
        }

        /// <summary>
        /// Setter for Zone Id on POBImpression object
        /// </summary>
        /// <param name="zoneId">Instance of string</param>
        public void SetZoneId(string zoneId)
        {
            ImpressionObject.Call("setPMZoneId", zoneId);
        }

        /// <summary>
        /// Getter for Ad unit id on POBImpression object
        /// </summary>
        /// <returns> Ad unit id as string</returns>
        public string GetAdUnitId()
        {
            return ImpressionObject.Call<string>("getAdUnitId");
        }
    }
}
#endif