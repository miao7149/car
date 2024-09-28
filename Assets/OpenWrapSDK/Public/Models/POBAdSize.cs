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

namespace OpenWrapSDK
{
    /// <summary>
    /// Defines the ad size
    /// </summary>
    public class POBAdSize
    {
        #region Private variables
        private readonly int width;
        private readonly int height;
        #endregion

        /// <summary>
        /// Getter for Ad size as 320x50
        /// </summary>
        public static POBAdSize Banner320x50 = new POBAdSize(320, 50);

        /// <summary>
        /// Getter for Ad size as 320x100
        /// </summary>
        public static POBAdSize Banner320x100 = new POBAdSize(320, 100);

        /// <summary>
        /// Getter for Ad size as 300x250
        /// </summary>
        public static POBAdSize Banner300x250 = new POBAdSize(300, 250);

        /// <summary>
        /// Getter for Ad size as 250x250
        /// </summary>
        public static POBAdSize Banner250x250 = new POBAdSize(250, 250);

        /// <summary>
        /// Getter for Ad size as 468x60
        /// </summary>
        public static POBAdSize Banner468x60 = new POBAdSize(468, 60);

        /// <summary>
        /// Getter for Ad size as 768x90
        /// </summary>
        public static POBAdSize Banner768x90 = new POBAdSize(768, 90);

        /// <summary>
        /// Getter for Ad size as 120x600
        /// </summary>
        public static POBAdSize Banner120x600 = new POBAdSize(120, 600);


        #region Constructor
        public POBAdSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        #endregion

        /// <summary>
        /// Getter for width of the ad
        /// </summary>
        /// <returns>width</returns>
        public int GetWidth()
        {
            return width;
        }

        /// <summary>
        /// Getter for height of the ad
        /// </summary>
        /// <returns>height</returns>
        public int GetHeight()
        {
            return height;
        }

        /// <summary>
        /// It logs ad size in string format
        /// </summary>
        public override string ToString()
        {
            return width.ToString() + "x" + height.ToString();
        }
    }
}
#endif
