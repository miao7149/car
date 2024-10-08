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
using OpenWrapSDK.Common;

namespace OpenWrapSDK
{
    /// <summary>
    /// Class to hold the user's identity data.
    /// This data will be fetched by the application from identity vendors which is required by different identity partners.
    /// Application should pass this data to OpenWrap SDK which will be sent in each ad request.
    /// </summary>
    public class POBExternalUserId
    {
        #region Private variables
        internal IPOBExternalUserId externalUserIdClient;
        private readonly string Tag = "POBExternalUserId";
        private int aType;

        private Dictionary<string, string> extension;

        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Method to instantiate POBExternalUserId
        /// </summary>
        /// <param name="source">Name of the data partner</param>
        /// <param name="externalUserId">Id of the data partner</param>
        public POBExternalUserId(string source, string externalUserId)
        {
            if (source != null && externalUserId != null)
            {
            Source = source;
            Id = externalUserId;
            #if UNITY_IOS
                 externalUserIdClient = new iOS.POBExternalUserIdClient(source, externalUserId);
            #else
                 externalUserIdClient = new Android.POBExternalUserIdClient(source, externalUserId);
            #endif
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.InvalidSourceAndId);
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBExternalUserId()
        {
            if (externalUserIdClient != null)
            {
                externalUserIdClient.Destroy();
                externalUserIdClient = null;
            }
        }
        #endregion

        #region Public APIs
        /// <summary>
        /// Name of the data partner
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Id of the data partner
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// A non-zero value for user agent type
        /// </summary>
        public int Atype
        {
            get => aType;
            set
            {
                aType = value;
                if (externalUserIdClient != null)
                {
                    externalUserIdClient.Atype = value;
                }
            }
        }

        /// <summary>
        /// RTB extension object for POBExternalUserId
        /// </summary>
        public Dictionary<string, string> Extension
        {
            get => extension;
            set
            {
                if (externalUserIdClient != null && value != null)
                {
                    extension = value;
                    externalUserIdClient.Extension = value;
                }
            }
        }
        #endregion
    }
}
#endif