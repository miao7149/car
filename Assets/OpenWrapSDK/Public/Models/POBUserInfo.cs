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
* information that is confidential and/or proprietary, and is a trade secret, of  PubMatic.   ANY REPRODUCTION, MODIFICATION, DISTRIBUTION,  PERFORMANCE,
* OR DISPLAY OF OR THROUGH USE  OF THIS  SOURCE CODE  WITHOUT  THE EXPRESS WRITTEN CONSENT OF PUBMATIC IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE
* LAWS AND INTERNATIONAL TREATIES.  THE RECEIPT OR POSSESSION OF  THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT  MAY DESCRIBE, IN WHOLE OR IN PART.
*/

using System.Collections.Generic;
using System.Linq;
using OpenWrapSDK.Common;

namespace OpenWrapSDK
{
    /// <summary>
    /// Provides setters to pass user information
    /// </summary>
    public class POBUserInfo
    {
        #region Private members
        private readonly string Tag = "POBUserInfo";
        internal IPOBUserInfoClient client;
        private List<POBDataProvider> dataProviders;

        private int birthYear;
        private POBGender gender;
        private string metro;
        private string zip;
        private string city;
        private string region;
        private string keywords;
        #endregion

        #region Constructor
        public POBUserInfo()
        {
            gender = POBGender.None;
#if UNITY_IOS
            client = new iOS.POBUserInfoClient();
#else
            client = new Android.POBUserInfoClient();
#endif
            dataProviders = new List<POBDataProvider>();
        }

        ~POBUserInfo()
        {
            if (client != null)
            {
                client.Destroy();
                client = null;
            }

            ClearDataProviders();
            dataProviders = null;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Clear data providers list
        /// </summary>
        private void ClearDataProviders()
        {
            // Clear data providers list.
            if (dataProviders != null && dataProviders.Count > 0)
            {
                // Call destroy on each dataprovider to cleanup the native instances,
                // which will prevent accessing dangling pointers in future.
                foreach (POBDataProvider dataProvider in dataProviders)
                {
                    dataProvider.Destroy();
                }

                // Clear the local list of C# dataprovider instances
                dataProviders.Clear();
            }
        }
        #endregion

        #region Public APIs
        /// <summary>
        /// The year of birth in YYYY format.
        /// <br/>Example :
        /// <br/>adRequest.setBirthYear(1988);
        /// </summary>
        public int BirthYear
        {
            get => birthYear;

            set
            {
                birthYear = value;
                if (client != null)
                {
                    client.BirthYear = value;
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
            get => gender;

            set
            {
                gender = value;
                if (client != null)
                {
                    client.Gender = value;
                }
            }
        }

        /// <summary>
        /// City of user
        /// </summary>
        public string City
        {
            get => city;

            set
            {
                city = value;
                if (client != null)
                {
                    client.City = value;
                }
            }
        }

        /// <summary>
        /// Google metro code, You can set Designated market area (DMA) code of the user in this
        /// <br/>field. This field is applicable for US users only
        /// </summary>
        public string Metro
        {
            get => metro;

            set
            {
                metro = value;
                if (client != null)
                {
                    client.Metro = value;
                }
            }
        }
        /// <summary>
        /// The user's zip code may be useful in delivering geographically relevant ads
        /// </summary>
        public string Zip
        {
            get => zip;

            set
            {
                zip = value;
                if (client != null)
                {
                    client.Zip = value;
                }
            }
        }

        /// <summary>
        /// Region code using ISO-3166-2; 2-letter state code if USA
        /// </summary>
        public string Region
        {
            get => region;

            set
            {
                region = value;
                if (client != null)
                {
                    client.Region = value;
                }
            }
        }

        /// <summary>
        /// Comma separated list of keywords, interests, or intent.
        /// </summary>
        public string Keywords
        {
            get => keywords;

            set
            {
                keywords = value;
                if (client != null)
                {
                    client.Keywords = value;
                }
            }
        }

        /// <summary>
        /// Adds data containing user segment details
        /// </summary>
        /// <param name="dataProvider">Data to be added</param>
        public void AddDataProvider(POBDataProvider dataProvider)
        {
            if (dataProvider != null)
            {
                if (client != null)
                {
                    bool isDuplicate = dataProviders.Any(provider => provider.Name.Equals(dataProvider.Name));
                    if (!isDuplicate && dataProvider.Name != null &&
                        dataProvider.Name.Length > 0 && dataProvider.GetSegments().Count > 0)
                    {
                        POBLog.Info(Tag, string.Format(POBLogStrings.AddDataProviderSuccessLog, dataProvider.Identifier));
                        client.AddDataProvider(dataProvider);
                        // Add data provider if it is not yet added
                        dataProviders.Add(dataProvider);
                    }
                }
            }
        }

        /// <summary>
        /// Removes data for a specific provider from the user object
        /// </summary>
        /// <param name="name">Name of the data provider to remove data for</param>
        public void RemoveDataProvider(string name)
        {
            if (dataProviders.Count > 0)
            {
                POBDataProvider dataProvider = dataProviders.Find(provider => provider.Name.Equals(name));
                POBLog.Info(Tag, string.Format(POBLogStrings.RemoveDataProviderSuccessLog, name));
                
                if (client != null)
                {
                    client.RemoveDataProvider(dataProvider);
                }
                _ = dataProviders.Remove(dataProvider);
                dataProvider.Destroy();
            }
            else
            {
                POBLog.Warning(Tag, string.Format(POBLogStrings.RemoveDataProviderFailedWithNameLog, name));
            }
        }

        /// <summary>
        /// Removes data for all providers from the user object
        /// </summary>
        public void RemoveAllDataProviders()
        {
            POBLog.Info(Tag, POBLogStrings.RemoveAllDataProvidersLog);

            if (client != null)
            {
                client.RemoveAllDataProviders();
            }

            // Clear data providers
            ClearDataProviders();
        }

        /// <summary>
        /// Returns user data with given name
        /// </summary>
        /// <param name="name">Data provider name</param>
        /// <returns>Reference of POBDataProvider object associated with name</returns>
        public POBDataProvider GetDataProvider(string name) {
            foreach (POBDataProvider dataProvider in dataProviders) {
                if (dataProvider.Name.Equals(name)) {
                    return dataProvider;
                }
            }
            return null;
        }

        /// <summary>
        /// Get list of POBDataProvider instances
        /// </summary>
        /// <returns>Get List of POBDataProvider</returns>
        public List<POBDataProvider> GetDataProviders()
        {
            return dataProviders;
        }
        #endregion
    }

    /// <summary>
    /// The gender of the user.
    /// </summary>
    public enum POBGender
    {
        None = -1,
        Other = 0,
        Male = 1,
        Female = 2
    }
}
#endif
