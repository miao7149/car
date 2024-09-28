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

#if UNITY_IOS
using System;
#endif
#if UNITY_ANDROID
using UnityEngine;
#endif

namespace OpenWrapSDK.Common
{
    /// <summary>
    /// Interface for common APIs for iOS/Android POBUserInfo clients.
    /// </summary>
    public interface IPOBUserInfoClient
    {
        /// <summary>
        /// The year of birth in YYYY format.
        /// <br/> Example : birthYear = 1988;
        /// </summary>
        int BirthYear { set; }

        /// <summary>
        /// Set the user gender,
        /// <br/>Possible options are:
        /// <br/>OTHER
        /// <br/>MALE
        /// <br/>FEMALE
        /// </summary>
        POBGender Gender { set; }

        /// <summary>
        /// City of user
        /// </summary>
        string City { set; }

        /// <summary>
        /// Google metro code, You can set Designated market area (DMA) code of the user in this
        /// <br/>field. This field is applicable for US users only
        /// </summary>
        string Metro { set; }

        /// <summary>
        /// The user's zip code may be useful in delivering geographically relevant ads
        /// </summary>
        string Zip { set; }

        /// <summary>
        /// Region code using ISO-3166-2; 2-letter state code if USA
        /// </summary>
        string Region { set; }

        /// <summary>
        /// Comma separated list of keywords, interests, or intent.
        /// </summary>
        string Keywords { set; }

        /// <summary>
        /// Adds data containing user segment details
        /// </summary>
        /// <param name="dataProvider">Data provider to be added</param>
        void AddDataProvider(POBDataProvider dataProvider);

        /// <summary>
        /// Removes data for a specific provider from the user object
        /// </summary>
        /// <param name="dataProvider">Data provider to remove data for</param>
        void RemoveDataProvider(POBDataProvider dataProvider);

        /// <summary>
        /// Removes data for all providers from the user object
        /// </summary>
        void RemoveAllDataProviders();

        /// <summary>
        /// Cleanup
        /// </summary>
        void Destroy();

#if UNITY_IOS
        /// <summary>
        /// Getter for reference of iOS's POBUserInfo instance
        /// </summary>
        /// <returns>IntPtr of iOS's POBUserInfo instance</returns>
        IntPtr GetNativePtr();
#else
        /// <summary>
        /// Getter for reference of Android's POBUserInfo instance
        /// </summary>
        /// <returns>AndroidJavaObject of Android's POBUserInfo instance</returns>
        AndroidJavaObject GetNativeObject();
#endif
    }
}
#endif