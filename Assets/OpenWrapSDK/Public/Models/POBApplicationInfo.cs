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
* information that is confidential and/or proprietary, and is a trade secret, of  PubMatic.   ANY REPRODUCTION, MODIFICATION, DISTRIBUTION, PUBLIC  PERFORMANCE,
* OR PUBLIC DISPLAY OF OR THROUGH USE  OF THIS  SOURCE CODE  WITHOUT  THE EXPRESS WRITTEN CONSENT OF PUBMATIC IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE
* LAWS AND INTERNATIONAL TREATIES.  THE RECEIPT OR POSSESSION OF  THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT  MAY DESCRIBE, IN WHOLE OR IN PART.
*/

using System;

namespace OpenWrapSDK
{
    /// <summary>
    /// Provides setters to pass application information like store URL, domain, IAB categories etc.
    /// <br/>It is very important to provide transparency for buyers of your app inventory.
    /// </summary>
    public class POBApplicationInfo
    {

        /// <summary>
        /// Indicates the domain of the mobile application (e.g., "mygame.foo.com")
        /// <summary>
        public string Domain { get; set; }

        /// <summary>
        /// URL of application on Play store
        /// </summary>
        public Uri StoreURL { get; set; }

        /// <summary>
        /// Indicates whether the mobile application is a paid version or not. Possible values are:
        /// <br/>POBBool.False - Free version
        /// <br/>POBBool.True - Paid version
        /// <br/>POBBool.Unknown - unknown
        /// </summary>
        public POBBool Paid { get; set; } = POBBool.Unknown;

        /// <summary>
        /// Comma separated list of IAB categories for the application. e.g. "IAB-1, IAB-2"
        /// </summary>
        public string Categories { get; set; }

        /// <summary>
        /// Comma separated list of keywords about the app
        /// </summary>
        public string Keywords { get; set; }

    }
    /// <summary>
    /// Describes values for paid parameter
    /// </summary>
    public enum POBBool
    {
        Unknown = -1,
        False = 0,
        True = 1
    }
}
#endif