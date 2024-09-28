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
using OpenWrapSDK.Common;

namespace OpenWrapSDK.iOS
{
    /// <summary>
    /// iOS Specific banner client interacting with iOS Plugins
    /// </summary>
    internal class POBBannerViewClient : IPOBBannerViewClient
    {
        #region Private variables
        private IntPtr bannerViewClientPtr = IntPtr.Zero;
        private IntPtr bannerViewPtr = IntPtr.Zero;
        private readonly IntPtr impressionPtr = IntPtr.Zero;
        private readonly IntPtr requestPtr = IntPtr.Zero;
        private readonly IPOBImpression impression;
        private readonly IPOBRequest request;
        private readonly POBBidClient bid;
        private readonly string Tag = "POBBannerViewClient";
        #endregion

        #region Constructors/Destructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="publisherId">OpenWrap's publisher id</param>
        /// <param name="profileId">OpenWrap's profile id</param>
        /// <param name="adUnitId">OpenWrap's ad unit id</param>
        /// <param name="adSize">Banner ad size as POBAdSize</param>
        internal POBBannerViewClient(
            string publisherId,
            int profileId,
            string adUnitId,
            POBAdSize adSize)
        {
            CreateBannerView(publisherId, profileId, adUnitId, adSize);

            // Get request
            POBLog.Info(Tag, POBLogStrings.GetBannerRequest);
            requestPtr = POBUGetBannerRequest(bannerViewPtr);
            request = new POBRequestClient(requestPtr);

            // POBBid
            bid = new POBBidClient();

            // Get impression
            POBLog.Info(Tag, POBLogStrings.GetBannerImpression);
            impressionPtr = POBUGetBannerImpression(bannerViewPtr);
            impression = new POBImpressionClient(impressionPtr);
        }

        ~POBBannerViewClient()
        {
            Destroy();
        }
        #endregion

        #region iOS Plugin imports

        [DllImport("__Internal")]
        internal static extern IntPtr POBUCreateBanner(IntPtr bannerViewClient,
                                     string publisherId,
                                     int profileId,
                                     string adUnitId,
                                     int width,
                                     int height);
        [DllImport("__Internal")]
        internal static extern void POBUSetBannerViewCallbacks(IntPtr bannerView,
                            POBUAdCallback didLoadAdCallback,
                            POBUAdFailureCallback didFailToLoadAdCallback,
                            POBUAdCallback didPresentCallback,
                            POBUAdCallback didDismissCallback,
                            POBUAdCallback willLeaveAppCallback,
                            POBUAdCallback didClickAdCallback);

        [DllImport("__Internal")]
        internal static extern void POBUSetBannerPosition(IntPtr bannerView, int position);

        [DllImport("__Internal")]
        internal static extern void POBUSetBannerCustomPostion(IntPtr bannerView, float x, float y);

        [DllImport("__Internal")]
        internal static extern void POBULoadBanner(IntPtr bannerView);

        [DllImport("__Internal")]
        internal static extern void POBUDestroyBanner(IntPtr bannerView);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetBannerBid(IntPtr bannerView);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetBannerRequest(IntPtr bannerView);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetBannerImpression(IntPtr bannerView);

        [DllImport("__Internal")]
        internal static extern string POBUGetBannerCreativeSize(IntPtr bannerView);

        [DllImport("__Internal")]
        internal static extern void POBUPauseAutoRefresh(IntPtr bannerView);

        [DllImport("__Internal")]
        internal static extern void POBUResumeAutoRefresh(IntPtr bannerView);

        [DllImport("__Internal")]
        internal static extern bool POBUForceRefresh(IntPtr bannerView);
        #endregion

        #region Callbacks
        public event EventHandler<EventArgs> OnAdLoaded;
        public event EventHandler<POBErrorEventArgs> OnAdFailedToLoad;
        public event EventHandler<EventArgs> OnAppLeaving;
        public event EventHandler<EventArgs> OnAdOpened;
        public event EventHandler<EventArgs> OnAdClosed;
        public event EventHandler<EventArgs> OnAdClicked;

        // Declaration of delegates loaded from iOS banner plugin
        internal delegate void POBUAdCallback(IntPtr bannerViewClientPtr);
        internal delegate void POBUAdFailureCallback(IntPtr bannerViewClientPtr, int errorCode, string errorMessage);
        
