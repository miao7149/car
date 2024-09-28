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

using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.Linq;
using OpenWrapSDK.Common;

namespace OpenWrapSDK.iOS
{
    /// <summary>
    /// iOS POBExternalUserId client to set or get parameters on POBExternalUserId object.
    /// </summary>
    internal class POBExternalUserIdClient : IPOBExternalUserId
    {
        #region Private variables
        /// Reference of POBExternalUserId instance from OpenWrap SDK
        private IntPtr externalUserIdPtr = IntPtr.Zero;
        private readonly string Tag = "POBExternalUserIdClient";
        #endregion

        #region Internal methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="externalUserPtr">IntPtr of POBExternalUserId</param>
        internal POBExternalUserIdClient(string source, string externalUserId)
        {
            if (source != null && externalUserId != null)
            {
                externalUserIdPtr = POBUCreateExternalUserId(source, externalUserId);
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBExternalUserIdClient()
        {
            Destroy();
        }
        #endregion

        #region iOS Plugin imports
        [DllImport("__Internal")]
        internal static extern IntPtr POBUCreateExternalUserId(string source,string userId);

        [DllImport("__Internal")]
        internal static extern string POBUGetSource(IntPtr externalUserIdPtr);

        [DllImport("__Internal")]
        internal static extern string POBUGetExternalUserId(IntPtr externalUserIdPtr);

        [DllImport("__Internal")]
        internal static extern void POBUSetAType(IntPtr externalUserIdPtr, int atype);

        [DllImport("__Internal")]
        internal static extern int POBUGetAType(IntPtr externalUserIdPtr);

        [DllImport("__Internal")]
        internal static extern void POBUSetExtension(IntPtr externalUserIdPtr, string[] keysList, string[] valuesList, int dictCount);

        [DllImport("__Internal")]
        internal static extern void POBUDestroyExternalUserId(IntPtr externalUserIdPtr);
        #endregion

        #region IPOBExternalUserId APIs
        /// <summary>
        /// Getter for name of the data partner
        /// </summary>
        public string Source
        {
            get
            {
                if (externalUserIdPtr != IntPtr.Zero)
                {
                    return POBUGetSource(externalUserIdPtr);
                }
                return null;
            }
        }

        /// <summary>
        /// Getter for Id of the data partner
        /// </summary>
        public string Id
        {
            get
            {
                if (externalUserIdPtr != IntPtr.Zero)
                {
                    return POBUGetExternalUserId(externalUserIdPtr);
                }
                return null;
            }
        }

        /// <summary>
        /// Setter and Getter for a non-zero value for user agent type
        /// </summary>
        public int Atype
        {
            get
            {
                if (externalUserIdPtr != IntPtr.Zero)
                {
                    return POBUGetAType(externalUserIdPtr);
                }
                return 0;
            }
            set
            {
                if (externalUserIdPtr != IntPtr.Zero)
                {
                    POBUSetAType(externalUserIdPtr, value);
                }
            }
        }

        public Dictionary<string, string> Extension
        {
            set => SetExtensionInNative(value);
        }

        /// <summary>
        /// Getter for POBExternalUserIdClient reference
        /// </summary>
        /// <returns></returns>
        public IntPtr GetNativeExternalUserIdPtr()
        {
            return externalUserIdPtr;
        }

        /// <summary>
        /// Cleanup API
        /// </summary>
        public void Destroy()
        {
            if (externalUserIdPtr != IntPtr.Zero && externalUserIdPtr != null)
            {
                POBLog.Info(Tag, POBLogStrings.DestroyExternalUserId);
                POBUDestroyExternalUserId(externalUserIdPtr);
                externalUserIdPtr = IntPtr.Zero;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Converting Dictionary to array of values and keys and pass to native
        /// </summary>
        /// <param name="extension">Dictionary of string keys and string values</param>
        private void SetExtensionInNative(Dictionary<string, string> extension)
        {
            if (externalUserIdPtr != IntPtr.Zero && extension != null)
            {
                string[] keys = extension.Keys.ToArray();
                string[] values = extension.Values.ToArray();
                POBLog.Info(Tag, POBLogStrings.SetExtension);
                POBUSetExtension(externalUserIdPtr, keys, values, keys.Length);
            }
        }
        #endregion
    }
}
#endif