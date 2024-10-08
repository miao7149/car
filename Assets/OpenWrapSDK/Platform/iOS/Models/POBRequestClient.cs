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
using System.Runtime.InteropServices;

namespace OpenWrapSDK.iOS
{
    /// <summary>
    /// iOS request client to set or get parameters on request object.
    /// </summary>
    internal class POBRequestClient : IPOBRequest
    {
        // Reference to iOS's POBRequest
        private readonly IntPtr requestPtr;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">IntPtr of POBRequest</param>
        internal POBRequestClient(IntPtr request)
        {
            requestPtr = request;
        }

        #region
        [DllImport("__Internal")]
        internal static extern void POBUSetVersionId(IntPtr request, int versionId);

        [DllImport("__Internal")]
        internal static extern int POBUGetVersionId(IntPtr request);

        [DllImport("__Internal")]
        internal static extern void POBUSetNetworkTimeout(IntPtr request, int timeout);

        [DllImport("__Internal")]
        internal static extern int POBUGetNetworkTimeout(IntPtr request);

        [DllImport("__Internal")]
        internal static extern void POBUSetAdServerUrl(IntPtr request, string adServerUrl);

        [DllImport("__Internal")]
        internal static extern string POBUGetAdServerUrl(IntPtr request);

        [DllImport("__Internal")]
        internal static extern void POBUSetTestModeEnabled(IntPtr request, bool enable);

        [DllImport("__Internal")]
        internal static extern bool POBUIsTestModeEnabled(IntPtr request);

        [DllImport("__Internal")]
        internal static extern void POBUSetDebugStateEnabled(IntPtr request, bool enable);

        [DllImport("__Internal")]
        internal static extern bool POBUIsDebugStateEnabled(IntPtr request);

        [DllImport("__Internal")]
        internal static extern void POBUSetBidSummaryEnabled(IntPtr request, bool enable);

        [DllImport("__Internal")]
        internal static extern bool POBUIsBidSummaryEnabled(IntPtr request);

        [DllImport("__Internal")]
        internal static extern string POBUGetPubId(IntPtr request);

        [DllImport("__Internal")]
        internal static extern int POBUGetProfileId(IntPtr request);
        #endregion

        /// <summary>
        /// Getter for Publisher id on POBRequest object
        /// </summary>
        /// <returns>Publisher id as string</returns>
        public string GetPublisherId()
        {
            return POBUGetPubId(requestPtr);
        }

        /// <summary>
        /// Getter for Profile id on POBRequest object
        /// </summary>
        /// <returns>Profile id as integer</returns>
        public int GetProfileId()
        {
            return POBUGetProfileId(requestPtr);
        }

        /// <summary>
        /// Method to check if bid summary is enabled.
        /// </summary>
        /// <returns>true/false representing bid summary is enabled or not.</returns>
        public bool IsBidSummaryEnabled()
        {
            return POBUIsBidSummaryEnabled(requestPtr);
        }

        /// <summary>
        /// Setter for bid summary on POBRequest object
        /// </summary>
        /// <param name="enable">bool</param>
        public void EnableBidSummary(bool enable)
        {
            POBUSetBidSummaryEnabled(requestPtr, enable);
        }

        /// <summary>
        /// Method to check if the debug mode is enabled.
        /// </summary>
        /// <returns>true/false representing debug mode is enabled or not.</returns>
        public bool IsDebugStateEnabled()
        {
            return POBUIsDebugStateEnabled(requestPtr);
        }

        /// <summary>
        /// Setter for debug mode on POBRequest object
        /// </summary>
        /// <param name="enable">bool</param>
        public void EnableDebugState(bool enable)
        {
            POBUSetDebugStateEnabled(requestPtr, enable);
        }

        /// <summary>
        /// Method to check if test mode is enabled.
        /// </summary>
        /// <returns>true/false representing test mode is enabled or not.</returns>
        public bool IsTestModeEnabled()
        {
            return POBUIsTestModeEnabled(requestPtr);
        }

        /// <summary>
        /// Setter for test mode on POBRequest object
        /// </summary>
        /// <param name="enable">bool</param>
        public void EnableTestMode(bool enable)
        {
            POBUSetTestModeEnabled(requestPtr, enable);
        }

        /// <summary>
        /// Getter for Ad Server Url on POBRequest object
        /// </summary>
        /// <returns>Ad server url as string</returns>
        public string GetAdServerUrl()
        {
            return POBUGetAdServerUrl(requestPtr);
        }

        /// <summary>
        /// Setter for Ad server url on POBRequest object
        /// </summary>
        /// <param name="url">ad server url as string</param>
        public void SetAdServerUrl(string url)
        {
            POBUSetAdServerUrl(requestPtr, url);
        }

        /// <summary>
        /// Getter for Network timeout on POBRequest object
        /// </summary>
        /// <returns>Network timeout</returns>
        public int GetNetworkTimeout()
        {
            return POBUGetNetworkTimeout(requestPtr);
        }

        /// <summary>
        /// Setter for Network timeout on POBRequest object
        /// </summary>
        /// <param name="timeout">int</param>
        public void SetNetworkTimeout(int timeout)
        {
            POBUSetNetworkTimeout(requestPtr, timeout);
        }

        /// <summary>
        /// Getter for Version id on POBRequest object
        /// </summary>
        /// <returns>Version id</returns>
        public int GetVersionId()
        {
            return POBUGetVersionId(requestPtr);
        }

        /// <summary>
        /// Setter for Version id on POBRequest object
        /// </summary>
        /// <param name="versionId">int</param>
        public void SetVersionId(int versionId)
        {
            POBUSetVersionId(requestPtr, versionId);
        }
    }
}
#endif