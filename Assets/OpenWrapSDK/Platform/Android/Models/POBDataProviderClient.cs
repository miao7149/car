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
    internal class POBDataProviderClient : IPOBDataProviderClient
    {
        #region Private variables
        private readonly string Tag = "POBDataProviderClient";
        private AndroidJavaObject dataProvider;
        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Constructor with data provider name
        /// </summary>
        /// <param name="name">Data provider's name</param>
        internal POBDataProviderClient(string name)
        {
            dataProvider = new AndroidJavaObject(POBConstants.POBDataProviderClassName, name);
        }

        /// <summary>
        /// Constructor with data provider identifier & name
        /// </summary>
        /// <param name="name">Data provider's name</param>
        /// <param name="identifier">Data provider's id</param>
        internal POBDataProviderClient(string name, string identifier)
        {
            dataProvider = new AndroidJavaObject(POBConstants.POBDataProviderClassName, name, identifier);
        }

        ~POBDataProviderClient()
        {
            Destroy();
        }

#endregion

#region IPOBDataProviderClient
        /// <summary>
        /// segment taxonomy id. Reference: https://github.com/InteractiveAdvertisingBureau/AdCOM/blob/master/AdCOM%20v1.0%20FINAL.md#list--category-taxonomies
        /// </summary>
        public int SegTax
        {
            set
            {
                if (dataProvider != null)
                {
                    dataProvider.Call("setSegTax",value);
                }
            }
        }

        /// <summary>
        /// RTB extension object for this data
        /// </summary>
        public Dictionary<string, string> Extension
        {
            set
            {
                SetExtensionInNative(value);
            }
        }

        /// <summary>
        /// Adds a segment details to data object
        /// </summary>
        /// <param name="segment">Segment details to be added to data object</param>
        public void AddSegment(POBSegment segment)
        {
            if(dataProvider != null && segment != null && segment.segmentClient != null)
            {
                AndroidJavaObject segmentObject = segment.segmentClient.GetNativeObject();
                if(segmentObject != null)
                {
                    POBLog.Info(Tag, POBLogStrings.ClientAddSegmentLog);
                    dataProvider.Call("addSegment", segmentObject);
                }
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.AddSegmentFailedLog);
            }
        }

        /// <summary>
        /// Removes a segment details from data object
        /// </summary>
        /// <param name="identifier">Identifier for which a segment is to be removed</param>
        public void RemoveSegment(POBSegment segment)
        {
            if (dataProvider != null && segment != null)
            {
                POBLog.Info(Tag, POBLogStrings.ClientRemoveSegmentLog);
                dataProvider.Call<AndroidJavaObject>("removeSegment", segment.Identifier);
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.RemoveSegmentFailedLog);
            }
        }

        /// <summary>
        /// Removes all segments from the data object
        /// </summary>
        public void RemoveAllSegments()
        {
            if(dataProvider != null)
            {
                POBLog.Info(Tag, POBLogStrings.ClientRemoveAllSegmentsLog);
                dataProvider.Call("removeAllSegments");
            }
        }

        /// <summary>
        /// Cleanup API
        /// </summary>
        public void Destroy()
        {
            POBLog.Info(Tag, POBLogStrings.DestroyMethodLog);
            dataProvider = null;
        }

        /// <summary>
        /// Get reference of Java POBDataProvider instance
        /// </summary>
        /// <returns>AndroidJavaObject of native POBDataProvider Class</returns>
        public AndroidJavaObject GetNativeObject()
        {
            return dataProvider;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Converting Dictionary to array of values and keys and pass to native
        /// </summary>
        /// <param name="extension">Dictionary of string keys and string values</param>
        private void SetExtensionInNative(Dictionary<string, string> extension)
        {
            AndroidJavaObject JSONObject = POBAndroidUtils.ConvertDictionaryToJavaJSONObject(extension);
            if(dataProvider != null)
            {
                dataProvider.Call("setExt", JSONObject);
            }
        }
        #endregion
    }
}
#endif