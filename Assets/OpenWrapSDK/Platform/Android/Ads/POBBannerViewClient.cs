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

using UnityEngine;
using OpenWrapSDK.Common;
using System;

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Android client of Banner ad which communicates with java native code
    /// </summary>
    internal class POBBannerViewClient : AndroidJavaProxy, IPOBBannerViewClient
    {
        #region Private variables
        private readonly string Tag = "POBBannerViewClient";
        private readonly AndroidJavaObject androidBannerAd;
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="publisherId">OpenWrap's publisher id</param>
        /// <param name="profileId">OpenWrap's profile id</param>
        /// <param name="adUnitId">OpenWrap's ad unit id</param>
        /// <param name="adSize">banner ad size</param>
        #region Constructors/Destructor
        internal POBBannerViewClient(string owPublisherId, int owProfileId, string owAdUnitId, POBAdSize adSize)
            : base(POBConstants.POBBannerAdCallbackInterfaceName)
        {
            AndroidJavaObject activityContext = POBAndroidUtils.getActivity();
            this.androidBannerAd = new AndroidJavaObject(POBConstants.POBBannerViewClassName, activityContext, owPublisherId, owProfileId, owAdUnitId, POBAndroidUtils.ConvertToPOBAdSize(adSize), this);
            // Initialize the event dispatcher 
            POBEventsDispatcher.Initialize();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBBannerViewClient()
        {
            Destroy();
        }
        #endregion

        /// <summary>
        /// Callback method notifies that an ad has been loaded successfully.
        /// </summary>
        public event EventHandler<EventArgs> OnAdLoaded;

        /// <summary>
        /// Callback method notifies an error encountered while loading or rendering an ad.
        /// </summary>
        public event EventHandler<POBErrorEventArgs> OnAdFailedToLoad;

        /// <summary>
        /// Callback method notifies that a user interaction will open another app (for example, App Store), leaving the current app.
        /// </summary>
        public event EventHandler<EventArgs> OnAppLeaving;

        /// <summary>
        /// Callback method notifies that the banner ad will be presented as a modal on top
        /// </summary>
        public event EventHandler<EventArgs> OnAdOpened;

        /// <summary>
        /// Callback method notifies that the banner ad has been animated off the screen.
        /// </summary>
        public event EventHandler<EventArgs> OnAdClosed;

        /// <summary>
        /// Callback method notifies that the banner ad has been clicked
        /// </summary>
        public event EventHandler<EventArgs> OnAdClicked;

        /// <summary>
        /// Destructor for POBBannerView
        /// </summary>
        public void Destroy()
        {
            POBLog.Info(Tag, POBLogStrings.ClientDestroyLog);
            this.androidBannerAd.Call("destroy");
        }

        /// <summary>
        /// Method to get the banner bid
        /// </summary>
        public IPOBBid GetBid()
        {
            return new POBBidClient(this.androidBannerAd.Call<AndroidJavaObject>("getBid"));
        }

        /// <summary>
        /// Method to get the banner impression
        /// </summary>
        /// <returns>Instance of type IPOBImpression</returns>
        public IPOBImpression GetImpression()
        {
            return new POBImpressionClient(this.androidBannerAd.Call<AndroidJavaObject>("getImpression"));
        }

        /// <summary>
        /// Method to get the banner request
        /// </summary>
        /// <returns>Instance of type IPOBRequest</returns>
        public IPOBRequest GetRequest()
        {
            return new POBRequestClient(this.androidBannerAd.Call<AndroidJavaObject>("getAdRequest"));
        }

        public POBAdSize GetCreativeSize()
        {
            AndroidJavaObject nativeAdSize = this.androidBannerAd.Call<AndroidJavaObject>("getCreativeSize");
            if (nativeAdSize != null)
            {
                int width = nativeAdSize.Call<int>("getAdWidth");
                int height = nativeAdSize.Call<int>("getAdHeight");
                return new POBAdSize(width, height);
            }
            return null;
        }


        /// <summary>
        /// Method to load banner ad
        /// </summary>
        public void LoadAd()
        {
            POBLog.Info(Tag, POBLogStrings.ClientLoadAdLog);
            this.androidBannerAd.Call("loadAd");
        }

        public void SetBannerPosition(POBBannerPosition position)
        {
            this.androidBannerAd.Call("setBannerPosition", (int)position);
        }

        public void SetBannerCustomPosition(float x, float y)
        {
            this.androidBannerAd.Call("setBannerCustomPosition", x, y);
        }

        /// <summary>
        /// Method to pause refresh of OpenWrap Banner ad.
        /// </summary>
        public void PauseAutoRefresh()
        {
            POBLog.Info(Tag, POBLogStrings.ClientPauseAutoRefreshLog);
            this.androidBannerAd.Call("pauseAutoRefresh");
        }

        /// <summary>
        /// Method to resume refresh of OpenWrap Banner ad.
        /// </summary>
        public void ResumeAutoRefresh()
        {
            POBLog.Info(Tag, POBLogStrings.ClientResumeAutoRefreshLog);
            this.androidBannerAd.Call("resumeAutoRefresh");
        }

        /// <summary>
        /// Method to force refresh OpenWrap Banner ad.
        /// <returns>boolean status of banner force refresh</returns>
        /// </summary>
        public bool ForceRefresh()
        {
            POBLog.Info(Tag, POBLogStrings.ClientForceRefreshLog);
            return this.androidBannerAd.Call<bool>("forceRefresh");
        }

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
        /// Notifies listener that the banner view has open the ad on top of the current
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
        /// Notifies that the banner view has closed the ad on top of the current view.
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

        #endregion
    }

}
#endif