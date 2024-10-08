#if UNITY_IOS || UNITY_ANDROID
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

namespace OpenWrapSDK
{
    public class POBErrorEventArgs : EventArgs
    {
        /// <summary>
        /// You may not passing mandatory parameters like Publisher ID, Sizes, and other ad tag details.
        /// </summary>
        public readonly int InvalidRequest = 1001;

        /// <summary>
        /// There was no ads available to deliver for ad request.
        /// </summary>
        public readonly int NoAdsAvailable = 1002;

        /// <summary>
        /// There was an error while retrieving the data from the network.
        /// </summary>
        public readonly int NetworkError = 1003;

        /// <summary>
        /// Failed to process ad request by Ad Server.
        /// </summary>
        public readonly int ServerError = 1004;

        /// <summary>
        /// Ad request was timed out
        /// </summary>
        public readonly int TimeoutError = 1005;

        /// <summary>
        /// Internal error
        /// </summary>
        public readonly int InternalError = 1006;

        /// <summary>
        /// Invalid ad response. SDK does not able to parse the response received from Server.
        /// </summary>
        public readonly int InvalidResponse = 1007;

        /// <summary>
        /// Ad request gets cancelled.
        /// </summary>
        public readonly int RequestCancelled = 1008;

        /// <summary>
        /// There was some issue while rendering the creative.
        /// </summary>
        public readonly int RenderError = 1009;

        /// <summary>
        /// Ad server SDK sent unexpectd/delayed OpenWrap win response
        /// </summary>
        public readonly int OpenwrapSignalingError = 1010;

        /// <summary>
        /// Indicates an ad is expired
        /// </summary>
        public readonly int AdExpired = 1011;

        /// <summary>
        /// Indicates the Ad is already show
        /// </summary>
        public readonly int AdAlreadyShown = 2001;

        /// <summary>
        /// Indicated the Ad is not ready to show yet.
        /// </summary>
        public readonly int AdNotReady = 2002;

        /// <summary>
        /// Indicates error due to client side auction loss
        /// </summary>
        public readonly int ClientSideAuctionLost = 3001;

        /// <summary>
        /// Indicates error due to ad server side auction loss
        /// </summary>
        public readonly int AdServerAuctionLost = 3002;

        /// <summary>
        /// Indicates error due to ad not used
        /// </summary>
        public readonly int AdNotUsed = 3003;

        /// <summary>
        /// Indicates partner details are not found
        /// </summary>
        public readonly int NoPartnerDetails = 4001;

        /// <summary>
        /// Indicates invalid reward selection
        /// </summary>
        public readonly int InvalidRewardSelected = 5001;

        /// <summary>
        /// Indicates Rewarded ad has encountered some configuration error
        /// </summary>
        public readonly int RewardNotSelected = 5002;

        /// <summary>
        /// The integer value of the error code.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// The descriptive message for probable fix
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return "POBError{" +
                "errorCode=" + ErrorCode +
                ", errorMessage='" + Message + '\'' +
                '}';
        }
    }
}
#endif