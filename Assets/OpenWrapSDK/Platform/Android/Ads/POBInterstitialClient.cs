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

using System;
using OpenWrapSDK.Common;
using UnityEngine;

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Android client of Interstitial ad which communicates with java native code
    /// </summary>
    internal class POBInterstitialClient : AndroidJavaProxy, IPOBInterstitialClient
    {
        #region Private variables
        private readonly string Tag = "POBInterstitialClient";
        private readonly AndroidJavaObject androidInterstitialAd;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="publisherId">OpenWrap's publisher id</param>
        /// <param name="profileId">OpenWrap's profile id</param>
        /// <param name="adUnitId">OpenWrap's ad unit id</param>
        #region Constructors/Destructor
        internal POBInterstitialClient(string owPublisherId, int owProfileId, string owAdUnitId) : base(POBConstants.POBInterstitialAdCallbackInterfaceName)
        {
            AndroidJavaObject activityContext = POBAndroidUtils.getActivity();
            this.androidInterstitialAd = new AndroidJavaObject(POBConstants.POBInterstitialClassName, activityContext, owPublisherId, owProfileId, owAdUnitId, this);
            // Initialize the event dispatcher 
            POBEventsDispatcher.Initialize();

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBInterstitialClient()
        {
            Destroy();
        }
        #endregion

        #region IPOBInterstitialClient APIs
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
        /// Callback method notifies that a user interaction will open another app, leaving the current app.
        /// </summary>
        public event EventHandler<EventArgs> OnAppLeaving;

        /// <summary>
        /// Callback method notifies that the interstitial ad will be presented as a modal on tops
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

        /// <summary>
        /// Method to get the interstitial bid
        /// </summary>
        public IPOBBid GetBid()
        {
            return new POBBidClient(this.androidInterstitialAd.Call<AndroidJavaObject>("getBid"));
        }

        /// <summary>
        /// Method to get the interstitial impression
        /// </summary>
        /// <returns>Instance of type IPOBImpression</returns>
        public IPOBImpression GetImpression()
        {
            return new POBImpressionClient(this.androidInterstitialAd.Call<AndroidJavaObject>("getImpression"));
        }

        /// <summary>
        /// Method to get the interstitial request
        /// </summary>
        /// <returns>Instance of type IPOBRequest</returns>
        public IPOBRequest GetRequest()
        {
            return new POBRequestClient(this.androidInterstitialAd.Call<AndroidJavaObject>("getAdRequest"));
        }

        /// <summary>
        /// Method to check if ad is ready
        /// </summary>
        /// <returns>is ready status</returns>
        public bool IsReady()
        {
            POBLog.Info(Tag, POBLogStrings.ClientIsReadyLog);
            return this.androidInterstitialAd.Call<bool>("isReady");
        }

        /// <summary>
        /// Method to load interstitial ad 
        /// </summary>
        public void LoadAd()
        {
            POBLog.Info(Tag, POBLogStrings.ClientLoadAdLog);
            this.androidInterstitialAd.Call("loadAd");
        }

        /// <summary>
        /// Method to show interstitial ad
        /// </summary>
        public void ShowAd()
        {
            POBLog.Info(Tag, POBLogStrings.ClientShowAdLog);
            this.androidInterstitialAd.Call("show");
        }

        /// <summary>
        ///  Method to Destroy interstitial ad
        /// </summary>
        public void Destroy()
        {
            POBLog.Info(Tag, POBLogStrings.ClientDestroyLog);
            this.androidInterstitialAd.Call("destroy");
        }
        #endregion

        #region Callbacks from Unity
        /// <summary>
        /// Notifies the listener that an ad has been successfully loaded and rendered.
        /// </summary>
        public void onAdLoaded()
        {
            if (OnAdLoaded != null)
            {
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnAdLoaded(this, EventArgs.Empty);
                });
                
            }
        }

        /// <summary>
        /// Notifies the listener of an error encountered while loading an ad.
        /// </summary>
        /// <param name="error">POBError instance</param>
        public void onAdFailedToLoad(AndroidJavaObject error)
        {
            if (OnAdFailedToLoad != null)
            {
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnAdFailedToLoad(this, POBAndroidUtils.ConvertToPOBErrorEventArgs(error));
                });
                
            }
        }

        /// <summary>
        /// Notifies the listener of an error encountered while rendering an ad.
        /// </summary>
        /// <param name="error">POBError instance</param>
        public void onAdFailedToShow(AndroidJavaObject error)
        {
            if (OnAdFailedToShow != null)
            {
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnAdFailedToShow(this, POBAndroidUtils.ConvertToPOBErrorEventArgs(error));
                });
                
            }
        }

        /// <summary>
        /// Notifies the listener whenever current app goes in the background due to user click
        /// </summary>
        public void onAppLeaving()
        {
            if (OnAppLeaving != null)
            {
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnAppLeaving(this, EventArgs.Empty);
                });
                
            }
        }

        /// <summary>
        /// Notifies that the OpenWrap view will open an ad on top of the current view.
        /// </summary>
        public void onAdOpened()
        {
            if (OnAdOpened != null)
            {
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnAdOpened(this, EventArgs.Empty);
                });
                
            }
        }

        /// <summary>
        /// Notifies that the OpenWrap view has closed the ad on top of the current view.
        /// </summary>
        public void onAdClosed()
        {
            if (OnAdClosed != null)
            {
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnAdClosed(this, EventArgs.Empty);
                });
                
            }
        }

        /// <summary>
        /// Notifies that the user has clicked the ad view.
        /// </summary>
        public void onAdClicked()
        {
            if (OnAdClicked != null)
            {
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnAdClicked(this, EventArgs.Empty);
                });
                
            }
        }

        /// <summary>
        /// Notifies the listener that an ad has been expired
        /// </summary>
        public void onAdExpired()
        {
            if (OnAdExpired != null)
            {
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnAdExpired(this, EventArgs.Empty);
                });
                
            }
        }

        /// <summary>
        /// Notifies the listener that the playback of the video ad has been completed.
        /// </summary>
        public void onVideoPlaybackCompleted()
        {
            if (OnVideoPlaybackCompleted != null) 
            {
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnVideoPlaybackCompleted(this, EventArgs.Empty);
                });
                
            }
        }
#endregion
    }
}
#endif