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

namespace OpenWrapSDK.Common
{
    /// <summary>
    /// Interface for common APIs for iOS/Android RewardedAd clients.
    /// </summary>
    public interface IPOBRewardedAdClient
    {
        #region Public APIs
        /// <summary>
        /// Getter for OpenWrap rewarded ad POBBid
        /// </summary>
        /// <returns>Instance of POBBid</returns>
        IPOBBid GetBid();

        /// <summary>
        /// Getter for OpenWrap rewarded ad POBImpression
        /// </summary>
        /// <returns>Instance of POBImpression</returns>
        IPOBImpression GetImpression();

        /// <summary>
        /// Getter for OpenWrap rewarded ad POBRequest
        /// </summary>
        /// <returns>Instance of POBRequest</returns>
        IPOBRequest GetRequest();

        /// <summary>
        /// Method to check if the rewarded ad is ready to show.
        /// </summary>
        /// <returns>true/false representing ready state.</returns>
        bool IsReady();

        /// <summary>
        /// Method to load OpenWrap rewarded ad.
        /// </summary>
        void LoadAd();

        /// <summary>
        /// Method to show OpenWrap rewarded ad.
        /// </summary>
        void ShowAd();

        /// <summary>
        /// Method to set skip alert info
        /// </summary>
        /// <param name="title">skip alert title</param>
        /// <param name="message">skip alert message</param>
        /// <param name="closeTitle">close button title for skip alert</param>
        /// <param name="resumeTitle">resume button title for skip alert</param>
        void SetSkipAlertInfo(string title, string message, string closeTitle, string resumeTitle);

        /// <summary>
        /// Method to destroy POBRewardedAd
        /// </summary>
        void Destroy();
        #endregion

        #region Callbacks
        /// <summary>
        /// Callback method notifies that an ad has been received successfully.
        /// </summary>
        event EventHandler<EventArgs> OnAdLoaded;

        /// <summary>
        /// Callback method notifies an error encountered while loading or rendering an ad.
        /// </summary>
        event EventHandler<POBErrorEventArgs> OnAdFailedToLoad;

        /// <summary>
        /// Callback method notifies an error encountered while showing an ad.
        /// </summary>
        event EventHandler<POBErrorEventArgs> OnAdFailedToShow;

        /// <summary>
        /// Callback method notifies that a user interaction will open another app (for example, App Store), leaving the current app.
        /// </summary>
        event EventHandler<EventArgs> OnAppLeaving;

        /// <summary>
        /// Callback method notifies that the rewarded ad will be presented as a modal on top of the current view controller
        /// </summary>
        event EventHandler<EventArgs> OnAdOpened;

        /// <summary>
        /// Callback method notifies that the rewarded ad has been animated off the screen.
        /// </summary>
        event EventHandler<EventArgs> OnAdClosed;

        /// <summary>
        /// Callback method notifies that the rewarded ad has been clicked
        /// </summary>
        event EventHandler<EventArgs> OnAdClicked;

        /// <summary>
        /// Callback method notifies that the rewarded ad has expired
        /// </summary>
        event EventHandler<EventArgs> OnAdExpired;

        /// <summary>
        /// Callback method notifies when an Ad has completed the minimum required viewing, and user should be rewarded.
        /// </summary>
        event EventHandler<POBRewardEventArgs> OnReceiveReward;
        #endregion
    }
}
#endif