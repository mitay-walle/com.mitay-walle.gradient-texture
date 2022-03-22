using Packages.GradientTextureGenerator.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Packages.GradientTextureGenerator.Editor
{
    public class ProjectIconsUtility : MonoBehaviour
    {
        //[DidReloadScripts]
        static ProjectIconsUtility()
        {
            EditorApplication.projectWindowItemOnGUI = ItemOnGUI;
        }

        static void ItemOnGUI(string guid, Rect rect)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<GradientTexture>(assetPath);
            if (asset == null) return;

            rect.width = rect.height;
            rect.height *= .8f;
            GUI.DrawTexture(rect, asset.GetTexture());
        }
    }
}
