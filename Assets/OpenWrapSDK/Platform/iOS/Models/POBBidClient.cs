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
* information that is confidential and/or proprietary, and is a trade secret, of  PubMatic.   ANY REPRODUCTION, MODIFICATION, DISTRIBUTION,  PERFORMANCE,
* OR DISPLAY OF OR THROUGH USE  OF THIS  SOURCE CODE  WITHOUT  THE EXPRESS WRITTEN CONSENT OF PUBMATIC IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE
* LAWS AND INTERNATIONAL TREATIES.  THE RECEIPT OR POSSESSION OF  THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT  MAY DESCRIBE, IN WHOLE OR IN PART.
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenWrapSDK.Common;

namespace OpenWrapSDK.iOS
{
    /// <summary>
    /// iOS client for POBBid class, which provides the bid information. It will be common across all the ad formats in iOS.
    /// </summary>
    internal class POBBidClient : IPOBBid
    {
        #region Private variables
        /// Reference of POBBid instance from OpenWrap SDK
        private IntPtr bidPtr;
        private readonly string Tag = "POBBidClient";

        #endregion

        #region Internal methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bid">Reference of POBBid instance from OpenWrap SDK</param>
        internal void SetBid(IntPtr bid)
        {
            bidPtr = bid;
        }

        #endregion

