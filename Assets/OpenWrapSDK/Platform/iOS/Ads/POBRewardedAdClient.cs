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
    /// iOS Specific rewarded ad client interacting with iOS Plugins
    /// </summary>
    internal class POBRewardedAdClient : IPOBRewardedAdClient
    {
        #region Private variables
        private IntPtr rewardedClientPtr = IntPtr.Zero;
        private IntPtr rewardedPtr = IntPtr.Zero;
        private readonly IntPtr impressionPtr = IntPtr.Zero;
        private readonly IntPtr requestPtr = IntPtr.Zero;
        private readonly IPOBImpression impression;
        private readonly IPOBRequest request;
        private readonly POBBidClient bid;
        private readonly string Tag = "POBRewardedAdClient";
        #endregion

        #region Constructors/Destructor
        /// <summary>
        /// Method to return POBRewardedAdClient instance
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="profileId"></param>
        /// <param name="adUnitId"></param>
        /// <returns></returns>
        public static POBRewardedAdClient GetRewardedAdClient(string publisherId, int profileId, string adUnitId)
        {
            //create native rewarded ad
            IntPtr rewardedPtr = POBUCreateRewardedAd(publisherId, profileId, adUnitId);
            if (rewardedPtr != null)
            {
                //create rewarded ad client 
                return new POBRewardedAdClient(rewardedPtr);
            }           
            return null;
        }


        internal POBRewardedAdClient(IntPtr rewardedNativePtr)
        {
            rewardedPtr = rewardedNativePtr;
            SetupRewardedAd();
            // Get request
            POBLog.Info(Tag, POBLogStrings.GetRewardedAdRequest);
            requestPtr = POBUGetRewardedAdRequest(rewardedPtr);
            request = new POBRequestClient(requestPtr);

            // POBBid
            bid = new POBBidClient();

            // Get impression
            POBLog.Info(Tag, POBLogStrings.GetRewardedAdImpression);
            impressionPtr = POBUGetRewardedAdImpression(rewardedPtr);
            impression = new POBImpressionClient(impressionPtr);
        }

        ~POBRewardedAdClient()
        {
            Destroy();
        }
#endregion

#region iOS Plugin imports
        [DllImport("__Internal")]
        internal static extern IntPtr POBUCreateRewardedAd(string publisherId, int profileId, string adUnitId);

        [DllImport("__Internal")]
        internal static extern void POBURewardedAdSetClient(IntPtr rewardedAd, IntPtr rewardedAdClient);

            [DllImport("__Internal")]
        internal static extern void POBUSetRewardedAdCallbacks(IntPtr rewardedAd,
                                  POBUAdCallback adReceivedCallback,
                                  POBUAdFailureCallback adFailedtoLoadCallback,
                                  POBUAdFailureCallback adFailedToShowCallback,
                                  POBUAdCallback didPresentCallback,
                                  POBUAdCallback didDismissCallback,
                                  POBUAdCallback willLeaveAppCallback,
                                  POBUAdCallback didClickAdCallback,
                                  POBUAdCallback didExpireAdCallback,
                                  POBUAdRewardCallback shouldRewardAdCallback);

        [DllImport("__Internal")]
        internal static extern void POBULoadRewardedAd(IntPtr rewardedAd);

        [DllImport("__Internal")]
        internal static extern void POBUShowRewardedAd(IntPtr rewardedAd);

        [DllImport("__Internal")]
        internal static extern void POBUSetRewardedSkipAlertInfo(string title, string message, string closeTitle, string resumeTitle, IntPtr rewardedPtr);

        [DllImport("__Internal")]
        internal static extern void POBUDestroyRewardedAd(IntPtr rewardedAd);

        [DllImport("__Internal")]
        internal static extern bool POBUIsRewardedAdReady(IntPtr rewardedAd);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetRewardedAdRequest(IntPtr rewardedAd);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetRewardedBid(IntPtr rewardedAd);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetRewardedAdImpression(IntPtr rewardedAd);
#endregion

#region IPOBRewardedAdClient Callbacks
        // Callbacks to be set by POBRewardedAd
        public event EventHandler<EventArgs> OnAdLoaded;
        public event EventHandler<POBErrorEventArgs> OnAdFailedToLoad;
        public event EventHandler<POBErrorEventArgs> OnAdFailedToShow;
        public event EventHandler<EventArgs> OnAppLeaving;
        public event EventHandler<EventArgs> OnAdOpened;
        public event EventHandler<EventArgs> OnAdClosed;
        public event EventHandler<EventArgs> OnAdClicked;
        public event EventHandler<EventArgs> OnAdExpired;

        /// <summary>
        /// Callback method notifies when an Ad has completed the minimum required viewing, and user should be rewarded
        /// </summary>
        public event EventHandler<POBRewardEventArgs> OnReceiveReward;

        // Declaration of delegates received from iOS rewarded plugin
        internal delegate void POBUAdCallback(IntPtr rewardedAdClient);
        internal delegate void POBUAdFailureCallback(IntPtr rewardedAdClient, int errorCode, string errorMessage);
        internal delegate void POBUAdRewardCallback(IntPtr rewardedAdClient, int rewardVal, string currency);

        // Ad received callback received from iOS rewarded plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void RewardedAdDidReceiveCallback(IntPtr rewardedAdClient)
        {
            POBRewardedAdClient rwrdClient = IntPtrToPOBRewardedAdClient(rewardedAdClient);
            if (rwrdClient != null && rwrdClient.OnAdLoaded != null)
            {
                rwrdClient.OnAdLoaded(rwrdClient, EventArgs.Empty);
            }
        }

        // Ad failed to load callback received from iOS rewardedAd plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdFailureCallback))]
        private static void RewardedAdDidFailToLoad(IntPtr rewardedAdClient, int errorCode, string errorMessage)
        {
            POBRewardedAdClient rwrdClient = IntPtrToPOBRewardedAdClient(rewardedAdClient);
            if (rwrdClient != null && rwrdClient.OnAdFailedToLoad != null)
            {
                rwrdClient.OnAdFailedToLoad(rwrdClient, POBIOSUtils.ConvertToPOBErrorEventArgs(errorCode, errorMessage));
            }
        }

        // Ad failed to show callback received from iOS Rewarded plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdFailureCallback))]
        private static void RewardedAdDidFailToShow(IntPtr rewardedAdClient, int errorCode, string errorMessage)
        {
            POBRewardedAdClient rwrdClient = IntPtrToPOBRewardedAdClient(rewardedAdClient);
            if (rwrdClient != null && rwrdClient.OnAdFailedToShow != null)
            {
                rwrdClient.OnAdFailedToShow(rwrdClient, POBIOSUtils.ConvertToPOBErrorEventArgs(errorCode, errorMessage));
            }
        }

        // Callback of screen presented over the ad due to ad click, received from iOS rewarded plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void RewardedAdDidPresentScreen(IntPtr rewardedAdClient)
        {
            POBRewardedAdClient rwrdClient = IntPtrToPOBRewardedAdClient(rewardedAdClient);
            if (rwrdClient != null && rwrdClient.OnAdOpened != null)
            {
                rwrdClient.OnAdOpened(rwrdClient, EventArgs.Empty);
            }
        }

        // Callback of presented screen dismissed, received from iOS RewardedAd plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void RewardedAdDidDismissScreen(IntPtr rewardedAdClient)
        {
            POBRewardedAdClient rwrdClient = IntPtrToPOBRewardedAdClient(rewardedAdClient);
            if (rwrdClient != null && rwrdClient.OnAdClosed != null)
            {
                rwrdClient.OnAdClosed(rwrdClient, EventArgs.Empty);
            }
        }

        // Callback of presented screen dismissed, received from iOS RewardedAd plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void RewardedAdWillLeaveApplication(IntPtr rewardedAdClient)
        {
            POBRewardedAdClient rwrdClient = IntPtrToPOBRewardedAdClient(rewardedAdClient);
            if (rwrdClient != null && rwrdClient.OnAppLeaving != null)
            {
                rwrdClient.OnAppLeaving(rwrdClient, EventArgs.Empty);
            }
        }

        // Ad clicked callback received from iOS RewardedAd plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void RewardedAdDidClickAd(IntPtr rewardedAdClient)
        {
            POBRewardedAdClient rwrdClient = IntPtrToPOBRewardedAdClient(rewardedAdClient);
            if (rwrdClient != null && rwrdClient.OnAdClicked != null)
            {
                rwrdClient.OnAdClicked(rwrdClient, EventArgs.Empty);
            }
        }

        // Ad expired callback received from iOS RewardedAd plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdCallback))]
        private static void RewardedAdDidExpireAd(IntPtr rewardedAdClient)
        {
            POBRewardedAdClient rwrdClient = IntPtrToPOBRewardedAdClient(rewardedAdClient);
            if (rwrdClient != null && rwrdClient.OnAdExpired != null)
            {
                rwrdClient.OnAdExpired(rwrdClient, EventArgs.Empty);
            }
        }

        // Should reward callback received from iOS RewardedAd plugin
        [AOT.MonoPInvokeCallback(typeof(POBUAdRewardCallback))]
        private static void RewardedShouldRewardUser(IntPtr rewardedAdClient, int rewardVal, string currency)
        {
            POBRewardedAdClient rwrdClient = IntPtrToPOBRewardedAdClient(rewardedAdClient);
            if (rwrdClient != null && rwrdClient.OnReceiveReward != null)
            {
                rwrdClient.OnReceiveReward(rwrdClient, POBIOSUtils.ConvertToPOBRewardEventArgs(rewardVal,currency));
            }
        }

#endregion

#region IPOBRewardedAdClient Public APIs
        /// <summary>
        /// Method to get the RewardedAd bid
        /// </summary>
        /// <returns>Instance of POBBid</returns>
        public IPOBBid GetBid()
        {
            IntPtr bidPtr = POBUGetRewardedBid(rewardedPtr);
            bid.SetBid(bidPtr);
            return bid;
        }

        /// <summary>
        /// Method to get the rewarded impression
        /// </summary>
        /// <returns>Instance of type IPOBImpression</returns>
        public IPOBImpression GetImpression()
        {
            return impression;
        }

        /// <summary>
        /// Method to get the RewardedAd request
        /// </summary>
        /// <returns>Instance of type IPOBRequest</returns>
        public IPOBRequest GetRequest()
        {
            return request;
        }

        /// <summary>
        /// Method to check if RewardedAd ad is ready to show
        /// </summary>
        /// <returns>true if it is ready, else false</returns>
        public bool IsReady()
        {
            return POBUIsRewardedAdReady(rewardedPtr);
        }

        /// <summary>
        /// Method to load rewarded
        /// </summary>
        public void LoadAd()
        {
            POBLog.Info(Tag, POBLogStrings.LoadRewardedAd);
            POBULoadRewardedAd(rewardedPtr);
        }

        /// <summary>
        /// Method to show loaded rewarded
        /// </summary>
        public void ShowAd()
        {
            POBLog.Info(Tag,POBLogStrings.ShowRewardedAd);
            POBUShowRewardedAd(rewardedPtr);
        }

        /// <summary>
        /// Method to set skip alert info 
        /// </summary>
        /// <param name="title">skip alert title</param>
        /// <param name="message">skip alert message</param>
        /// <param name="closeTitle">close button title for skip alert</param>
        /// <param name="resumeTitle">resume button title for skip alert</param>
        public void SetSkipAlertInfo(string title, string message, string closeTitle, string resumeTitle)
        {
            POBLog.Info(Tag, POBLogStrings.SetRewardedSkipAlertInfo);
            POBUSetRewardedSkipAlertInfo(title, message, closeTitle, resumeTitle, rewardedPtr);
        }

        /// <summary>
        /// Method to cleanup rewarded instances, references.
        /// </summary>
        public void Destroy()
        {
            if (rewardedPtr != IntPtr.Zero)
            {
                POBLog.Info(Tag, POBLogStrings.DestroyRewardedAd);
                POBUDestroyRewardedAd(rewardedPtr);
                rewardedPtr = IntPtr.Zero;
            }

            if (rewardedClientPtr != IntPtr.Zero)
            {
                ((GCHandle)rewardedClientPtr).Free();
                rewardedClientPtr = IntPtr.Zero;
            }
        }
#endregion

#region Private methods
        /// <summary>
        /// Create rewarded instance with provided placement details
        /// </summary>
        private void SetupRewardedAd()
        {
            rewardedClientPtr = (IntPtr)GCHandle.Alloc(this);
            //Set client ptr to the native obj
            POBLog.Info(Tag, POBLogStrings.RewardedAdSetClient);
            POBURewardedAdSetClient(rewardedPtr, rewardedClientPtr);
            //rewardedPtr = POBUCreateRewardedAd(rewardedClientPtr, publisherId, profileId, adUnitId);
            POBUSetRewardedAdCallbacks(rewardedPtr,
                RewardedAdDidReceiveCallback,
                RewardedAdDidFailToLoad,
                RewardedAdDidFailToShow,
                RewardedAdDidPresentScreen,
                RewardedAdDidDismissScreen,
                RewardedAdWillLeaveApplication,
                RewardedAdDidClickAd,
                RewardedAdDidExpireAd,
                RewardedShouldRewardUser);
        }

        /// <summary>
        /// Method to convert IntPtr to POBRewardedAdClient
        /// </summary>
        /// <param name="rewardedAdClient">IntPtr of POBRewardedAdClient</param>
        /// <returns>POBRewardedAdClient type instance</returns>
        private static POBRewardedAdClient IntPtrToPOBRewardedAdClient(IntPtr rewardedAdClient)
        {
            if (rewardedAdClient != IntPtr.Zero)
            {
                GCHandle handle = (GCHandle)rewardedAdClient;
                return handle.Target as POBRewardedAdClient;
            }
            return null;
        }
        #endregion
    }
}
#endif