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
using OpenWrapSDK.Common;

namespace OpenWrapSDK.iOS
{
    /// <summary>
    /// iOS POBSegment client to set or get parameters on POBSegment object.
    /// </summary>
    internal class POBSegmentClient : IPOBSegmentClient
    {
        #region Private variables

        /// Reference to iOS's POBSegment
        private IntPtr segmentPtr = IntPtr.Zero;
        private readonly string Tag = "POBSegmentClient";
        #endregion

        #region iOS Plugin methods
        [DllImport("__Internal")]
        internal static extern IntPtr POBUCreateSegment(string identifier, string name);

        [DllImport("__Internal")]
        internal static extern void POBUDestroySegment(IntPtr segment);

        [DllImport("__Internal")]
        internal static extern void POBUSetSegmentValue(IntPtr segment, string value);
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Constructor for segment client object with identifier and name
        /// </summary>
        /// <param name="identifier">Segment identifier</param>
        /// <param name="name">Segment name</param>
        internal POBSegmentClient(string identifier, string name)
        {
            segmentPtr = POBUCreateSegment(identifier, name);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBSegmentClient()
        {
            if (segmentPtr != IntPtr.Zero)
            {
                POBLog.Info(Tag, POBLogStrings.DestroySegment);
                // In a case - 
                // When any publisher creates segment in C#, it's Obj-C instance gets added in iOS's local cache
                // maintained by plugin.
                // If it is not added further in POBDataProvider and destructor of C# instance is called, cleanup of C# object is done properly
                // but the Obj-C instance stored in iOS's local cache is not removed automatically, which results in memory leak.
                // To avoid this, it is MANDATORY to call destroy manually from destructor.
                POBUDestroySegment(segmentPtr);
                segmentPtr = IntPtr.Zero;
            }
        }
        #endregion

        #region IPOBSegmentClient
        /// <summary>
        /// Getter setter for the segment value
        /// </summary>
        public string Value
        {
            set
            {
                if (segmentPtr != IntPtr.Zero)
                { 
                    POBUSetSegmentValue(segmentPtr, value);
                }
            }
        }

        /// <summary>
        /// Getter for the iOS's POBSegment reference
        /// </summary>
        /// <returns>IntPtr of POBSegment from iOS</returns>
        public IntPtr GetNativePtr()
        {
            return segmentPtr;
        }

        /// <summary>
        /// Cleanup API
        /// </summary>
        public void Destroy()
        {
            // This is a cleanup API, which is expected to be used only by internal classes.
            // When any segment is removed from native iOS, it's Objective-C instance also gets deallocated
            // as no other object retains it. So we MUST cleanup it's reference from this client class
            // as soon as it is removed, to avoid dangling pointer access in future.
            segmentPtr = IntPtr.Zero;
        }
        #endregion
    }
}
#endif