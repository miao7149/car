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
* LAWS AND INTERNATIONAL TREATIES.  THE RECEIPT OR POSSESSION OF  THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT  MAY DESCRIBE, IN WHOLE OR IN PART.
*/

using System;
using OpenWrapSDK.Common;

namespace OpenWrapSDK
{
    /// <summary>
    /// Displays Rewarded Ads.
    /// </summary>
    public class POBRewardedAd
    {
        #region Public callbacks
        /// <summary>
        /// Callback method notifies that an ad has been received successfully.
        /// </summary>
        public event EventHandler<EventArgs> OnAdLoaded;

        /// <summary>
        /// Callback method notifies an error encountered while loading or rendering an ad.
        /// </summary>
        public event EventHandler<POBErrorEventArgs> OnAdFailedToLoad;

        /// <summary>
        /// Callback method notifies an error encountered while showing an ad.
        /// </summary>
        public event EventHandler<POBErrorEventArgs> OnAdFailedToShow;

        /// <summary>
        /// Callback method notifies that a user interaction will open another app (for example, App Store), leaving the current app.
        /// </summary>
        public event EventHandler<EventArgs> OnAppLeaving;

        /// <summary>
        /// Callback method notifies that the rewarded ad will be presented as a modal on top of the current view controller
        /// </summary>
        public event EventHandler<EventArgs> OnAdOpened;

        /// <summary>
        /// Callback method notifies that the rewarded ad has been animated off the screen.
        /// </summary>
        public event EventHandler<EventArgs> OnAdClosed;

        /// <summary>
        /// Callback method notifies that the rewarded ad has been clicked
        /// </summary>
        public event EventHandler<EventArgs> OnAdClicked;

        /// <summary>
        /// Callback method notifies that the rewarded ad has expired
        /// </summary>
        public event EventHandler<EventArgs> OnAdExpired;

        /// <summary>
        /// Callback method notifies when an Ad has completed the minimum required viewing, and user should be rewarded
        /// </summary>
        public event EventHandler<POBRewardEventArgs> OnReceiveReward;
        #endregion

        #region Private variables
        private readonly IPOBRewardedAdClient rewardedAdClient;
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Constructor for POBRewardedAd
        /// </summary>
        /// <param name="publisherId">OpenWrap publisher id</param>
        /// <param name="profileId">OpenWrap profile id</param>
        /// <param name="adUnitId">OpenWrap ad unit id</param>
        private POBRewardedAd(IPOBRewardedAdClient client)
        {
            rewardedAdClient = client;
            SetCallbacks();

        }

        /// <summary>
        /// Initializes and returns newly allocated rewarded ad object.
        /// </summary>
        /// <returns>POBRewardedAd object</returns>
        public static POBRewardedAd GetRewardedAd(string publisherId, int profileId, string adUnitId)
        {
            IPOBRewardedAdClient client;
#if UNITY_IOS
            client = iOS.POBRewardedAdClient.GetRewardedAdClient(publisherId, profileId, adUnitId);
#else
            client = Android.POBRewardedAdClient.GetRewardedAdClient(publisherId, profileId, adUnitId);
#endif
            if(client != null)
            {
                return new POBRewardedAd(client);
            }
            return null;
        }

        /// <summary>
        /// Destructor for POBRewardedAd
        /// </summary>
        ~POBRewardedAd()
        {
            Destroy();
        }
        #endregion

        #region Public APIs
        /// <summary>
        /// Getter for OpenWrap RewardedAd POBBid
        /// </summary>
        /// <returns>Instance of POBBid</returns>
        public IPOBBid GetBid()
        {
            return rewardedAdClient.GetBid(); 
        }

        /// <summary>
        /// Getter for OpenWrap RewardedAd POBImpression
        /// </summary>
        /// <returns>Instance of POBImpression</returns>
        public IPOBImpression GetImpression()
        {
            return rewardedAdClient.GetImpression();
        }

