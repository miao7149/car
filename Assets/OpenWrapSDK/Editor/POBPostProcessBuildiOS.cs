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

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;

public class POBPostProcessBuildiOS
{
#if !UNITY_2019_3_OR_NEWER
        private const string UnityMainTargetName = "Unity-iPhone";
#endif

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        // Add OMSDK manually in Embeded frameworks settings of xcode project
        // Get Xcode project file path
        if (path != null)
        {
            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject pbxProj = new PBXProject();

            pbxProj.ReadFromString(File.ReadAllText(projPath));
            // Search for "UnityFramework" target
#if UNITY_2019_3_OR_NEWER
            string unityMainTargetGuid = pbxProj.GetUnityMainTargetGuid();
            string unityFrameworkTargetGuid = pbxProj.GetUnityFrameworkTargetGuid();
#else
            var unityMainTargetGuid = pbxProj.TargetGuidByName(UnityMainTargetName);
            var unityFrameworkTargetGuid = pbxProj.TargetGuidByName(UnityMainTargetName);
#endif

            if (unityMainTargetGuid != null)
            {
#if UNITY_2019_3_OR_NEWER
                // OMSDK will already be added in OpenWrap SDK pods. Get it's path.
                string framework = path + "/Pods/OpenWrapSDK/OpenWrapSDK/OMSDK_Pubmatic.xcframework";

                // Add it in Embeded frameworks
                string fileGuid = pbxProj.AddFile(framework, "Frameworks/" + framework);
                pbxProj.AddFileToEmbedFrameworks(unityMainTargetGuid, fileGuid);
#endif
                pbxProj.SetBuildProperty(unityMainTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
                pbxProj.AddBuildProperty(unityMainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
                pbxProj.AddBuildProperty(unityMainTargetGuid, "CLANG_ENABLE_MODULES", "YES");

                // Update the Xcode project file at it's path
                pbxProj.WriteToFile(projPath);
            }
        }
    }
}
#endif