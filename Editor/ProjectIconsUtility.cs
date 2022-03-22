using Packages.GradientTextureGenerator.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Packages.GradientTextureGenerator.Editor
{
    public class ProjectIconsUtility : MonoBehaviour
    {
        [DidReloadScripts]
        static ProjectIconsUtility()
        {
            EditorApplication.projectWindowItemOnGUI -= ItemOnGUI;
            EditorApplication.projectWindowItemOnGUI += ItemOnGUI;
        }

        static void ItemOnGUI(string guid, Rect rect)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<GradientTexture>(assetPath);

            if (asset == null) return;
            if (!asset.GetTexture()) return;


            if (rect.height > 30)
            {
                // rect.position = new Vector2(rect.position.x + rect.height * .1f + rect.width - rect.height, rect.position.y);
                // rect.height *= .87f;
                // rect.width = rect.height - 5;
                // rect.height *= .95f;
            }
            else
            {
                //rect.position = new Vector2(rect.position.x-2 + rect.width - rect.height, rect.position.y);
                rect.width = rect.height *= .9f;
                GUI.DrawTexture(rect, asset.GetTexture());
            }

        }
    }
}
