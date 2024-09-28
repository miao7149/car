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

using System.Collections.Generic;
using System.Linq;
using OpenWrapSDK.Common;

namespace OpenWrapSDK
{
    /// <summary>
    /// Provides global configurations for the OpenWrap SDK, e.g. enabling logging, location access, GDPR, etc.
    /// <br/>These configurations are globally applicable for OpenWrap SDK; you don't have to set these for every ad request.
    /// </summary>
    public static class POBOpenWrapSDK
    {
        private static List<POBExternalUserId> externalUserIds = new List<POBExternalUserId>();

        // OpenWrap SDK plugin version. Please make sure to update it with every release.
        private static readonly string OpenWrapSDKPluginVersion = "3.1.0";

        private static readonly string Tag = "POBOpenWrapSDK";

        /// <summary>
        /// Static constructor to print unity plugin log
        /// </summary>
        static POBOpenWrapSDK()
        {
            POBLog.Info(Tag, $"OpenWrap SDK Unity plugin version: {OpenWrapSDKPluginVersion}");
        }

        /// <summary>
        /// Returns the OpenWrap SDK's version.
        /// </summary>
        /// <returns>OpenWrap SDK version string</returns>
        public static string GetVersion()
        {
#if UNITY_IOS
            return iOS.OpenWrapSDKClient.GetVersion();        
#else
            return Android.OpenWrapSDKClient.GetVersion();
#endif
        }

        /// <summary>
        /// Sets log level across all ad formats. Default log level is POBSDKLogLevelWarn
        /// </summary>
        /// <param name="logLevel">Log level to set.</param>
        /// <seealso cref="POBSDKLogLevel"/>
        public static void SetLogLevel(POBSDKLogLevel logLevel)
        {
#if UNITY_IOS
            iOS.OpenWrapSDKClient.SetLogLevel(logLevel);
#else
            Android.OpenWrapSDKClient.SetLogLevel(logLevel);
#endif
        }

        /// <summary>
        /// Used to enable/disable location access.
        /// <br/>This value decides whether the OpenWrap SDK should access device location usings Core Location APIs to serve location-based ads. When set to NO, the SDK will not attempt to access device location. When set to YES, the SDK will periodically try to fetch location efficiently.
        /// <br/>Note that, this only occurs if location services are enabled and the user has already authorized the use of location services for the application.The OpenWrap SDK never asks permission to use location services by itself.
        /// <br/>The default value is true.
        /// </summary>
        /// <param name="allow">true/false value</param>
        public static void AllowLocationAccess(bool allow)
        {
#if UNITY_IOS
            iOS.OpenWrapSDKClient.AllowLocationAccess(allow);
#else
            Android.OpenWrapSDKClient.AllowLocationAccess(allow);
#endif
        }

        /// <summary>
        /// Tells OpenWrap SDK to use internal SDK browser, instead of the default device browser, for opening landing pages when the user clicks on an ad.
        /// <br/>By default, use of internal browser is enabled.
        /// </summary>
        /// <param name="use"> bool value that enables/disables the use of internal browser.</param>
        public static void SetUseInternalBrowser(bool use)
        {
#if UNITY_IOS
            iOS.OpenWrapSDKClient.SetUseInternalBrowser(use);
#else
            Android.OpenWrapSDKClient.SetUseInternalBrowser(use);
#endif
        }

        /// <summary>
        /// Sets user's location and its source. It is useful in delivering geographically relevant ads.
        /// <br/>If your application is already accessing the device location, it is highly recommended to set the location coordinates inferred from the device GPS. If you are inferring location from any other source, make sure you set the appropriate location source.
        /// </summary>
        /// <seealso cref="POBLocSource"/>
        /// <param name="longitude">Longitude of user's current location</param>
        /// <param name="latitude">Latitude of user's current location</param>
        /// <param name="source">Source of user's location.</param>
        public static void SetLocation(double longitude, double latitude, POBLocSource source)
        {
#if UNITY_IOS
            iOS.OpenWrapSDKClient.SetLocation(longitude, latitude, source);
#else
            Android.OpenWrapSDKClient.SetLocation(longitude, latitude, source);
#endif
        }

