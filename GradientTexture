using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace mitaywalle.GradientTexture
{
    [CreateAssetMenu]
    public class GradientTexture : ScriptableObject, IEquatable<Texture2D>
    {
        [SerializeField] private Vector2Int _resolution = new Vector2Int(256, 256);
        [Range(0, 1), SerializeField] private float _dithering;


        [GradientUsage(true), SerializeField] private Gradient _horizontalTop = new Gradient
        {
            alphaKeys = new[] { new GradientAlphaKey(1, 1) },
            colorKeys = new[]
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1)
            }
        };

        [GradientUsage(true), SerializeField] private Gradient _horizontalBottom = new Gradient
        {
            alphaKeys = new[] { new GradientAlphaKey(1, 1) },
            colorKeys = new[]
            {
                new GradientColorKey(Color.white, 0)
            }
        };

        [SerializeField] private Gradient _verticalLerp = new Gradient
        {
            alphaKeys = new[] { new GradientAlphaKey(1, 1) },
            colorKeys = new[]
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1)
            }
        };

        [HideInInspector, SerializeField] private Texture2D _texture;

        private int _width => _resolution.x;
        private int _height => _resolution.y;

        public void FillColors()
        {
            float tVertical = 0;

            for (int y = 0; y < _height; y++)
            {
                tVertical = _verticalLerp.Evaluate((float)y / _height).r;
                for (int x = 0; x < _width; x++)
                {
                    float tHorizontal = (float)x / _width;
                    Color col = Color.Lerp(_horizontalBottom.Evaluate(tHorizontal),
                        _horizontalTop.Evaluate(tHorizontal), tVertical);
                    if (_dithering > 0)
                    {
                        bool dither1 = x % 2 == 0 && y % 2 != 0;
                        bool dither2 = x % 2 != 0 && y % 2 == 0;
                        if (dither1 || dither2)
                        {
                            col.r *= 1-_dithering;
                            col.g *= 1-_dithering;
                            col.b *= 1-_dithering;
                        }
                    }
                    _texture.SetPixel(x, y, col);
                }
            }

            _texture.Apply();
        }

        public bool Equals(Texture2D other)
        {
            return _texture.Equals(other);
        }

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (!_texture)
            {
                _texture = new Texture2D(_resolution.x, _resolution.y, DefaultFormat.LDR, TextureCreationFlags.None);
                _texture.name = $"Gradient";
                if (EditorUtility.IsPersistent(this))
                {
                    AssetDatabase.AddObjectToAsset(_texture, this);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(this));
                }
            }

            if (_texture.width != _resolution.x ||
                _texture.height != _resolution.y)
            {
                _texture.Resize(_resolution.x, _resolution.y);
            }

            _texture.alphaIsTransparency = true;


            FillColors();
            SetDirtyTexture();
        }

        [Conditional("UNITY_EDITOR")]
        private void SetDirtyTexture()
        {
            if (!_texture) return;
            EditorUtility.SetDirty(_texture);
        }

        #region Editor

#if UNITY_EDITOR


        [CustomEditor(typeof(GradientTexture), true), CanEditMultipleObjects]
        public class GradientTextureEditor : Editor
        {
            private GradientTexture GradientTexture;
            private Editor _editor;

            public override bool HasPreviewGUI() => true;

            private void OnEnable()
            {
                GradientTexture = target as GradientTexture;
            }

            public override void DrawPreview(Rect previewArea)
            {
                bool check = !_editor || _editor.target != GradientTexture?._texture;

                if (check && GradientTexture._texture)
                {
                    _editor = CreateEditor(GradientTexture._texture);
                }

                if (_editor)
                {
                    _editor.DrawPreview(previewArea);
                }
            }
        }
#endif

        #endregion
    }
}
