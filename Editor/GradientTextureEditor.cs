using System.IO;
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
				string path = EditorUtility.SaveFilePanelInProject("Save file", $"{GradientTexture.name}_baked", "png", "Choose path to save file");

				if (string.IsNullOrEmpty(path))
				{
					Debug.LogError("[ GradientTextureEditor ] EncodeToPNG() save path is empty! canceled", GradientTexture);
					return;
				}

				byte[] bytes = ImageConversion.EncodeToPNG(GradientTexture.GetTexture());

				int length = "Assets".Length;
				string dataPath = Application.dataPath;
				dataPath = dataPath.Remove(dataPath.Length - length,length);
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

		public override void DrawPreview(Rect previewArea)
		{
			Texture2D texture = GradientTexture.GetTexture();
			bool check = !_editor || _editor.target != texture;

			if (check && texture)
			{
				_editor = CreateEditor(texture);
			}

			if (_editor && _editor.target)
			{
				_editor.DrawPreview(previewArea);
			}
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