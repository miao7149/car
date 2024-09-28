#if UNITY_IOS || UNITY_ANDROID
/*
*PubMatic Inc. ("PubMatic") CONFIDENTIAL
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

using OpenWrapSDK.Common;
namespace OpenWrapSDK
{
    /// <summary>
    /// This class represents RTB 'Segment' object
    /// </summary>
    public class POBSegment
    {
        #region Private variables
        /// <summary>
        /// iOS/Android segment client
        /// </summary>
        internal IPOBSegmentClient segmentClient;

        /// Segment value
        private string segValue;
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Constructor for segment object with identifier
        /// </summary>
        /// <param name="identifier">Segment identifier</param>
        public POBSegment(string identifier)
        {
            Identifier = identifier;
#if UNITY_IOS
            segmentClient = new iOS.POBSegmentClient(identifier, null);
#else
            segmentClient = new Android.POBSegmentClient(identifier,null);
#endif
        }

        /// <summary>
        /// Constructor for segment object with identifier and name
        /// </summary>
        /// <param name="identifier">Segment identifier</param>
        /// <param name="name">Segment name</param>
        public POBSegment(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;
#if UNITY_IOS
            segmentClient = new iOS.POBSegmentClient(identifier, name);
#else
            segmentClient = new Android.POBSegmentClient(identifier, name);
#endif
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBSegment()
        {
            Destroy();
        }
        #endregion

        #region Internal methods
        internal void Destroy()
        {
            Name = null;
            Identifier = null;
            segValue = null;

            if (segmentClient != null)
            {
                segmentClient.Destroy();
                segmentClient = null;
            }
        }
        #endregion

        #region Public APIs
        /// <summary>
        /// Getter for the segment name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Getter setter for the segment value
        /// </summary>
        public string Value
        {
            get => segValue;

            set
            {
                segValue = value;
                if (segmentClient != null)
                {
                    segmentClient.Value = value;
                }
            }
        }

        /// <summary>
        /// Getter for the segment identifier
        /// </summary>
        /// <returns>segment identifier as string</returns>
        public string Identifier { get; private set; }
        #endregion
    }
}
#endif