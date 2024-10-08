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

using OpenWrapSDK.Common;
using UnityEngine;

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Android POBUserInfo client to set or get parameters on userInfo object.
    /// </summary>
    internal class POBUserInfoClient : IPOBUserInfoClient
    {
        #region Private members
        private readonly string Tag = "POBUserInfoClient";
        // Reference to Android's POBUSerInfo instance
        private AndroidJavaObject userInfo;
        #endregion

        #region Constructor / Destructor
        /// <summary>
        /// Constructor
        /// </summary>
        internal POBUserInfoClient()
        {
            userInfo = new AndroidJavaObject(POBConstants.POBUserInfoClassName);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~POBUserInfoClient()
        {
            Destroy();
        }
        #endregion

        #region IPOBUserInfoClient
        /// <summary>
        /// The year of birth in YYYY format.
        /// <br/> Example : birthYear = 1988;
        /// </summary>
        public int BirthYear {
            set
            {
                if(userInfo != null)
                {
                    userInfo.Call("setBirthYear", value);
                }
            }
        }

        /// <summary>
        /// Set the user gender,
        /// <br/>Possible options are:
        /// <br/>OTHER
        /// <br/>MALE
        /// <br/>FEMALE
        /// </summary>
        public POBGender Gender
        {
            set
            {
                if (userInfo != null)
                {
                    AndroidJavaObject javaObject = POBAndroidUtils.ConvertGenderToJavaObject(value);
                    if (javaObject != null)
                    {
                        userInfo.Call("setGender", javaObject);
                    }
                }
            }
        }

        /// <summary>
        /// City of user
        /// </summary>
        public string City
        {
            set
            {
                if (userInfo != null)
                {
                    userInfo.Call("setCity", value);
                }
            }
        }

        /// <summary>
        /// Google metro code, You can set Designated market area (DMA) code of the user in this
        /// <br/>field. This field is applicable for US users only
        /// </summary>
        public string Metro
        {
            set
            {
                if (userInfo != null)
                {
                    userInfo.Call("setMetro", value);
                }
            }
        }

        /// <summary>
        /// The user's zip code may be useful in delivering geographically relevant ads
        /// </summary>
        public string Zip
        {
            set
            {
                if (userInfo != null)
                {
                    userInfo.Call("setZip", value);
                }
            }
        }

        /// <summary>
        /// Region code using ISO-3166-2; 2-letter state code if USA
        /// </summary>
        public string Region
        {
            set
            {
                if (userInfo != null)
                {
                    userInfo.Call("setRegion", value);
                }
            }
        }

        /// <summary>
        /// Comma separated list of keywords, interests, or intent.
        /// </summary>
        public string Keywords
        {
            set
            {
                if (userInfo != null)
                {
                    userInfo.Call("setKeywords", value);
                }
            }
        }

        /// <summary>
        /// Adds data containing user segment details
        /// </summary>
        /// <param name="dataProvider">Data to be added</param>
        public void AddDataProvider(POBDataProvider dataProvider)
        {
            if(userInfo != null && dataProvider != null && dataProvider.dataProviderClient != null)
            {
                AndroidJavaObject dataProviderObject = dataProvider.dataProviderClient.GetNativeObject();
                if(dataProviderObject != null)
                {
                    POBLog.Info(Tag, POBLogStrings.ClientAddDataProviderLog);
                    userInfo.Call("addDataProvider", dataProviderObject);
                }
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.AddDataProviderFailedLog);
            }
        }

        public void Destroy()
        {
            userInfo = null;
        }

        /// <summary>
        /// Getter for reference of Android's POBUserInfo instance
        /// </summary>
        /// <returns>AndroidJavaObject of Android's POBUserInfo instance</returns>
        /// 
        public AndroidJavaObject GetNativeObject()
        {
            return userInfo;
        }

        /// <summary>
        /// Removes data for all providers from the user object
        /// </summary>
        public void RemoveAllDataProviders()
        {
            if(userInfo != null)
            {
                POBLog.Info(Tag, POBLogStrings.ClientRemoveAllDataProvidersLog);
                userInfo.Call("removeAllDataProviders");
            }
        }

        /// <summary>
        /// Removes data for a specific provider from the user object
        /// </summary>
        /// <param name="name">Name of the data provider to remove data for</param>
        public void RemoveDataProvider(POBDataProvider dataProvider)
        {
            if(userInfo != null && dataProvider != null)
            {
                POBLog.Info(Tag, POBLogStrings.ClientRemoveDataProviderLog);
                userInfo.Call<AndroidJavaObject>("removeDataProvider", dataProvider.Name);
            }
            else
            {
                POBLog.Warning(Tag, POBLogStrings.RemoveDataProviderFailedLog);
            }
        }
        #endregion
    }
}
#endif