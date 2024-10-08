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

namespace OpenWrapSDK
{
    public interface IPOBRequest
    {
        /// <summary>
        /// Disables bid summary that is sent in the response, if true.
        /// Default value is true.
        /// </summary>
        /// <param name="enable">true if summary in response is expected</param>
        void EnableBidSummary(bool enable);

        /// <summary>
        /// True if summary in response is enabled else return false
        /// </summary>
        /// <returns>summary state</returns>
        bool IsBidSummaryEnabled();

        /// <summary>
        /// Returns the network timeout (in seconds) for making an Ad request.
        /// </summary>
        /// <returns>network timeout (in seconds).</returns>
        int GetNetworkTimeout();

        /// <summary>
        /// The network timeout (in seconds) for making an Ad request.
        /// </summary>
        /// <param name="timeout">value in seconds</param>
        void SetNetworkTimeout(int timeout);

        /// <summary>
        /// OpenWrap version Id
        /// </summary>
        /// <returns>version Id</returns>
        int GetVersionId();

        /// <summary>
        /// This is an optional OpenWrap version Id of the publisher.
        /// If this is not specified, live version of the above profile is considered.
        /// </summary>
        /// <param name="versionId">OpenWrap version Id</param>
        void SetVersionId(int versionId);

        /// <summary>
        /// Enable/Disable debug information in the response.
        /// By default, debug is disabled and no debug information will be available in bid response.
        /// </summary>
        /// <param name="enable">debug state</param>
        void EnableDebugState(bool enable);

        /// <summary>
        /// Returns the debug state. False means no debug information will be available in bid response.
        /// </summary>
        /// <returns>debug state</returns>
        bool IsDebugStateEnabled();

        /// <summary>
        /// Indicates whether this request is a test request.
        /// By default, Test Mode is disabled.
        /// When enabled, this request is treated as a test request.
        /// PubMatic may deliver only test ads which are not billable.
        /// Please disable the Test Mode for requests before you submit your application to the play store.
        /// </summary>
        /// <param name="enable">boolean value of test mode</param>
        void EnableTestMode(bool enable);

        /// <summary>
        /// Returns test Mode of the OpenWrap request
        /// </summary>
        /// <returns>boolean value of test mode</returns>
        bool IsTestModeEnabled();

        // <summary>
        /// Custom server URL for debugging purpose.
        /// If it is set, ad request will be made to the provided url
        /// </summary>
        /// <returns>ad server url</returns>
        string GetAdServerUrl();

        /// <summary>
        /// Set's custom server URL for debugging purpose.
        /// </summary>
        /// <returns>ad server url</returns>
        void SetAdServerUrl(string url);

        /// <summary>
        /// Returns the publisher Id.
        /// </summary>
        /// <returns>publisher Id</returns>
        string GetPublisherId();

        /// <summary>
        /// Returns the profile Id.
        /// </summary>
        /// <returns>profile Id</returns>
        int GetProfileId();
    }
}
#endif