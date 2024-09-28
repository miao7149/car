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

using System.Collections.Generic;
using UnityEngine;
using OpenWrapSDK.Common;

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Android client for POBBid class, which provides the bid information. It will be common across all the ad formats in Android.
    /// </summary>
    internal class POBBidClient : IPOBBid
    {
        #region Private Variables
        private readonly string Tag = "POBBidClient";
        private readonly AndroidJavaObject bidObject;
        #endregion

        /// <summary>
        /// Constructor of POBBid Client
        /// </summary>
        /// <param name="javaObject">AndroidJavaObject of POBBid</param>
        internal POBBidClient(AndroidJavaObject javaObject)
        {
            bidObject = javaObject;   
        }

        /// <summary>
        ///  Returns creative id of bid
        /// </summary>
        /// <returns>creative id</returns>
        public string GetCreativeId()
        {
            if (bidObject != null)
            {
                return bidObject.Call<string>("getCreativeId");
            }
            return null;
        }

        /// <summary>
        /// Returns creative tag of bid
        /// </summary>
        /// <returns>creative tag</returns>
        public string GetCreativeTag()
        {
            if (bidObject != null)
            {
                return bidObject.Call<string>("getCreative");
            }
            return null;
        }

        /// <summary>
        /// Returns creative type of bid
        /// </summary>
        /// <returns>creative type</returns>
        public string GetCreativeType()
        {
            if (bidObject != null)
            {
                return bidObject.Call<string>("getCreativeType");
            }
            return null;
        }

        /// <summary>
        /// Returns deal id of bid
        /// </summary>
        /// <returns>deal id</returns>
        public string GetDealId()
        {
            if (bidObject != null)
            {
                return bidObject.Call<string>("getDealId");
            }
            return null;
        }

        /// <summary>
        /// Returns if bid has won or not
        /// </summary>
        /// <returns>boolean vanlue if bid win</returns>
        public bool GetHasWon()
        {
            if (bidObject != null)
            {
                return bidObject.Call<bool>("hasWon");
            }
            return false;
        }

        /// <summary>
        /// Returns height of creative of bid
        /// </summary>
        /// <returns>height</returns>
        public int GetHeight()
        {
            if (bidObject != null)
            {
                return bidObject.Call<int>("getHeight");
            }
            return 0;
        }

        /// <summary>
        /// Returns the name of the winning partner
        /// </summary>
        /// <returns>Name of the winning partner</returns>
        public string GetPartner()
        {
            if (bidObject != null)
            {
                return bidObject.Call<string>("getPartnerName");
            }
            return null;
        }

        /// <summary>
        /// Returns Net Price/bid value
        /// This method is updated to return net price from OW SDK v2.4.0.
        /// </summary>
        /// <returns>net ecm/price</returns>
        public double GetPrice()
        {
            if (bidObject != null)
            {
                return bidObject.Call<double>("getPrice");
            }
            return 0;
        }

        /// <summary>
        /// Returns the partner id of bid 
        /// </summary>
        /// <returns>partner id</returns>
        public string GetPubmaticPartnerId()
        {
            if (bidObject != null)
            {
                return bidObject.Call<string>("getPartnerId");
            }
            return null;
        }

        /// <summary>
        /// Returns winning  status of bid
        /// </summary>
        /// <returns>status</returns>
        public int GetStatus()
        {
            if (bidObject != null)
            {
                return bidObject.Call<int>("getStatus");
            }
            return 0;
        }

        /// <summary>
        /// Returns targeting info of bid
        /// </summary>
        /// <returns>targeting info</returns>
        public Dictionary<string, string> GetTargetingInfo()
        {
            if (bidObject != null)
            {
                AndroidJavaObject targetingInfo = bidObject.Call<AndroidJavaObject>("getTargetingInfo");
                Dictionary<string, string> targetingMap = POBAndroidUtils.ConvertJavaMapToDictionary<string, string>(targetingInfo);

                return targetingMap;
            }
            return null;

        }

        /// <summary>
        /// Returns width of bid creative
        /// </summary>
        /// <returns>width</returns>
        public int GetWidth()
        {
            if (bidObject != null)
            {
                return bidObject.Call<int>("getWidth");
            }
            return 0;
        }

        /// <summary>
        /// Returns if bid expired or not
        /// </summary>
        /// <returns>bid expiry state</returns>
        public bool IsExpired()
        {
            if (bidObject != null)
            {
                POBLog.Info(Tag, POBLogStrings.ClientIsExpiredLog);
                return bidObject.Call<bool>("isExpired");
            }
            return false;
        }

        /// <summary>
        /// Sets the current bid as winnging bid
        /// </summary>
        /// <param name="hasWon">bid win status</param>
        public void SetHasWon(bool hasWon)
        {
            if (bidObject != null)
            {
                bidObject.Call("setHasWon", hasWon);
            }
        }

        /// <summary>
        /// Return id of bid
        /// </summary>
        /// <returns>bid id</returns>
        public string GetBidId()
        {
            if (bidObject != null)
            {
                return bidObject.Call<string>("getId");
            }
            return null;
        }

        /// <summary>
        /// Returns gross price of bid 
        /// </summary>
        /// <returns>gross price</returns>
        public double GetGrossPrice()
        {
            if (bidObject != null)
            {
                return bidObject.Call<double>("getGrossPrice");
            }
            return 0;
        }

        public override string ToString()
        {
            if (bidObject != null)
            {
                return bidObject.Call<string>("toString");
            }
            return null;
        }
    }
}
#endif