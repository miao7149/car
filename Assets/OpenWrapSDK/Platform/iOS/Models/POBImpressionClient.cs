#if UNITY_IOS
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
using System.Runtime.InteropServices;
using OpenWrapSDK.Common;

namespace OpenWrapSDK.iOS
{
    /// <summary>
    /// iOS impression client to set or get parameters on impression object.
    /// </summary>
    internal class POBImpressionClient : IPOBImpression
    {
        // Reference to iOS's POBImpression
        private readonly IntPtr impressionPtr;
        private readonly string Tag = "POBImpressionClient";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="impression">IntPtr of POBImpression</param>
        internal POBImpressionClient(IntPtr impression)
        {
            impressionPtr = impression;
        }

        #region
        [DllImport("__Internal")]
        internal static extern void POBUSetZoneId(IntPtr impression, string zoneid);

        [DllImport("__Internal")]
        internal static extern string POBUGetZoneId(IntPtr impression);

        [DllImport("__Internal")]
        internal static extern void POBUSetTestCreativeId(IntPtr impression, string creativeid);

        [DllImport("__Internal")]
        internal static extern string POBUGetTestCreativeId(IntPtr impression);

        [DllImport("__Internal")]
        internal static extern void POBUSetCustomParams(IntPtr impression, string key, string[] valuesList, int valuesCount);

        [DllImport("__Internal")]
        internal static extern void POBUSetAdPosition(IntPtr impression, int position);

        [DllImport("__Internal")]
        internal static extern int POBUGetAdPosition(IntPtr impression);

        [DllImport("__Internal")]
        internal static extern string POBUGetAdUnitId(IntPtr request);
        #endregion

        /// <summary>
        /// Getter for Ad unit id on POBImpression object
        /// </summary>
        /// <returns> Ad unit id as string</returns>
        public string GetAdUnitId()
        {
            return POBUGetAdUnitId(impressionPtr);
        }

        /// <summary>
        /// Getter for Ad Position on POBImpression object
        /// </summary>
        /// <returns>POBAdPosition</returns>
        public POBAdPosition GetAdPosition()
        {
            return (POBAdPosition)POBUGetAdPosition(impressionPtr);
        }

        /// <summary>
        /// Setter for ad position on POBImpression object
        /// </summary>
        /// <param name="position">Instance of POBAdPosition</param>
        public void SetAdPosition(POBAdPosition position)
        {
            POBUSetAdPosition(impressionPtr, (int)position);
        }

        /// <summary>
        /// Setter for Custom parameters on POBImpression object
        /// </summary>
        /// <param name="position">Instance of POBAdPosition</param>
        public void SetCustomParams(Dictionary<string, List<string>> customParams)
        {
            foreach (KeyValuePair<string, List<string>> keyValuePair in customParams)
            {
                List<string> valueList = keyValuePair.Value;
                POBLog.Info(Tag, POBLogStrings.SetCustomParams);
                POBUSetCustomParams(impressionPtr, keyValuePair.Key, valueList.ToArray(), valueList.Count);
            }
        }

        /// <summary>
        /// Getter for test creative id
        /// </summary>
        /// <returns>Test creative Id string</returns>
        public string GetTestCreativeId()
        {
            return POBUGetTestCreativeId(impressionPtr);
        }

        /// <summary>
        /// Setter for Test creative id on POBImpression object
        /// </summary>
        /// <param name="creativeId">Instance of string</param>
        public void SetTestCreativeId(string creativeId)
        {
            POBUSetTestCreativeId(impressionPtr, creativeId);
        }

        /// <summary>
        /// Returns the zone id required for reporting purpose.
        /// </summary>
        /// <returns>zone id</returns>
        public string GetZoneId()
        {
            return POBUGetZoneId(impressionPtr);
        }

        /// <summary>
        /// Setter for Zone Id on POBImpression object
        /// </summary>
        /// <param name="zoneId">Instance of string</param>
        public void SetZoneId(string zoneId)
        {
            POBUSetZoneId(impressionPtr, zoneId);
        }
    }
}
#endif