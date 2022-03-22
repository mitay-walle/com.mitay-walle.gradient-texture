using UnityEditor;
using UnityEngine;
using Assets.mitaywalle.GradientTextureGenerator.Runtime;

namespace Assets.mitaywalle.GradientTextureGenerator.Editor
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

        public override void DrawPreview(Rect previewArea)
        {
            Texture2D texture = GradientTexture.GetTexture();
            bool check = !_editor || _editor.target != texture;

            if (check && texture)
            {
                _editor = CreateEditor(texture);
            }

            if (_editor)
            {
                _editor.DrawPreview(previewArea);
            }
        }
    }
}