        /// <summary>
        /// Getter for OpenWrap RewardedAd POBRequest
        /// </summary>
        /// <returns>Instance of POBRequest</returns>
        public IPOBRequest GetRequest()
        {
            return rewardedAdClient.GetRequest();
        }

        /// <summary>
        /// Method to check if the rewarded ad is ready to show.
        /// </summary>
        /// <returns>true/false representing ready state.</returns>
        public bool IsReady()
        {
            return rewardedAdClient.IsReady();
        }

        /// <summary>
        /// Method to load OpenWrap rewarded ad.
        /// </summary>
        public void LoadAd()
        {
            rewardedAdClient.LoadAd();
        }

        /// <summary>
        /// Method to show OpenWrap rewarded ad.
        /// </summary>
        public void ShowAd()
        {
            rewardedAdClient.ShowAd();
        }

        /// <summary>
        /// Method to set custom skip alert info - alert title, message, close & resume button titles
        /// </summary>
        /// <param name="title">skip alert title</param>
        /// <param name="message">skip alert message</param>
        /// <param name="closeTitle">close button title for skip alert</param>
        /// <param name="resumeTitle">resume button title for skip alert</param>
        public void SetSkipAlertInfo(string title, string message, string closeTitle, string resumeTitle)
        {
            rewardedAdClient.SetSkipAlertInfo(title, message, closeTitle, resumeTitle);
        }

        /// <summary>
        /// Method to destroy POBRewardedAd
        /// </summary>
        public void Destroy()
        {
            rewardedAdClient.Destroy();
        }
        #endregion

        #region
        private void SetCallbacks()
        {
            if (rewardedAdClient != null)
            {
                /// <summary>
                /// Callback method notifies that an ad has been received successfully.
                /// </summary>
                rewardedAdClient.OnAdLoaded += (sender, args) =>
                {

                    OnAdLoaded?.Invoke(this, args);

                };

                /// <summary>
                /// Callback method notifies an error encountered while loading or rendering an ad.
                /// </summary>
                rewardedAdClient.OnAdFailedToLoad += (sender, args) =>
                {
                    OnAdFailedToLoad?.Invoke(this, args);
                };

                /// <summary>
                /// Callback method notifies an error encountered while showing an ad.
                /// </summary>
                rewardedAdClient.OnAdFailedToShow += (sender, args) =>
                {
                    OnAdFailedToShow?.Invoke(this, args);
                };

                /// <summary>
                /// Callback method notifies that the rewarded ad will be presented as a modal on top of the current view controller
                /// </summary>
                rewardedAdClient.OnAdOpened += (sender, args) =>
                {
                    OnAdOpened?.Invoke(this, args);
                };

                /// <summary>
                /// Callback method notifies that the rewarded ad has been animated off the screen.
                /// </summary>
                rewardedAdClient.OnAdClosed += (sender, args) =>
                {
                    OnAdClosed?.Invoke(this, args);
                };

                /// <summary>
                /// Callback method notifies that the rewarded ad has been clicked
                /// </summary>
                rewardedAdClient.OnAdClicked += (sender, args) =>
                {
                    OnAdClicked?.Invoke(this, args);
                };

                /// <summary>
                /// Callback method notifies that the rewarded ad has expired
                /// </summary>
                rewardedAdClient.OnAdExpired += (sender, args) =>
                {
                    OnAdExpired?.Invoke(this, args);
                };

                /// <summary>
                /// Callback method notifies that a user interaction will open another app (for example, App Store), leaving the current app.
                /// </summary>
                rewardedAdClient.OnAppLeaving += (sender, args) =>
                {
                    OnAppLeaving?.Invoke(this, args);
                };

                /// <summary>
                /// Callback method notifies when an Ad has completed the minimum required viewing, and user should be rewarded
                /// </summary>
                rewardedAdClient.OnReceiveReward += (sender, args) =>
                {
                    OnReceiveReward?.Invoke(this, args);
                };
            }
        }
        #endregion
    }
}
#endif