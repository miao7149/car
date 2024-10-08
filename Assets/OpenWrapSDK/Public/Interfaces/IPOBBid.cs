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
* information that is confidential and/or proprietary, and is a trade secret, of  PubMatic.   ANY REPRODUCTION, MODIFICATION, DISTRIBUTION,  PERFORMANCE,
* OR DISPLAY OF OR THROUGH USE  OF THIS  SOURCE CODE  WITHOUT  THE EXPRESS WRITTEN CONSENT OF PUBMATIC IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE
* LAWS AND INTERNATIONAL TREATIES.  THE RECEIPT OR POSSESSION OF  THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT  MAY DESCRIBE, IN WHOLE OR IN PART.
*/

using System.Collections.Generic;

namespace OpenWrapSDK
{
    /// <summary>
    /// Holds information of winning bid along with all the bids that participated in the auction.
    /// </summary>
    public interface IPOBBid
    {
        /// <summary>
        /// Getter for bid id
        /// </summary>
        /// <returns>bid id</returns>
        string GetBidId();

        /// <summary>
        /// Getter for bid price
        /// </summary>
        /// <returns>Bid price float value</returns>
        double GetPrice();

        /// <summary>
        /// Getter for gross bid price
        /// </summary>
        /// <returns>Gross bid price as float value </returns>
        double GetGrossPrice();

        /// <summary>
        /// Getter for Ad width
        /// </summary>
        /// <returns>Ad width</returns>
        int GetWidth();

        /// <summary>
        /// Getter for Ad height
        /// </summary>
        /// <returns>Ad height</returns>
        int GetHeight();

        /// <summary>
        /// Getter for Bid status 
        /// </summary>
        /// <returns>0 - invalid bid, 1 - Valid bid</returns>
        int GetStatus();

        /// <summary>
        /// Getter for bid creative identifier
        /// </summary>
        /// <returns>Identifier of the bid creative</returns>
        string GetCreativeId();

        /// <summary>
        /// Getter for Creative tag 
        /// </summary>
        /// <returns>Creative tag as string</returns>
        string GetCreativeTag();

        /// <summary>
        /// Getter for creative type
        /// </summary>
        /// <returns>Video, display etc.</returns>
        string GetCreativeType();

        /// <summary>
        /// Getter for Partner name
        /// </summary>
        /// <returns>Partner name as string</returns>
        string GetPartner();

        /// <summary>
        /// Getter for Creative tag
        /// </summary>
        /// <returns>Creative tag as string</returns>
        string GetDealId();

        /// <summary>
        /// Winning status with respect to primary ad server, expected to be updated externally.
        /// </summary>
        /// <returns>true if bid has won</returns>
        bool GetHasWon();

        /// <summary>
        /// Method to set Winning status.
        /// </summary>
        /// <param name="hasWon">true if bid has won</param>
        void SetHasWon(bool hasWon);

        /// <summary>
        /// Method to get PubMatic (Global) partner ID
        /// </summary>
        /// <returns>PubMatic (Global) partner ID</returns>
        string GetPubmaticPartnerId();


        /// <summary>
        /// Returns targeting information that needs to be passed to the ad server SDK.
        /// </summary>
        /// <returns>Dictionary of standard key-value pairs for targeting</returns>
        Dictionary<string, string> GetTargetingInfo();

        /// <summary>
        /// Returns true if bid is expired
        /// <para> SDK do not render the expired bid. </para>
        /// </summary>
        /// <returns>bool value</returns>
        bool IsExpired();
    }
}
#endif