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
    /// Android client of RewardedAd which communicates with java native code
    /// </summary>
    internal class POBRewardedAdClient: AndroidJavaProxy, IPOBRewardedAdClient
    {
        #region Private variables
        private readonly string Tag = "POBRewardedAdClient";
        private readonly AndroidJavaObject androidRewardedAd;
        #endregion
        #region Constructors/Destructor
        /// Private constructor to avoid direct instance creation, To create instance of
        /// POBRewardedAdClient use POBRewardedAdClient.GetRewardedAdClient() method.
        /// </summary>
        /// <param name="androidRewardedAd"> Valid instance of AndroidJavaObject which is proxy object of
        /// com.pubmatic.unity.openwrapsdk.POBUnityRewardedAd </param>
        private POBRewardedAdClient(AndroidJavaObject androidRewardedAd) : base(POBConstants.POBRewardedAdCallbackInterfaceName)
        {           
            this.androidRewardedAd = androidRewardedAd;
            // Set listener to receive Rewarded Ad event calls
            this.androidRewardedAd.Call("setListener", this);
            // Initialize the event dispatcher
            POBEventsDispatcher.Initialize();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBRewardedAdClient()
        {
            Destroy();
        }
        #endregion

        /// Returns POBRewardedAdClient instance if all the params are valid else it would return
        /// null
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="profileId"></param>
        /// <param name="adUnitId"></param>
        /// <returns></returns>
        public static POBRewardedAdClient GetRewardedAdClient(string publisherId, int profileId, string adUnitId)
        {
            // Get activity instance
            AndroidJavaObject activity = POBAndroidUtils.getActivity();
            // Android Java class for RewardedAd ad
            AndroidJavaClass androidRewardedAdClass = new AndroidJavaClass(POBConstants.POBRewardedAdClassName);

            // Get valid instance of RewardedAd using getInstance native method.
            AndroidJavaObject androidRewardedAd = androidRewardedAdClass.CallStatic<AndroidJavaObject>("getInstance",
                activity, publisherId, profileId, adUnitId);
            if (androidRewardedAd != null){
                return new POBRewardedAdClient(androidRewardedAd);
            }
            return null;
        }

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


        #region IPOBRewardedAdClient APIs

        /// <summary>
        /// Getter for OpenWrap rewarded ad POBBid
        /// </summary>
        /// <returns>Instance of POBBid</returns>
        public IPOBBid GetBid()
        {
            return new POBBidClient(this.androidRewardedAd.Call<AndroidJavaObject>("getBid"));
        }

        /// <summary>
        /// To get the rewarded ad impression
        /// </summary>
        /// <returns>Instance of type IPOBImpression</returns>
        public IPOBImpression GetImpression()
        {
            return new POBImpressionClient(this.androidRewardedAd.Call<AndroidJavaObject>("getImpression"));
        }

        /// <summary>
        /// To get the rewarded ad request
        /// </summary>
        /// <returns>Instance of type IPOBRequest</returns>
        public IPOBRequest GetRequest()
        {
            return new POBRequestClient(this.androidRewardedAd.Call<AndroidJavaObject>("getAdRequest"));
        }

        /// <summary>
        /// To check if ad is ready to show
        /// </summary>
        /// <returns>is ready status</returns>
        public bool IsReady()
        {
            POBLog.Info(Tag, POBLogStrings.ClientIsReadyLog);
            return this.androidRewardedAd.Call<bool>("isReady");
        }

        /// <summary>
        /// To load rewarded ad 
        /// </summary>
        public void LoadAd()
        {
            POBLog.Info(Tag, POBLogStrings.ClientLoadAdLog);
            this.androidRewardedAd.Call("loadAd");
        }

        /// <summary>
        /// To show rewarded ad
        /// </summary>
        public void ShowAd()
        {
            POBLog.Info(Tag, POBLogStrings.ClientShowAdLog);
            this.androidRewardedAd.Call("show");
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
            this.androidRewardedAd.Call("setSkipAlertDialogInfo", title, message, resumeTitle, closeTitle);
        }

        /// <summary>
        ///  To clean up rewarded ad
        /// </summary>
        public void Destroy()
        {
            POBLog.Info(Tag, POBLogStrings.ClientDestroyLog);
            this.androidRewardedAd.Call("destroy");
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
        /// Notifies when an Ad has completed the minimum required viewing, and user should be rewarded.
        /// </summary>
        /// <param name="reward">AndroidJavaObject of POBReward</param>
        public void onReceiveReward(AndroidJavaObject reward)
        {
            if (OnReceiveReward != null)
            {
                POBRewardEventArgs rewardEventArgs = POBAndroidUtils.GetPOBRewardEventArgs(reward);
                POBEventsDispatcher.ScheduleInUpdate(() => {
                    OnReceiveReward(this, rewardEventArgs);
                });
                
                
            }
        }
        #endregion
    }
}
#endif