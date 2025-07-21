using UnityEngine;
using UnityEditor;
using System.IO;

namespace EasySingletons.Editor
{
    public class EditorUtils
    {
        [MenuItem("Assets/Create/Scripting/Singleton Script", false, 10)]
        private static void CreateSingletonScriptAsset()
        {
            string path = "Packages/com.jjasundry.easy-singletons/Editor/SingletonTemplate.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(path, "NewSingletonScript.cs");
        }
    }
}