        // Ad loaded callback loaded from iOS banner plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void BannerViewDidLoadAdCallback(IntPtr bannerViewClientPtr)
        {
            POBBannerViewClient bannerViewClient = IntPtrToPOBBannerViewClient(bannerViewClientPtr);
            if (bannerViewClient != null && bannerViewClient.OnAdLoaded != null)
            {
                bannerViewClient.OnAdLoaded(bannerViewClient, EventArgs.Empty);
            }
        }

        // Ad failed to load callback loaded from iOS banner plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdFailureCallback))]
        private static void BannerViewDidFailToLoadAd(IntPtr bannerViewClientPtr, int errorCode, string errorMessage)
        {
            POBBannerViewClient bannerViewClient = IntPtrToPOBBannerViewClient(bannerViewClientPtr);
            if (bannerViewClient != null && bannerViewClient.OnAdLoaded != null)
            {
                bannerViewClient.OnAdFailedToLoad(bannerViewClient, POBIOSUtils.ConvertToPOBErrorEventArgs(errorCode, errorMessage));
            }
        }

        // Callback of screen presented over the ad due to ad click, loaded from iOS banner plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void BannerViewDidPresentScreen(IntPtr bannerViewClientPtr)
        {
            POBBannerViewClient bannerViewClient = IntPtrToPOBBannerViewClient(bannerViewClientPtr);
            if (bannerViewClient != null && bannerViewClient.OnAdLoaded != null)
            {
                bannerViewClient.OnAdOpened(bannerViewClient, EventArgs.Empty);
            }
        }

        // Callback of presented screen dismissed, loaded from iOS banner plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void BannerViewDidDismissScreen(IntPtr bannerViewClientPtr)
        {
            POBBannerViewClient bannerViewClient = IntPtrToPOBBannerViewClient(bannerViewClientPtr);
            if (bannerViewClient != null && bannerViewClient.OnAdLoaded != null)
            {
                bannerViewClient.OnAdClosed(bannerViewClient, EventArgs.Empty);
            }
        }

        // Callback of presented screen dismissed, loaded from iOS banner plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void BannerViewWillLeaveApplication(IntPtr bannerViewClientPtr)
        {
            POBBannerViewClient bannerViewClient = IntPtrToPOBBannerViewClient(bannerViewClientPtr);
            if (bannerViewClient != null && bannerViewClient.OnAdLoaded != null)
            {
                bannerViewClient.OnAppLeaving(bannerViewClient, EventArgs.Empty);
            }
        }

        // Ad clicked callback loaded from iOS banner plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void BannerViewDidClickAd(IntPtr bannerViewClientPtr)
        {
            POBBannerViewClient bannerViewClient = IntPtrToPOBBannerViewClient(bannerViewClientPtr);
            if (bannerViewClient != null && bannerViewClient.OnAdLoaded != null)
            {
                bannerViewClient.OnAdClicked(bannerViewClient, EventArgs.Empty);
            }
        }
        #endregion

