using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Packages.GradientTextureGenerator.Runtime
{
    public interface IGradientTextureForEditor
    {
        void CreateTexture();

        Texture2D GetTexture();

        void LoadExisitingTexture();
    }

    /// <summary>
    /// Main Asset, holds settings, create, hold and change Texture2D's pixels, name
    /// </summary>
    [CreateAssetMenu(fileName = "NewGradientName", menuName = "Texture/Gradient")]
    public class GradientTexture : ScriptableObject, IEquatable<Texture2D>, ISerializationCallbackReceiver,
        IGradientTextureForEditor
    {
        [SerializeField] Vector2Int _resolution = new Vector2Int(256, 256);
        [SerializeField] bool _sRGB = true;
        [SerializeField] AnimationCurve _verticalLerp = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField, GradientUsage(true)] Gradient _horizontalTop = GetDefaultGradient();
        [SerializeField, GradientUsage(true)] Gradient _horizontalBottom = GetDefaultGradient();
        [SerializeField, HideInInspector] Texture2D _texture = default;

        public Texture2D GetTexture() => _texture;

        public bool GetSRGB() => _sRGB;

        public void SetSRGB(bool value)
        {
            _sRGB = value;
            OnValidate();
        }

        int _width => _resolution.x;
        int _height => _resolution.y;

        public static implicit operator Texture2D(GradientTexture asset) => asset.GetTexture();

        static Gradient GetDefaultGradient() => new Gradient
        {
            alphaKeys = new[] { new GradientAlphaKey(1, 1) },
            colorKeys = new[]
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1)
            }
        };

        public void FillColors(bool useRGB)
        {
            bool isLinear = QualitySettings.activeColorSpace == ColorSpace.Linear;

            float tVertical = 0;

            for (int y = 0; y < _height; y++)
            {
                tVertical = _verticalLerp.Evaluate((float) y / _height);

                for (int x = 0; x < _width; x++)
                {
                    float tHorizontal = (float) x / _width;

                    Color color = Color.Lerp(_horizontalBottom.Evaluate(tHorizontal),
                        _horizontalTop.Evaluate(tHorizontal),
                        tVertical);

                    color = useRGB && isLinear ? color.linear : color;
                    _texture.SetPixel(x, y, color);
                }
            }

            _texture.Apply();
        }

        public bool Equals(Texture2D other)
        {
            return _texture.Equals(other);
        }

        void OnValidate() => ValidateTextureValues();

        void IGradientTextureForEditor.LoadExisitingTexture()
        {
            #if UNITY_EDITOR
            if (!_texture)
            {
                string assetPath = AssetDatabase.GetAssetPath(this);
                _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            }
            #endif
        }

        void IGradientTextureForEditor.CreateTexture()
        {
            #if UNITY_EDITOR
            //if (EditorApplication.isUpdating) return;

            string assetPath = AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(assetPath)) return;

            if (!_texture && this != null && !EditorApplication.isUpdating)
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

            ValidateTextureValues();

            if (!EditorUtility.IsPersistent(this)) return;
            if (AssetDatabase.IsSubAsset(_texture)) return;
            if (AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath)) return;

            #if UNITY_2020_1_OR_NEWER
            if (AssetDatabase.IsAssetImportWorkerProcess()) return;
            #endif
            AssetDatabase.AddObjectToAsset(_texture, this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
#endif
        }

        void ValidateTextureValues()
        {
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

#if UNITY_EDITOR
                _texture.alphaIsTransparency = true;
#endif
                FillColors(_sRGB);

                SetDirtyTexture();
            }
        }

        #region Editor

        [Conditional("UNITY_EDITOR")]
        void SetDirtyTexture()
        {
#if UNITY_EDITOR
            if (!_texture) return;

            EditorUtility.SetDirty(_texture);
#endif
        }

        #endregion

        public void OnAfterDeserialize()
        {
        }

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
