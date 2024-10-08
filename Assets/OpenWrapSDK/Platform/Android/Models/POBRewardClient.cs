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

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Android reward client to hold information of rewards.
    /// </summary>
    internal class POBRewardClient : IPOBReward
    {
        // Reference to Android's POBRequest
        private readonly AndroidJavaObject rewardObject;

        private readonly string currencyType;

        private readonly int amount;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reward">AndroidJavaObject of POBReward</param>
        internal POBRewardClient(AndroidJavaObject javaObject)
        {
            if(javaObject != null)
            {
                rewardObject = javaObject;
                currencyType = rewardObject.Call<string>("getCurrencyType");
                amount = rewardObject.Call<int>("getAmount");
            }
        }

        /// <summary>
        /// Call native equals() method with rewardObject as argument.
        /// </summary>
        /// <param name="reward">Contains the instance of POBRewardClient</param>
        /// <returns>true if both instance are equal</returns>
        public bool Equals(IPOBReward reward)
        {
            if(rewardObject != null)
            {
                POBRewardClient rewardClient = (POBRewardClient)reward;
                return rewardObject.Call<bool>("equals",rewardClient.rewardObject);
            }
            return false;
        }

        /// <summary>
        /// Getter for Reward amount
        /// </summary>
        /// <returns>Reward amount</returns>
        public int GetAmount()
        {
            return amount;
        }

        /// <summary>
        /// Getter for Reward currency type
        /// </summary>
        /// <returns>Reward currency type as string</returns>
        public string GetCurrencyType()
        {
            return currencyType;
        }

        /// <summary>
        /// Calls the native toString() method.
        /// </summary>
        /// <returns>String containing currency type and amount</returns>
        public override string ToString()
        {
            if (rewardObject != null)
            {
                return rewardObject.Call<string>("toString");
            }
            return null;
        }
    }
}
#endif