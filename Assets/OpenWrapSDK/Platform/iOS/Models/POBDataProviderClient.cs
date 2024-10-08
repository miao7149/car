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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenWrapSDK.Common;

namespace OpenWrapSDK.iOS
{
    /// <summary>
    /// iOS data provider client to set or get parameters on data provider object.
    /// </summary>
    internal class POBDataProviderClient : IPOBDataProviderClient
    {
        #region Private variables
        private IntPtr dataProviderPtr;
        private readonly string Tag = "POBDataProviderClient";
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Constructor with data provider identifier & name
        /// </summary>
        /// <param name="name">Data provider's name</param>
        /// <param name="identifier">Data provider's id</param>
        internal POBDataProviderClient(string name, string identifier)
        {
            dataProviderPtr = POBUCreateDataProvider(name, identifier);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBDataProviderClient()
        {
            if (dataProviderPtr != IntPtr.Zero)
            {
                POBLog.Info(Tag, POBLogStrings.DestroyDataProvider);
                // In a case - 
                // When any publisher creates dataprovider in C#, it's Obj-C instance gets added in iOS's local cache
                // maintained by plugin.
                // If it is not added further in POBUserInfo and destructor of C# instance is called, cleanup of C# object is done properly
                // but the Obj-C instance stored in iOS's local cache is not removed automatically, which results in memory leak.
                // To avoid this, it is MANDATORY to call destroy manually from destructor.
                POBUDestroyDataProvider(dataProviderPtr);
                dataProviderPtr = IntPtr.Zero;
            }
        }
        #endregion

        #region iOS Plugin methods
        [DllImport("__Internal")]
        internal static extern IntPtr POBUCreateDataProvider(string name, string identifier);

        [DllImport("__Internal")]
        internal static extern void POBUAddSegment(IntPtr dataProvider, IntPtr segment);

        [DllImport("__Internal")]
        internal static extern void POBURemoveSegment(IntPtr dataProvider, IntPtr segment);

        [DllImport("__Internal")]
        internal static extern void POBURemoveAllSegments(IntPtr dataProvider);

        [DllImport("__Internal")]
        internal static extern void POBUSetDataProviderSegTax(IntPtr dataProvider, int segTax);

        [DllImport("__Internal")]
        internal static extern void POBUSetDataProviderExtension(IntPtr dataProvider, string[] keysList, string[] valuesList, int dictCount);

        [DllImport("__Internal")]
        internal static extern void POBUDestroyDataProvider(IntPtr dataProvider);
        #endregion

        #region IPOBDataProviderClient
        /// <summary>
        /// segment taxonomy id. Reference: https://github.com/InteractiveAdvertisingBureau/AdCOM/blob/master/AdCOM%20v1.0%20FINAL.md#list--category-taxonomies
        /// </summary>
        public int SegTax
        {
            set
            {
                if (dataProviderPtr != IntPtr.Zero)
                {
                    POBUSetDataProviderSegTax(dataProviderPtr, value);
                }
            }
        }

        /// <summary>
        /// RTB extension object for this data
        /// </summary>
        public Dictionary<string, string> Extension
        {
            set => SetExtensionInNative(value);
        }

        /// <summary>
        /// Adds a segment details to data object
        /// </summary>
        /// <param name="segment">Segment details to be added to data object</param>
        public void AddSegment(POBSegment segment)
        {
            if (dataProviderPtr != null && segment != null)
            {
                if (segment.segmentClient != null)
                {
                    IntPtr segmentPtr = segment.segmentClient.GetNativePtr();
                    if (segmentPtr != IntPtr.Zero)
                    {
                        POBLog.Info(Tag, POBLogStrings.AddSegment);
                        POBUAddSegment(dataProviderPtr, segmentPtr);
                    }
                }
            }
        }

        /// <summary>
        /// Removes a segment details from data object
        /// </summary>
        /// <param name="segment">Segment details to be removed</param>
        public void RemoveSegment(POBSegment segment)
        {
            if (dataProviderPtr != IntPtr.Zero && segment != null && segment.segmentClient != null)
            {
                IntPtr segmentPtr = segment.segmentClient.GetNativePtr();
                if (segmentPtr != IntPtr.Zero)
                {
                    POBLog.Info(Tag, POBLogStrings.RemoveSegment);
                    POBURemoveSegment(dataProviderPtr, segmentPtr);
                }
            }
        }

        /// <summary>
        /// Removes all segments from the data object
        /// </summary>
        public void RemoveAllSegments()
        {
            if (dataProviderPtr != IntPtr.Zero)
            {
                POBLog.Info(Tag, POBLogStrings.RemoveAllSegments);
                POBURemoveAllSegments(dataProviderPtr);
            }
        }

        /// <summary>
        /// Cleanup API
        /// </summary>
        public void Destroy()
        {
            // This is a cleanup API, which is expected to be used only by internal classes.
            // When any data provider is removed from native iOS, it's Objective-C instance also gets deallocated
            // as no other object retains it. So we MUST cleanup it's reference from this client class
            // as soon as it is removed, to avoid dangling pointer access in future.
            dataProviderPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Get reference of native (ObjC) instance of POBDataProvider
        /// </summary>
        /// <returns>ObjC reference as IntPtr</returns>
        public IntPtr GetNativePtr()
        {
            return dataProviderPtr;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Converting Dictionary to array of values and keys and pass to native
        /// </summary>
        /// <param name="extension">Dictionary of string keys and string values</param>
        private void SetExtensionInNative(Dictionary<string, string> extension)
        {
            if (dataProviderPtr != IntPtr.Zero && extension != null)
            {
                string[] keys = extension.Keys.ToArray();
                string[] values = extension.Values.ToArray();
                POBLog.Info(Tag, POBLogStrings.SetDataProviderExtension);
                POBUSetDataProviderExtension(dataProviderPtr, keys, values, keys.Length);
            }
        }
        #endregion
    }
}
#endif
