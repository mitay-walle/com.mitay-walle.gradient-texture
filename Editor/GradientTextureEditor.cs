using System.IO;
using System.Linq;
using System.Reflection;
using Packages.GradientTextureGenerator.Runtime;
using UnityEditor;
using UnityEngine;

namespace Packages.GradientTextureGenerator.Editor
{
	[CustomEditor(typeof(GradientTexture), true), CanEditMultipleObjects]
	public class GradientTextureEditor : UnityEditor.Editor
	{
		private GradientTexture GradientTexture;
		private UnityEditor.Editor _editor;

		public override bool HasPreviewGUI() => true;

		private void OnEnable()
		{
			GradientTexture = target as GradientTexture;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("Encode to PNG"))
			{
				foreach (Object target in targets)
				{

					string path = EditorUtility.SaveFilePanelInProject("Save file", $"{GradientTexture.name}_baked", "png", "Choose path to save file");

					if (string.IsNullOrEmpty(path))
					{
						Debug.LogError("[ GradientTextureEditor ] EncodeToPNG() save path is empty! canceled", GradientTexture);
						return;
					}

					byte[] bytes = ImageConversion.EncodeToPNG(GradientTexture.GetTexture());

					int length = "Assets".Length;
					string dataPath = Application.dataPath;
					dataPath = dataPath.Remove(dataPath.Length - length, length);
					dataPath += path;
					File.WriteAllBytes(dataPath, bytes);

					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
					AssetDatabase.ImportAsset(path);
					Texture2D image = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

					Debug.Log($"[ GradientTextureEditor ] EncodeToPNG() Success! png-gradient saved at '{path}'", image);
					EditorGUIUtility.PingObject(image);
					Selection.activeObject = image;
				}
			}
		}

		public override void DrawPreview(Rect previewArea)
		{
			Texture2D texture = GradientTexture.GetTexture();
			bool check = !_editor || _editor.target != texture;

			if (check && texture && (_editor == null || _editor.target != texture))
			{
				_editor = CreateEditor(targets.Select(t => (t as GradientTexture)?.GetTexture()).ToArray());
			}

			if (_editor && _editor.target)
			{
				_editor.DrawPreview(previewArea);
			}
		}

		public override void OnPreviewSettings()
		{
			if (_editor && _editor.target)
			{
				_editor.OnPreviewSettings();
			}
		}

		public override void ReloadPreviewInstances()
		{
			if (_editor && _editor.target)
			{
				_editor.ReloadPreviewInstances();
			}
		}


		public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
		{
			if (_editor && _editor.target)
			{
				_editor.OnInteractivePreviewGUI(r, background);
			}
		}
		public override void OnPreviewGUI(Rect r, GUIStyle background)
		{
			if (_editor && _editor.target)
			{
				_editor.OnPreviewGUI(r, background);
			}
		}


		public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
		{
			Texture2D tex = new Texture2D(width, height);
			EditorUtility.CopySerialized(GradientTexture.GetTexture(), tex);
			return tex;
		}

		private void OnDisable()
		{
			if (_editor)
			{
				_editor.GetType().GetMethod("OnDisable", BindingFlags.NonPublic)?.Invoke(_editor, null);
			}
		}
	}

}