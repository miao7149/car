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

using System.Collections.Generic;
using OpenWrapSDK.Common;
using UnityEngine;

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Android data provider client to set or get parameters on data provider object.
    /// </summary>

    internal class POBExternalUserIdClient : IPOBExternalUserId
    {
        #region Private variable
        private readonly string Tag = "POBExternalUserIdClient";
        private AndroidJavaObject externalUserIdObject;
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Constructor with data provider identifier & name
        /// </summary>
        /// <param name="name">Data provider's name</param>
        /// <param name="identifier">Data provider's id</param>
        internal POBExternalUserIdClient(string source, string externalUserId)
        {
            externalUserIdObject = new AndroidJavaObject(POBConstants.POBExternalUserIdClassName,
                source, externalUserId);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBExternalUserIdClient()
        {
            Destroy();
        }
        #endregion

        /// <summary>
        /// Setter for a non-zero value for user agent type
        /// </summary>
        public int Atype
        {
            set
            {
                if (externalUserIdObject != null)
                {
                    externalUserIdObject.Call("setAtype", value);
                }
            }
        }

        /// <summary>
        /// Setter for a extension object
        /// </summary>
        public Dictionary<string,string> Extension
        {
            set
            {
                if (externalUserIdObject != null)
                {
                    externalUserIdObject.Call("setExtension", POBAndroidUtils.ConvertDictionaryToJavaJSONObject(value));
                }
                
            }
        }

        /// <summary>
        /// Cleanup API
        /// </summary>
        public void Destroy()
        {
            POBLog.Info(Tag, POBLogStrings.ClientDestroyLog);
            externalUserIdObject = null;
        }

        /// <summary>
        /// Getter for POBExternalUserId's AndroidJavaObject instance
        /// </summary>
        /// <returns></returns>
        public AndroidJavaObject GetNativeObject()
        {
            return externalUserIdObject;
        }
    }
}
#endif