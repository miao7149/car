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

namespace OpenWrapSDK
{
    public interface IPOBImpression
    {
        /// <summary>
        /// Returns the Ad unit ID from the 0th index of impression array. In case Ad unit ID is
        /// not available then returns empty string.
        /// </summary>
        /// <returns>ad unit id</returns>
        string GetAdUnitId();

        /// <summary>
        /// Returns the zone id required for reporting purpose.
        /// </summary>
        /// <returns>zone id</returns>
        string GetZoneId();

        /// <summary>
        /// Set the zone id. This parameter is used to pass a zone ID for reporting.
        /// </summary>
        /// <param name="zoneId">zone id</param>
        void SetZoneId(string zoneId);

        /// <summary>
        /// Returns a test creative id.
        /// </summary>
        /// <returns>test creative id.</returns>
        string GetTestCreativeId();

        /// <summary>
        /// This parameter is used to request a test creative.
        /// </summary>
        /// <param name="creativeId">test creative id.</param>
        void SetTestCreativeId(string creativeId);


        /// <summary>
        /// Returns the Fold placement of the ad to be served.
        /// </summary>
        /// <returns>Fold placement of the ad</returns>
        POBAdPosition GetAdPosition();

        /// <summary>
        /// The fold placement of the ad to be served. Possible values are:
        /// <br/>Unknown
        /// <br/>AboveTheFold
        /// <br/>BelowTheFold
        /// <br/>Header
        /// <br/>Footer
        /// <br/>Sidebar
        /// <br/>Fullscreen
        /// </summary>
        /// <param name="position">Fold placement of the ad</param>
        void SetAdPosition(POBAdPosition position);

        /// <summary>
        /// Adds custom key-value parameters in an Ad request.
        /// Multiple values against the same key canbe passed in list.
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        void SetCustomParams(Dictionary<string, List<string>> customParams);
    }
}
#endif