        /// <summary>
        /// Indicates whether the visitor is COPPA-specific or not. <br/> For COPPA (Children's Online Privacy Protection Act) compliance, if the visitor's age is below 13, then such visitors should not be served targeted ads.
        /// <para>- false : Indicates that the visitor is not COPPA-specific and can be served targeted ads
        /// <br/>- true : Indicates that the visitor is COPPA-specific and should be served only COPPA-compliant ads.</para>
        /// </summary>
        /// <param name="enable">bool value</param>
        public static void SetCOPPAEnabled(bool enable)
        {
#if UNITY_IOS
            iOS.OpenWrapSDKClient.SetCOPPAEnabled(enable);
#else
            Android.OpenWrapSDKClient.SetCoppaEnabled(enable);
#endif
        }

        /// <summary>
        /// Enable/disable secure ad calls. <br/> By default, OpenWrap SDK initiates secure ad calls from an application to the ad server and delivers only secure ads. You can allow non secure ads by passing false to this method.
        /// </summary>
        /// <param name="enable">bool value</param>
        public static void SetSSLEnabled(bool enable)
        {
#if UNITY_IOS
            iOS.OpenWrapSDKClient.SetSSLEnabled(enable);
#else
            Android.OpenWrapSDKClient.SetSSLEnabled(enable);
#endif
        }

        /// <summary>
        /// Indicates whether Advertisment ID(IDFA) should be sent in the request.
        /// <br/> true : Advertising Identifier will be sent in the request.
        /// <br/> false : Advertising Identifier will be masked in the request.
        /// </summary>
        /// <param name="allow">bool value </param>
        public static void AllowAdvertisingId(bool allow)
        {
#if UNITY_IOS
            iOS.OpenWrapSDKClient.AllowAdvertisingId(allow);
#else
            Android.OpenWrapSDKClient.AllowAdvertisingId(allow);
#endif
        }

#if UNITY_IOS
        /// <summary>
        /// Indicates whether the OW SDK is allowed to access the shared AVAudioSession
        /// <br/>This method disables the audio session access only for OpenWrap SDK. You may have to use a similar provision from ad server sdk, if available, to restrict it from potentially interfering with your app's audio.
        /// <para>- true(Default) : SDK may access the shared AVAudioSession
        /// <br/>- false : SDK should not access the shared AVAudioSession</para>
        /// </summary>
        /// <param name="allow">bool value</param>
        public static void SetAllowAVAudioSessionAccess(bool allow)
        {
            iOS.OpenWrapSDKClient.SetAllowAVAudioSessionAccess(allow);
        }
#endif

        /// <summary>
        /// Sets Application information, which contains various attributes about app, such as
        /// <br/>application category, store URL, domain, etc for more relevant ads.
        /// 
        /// </summary>
        /// <param name="applicationInfo">Instance of POBApplicationInfo class with required application details</param>
        public static void SetApplicationInfo(POBApplicationInfo applicationInfo)
        {
#if UNITY_IOS
            iOS.OpenWrapSDKClient.SetApplicationInfo(applicationInfo);
#else
            Android.OpenWrapSDKClient.SetApplicationInfo(applicationInfo);
#endif
        }

        /// <summary>
        /// Gets Application info set by publisher to OpenWrap SDK
        /// </summary>
        /// <returns>instance of POBApplicationInfo class</returns>
        public static POBApplicationInfo GetApplicationInfo()
        {
#if UNITY_IOS
            return iOS.OpenWrapSDKClient.GetApplicationInfo();
#else
            return Android.OpenWrapSDKClient.GetApplicationInfo();
#endif   
        }

