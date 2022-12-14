using System.IO;
using System.Linq;
using System.Reflection;
using Packages.GradientTextureGenerator.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.GradientTextureGenerator.Editor
{
    [CustomEditor(typeof(GradientTexture), true), CanEditMultipleObjects]
    public class GradientTextureEditor : UnityEditor.Editor
    {
        GradientTexture _gradientTexture;
        UnityEditor.Editor _editor;

        public override bool HasPreviewGUI() => true;

        void OnEnable()
        {
            _gradientTexture = target as GradientTexture;
        }

        public override void OnInspectorGUI()
        {
            if (_gradientTexture.GetTexture() == null)
            {
                (_gradientTexture as IGradientTextureForEditor).CreateTexture();
            }

            base.OnInspectorGUI();

            string buttonText = "Encode to PNG" + (targets.Length > 1 ? $" ({targets.Length})" : "");

            if (GUILayout.Button(buttonText))
            {
                foreach (Object target in targets)
                {
                    GradientTexture targetTexture = target as GradientTexture;

                    string path = EditorUtility.SaveFilePanelInProject("Save file",
                        $"{targetTexture.name}_baked",
                        "png",
                        "Choose path to save file");

                    if (string.IsNullOrEmpty(path))
                    {
                        Debug.LogError("[ GradientTextureEditor ] EncodeToPNG() save path is empty! canceled",
                            targetTexture);

                        return;
                    }

                    bool wasSRGB = targetTexture.GetSRGB();
                    if (wasSRGB) targetTexture.SetSRGB(false);
                    byte[] bytes = ImageConversion.EncodeToPNG(targetTexture.GetTexture());
                    targetTexture.SetSRGB(wasSRGB);

                    int length = "Assets".Length;
                    string dataPath = Application.dataPath;
                    dataPath = dataPath.Remove(dataPath.Length - length, length);
                    dataPath += path;
                    File.WriteAllBytes(dataPath, bytes);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    AssetDatabase.ImportAsset(path);
                    Texture2D image = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                    TextureImporter importer = (TextureImporter) AssetImporter.GetAtPath(path);
                    importer.sRGBTexture = targetTexture.GetSRGB();
                    importer.SaveAndReimport();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    Debug.Log($"[ GradientTextureEditor ] EncodeToPNG() Success! png-gradient saved at '{path}'",
                        image);

                    EditorGUIUtility.PingObject(image);
                    Selection.activeObject = image;
                }
            }
        }

        public override void DrawPreview(Rect previewArea)
        {
            Texture2D texture = _gradientTexture.GetTexture();
            bool check = !_editor || _editor.target != texture;

            if (check && texture && (_editor == null || _editor.target != texture))
            {
                try
                {
                    _editor = CreateEditor(targets.Select(t => (t as GradientTexture)?.GetTexture()).ToArray());
                }
                catch
                {
                    _editor = null;
                    //Debug.LogException(e);
                    //throw;
                }
            }

            if (_editor && _editor.target)
            {
                try
                {
                    _editor.DrawPreview(previewArea);
                }
                catch
                {
                    //Debug.LogException(e);
                    //throw;
                }
            }
        }

        public override void OnPreviewSettings()
        {
            if (_editor && _editor.target)
            {
                try
                {
                    _editor.OnPreviewSettings();
                }
                catch
                {
                    //Debug.LogException(e);
                    //throw;
                }
            }
        }

        public override void ReloadPreviewInstances()
        {
            if (_editor && _editor.target)
            {
                try
                {
                    _editor.ReloadPreviewInstances();
                }
                catch
                {
                    //Debug.LogException(e);
                    //throw;
                }
            }
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            if (_editor && _editor.target)
            {
                try
                {
                    _editor.OnInteractivePreviewGUI(r, background);
                }
                catch
                {
                    //Debug.LogException(e);
                    //throw;
                }
            }
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (_editor && _editor.target)
            {
                try
                {
                    _editor.OnPreviewGUI(r, background);
                }
                catch
                {
                    //Debug.LogException(e);
                    //throw;
                }
            }
        }

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            if (_gradientTexture == null) return null;
            if (_gradientTexture.GetTexture() == null) return null;
            Texture2D tex = new Texture2D(width, height);
            EditorUtility.CopySerialized(_gradientTexture.GetTexture(), tex);
            return tex;
        }

        void OnDisable()
        {
            if (_editor)
            {
                _editor.GetType().GetMethod("OnDisable", BindingFlags.NonPublic)?.Invoke(_editor, null);
            }
        }

        void OnDestroy()
        {
            if (_editor)
            {
                DestroyImmediate(_editor);
            }
        }
    }
}