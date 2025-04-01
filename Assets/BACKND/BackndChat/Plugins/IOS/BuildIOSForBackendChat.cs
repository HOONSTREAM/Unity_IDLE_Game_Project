#if UNITY_IOS && UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class BuildIOSForBackendChat
{

    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string targetGUID = proj.GetUnityFrameworkTargetGuid();

            // ENABLE_BITCODE를 false로 설정
            proj.SetBuildProperty(targetGUID, "ENABLE_BITCODE", "NO");

            // libz.tbd 라이브러리 추가
            string fileGuid = proj.AddFile("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk);
            proj.AddFileToBuild(targetGUID, fileGuid);

            File.WriteAllText(projPath, proj.WriteToString());
        }
    }
}
#endif