        /// <summary>
        /// Sets user information, such as birth year, gender, region, etc for more relevant ads.
        /// 
        /// </summary>
        /// <param name="userInfo">Instance of POBUserInfo class with required user details</param>
        public static void SetUserInfo(POBUserInfo userInfo)
        {
#if UNITY_IOS
            iOS.OpenWrapSDKClient.SetUserInfo(userInfo);
#else
            Android.OpenWrapSDKClient.SetUserInfo(userInfo);
#endif    
        }

        /**
         *  Gets user info set by publisher to OpenWrap SDK
         *
         * @return instance of POBUSerInfo class
         */
        /// <summary>
        /// Gets user info set by publisher to OpenWrap SDK
        /// </summary>
        /// <returns>instance of POBUSerInfo class</returns>
        public static POBUserInfo GetUserInfo()
        {
#if UNITY_IOS
            return iOS.OpenWrapSDKClient.GetUserInfo();
#else
            return Android.OpenWrapSDKClient.GetUserInfo();
#endif   
        }


        /// <summary>
        /// Add external User Id/s to OpenWrap SDK
        /// </summary>
        /// <param name="userId">Instance of POBExternalUserId class</param>
        public static void AddExternalUserId(POBExternalUserId userId)
        {
            if (userId != null && !POBUtils.IsNullOrEmpty(userId.Id) && !POBUtils.IsNullOrEmpty(userId.Source))
            {
                // check if user id with same partner id and source available
                bool isPartnerIdAvailable = externalUserIds.Any(extUserId => extUserId.Id.Equals(userId.Id) && extUserId.Source.Equals(userId.Source));
             
                if (!isPartnerIdAvailable)
                {     
                    externalUserIds.Add(userId);
#if UNITY_IOS
                    iOS.OpenWrapSDKClient.AddExternalUserId(userId);
#else
                    Android.OpenWrapSDKClient.AddExternalUserId(userId);
#endif  
                }
                else
                {
                    POBLog.Warning(Tag, POBLogStrings.AddExternalUserIdPartnerIdLog);
                }
            }
            else {
                POBLog.Warning(Tag, POBLogStrings.AddExternalUserIdUserIdLog);
            }
        }

        /// <summary>
        /// Get list of all the external user id's
        /// </summary>
        /// <returns>List of id's</returns>
        public static List<POBExternalUserId> GetAllExternalUserIds()
        {
            return externalUserIds; 
        }

        /// <summary>
        /// Removes all user id's
        /// </summary>
        public static void RemoveAllExternalUserIds()
        {
            if (externalUserIds.Count > 0)
            {
                foreach (POBExternalUserId userid in externalUserIds)
                {
                    userid.externalUserIdClient.Destroy();
                }
                externalUserIds.Clear();

                #if UNITY_IOS
                     iOS.OpenWrapSDKClient.RemoveAllExternalUserIds();
                #else
                     Android.OpenWrapSDKClient.RemoveAllExternalUserIds();
                #endif

            }
        }

        /// <summary>
        /// Removes specific user id according to given source
        /// </summary>
        /// <param name="source"></param>
        public static void RemoveExternalUserIds(string source)
        {
            if (externalUserIds.Count > 0)
            {
                List<POBExternalUserId> keepUserIds = new List<POBExternalUserId>();
                foreach (POBExternalUserId userid in externalUserIds)
                {
                    if (userid.Source.Equals(source))
                    {
                        userid.externalUserIdClient.Destroy();
                    }
                    else
                    {
                        keepUserIds.Add(userid);
                    }
                }
                externalUserIds.Clear();
                externalUserIds = keepUserIds;

#if UNITY_IOS
                iOS.OpenWrapSDKClient.RemoveExternalUserIdsWithSource(source);
#else
                Android.OpenWrapSDKClient.RemoveExternalUserIds(source);
#endif
            }
        }
    }
}
#endif