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
using System.Collections.Generic;
#if UNITY_ANDROID
using UnityEngine;
#endif
namespace OpenWrapSDK.Common
{
    public interface IPOBExternalUserId
    {

        /// <summary>
        /// Setter for a non-zero value for user agent type
        /// </summary>
        int Atype { set; }

        /// <summary>
        /// RTB extension object for POBExternalUserId
        /// </summary>
        Dictionary<string, string> Extension { set; }

        /// <summary>
        /// Cleanup API
        /// </summary>
        void Destroy();

#if UNITY_IOS
        /// <summary>
        /// Getter for reference of iOS's POBExternalUserId instance
        /// </summary>
        /// <returns>IntPtr of iOS's POBExternalUserId instance</returns>
        IntPtr GetNativeExternalUserIdPtr();
#else
        /// <summary>
        /// Get reference of Java POBDataProvider instance
        /// </summary>
        /// <returns></returns>
        AndroidJavaObject GetNativeObject();
#endif
    }
}
#endif
