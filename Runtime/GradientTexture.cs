using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Packages.GradientTextureGenerator.Runtime
{
    /// <summary>
    /// Main Asset, holds settings, create, hold and change Texture2D's pixels, name
    /// </summary>
    [CreateAssetMenu(fileName = "NewGradientName", menuName = "Texture/Gradient")]
    public class GradientTexture : ScriptableObject, IEquatable<Texture2D>, ISerializationCallbackReceiver
    {
        [SerializeField] private Vector2Int _resolution = new Vector2Int(256, 256);
        [Range(0, 1), SerializeField] private float _dithering = default;
        [SerializeField] private AnimationCurve _verticalLerp = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField, GradientUsage(true)] private Gradient _horizontalTop = GetDefaultGradient();
        [SerializeField, GradientUsage(true)] private Gradient _horizontalBottom = GetDefaultGradient();
        [HideInInspector, SerializeField] private Texture2D _texture = default;

        public Texture2D GetTexture() => _texture;
        private int _width => _resolution.x;
        private int _height => _resolution.y;

        private static Gradient GetDefaultGradient() => new Gradient
        {
            alphaKeys = new[] {new GradientAlphaKey(1, 1)},
            colorKeys = new[]
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1)
            }
        };

        public void FillColors()
        {
            float tVertical = 0;

            for (int y = 0; y < _height; y++)
            {
                tVertical = _verticalLerp.Evaluate((float)y / _height);

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
                            col.r *= 1 - _dithering;
                            col.g *= 1 - _dithering;
                            col.b *= 1 - _dithering;
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

        private void OnValidate()
        {
            string assetPath = AssetDatabase.GetAssetPath(this);

            if (!_texture && this != null)
            {
                AssetDatabase.ImportAsset(assetPath);
                _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            }

            if (!_texture)
            {
#if UNITY_2018
                _texture = new Texture2D(_resolution.x, _resolution.y);
#else
                _texture = new Texture2D(_resolution.x, _resolution.y, DefaultFormat.LDR, TextureCreationFlags.None);
#endif
                if (_texture.name != name) _texture.name = name;
            }

            if (!_texture) return;

            if (_texture.name != name)
            {
                _texture.name = name;
            }
            else
            {
                if (_texture.width != _resolution.x ||
                    _texture.height != _resolution.y)
                {
#if UNITY_2022_1_OR_NEWER
                    _texture.Reinitialize(_resolution.x, _resolution.y);
#else
                    _texture.Resize(_resolution.x, _resolution.y);
#endif
                }

                _texture.alphaIsTransparency = true;

                FillColors();

                SetDirtyTexture();
            }
            
#if UNITY_EDITOR
            if (!EditorUtility.IsPersistent(this)) return;
            if (AssetDatabase.IsSubAsset(_texture)) return;
            if (AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath)) return;

            #if UNITY_2020_1_OR_NEWER
            if (AssetDatabase.IsAssetImportWorkerProcess()) return;
            #endif
            AssetDatabase.AddObjectToAsset(_texture, this);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            //AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            //AssetDatabase.Refresh();
#endif
        }

        #region Editor

        [Conditional("UNITY_EDITOR")]
        private void SetDirtyTexture()
        {
#if UNITY_EDITOR
            if (!_texture) return;

            EditorUtility.SetDirty(_texture);
#endif
        }

        #endregion

        public void OnAfterDeserialize() { }
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (!_texture || _texture.name == name) return;

            _texture.name = name;

            //AssetDatabase.SaveAssets();
  #endif
        }
    }
}
