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
using System.Runtime.InteropServices;
using OpenWrapSDK.Common;

namespace OpenWrapSDK.iOS
{
    /// <summary>
    /// iOS POBUserInfo client to set or get parameters on userInfo object.
    /// </summary>
    internal class POBUserInfoClient : IPOBUserInfoClient
    {
        #region Private members
        // Reference to iOS's POBUserInfo instance
        private IntPtr userInfoPtr;
        private readonly string Tag = "POBUserInfoClient";
        #endregion

        #region iOS Plugin Methods

        [DllImport("__Internal")]
        internal static extern IntPtr POBUGetUserInfo();

        [DllImport("__Internal")]
        internal static extern void POBUSetUserInfoBirthYear(IntPtr userInfo, int birthYear);

        [DllImport("__Internal")]
        internal static extern void POBUSetUserInfoGender(IntPtr userInfo, int gender);

        [DllImport("__Internal")]
        internal static extern void POBUSetUserInfoMetro(IntPtr userInfo, string metro);

        [DllImport("__Internal")]
        internal static extern void POBUSetUserInfoZip(IntPtr userInfo, string zip);

        [DllImport("__Internal")]
        internal static extern void POBUSetUserInfoCity(IntPtr userInfo, string city);

        [DllImport("__Internal")]
        internal static extern void POBUSetUserInfoRegion(IntPtr userInfo, string region);

        [DllImport("__Internal")]
        internal static extern void POBUSetUserInfoKeywords(IntPtr userInfo, string keywords);

        [DllImport("__Internal")]
        internal static extern void POBUUserInfoAddDataProvider(IntPtr userInfo, IntPtr dataProvider);

        [DllImport("__Internal")]
        internal static extern void POBUUserInfoRemoveDataProvider(IntPtr userInfo, IntPtr dataProvider);

        [DllImport("__Internal")]
        internal static extern void POBUUserInfoRemoveAllDataProviders(IntPtr userInfo);

        [DllImport("__Internal")]
        internal static extern void POBUDestroyUserInfo(IntPtr userInfo);

        #endregion

        #region Constructor / Destructor
        /// <summary>
        /// Constructor
        /// </summary>
        internal POBUserInfoClient()
        {
            userInfoPtr = POBUGetUserInfo();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBUserInfoClient()
        {
            Destroy();
        }
        #endregion

        #region IPOBUserInfoClient
        /// <summary>
        /// The year of birth in YYYY format.
        /// <br/> Example : birthYear = 1988;
        /// </summary>
        public int BirthYear {
            set
            {
                if (userInfoPtr != IntPtr.Zero)
                {
                    POBUSetUserInfoBirthYear(userInfoPtr, value);
                }
            }
        }

        /// <summary>
        /// Set the user gender,
        /// <br/>Possible options are:
        /// <br/>OTHER
        /// <br/>MALE
        /// <br/>FEMALE
        /// </summary>
        public POBGender Gender
        {
            set
            {
                if (userInfoPtr != IntPtr.Zero)
                {
                    POBUSetUserInfoGender(userInfoPtr, (int)value);
                }
            }
        }

        /// <summary>
        /// City of user
        /// </summary>
        public string City
        {
            set
            {
                if (userInfoPtr != IntPtr.Zero)
                {
                    POBUSetUserInfoCity(userInfoPtr, value);
                }
            }
        }

        /// <summary>
        /// Google metro code, You can set Designated market area (DMA) code of the user in this
        /// <br/>field. This field is applicable for US users only
        /// </summary>
        public string Metro
        {
            set
            {
                if (userInfoPtr != IntPtr.Zero)
                {
                    POBUSetUserInfoMetro(userInfoPtr, value);
                }
            }
        }

        /// <summary>
        /// The user's zip code may be useful in delivering geographically relevant ads
        /// </summary>
        public string Zip
        {
            set
            {
                if (userInfoPtr != IntPtr.Zero)
                {
                    POBUSetUserInfoZip(userInfoPtr, value);
                }
            }
        }

        /// <summary>
        /// Region code using ISO-3166-2; 2-letter state code if USA
        /// </summary>
        public string Region
        {
            set
            {
                if (userInfoPtr != IntPtr.Zero)
                {
                    POBUSetUserInfoRegion(userInfoPtr, value);
                }
            }
        }

        /// <summary>
        /// Comma separated list of keywords, interests, or intent.
        /// </summary>
        public string Keywords
        {
            set
            {
                if (userInfoPtr != IntPtr.Zero)
                {
                    POBUSetUserInfoKeywords(userInfoPtr, value);
                }
            }
        }

        /// <summary>
        /// Adds data containing user segment details
        /// </summary>
        /// <param name="dataProvider">Data to be added</param>
        public void AddDataProvider(POBDataProvider dataProvider)
        {
            if (userInfoPtr != IntPtr.Zero && dataProvider != null && dataProvider.dataProviderClient != null)
            {
                IntPtr dataProviderPtr = dataProvider.dataProviderClient.GetNativePtr();
                if (dataProviderPtr != null)
                {
                    POBLog.Info(Tag, POBLogStrings.UserInfoAddDataProvider);
                    POBUUserInfoAddDataProvider(userInfoPtr, dataProviderPtr);
                }
            }
        }

        /// <summary>
        /// Removes data for all providers from the user object
        /// </summary>
        public void RemoveAllDataProviders()
        {
            if (userInfoPtr != IntPtr.Zero)
            {
                POBLog.Info(Tag, POBLogStrings.UserInfoRemoveAllDataProviders);
                POBUUserInfoRemoveAllDataProviders(userInfoPtr);
            }
        }

        /// <summary>
        /// Removes data for a specific provider from the user object
        /// </summary>
        /// <param name="dataProvider">Data provider to remove data for</param>
        public void RemoveDataProvider(POBDataProvider dataProvider)
        {
            if (userInfoPtr != IntPtr.Zero && dataProvider != null && dataProvider.dataProviderClient != null)
            {
                IntPtr dataProviderPtr = dataProvider.dataProviderClient.GetNativePtr();
                if (dataProviderPtr != IntPtr.Zero)
                {
                    POBLog.Info(Tag, POBLogStrings.UserInfoRemoveDataProvider);
                    POBUUserInfoRemoveDataProvider(userInfoPtr, dataProviderPtr);
                }
            }
        }

        /// <summary>
        /// Getter for reference of iOS's POBUserInfo instance
        /// </summary>
        /// <returns>IntPtr of iOS's POBUserInfo instance</returns>
        public IntPtr GetNativePtr()
        {
            return userInfoPtr;
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Destroy()
        {
            if (userInfoPtr != IntPtr.Zero)
            {
                POBLog.Info(Tag, POBLogStrings.DestroyUserInfo);
                POBUDestroyUserInfo(userInfoPtr);
                userInfoPtr = IntPtr.Zero;
            }
        }
        #endregion
    }
}
#endif