        #region iOS Plugin imports
        [DllImport("__Internal")]
        internal static extern string POBUBidGetBidId(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern string POBUBidGetImpressionId(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern double POBUBidGetPrice(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern double POBUBidGetGrossPrice(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern int POBUBidGetWidth(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern int POBUBidGetHeight(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern int POBUBidGetStatus(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern string POBUBidGetCreativeId(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern string POBUBidGetCreativeTag(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern string POBUBidGetCreativeType(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern string POBUBidGetPartner(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern string POBUBidGetDealId(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern bool POBUBidGetHasWon(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern void POBUBidSetHasWon(IntPtr bid, bool hasWon);

        [DllImport("__Internal")]
        internal static extern string POBUBidGetPubMaticPartnerId(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern bool POBUBidIsExpired(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUBidGetTargetingKeys(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern IntPtr POBUBidGetTargetingValues(IntPtr bid);

        [DllImport("__Internal")]
        internal static extern int POBUBidGetTargetingCount(IntPtr bid);
        #endregion

        #region IPOBBid APIs
        /// <summary>
        /// Getter for bid id
        /// </summary>
        /// <returns>bid id</returns>
        public string GetBidId()
        {
            return POBUBidGetBidId(bidPtr);
        }

        /// <summary>
        /// Getter for bid creative identifier
        /// </summary>
        /// <returns>Identifier of the bid creative</returns>
        public string GetCreativeId()
        {
            return POBUBidGetCreativeId(bidPtr);
        }

        /// <summary>
        /// Getter for Creative tag 
        /// </summary>
        /// <returns>Creative tag as string</returns>
        public string GetCreativeTag()
        {
            return POBUBidGetCreativeTag(bidPtr);
        }

        /// <summary>
        /// Getter for creative type
        /// </summary>
        /// <returns>Video, display etc.</returns>
        public string GetCreativeType()
        {
            return POBUBidGetCreativeType(bidPtr);
        }

        /// <summary>
        /// Getter for Creative tag
        /// </summary>
        /// <returns>Creative tag as string</returns>
        public string GetDealId()
        {
            return POBUBidGetDealId(bidPtr);
        }

        /// <summary>
        /// Getter for bid price
        /// </summary>
        /// <returns>Bid price float value</returns>
        public double GetPrice()
        {
            return POBUBidGetPrice(bidPtr);
        }

        /// <summary>
        /// Getter for gross bid price
        /// </summary>
        /// <returns>Gross bid price as float value </returns>
        public double GetGrossPrice()
        {
            return POBUBidGetGrossPrice(bidPtr);
        }

        /// <summary>
        /// Winning status with respect to primary ad server, expected to be updated externally.
        /// </summary>
        /// <returns>true if bid has won</returns>
        public bool GetHasWon()
        {
            return POBUBidGetHasWon(bidPtr);
        }

        /// <summary>
        /// Method to set Winning status.
        /// </summary>
        /// <param name="hasWon">true if bid has won</param>
        public void SetHasWon(bool hasWon)
        {
            POBUBidSetHasWon(bidPtr, hasWon);
        }

        /// <summary>
        /// Getter for Ad width
        /// </summary>
        /// <returns>Ad width</returns>
        public int GetWidth()
        {
            return POBUBidGetWidth(bidPtr);
        }

        /// <summary>
        /// Getter for Ad height
        /// </summary>
        /// <returns>Ad height</returns>
        public int GetHeight()
        {
            return POBUBidGetHeight(bidPtr);
        }

        /// <summary>
        /// Getter for Partner name
        /// </summary>
        /// <returns>Partner name as string</returns>
        public string GetPartner()
        {
            return POBUBidGetPartner(bidPtr);
        }

        /// <summary>
        /// Method to get PubMatic (Global) partner ID
        /// </summary>
        /// <returns>PubMatic (Global) partner ID</returns>
        public string GetPubmaticPartnerId()
        {
            return POBUBidGetPubMaticPartnerId(bidPtr);
        }

        /// <summary>
        /// Getter for Bid status 
        /// </summary>
        /// <returns>0 - invalid bid, 1 - Valid bid</returns>
        public int GetStatus()
        {
            return POBUBidGetStatus(bidPtr);
        }

        /// <summary>
        /// Returns targeting information that needs to be passed to the ad server SDK.
        /// </summary>
        /// <returns>Dictionary of standard key-value pairs for targeting</returns>
        public Dictionary<string, string> GetTargetingInfo()
        {
            POBLog.Info(Tag, POBLogStrings.BidGetTargetingKeys);
            IntPtr targetingKeysPtr = POBUBidGetTargetingKeys(bidPtr);
            POBLog.Info(Tag, POBLogStrings.BidGetTargetingValues);
            IntPtr targetingValuesPtr = POBUBidGetTargetingValues(bidPtr);
            int targetingCount = POBUBidGetTargetingCount(bidPtr);

            List<string> targetingKeys = PtrArrayToStringsList(targetingKeysPtr, targetingCount);
            List<string> targetingValues = PtrArrayToStringsList(targetingValuesPtr, targetingCount);

            // Re-create the dictionary from the targeting keys, values list
            Dictionary<string, string> targetingDict = new Dictionary<string, string>();
            for (int i = 0; i < targetingCount; i++)
            {
                string key = targetingKeys[i];
                string value = targetingValues[i];
                if (key != null && value != null)
                {
                    POBLog.Info(Tag,POBLogStrings.Targetting);
                    targetingDict.Add(key, value);
                }
            }

            return targetingDict;
        }

        /// <summary>
        /// Returns true if bid is expired
        /// <para> SDK do not render the expired bid. </para>
        /// </summary>
        /// <returns>bool value</returns>
        public bool IsExpired()
        {
            POBLog.Info(Tag, POBLogStrings.BidIsExpired);
            return POBUBidIsExpired(bidPtr);
        }
        #endregion

        /// <summary>
        /// Method to convert list of C-type strings into List<string>
        /// </summary>
        /// <param name="arrayPtr">List of C-type strings</param>
        /// <param name="count">Number of strings in the list</param>
        /// <returns>C# types List of strings</returns>
        public static List<string> PtrArrayToStringsList(IntPtr arrayPtr, int count)
        {
            IntPtr[] intPtrArray = new IntPtr[count];
            Marshal.Copy(arrayPtr, intPtrArray, 0, count);

            string[] stringsList = new string[count];
            for (int i = 0; i < count; i++)
            {
                stringsList[i] = Marshal.PtrToStringAuto(intPtrArray[i]);
                Marshal.FreeHGlobal(intPtrArray[i]);
            }

            Marshal.FreeHGlobal(arrayPtr);
            return new List<string>(stringsList);
        }
    }
}
#endif