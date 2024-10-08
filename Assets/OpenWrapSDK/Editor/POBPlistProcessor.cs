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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using System.Xml;

public class POBPlistProcessor
{
    private const string KEY_SK_ADNETWORK_ITEMS = "SKAdNetworkItems";
    private const string KEY_SK_ADNETWORK_ID = "SKAdNetworkIdentifier";
    private const string SKADNETWORKS_FILE_PATH = "OpenWrapSDK/Editor/OpenWrapSDKSKAdNetworkIDs.xml";

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget _, string path)
    {
        // Get Xcode project file path
        if (path != null)
        {
            // Add SKAdNetwork Ids into info.plist
            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            AddSkAdNetworksIds(plist);
            plist.WriteToFile(plistPath);
        }
    }

    private static void AddSkAdNetworksIds(PlistDocument plist)
    {
        List<string> skAdNetworkIds = ReadSKAdNetworkIDsFile();

        // Check if we have a vali  d list of SKAdNetworkIds that need to be added.
        if (skAdNetworkIds == null || skAdNetworkIds.Count < 1)
        {
            return;
        }

        _ = plist.root.values.TryGetValue(KEY_SK_ADNETWORK_ITEMS, out PlistElement skAdNetworkItems);

        HashSet<string> existingSkAdNetworkIds = new HashSet<string>();

        // If the SKAdNetworkItems are already in added the info.Plist, then get all the existing Ids
        if (skAdNetworkItems != null && skAdNetworkItems.GetType() == typeof(PlistElementArray))
        {
            IEnumerable<PlistElement> plistElementDictionaries = skAdNetworkItems.AsArray().values.Where(plistElement => plistElement.GetType() == typeof(PlistElementDict));
            foreach (PlistElement plistElement in plistElementDictionaries)
            {
                _ = plistElement.AsDict().values.TryGetValue(KEY_SK_ADNETWORK_ID, out PlistElement existingId);
                if (existingId == null || existingId.GetType() != typeof(PlistElementString) || string.IsNullOrEmpty(existingId.AsString()))
                {
                    continue;
                }

                _ = existingSkAdNetworkIds.Add(existingId.AsString());
            }
        }
        else
        {
            // The SKAdNetworkItems is not yet added. So, create an array of SKAdNetworkItems into which we will add our IDs.
            skAdNetworkItems = plist.root.CreateArray(KEY_SK_ADNETWORK_ITEMS);
        }

        foreach (string skAdNetworkId in skAdNetworkIds)
        {
            // Skip adding IDs that are already in the array.
            if (existingSkAdNetworkIds.Contains(skAdNetworkId))
            {
                continue;
            }

            PlistElementDict skAdNetworkItemDict = skAdNetworkItems.AsArray().AddDict();
            skAdNetworkItemDict.SetString(KEY_SK_ADNETWORK_ID, skAdNetworkId);
        }
    }

    private static List<string> ReadSKAdNetworkIDsFile()
    {
        List<string> skAdNetworkIds = new List<string>();
        string path = Path.Combine(Application.dataPath, SKADNETWORKS_FILE_PATH);
        try
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            using (FileStream fs = File.OpenRead(path))
            {
                XmlDocument document = new XmlDocument();
                document.Load(fs);

                XmlNode root = document.FirstChild;

                XmlNodeList nodes = root.SelectNodes(KEY_SK_ADNETWORK_ID);
                foreach (XmlNode node in nodes)
                {
                    skAdNetworkIds.Add(node.InnerText);
                }
            }
        }
#pragma warning disable 0168
        catch (FileNotFoundException e)
#pragma warning restore 0168
        {
            throw new BuildPlayerWindow.BuildMethodException("OpenWrapSDKSKAdNetworkIDs.xml not found");
        }
        catch (IOException e)
        {
            throw new BuildPlayerWindow.BuildMethodException("Failed to read OpenWrapSDKSKAdNetworkIDs.xml: " + e.Message);
        }
        return skAdNetworkIds;
    }



}
#endif