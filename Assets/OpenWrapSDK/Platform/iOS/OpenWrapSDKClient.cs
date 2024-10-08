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
using System.Collections.Generic;
using System.Linq;
using OpenWrapSDK.Common;

namespace OpenWrapSDK.iOS
{
    /// <summary>
    /// iOS client to set global parameters on OpenWrap SDK.
    /// </summary>
    internal static class OpenWrapSDKClient
    {
        #region Private/Internal members
        private static POBUserInfo internalUserInfo;
        private static string Tag = "OpenWrapSDKClient";
        /// POBApplicationInfo structure. Keep is similar to POBApplicationInfo class
        internal struct POBApplicationInfoInternal
        {
            internal string Domain;
            internal string StoreURL;
            internal int Paid;
            internal string Categories;
            internal string Keywords;
        }
        #endregion

        #region Plugin methods
        [DllImport("__Internal")]
        internal static extern string POBUGetOpenWrapSDKVersion();

        [DllImport("__Internal")]
        internal static extern void POBUSetLogLevel(int logLevel);

        [DllImport("__Internal")]
        internal static extern void POBUAllowLocationAccess(bool enable);

        [DllImport("__Internal")]
        internal static extern void POBUUseInternalBrowser(bool use);

        [DllImport("__Internal")]
        internal static extern void POBUSetLocation(double longitude, double latitude, int locSource);

        [DllImport("__Internal")]
        internal static extern void POBUSetCOPPAEnabled(bool enable);

        [DllImport("__Internal")]
        internal static extern void POBUSetSSLEnabled(bool enable);

        [DllImport("__Internal")]
        internal static extern void POBUAllowAdvertisingId(bool allow);

        [DllImport("__Internal")]
        internal static extern void POBUAllowAVAudioSessionAccess(bool allow);

        [DllImport("__Internal")]
        internal static extern void POBUSetApplicationInfo(ref POBApplicationInfoInternal appInfoInternal);

        [DllImport("__Internal")]
        internal static extern void POBUGetApplicationInfoInto(ref POBApplicationInfoInternal appInfoInternal);

        [DllImport("__Internal")]
        internal static extern void POBUAddExternalUserId(IntPtr externalUserId);

        [DllImport("__Internal")]
        internal static extern List<POBExternalUserId> POBUGetAllExternalUserIds(IntPtr externalUserId);

        [DllImport("__Internal")]
        internal static extern void POBURemoveAllExternalUserIds();

        [DllImport("__Internal")]
        internal static extern void POBURemoveExternalUserIdsWithSource(string source);

        [DllImport("__Internal")]
        internal static extern void POBUSetUserInfo(IntPtr userInfo);
        #endregion

        #region Public methods
        public static string GetVersion()
        {
            return POBUGetOpenWrapSDKVersion();
        }

        public static void AllowAdvertisingId(bool allow)
        {
            POBUAllowAdvertisingId(allow);
        }

        public static void SetAllowAVAudioSessionAccess(bool allow)
        {
            POBUAllowAVAudioSessionAccess(allow);
        }

        public static void AllowLocationAccess(bool allow)
        {
            POBUAllowLocationAccess(allow);
        }

        public static void SetCOPPAEnabled(bool enable)
        {
            POBUSetCOPPAEnabled(enable);
        }

        public static void SetLocation(double longitude, double latitude, POBLocSource source)
        {
            POBUSetLocation(longitude, latitude, ((int)source));
        }

        public static void SetLogLevel(POBSDKLogLevel logLevel)
        {
            POBUSetLogLevel(((int)logLevel));
        }

        public static void SetSSLEnabled(bool enable)
        {
            POBUSetSSLEnabled(enable);
        }

        public static void SetUseInternalBrowser(bool use)
        {
            POBUUseInternalBrowser(use);
        }

        public static void SetApplicationInfo(POBApplicationInfo applicationInfo)
        {
            POBApplicationInfoInternal appInfoInternal = new POBApplicationInfoInternal();
            appInfoInternal.Keywords = applicationInfo.Keywords;
            appInfoInternal.Categories = applicationInfo.Categories;
            appInfoInternal.Domain = applicationInfo.Domain;
            appInfoInternal.Paid = (int)applicationInfo.Paid;
            if (applicationInfo.StoreURL != null)
            {
                appInfoInternal.StoreURL = applicationInfo.StoreURL.ToString();
            }

            POBUSetApplicationInfo(ref appInfoInternal);
        }

        public static POBApplicationInfo GetApplicationInfo()
        {
            POBApplicationInfoInternal appInfoInternal = new POBApplicationInfoInternal();
            POBUGetApplicationInfoInto(ref appInfoInternal);

            // Create instance of POBApplicationInfo and copy all the values from the struct into it.
            POBApplicationInfo appInfo = new POBApplicationInfo();
            appInfo.Keywords = appInfoInternal.Keywords;
            appInfo.Categories = appInfoInternal.Categories;
            appInfo.Domain = appInfoInternal.Domain;

            appInfo.Paid = (POBBool)appInfoInternal.Paid;
            if (appInfoInternal.StoreURL != null)
            {
                appInfo.StoreURL = new Uri(appInfoInternal.StoreURL);
            }
            return appInfo;
        }

        public static void SetUserInfo(POBUserInfo userInfo)
        {
            if (userInfo != null && userInfo.client != null)
            {
                IntPtr userInfoPtr = userInfo.client.GetNativePtr();
                if (userInfoPtr != IntPtr.Zero)
                {
                    internalUserInfo = userInfo;
                    POBUSetUserInfo(userInfoPtr);
                }
            }
        }

        public static POBUserInfo GetUserInfo()
        {
            return internalUserInfo;

        }

        /// <summary>
        /// Adds external user id's to OpenWrapSDK
        /// </summary>
        /// <param name="userId">Instance of POBExternalUserId to add</param>
        public static void AddExternalUserId(POBExternalUserId userId)
        {
            if (userId != null)
            {
                IntPtr externalUserIdClientPtr = userId.externalUserIdClient.GetNativeExternalUserIdPtr();
                if (externalUserIdClientPtr != null)
                {
                    POBLog.Info(Tag, POBLogStrings.AddExternalUserId);
                    POBUAddExternalUserId(externalUserIdClientPtr);
                }
            }
        }

        /// <summary>
        /// Removes all external user id's from the sdk
        /// </summary>
        public static void RemoveAllExternalUserIds()
        {
            POBLog.Info(Tag, POBLogStrings.RemoveAllExternalUserIds);
            POBURemoveAllExternalUserIds();
        }

        /// <summary>
        /// Removes user id for a specific source from the sdk
        /// </summary>
        /// <param name="source">Name of the data partner to remove data for</param>
        public static void RemoveExternalUserIdsWithSource(string source)
        {
            POBLog.Info(Tag, POBLogStrings.RemoveExternalUserIdsWithSource);
            POBURemoveExternalUserIdsWithSource(source);
        }
        #endregion
    }
}
#endif