        #region IPOBBannerViewClient Public APIs
        /// <summary>
        /// Method to cleanup banner instances, references.
        /// </summary>
        public void Destroy()
        {
            if (bannerViewPtr != IntPtr.Zero)
            {
                POBLog.Info(Tag, POBLogStrings.DestroyBanner);
                POBUDestroyBanner(bannerViewPtr);
                bannerViewPtr = IntPtr.Zero;
            }
            if (bannerViewClientPtr != IntPtr.Zero)
            {
                ((GCHandle)bannerViewClientPtr).Free();
                bannerViewClientPtr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Method to get the banner bid
        /// </summary>
        /// <returns>Instance of POBBid</returns>
        public IPOBBid GetBid()
        {
            POBLog.Info(Tag, POBLogStrings.GetBannerBid);
            IntPtr bidPtr = POBUGetBannerBid(bannerViewPtr);
            bid.SetBid(bidPtr);
            return bid;
        }

        /// <summary>
        /// Method to get the banner impression
        /// </summary>
        /// <returns>Instance of type IPOBImpression</returns>
        public IPOBImpression GetImpression()
        {
            return impression;
        }

        /// <summary>
        /// Method to get the banner request
        /// </summary>
        /// <returns>Instance of type IPOBRequest</returns>
        public IPOBRequest GetRequest()
        {
            return request;
        }

        /// <summary>
        /// Method to load banner
        /// </summary>
        public void LoadAd()
        {
            POBULoadBanner(bannerViewPtr);
        }

        /// <summary>
        /// Method to get the creative size of banner view
        /// </summary>
        /// <returns>Ad size as POBAdSize</returns>
        public POBAdSize GetCreativeSize()
        {
            POBLog.Info(Tag, POBLogStrings.GetBannerCreativeSize);
            string adSizeStr = POBUGetBannerCreativeSize(bannerViewPtr);
            string[] sizes = adSizeStr.Split('x');

            if (sizes.Length == 2)
            {
                int width = int.Parse(sizes[0]);
                int height = int.Parse(sizes[1]);
                POBAdSize adSize = new POBAdSize(width, height);
                return adSize;
            }
            return null;
        }

        /// <summary>
        /// Setter for standard banner position as mentioned in POBBannerPosition
        /// </summary>
        /// <param name="position">Banner position</param>
        public void SetBannerPosition(POBBannerPosition position)
        {
            POBLog.Info(Tag, POBLogStrings.SetBannerPosition);
            POBUSetBannerPosition(bannerViewPtr, (int)position);
        }

        /// <summary>
        /// Setter for banner custom position with x and y coordiantes
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public void SetBannerCustomPosition(float x, float y)
        {
            POBLog.Info(Tag, POBLogStrings.SetBannerCustomPostion);
            POBUSetBannerCustomPostion(bannerViewPtr, x, y);
        }

        /// <summary>
        /// Method to pause auto refresh of banner
        /// </summary>
        public void PauseAutoRefresh()
        {
            POBLog.Info(Tag, POBLogStrings.PauseAutoRefresh);
            POBUPauseAutoRefresh(bannerViewPtr);
        }

        /// <summary>
        /// Method to resume refresh of banner
        /// </summary>
        public void ResumeAutoRefresh()
        {
            POBLog.Info(Tag, POBLogStrings.ResumeAutoRefresh);
            POBUResumeAutoRefresh(bannerViewPtr);
        }

        /// <summary>
        /// Method to force refresh of banner
        /// </summary>
        public bool ForceRefresh()
        {
            POBLog.Info(Tag, POBLogStrings.ForceRefresh);
            return POBUForceRefresh(bannerViewPtr);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Create banner instance with provided placement details
        /// </summary>
        /// <param name="publisherId">OpenWrap's publisher id</param>
        /// <param name="profileId">OpenWrap's profile id</param>
        /// <param name="adUnitId">OpenWrap's ad unit id</param>
        /// <param name="adSize">Banner's ad ad sizes</param>
        private void CreateBannerView(
            string publisherId,
            int profileId,
            string adUnitId,
            POBAdSize adSize)
        {
            bannerViewClientPtr = (IntPtr)GCHandle.Alloc(this);
            POBLog.Info(Tag, POBLogStrings.CreateBanner);
            bannerViewPtr = POBUCreateBanner(bannerViewClientPtr, publisherId, profileId, adUnitId, adSize.GetWidth(), adSize.GetHeight());
            POBLog.Info(Tag, POBLogStrings.SetBannerViewCallbacks);
            POBUSetBannerViewCallbacks(bannerViewPtr,
                BannerViewDidLoadAdCallback,
                BannerViewDidFailToLoadAd,
                BannerViewDidPresentScreen,
                BannerViewDidDismissScreen,
                BannerViewWillLeaveApplication,
                BannerViewDidClickAd);
        }

        /// <summary>
        /// Method to convert IntPtr to POBBannerViewClient
        /// </summary>
        /// <param name="bannerViewClient">IntPtr of POBBannerViewClient</param>
        /// <returns>POBBannerViewClient type instance</returns>
        private static POBBannerViewClient IntPtrToPOBBannerViewClient(IntPtr bannerViewClient)
        {
            if (bannerViewClient != IntPtr.Zero)
            {
                GCHandle handle = (GCHandle)bannerViewClient;
                return handle.Target as POBBannerViewClient;
            }
            return null;
        }
        #endregion
    }
}
#endif
