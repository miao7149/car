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
using OpenWrapSDK.Common;

namespace OpenWrapSDK
{
    /// <summary>
    /// Displays Interstitial Ads.
    /// </summary>
    public class POBInterstitial
    {
        #region Public callbacks
#pragma warning disable 67
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
        /// Callback method notifies that the interstitial ad will be presented as a modal on top of the current view controller
        /// </summary>
        public event EventHandler<EventArgs> OnAdOpened;

        /// <summary>
        /// Callback method notifies that the interstitial ad has been animated off the screen.
        /// </summary>
        public event EventHandler<EventArgs> OnAdClosed;

        /// <summary>
        /// Callback method notifies that the interstitial ad has been clicked
        /// </summary>
        public event EventHandler<EventArgs> OnAdClicked;

        /// <summary>
        /// Callback method notifies that the interstitial ad has expired
        /// </summary>
        public event EventHandler<EventArgs> OnAdExpired;

        /// <summary>
        /// Callback method notifies that the interstitial video playback have completed.
        /// </summary>
        public event EventHandler<EventArgs> OnVideoPlaybackCompleted;
#pragma warning restore 67
        #endregion

        #region Private variables
        private readonly IPOBInterstitialClient interstitialClient;
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Constructor for POBInterstitial ad
        /// </summary>
        /// <param name="publisherId">OpenWrap publisher id</param>
        /// <param name="profileId">OpenWrap profile id</param>
        /// <param name="adUnitId">OpenWrap ad unit id</param>
        public POBInterstitial(string publisherId, int profileId, string adUnitId)
        {
#if UNITY_IOS
            interstitialClient = new iOS.POBInterstitialClient(publisherId, profileId, adUnitId);
#else
            interstitialClient = new Android.POBInterstitialClient(publisherId, profileId, adUnitId);
#endif
            SetCallbacks();
        }

        /// <summary>
        /// Destructor for POBInterstitial ad
        /// </summary>
        ~POBInterstitial()
        {
            Destroy();
        }
        #endregion

        #region Public APIs
        /// <summary>
        /// Getter for OpenWrap Interstitial POBBid
        /// </summary>
        /// <returns>Instance of POBBid</returns>
        public IPOBBid GetBid()
        {
            if (interstitialClient != null)
            {
                return interstitialClient.GetBid();
            }
            return null;
        }

        /// <summary>
        /// Getter for OpenWrap Interstitial POBImpression
        /// </summary>
        /// <returns>Instance of POBImpression</returns>
        public IPOBImpression GetImpression()
        {
            if (interstitialClient != null)
            {
                return interstitialClient.GetImpression();
            }
            return null;
        }

        /// <summary>
        /// Getter for OpenWrap Interstitial POBRequest
        /// </summary>
        /// <returns>Instance of POBRequest</returns>
        public IPOBRequest GetRequest()
        {
            if (interstitialClient != null)
            {
                return interstitialClient.GetRequest();
            }
            return null;
        }

        /// <summary>
        /// Method to check if the interstitial ad is ready to show.
        /// </summary>
        /// <returns>true/false representing ready state.</returns>
        public bool IsReady()
        {
            if (interstitialClient != null)
            {
                return interstitialClient.IsReady();
            }
            return false;
        }

        /// <summary>
        /// Method to load OpenWrap interstitial ad.
        /// </summary>
        public void LoadAd()
        {
            if (interstitialClient != null)
            {
                interstitialClient.LoadAd();
            }
        }

        /// <summary>
        /// Method to show OpenWrap interstitial ad.
        /// </summary>
        public void ShowAd()
        {
            if (interstitialClient != null)
            {
                interstitialClient.ShowAd();
            }
        }

        /// <summary>
        /// Method to destroy POBInterstitial
        /// </summary>
        public void Destroy()
        {
            interstitialClient.Destroy();
        }
        #endregion

        #region
        private void SetCallbacks()
        {
            if (interstitialClient != null)
            {
                interstitialClient.OnAdLoaded += (sender, args) =>
                {
                    OnAdLoaded?.Invoke(this, args);
                };

                interstitialClient.OnAdFailedToLoad += (sender, args) =>
                {
                    OnAdFailedToLoad?.Invoke(this, args);
                };

                interstitialClient.OnAdFailedToShow += (sender, args) =>
                {
                    OnAdFailedToShow?.Invoke(this, args);
                };

                interstitialClient.OnAdOpened += (sender, args) =>
                {
                    OnAdOpened?.Invoke(this, args);
                };

                interstitialClient.OnAdClosed += (sender, args) =>
                {
                    OnAdClosed?.Invoke(this, args);
                };

                interstitialClient.OnAdClicked += (sender, args) =>
                {
                    OnAdClicked?.Invoke(this, args);
                };

                interstitialClient.OnAdExpired += (sender, args) =>
                {
                    OnAdExpired?.Invoke(this, args);
                };

                interstitialClient.OnAppLeaving += (sender, args) =>
                {
                    OnAppLeaving?.Invoke(this, args);
                };
                interstitialClient.OnVideoPlaybackCompleted += (sender, args) =>
                {
                    OnVideoPlaybackCompleted?.Invoke(this, args);
                };
            }
        }
        #endregion
    }
}
#endif