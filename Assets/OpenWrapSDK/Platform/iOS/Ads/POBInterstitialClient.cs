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
    /// iOS Specific interstitial client interacting with iOS Plugins
    /// </summary>
    internal class POBInterstitialClient : IPOBInterstitialClient
    {
        #region Private variables
        private IntPtr interstitiaClientPtr = IntPtr.Zero;
        private IntPtr interstitialPtr = IntPtr.Zero;
        private readonly IntPtr impressionPtr = IntPtr.Zero;
        private readonly IntPtr requestPtr = IntPtr.Zero;
        private IPOBImpression impression;
        private readonly IPOBRequest request;
        private readonly POBBidClient bid;
        private readonly string Tag = "POBInterstitialClient";
        #endregion

        #region Constructors/Destructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="publisherId">OpenWrap's publisher id</param>
        /// <param name="profileId">OpenWrap's profile id</param>
        /// <param name="adUnitId">OpenWrap's ad unit id</param>
        internal POBInterstitialClient(string publisherId, int profileId, string adUnitId)
        {
            CreateInterstitial(publisherId, profileId, adUnitId);

            // Get request
            POBLog.Info(Tag, POBLogStrings.GetInterstitialRequest);
            requestPtr = POBUGetInterstitialRequest(interstitialPtr);
            request = new POBRequestClient(requestPtr);

            // POBBid
            bid = new POBBidClient();

            // Get impression
            POBLog.Info(Tag, POBLogStrings.GetInterstitialImpression);
            impressionPtr = POBUGetInterstitialImpression(interstitialPtr);
            impression = new POBImpressionClient(impressionPtr);
        }

        ~POBInterstitialClient()
        {
            Destroy();
        }
        #endregion

        #region iOS Plugin imports
        [DllImport("__Internal")]
        internal static extern IntPtr POBUCreateInterstitial(IntPtr interstitialClient,
            string publisherId,
            int profileId,
            string adUnitId);

        [DllImport("__Internal")]
        internal static extern void POBUSetInterstitialCallbacks(IntPtr interstitial,
                                  POBUAdCallback adReceivedCallback,
                                  POBUAdFailureCallback adFailedtoLoadCallback,
                                  POBUAdFailureCallback adFailedToShowCallback,
                                  POBUAdCallback didPresentCallback,
                                  POBUAdCallback didDismissCallback,
                                  POBUAdCallback willLeaveAppCallback,
                                  POBUAdCallback didClickAdCallback,
                                  POBUAdCallback didExpireAdCallback);

        [DllImport("__Internal")]
        internal static extern void POBUSetVideoInterstitialCallbacks(IntPtr interstitial,
                                       POBUAdCallback didFinishVideoCallback);

        [DllImport("__Internal")]
        internal static extern void POBULoadInterstitial(IntPtr interstitial);

        [DllImport("__Internal")]
        internal static extern void POBUShowInterstitial(IntPtr interstitial);

        [DllImport("__Internal")]
        internal static extern void POBUDestroyInterstitial(IntPtr interstitial);

        [DllImport("__Internal")]
        internal static extern bool POBUIsInterstitialReady(IntPtr interstitial);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetInterstitialRequest(IntPtr interstitial);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetInterstitialBid(IntPtr interstitial);
        
        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetInterstitialImpression(IntPtr interstitial);
        #endregion

        #region IPOBInterstitialClient Callbacks
        // Callbacks to be set by POBInterstitial
        public event EventHandler<EventArgs> OnAdLoaded;
        public event EventHandler<POBErrorEventArgs> OnAdFailedToLoad;
        public event EventHandler<POBErrorEventArgs> OnAdFailedToShow;
        public event EventHandler<EventArgs> OnAppLeaving;
        public event EventHandler<EventArgs> OnAdOpened;
        public event EventHandler<EventArgs> OnAdClosed;
        public event EventHandler<EventArgs> OnAdClicked;
        public event EventHandler<EventArgs> OnAdExpired;
        public event EventHandler<EventArgs> OnVideoPlaybackCompleted;

        // Declaration of delegates received from iOS interstitial plugin
        internal delegate void POBUAdCallback(IntPtr interstitialClient);
        internal delegate void POBUAdFailureCallback(IntPtr interstitialClient, int errorCode, string errorMessage);
        
        // Ad received callback received from iOS interstitial plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void InterstitialDidReceiveAdCallback(IntPtr interstitialClient)
        {
            POBInterstitialClient instlClient = IntPtrToPOBInterstitialClient(interstitialClient);
            if (instlClient != null && instlClient.OnAdLoaded != null)
            {
                instlClient.OnAdLoaded(instlClient, EventArgs.Empty);
            }
        }

        // Ad failed to load callback received from iOS interstitial plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdFailureCallback))]
        private static void InterstitialDidFailToLoadAd(IntPtr interstitialClient, int errorCode, string errorMessage)
        {
            POBInterstitialClient instlClient = IntPtrToPOBInterstitialClient(interstitialClient);
            if (instlClient != null && instlClient.OnAdFailedToLoad != null)
            {
                instlClient.OnAdFailedToLoad(instlClient, POBIOSUtils.ConvertToPOBErrorEventArgs(errorCode, errorMessage));
            }
        }

        // Ad failed to show callback received from iOS interstitial plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdFailureCallback))]
        private static void InterstitialDidFailToShowAd(IntPtr interstitialClient, int errorCode, string errorMessage)
        {
            POBInterstitialClient instlClient = IntPtrToPOBInterstitialClient(interstitialClient);
            if (instlClient != null && instlClient.OnAdFailedToShow != null)
            {
                instlClient.OnAdFailedToShow(instlClient, POBIOSUtils.ConvertToPOBErrorEventArgs(errorCode, errorMessage));
            }
        }

        // Callback of screen presented over the ad due to ad click, received from iOS interstitial plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void InterstitialDidPresentScreen(IntPtr interstitialClient)
        {
            POBInterstitialClient instlClient = IntPtrToPOBInterstitialClient(interstitialClient);
            if (instlClient != null && instlClient.OnAdOpened != null)
            {
                instlClient.OnAdOpened(instlClient, EventArgs.Empty);
            }
        }

        // Callback of presented screen dismissed, received from iOS interstitial plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void InterstitialDidDismissScreen(IntPtr interstitialClient)
        {
            POBInterstitialClient instlClient = IntPtrToPOBInterstitialClient(interstitialClient);
            if (instlClient != null && instlClient.OnAdClosed != null)
            {
                instlClient.OnAdClosed(instlClient, EventArgs.Empty);
            }
        }

        // Callback of presented screen dismissed, received from iOS interstitial plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void InterstitialWillLeaveApplication(IntPtr interstitialClient)
        {
            POBInterstitialClient instlClient = IntPtrToPOBInterstitialClient(interstitialClient);
            if (instlClient != null && instlClient.OnAppLeaving != null)
            {
                instlClient.OnAppLeaving(instlClient, EventArgs.Empty);
            }
        }

        // Ad clicked callback received from iOS interstitial plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void InterstitialDidClickAd(IntPtr interstitialClient)
        {
            POBInterstitialClient instlClient = IntPtrToPOBInterstitialClient(interstitialClient);
            if (instlClient != null && instlClient.OnAdClicked != null)
            {
                instlClient.OnAdClicked(instlClient, EventArgs.Empty);
            }
        }

        // Ad expired callback received from iOS interstitial plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void InterstitialDidExpireAd(IntPtr interstitialClient)
        {
            POBInterstitialClient instlClient = IntPtrToPOBInterstitialClient(interstitialClient);
            if (instlClient != null && instlClient.OnAdExpired != null)
            {
                instlClient.OnAdExpired(instlClient, EventArgs.Empty);
            }
        }

        // Callback of video ad finished playing the playback, received from iOS interstitial plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void InterstitialVideoPlaybackCompleted(IntPtr interstitialClient)
        {
            POBInterstitialClient instlClient = IntPtrToPOBInterstitialClient(interstitialClient);
            if (instlClient != null && instlClient.OnVideoPlaybackCompleted != null)
            {
                instlClient.OnVideoPlaybackCompleted(instlClient, EventArgs.Empty);
            }
        }

        #endregion

        #region IPOBInterstitialClient Public APIs
        /// <summary>
        /// Method to get the interstitial bid
        /// </summary>
        /// <returns>Instance of POBBid</returns>
        public IPOBBid GetBid()
        {
            IntPtr bidPtr = POBUGetInterstitialBid(interstitialPtr);
            bid.SetBid(bidPtr);
            return bid;
        }

        /// <summary>
        /// Method to get the interstitial impression
        /// </summary>
        /// <returns>Instance of type IPOBImpression</returns>
        public IPOBImpression GetImpression()
        {
            return impression;
        }

        /// <summary>
        /// Method to set the updated impression over interstitial
        /// </summary>
        /// <param name="impression">Instance of type IPOBImpression</param>
        public void SetImpression(IPOBImpression impression)
        {
            this.impression = impression;
            // TODO: Set impression method of plugin
        }

        /// <summary>
        /// Method to get the interstitial request
        /// </summary>
        /// <returns>Instance of type IPOBRequest</returns>
        public IPOBRequest GetRequest()
        {
            return request;
        }

        /// <summary>
        /// Method to check if interstitial ad is ready to show
        /// </summary>
        /// <returns>true if it is ready, else false</returns>
        public bool IsReady()
        {
            return POBUIsInterstitialReady(interstitialPtr);
        }

        /// <summary>
        /// Method to load interstitial
        /// </summary>
        public void LoadAd()
        {
            POBLog.Info(Tag, POBLogStrings.LoadInterstitial);
            POBULoadInterstitial(interstitialPtr);
        }

        /// <summary>
        /// Method to show loaded interstitial
        /// </summary>
        public void ShowAd()
        {
            POBLog.Info(Tag, POBLogStrings.ShowInterstitial);
            POBUShowInterstitial(interstitialPtr);
        }

        /// <summary>
        /// Method to cleanup interstitial instances, references.
        /// </summary>
        public void Destroy()
        {
            if (interstitialPtr != IntPtr.Zero)
            {
                POBLog.Info(Tag, POBLogStrings.DestroyInterstitial);
                POBUDestroyInterstitial(interstitialPtr);
                interstitialPtr = IntPtr.Zero;
            }

            if (interstitiaClientPtr != IntPtr.Zero)
            {
                ((GCHandle)interstitiaClientPtr).Free();
                interstitiaClientPtr = IntPtr.Zero;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Create interstitial instance with provided placement details
        /// </summary>
        /// <param name="publisherId">OpenWrap's publisher id</param>
        /// <param name="profileId">OpenWrap's profile id</param>
        /// <param name="adUnitId">OpenWrap's ad unit id</param>
        private void CreateInterstitial(string publisherId, int profileId, string adUnitId)
        {
            interstitiaClientPtr = (IntPtr)GCHandle.Alloc(this);
            POBLog.Info(Tag, POBLogStrings.CreateInterstitial);
            interstitialPtr = POBUCreateInterstitial(interstitiaClientPtr, publisherId, profileId, adUnitId);
            POBUSetInterstitialCallbacks(interstitialPtr,
                InterstitialDidReceiveAdCallback,
                InterstitialDidFailToLoadAd,
                InterstitialDidFailToShowAd,
                InterstitialDidPresentScreen,
                InterstitialDidDismissScreen,
                InterstitialWillLeaveApplication,
                InterstitialDidClickAd,
                InterstitialDidExpireAd);
            POBUSetVideoInterstitialCallbacks(interstitialPtr, InterstitialVideoPlaybackCompleted);
        }

        /// <summary>
        /// Method to convert IntPtr to POBInterstitialClient
        /// </summary>
        /// <param name="interstitialClient">IntPtr of POBInterstitialClient</param>
        /// <returns>POBInterstitialClient type instance</returns>
        private static POBInterstitialClient IntPtrToPOBInterstitialClient(IntPtr interstitialClient)
        {
            if (interstitialClient != IntPtr.Zero)
            {
                GCHandle handle = (GCHandle)interstitialClient;
                return handle.Target as POBInterstitialClient;
            }
            return null;
        }
        #endregion
    }
}
#endif