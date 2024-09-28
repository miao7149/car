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
    /// Displays Banner Ads.
    /// </summary>
    public class POBBannerView
    {
        #region Public callbacks
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
        #endregion

        #region Private variables
        private readonly IPOBBannerViewClient bannerClient;
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Constructor for POBBannerView
        /// </summary>
        /// <param name="publisherId">OpenWrap publisher id</param>
        /// <param name="profileId">OpenWrap profile id</param>
        /// <param name="adUnitId">OpenWrap ad unit id</param>
        /// <param name="adSize">Banner ad size for this impression</param>
        /// <param name="position">Banner ad position on the screen</param>
        public POBBannerView(string publisherId, int profileId, string adUnitId, POBAdSize adSize, POBBannerPosition position)
        {
#if UNITY_IOS
            bannerClient = new iOS.POBBannerViewClient(publisherId, profileId, adUnitId, adSize);
#else
            bannerClient = new Android.POBBannerViewClient(publisherId, profileId, adUnitId, adSize);
#endif
            bannerClient.SetBannerPosition(position);
            SetCallbacks();
        }

        /// <summary>
        /// Constructor for POBBannerView
        /// </summary>
        /// <param name="publisherId">OpenWrap publisher id</param>
        /// <param name="profileId">OpenWrap profile id</param>
        /// <param name="adUnitId">OpenWrap ad unit id</param>
        /// <param name="adSize">Banner ad size for this impression</param>
        /// <param name="positionX">Banner ad position x-coordinate on the screen</param>
        /// <param name="positionY">Banner ad position y-coordinate on the screen</param>
        public POBBannerView(string publisherId, int profileId, string adUnitId, POBAdSize adSize, float positionX, float positionY)
        {
#if UNITY_IOS
            bannerClient = new iOS.POBBannerViewClient(publisherId, profileId, adUnitId, adSize);
#else
            bannerClient = new Android.POBBannerViewClient(publisherId, profileId, adUnitId, adSize);
#endif
            bannerClient.SetBannerCustomPosition(positionX, positionY);
            SetCallbacks();
        }

        /// <summary>
        /// Destructor for POBBannerView
        /// </summary>
        ~POBBannerView()
        {
            Destroy();
        }
        #endregion

        #region Public APIs
        /// <summary>
        /// Getter for OpenWrap Banner POBBid
        /// </summary>
        /// <returns>Instance of POBBid</returns>
        public IPOBBid GetBid()
        {
            if (bannerClient != null)
            {
                return bannerClient.GetBid();
            }
            return null;
        }

        /// <summary>
        /// Getter for OpenWrap Banner POBImpression
        /// </summary>
        /// <returns>Instance of POBImpression</returns>
        public IPOBImpression GetImpression()
        {
            if (bannerClient != null)
            {
                return bannerClient.GetImpression();
            }
            return null;
        }

        /// <summary>
        /// Getter for OpenWrap Banner POBRequest
        /// </summary>
        /// <returns>Instance of POBRequest</returns>
        public IPOBRequest GetRequest()
        {
            if (bannerClient != null)
            {
                return bannerClient.GetRequest();
            }
            return null;
        }

        /// <summary>
        /// Getter for OpenWrap Banner Ad size
        /// </summary>
        /// <returns>Instance of POBAdSize</returns>
        public POBAdSize GetCreativeSize()
        {
            if (bannerClient != null)
            {
                return bannerClient.GetCreativeSize();
            }
            return null;
        }

        /// <summary>
        /// Method to load OpenWrap banner ad.
        /// </summary>
        public void LoadAd()
        {
            if (bannerClient != null)
            {
                bannerClient.LoadAd();
            }
        }

        /// <summary>
        /// Pauses the auto refresh, By default, banner refreshes automatically as per
        /// configured refresh interval on OpenWrap portal.Calling this method
        /// prevents the refresh cycle to happen even if a refresh interval has been specified.
        /// It is recommended to use this method whenever the ad view is about to be hidden
        /// from the user for any period of time, to avoid unnecessary ad requests.You can
        /// then call `resumeAutoRefresh()` to resume the refresh when banner becomes
        /// visible.
        /// </summary>
        public void PauseAutoRefresh()
        {
            if (bannerClient != null)
            {
                bannerClient.PauseAutoRefresh();
            }
        }

        /// <summary>
        /// Resumes the auto-refresh as per configured refresh interval on OpenWrap portal, call this
        /// method only if you have previously paused auto-refresh using `pauseAutoRefresh()`.
        /// This method has no effect if a refresh interval has not been set.
        /// </summary>
        public void ResumeAutoRefresh()
        {
            if (bannerClient != null)
            {
                bannerClient.ResumeAutoRefresh();
            }
        }

        /// <summary>
        /// Cancels existing ad requests and initiates new ad request.
        /// It may skip force refresh in below cases:
        /// 1). Ad is being loaded
        /// 2). Waiting response from ad server SDK(if applicable)
        /// 3). User interacting with ad(Opening Internal browser or expanding ad)
        /// <returns>boolean status of banner force refresh</returns>
        /// </summary>
        public bool ForceRefresh()
        {
            if (bannerClient != null)
            {
                return bannerClient.ForceRefresh();
            }
            return false;
        }

        /// <summary>
        /// Method to destroy POBBannerView
        /// </summary>
        public void Destroy()
        {
            if (bannerClient != null)
            {
                bannerClient.Destroy();
            }
        }
        #endregion

        #region
        /// <summary>
        /// Method to register callbacks for banner events
        /// </summary>
        private void SetCallbacks()
        {
            if (bannerClient != null)
            {
                bannerClient.OnAdLoaded += (sender, args) =>
                {
                    OnAdLoaded?.Invoke(this, args);
                };

                bannerClient.OnAdFailedToLoad += (sender, args) =>
                {
                    OnAdFailedToLoad?.Invoke(this, args);
                };

                bannerClient.OnAdOpened += (sender, args) =>
                {
                    OnAdOpened?.Invoke(this, args);
                };

                bannerClient.OnAdClosed += (sender, args) =>
                {
                    OnAdClosed?.Invoke(this, args);
                };

                bannerClient.OnAdClicked += (sender, args) =>
                {
                    OnAdClicked?.Invoke(this, args);
                };

                bannerClient.OnAppLeaving += (sender, args) =>
                {
                    OnAppLeaving?.Invoke(this, args);
                };
            }
        }
        #endregion
    }
}
#endif