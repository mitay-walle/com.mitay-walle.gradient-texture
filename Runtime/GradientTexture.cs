using System;
using System.Diagnostics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.mitaywalle.GradientTextureGenerator.Runtime
{
    [CreateAssetMenu(fileName = "NewGradientName", menuName = "Texture/Gradient")]
    public class GradientTexture : ScriptableObject, IEquatable<Texture2D>, ISerializationCallbackReceiver
    {
        [SerializeField] private Vector2Int _resolution = new Vector2Int(256, 256);
        [Range(0, 1), SerializeField] private float _dithering = default;


        [GradientUsage(true), SerializeField] private Gradient _horizontalTop = new Gradient
        {
            alphaKeys = new[] {new GradientAlphaKey(1, 1)},
            colorKeys = new[]
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1)
            }
        };

        [GradientUsage(true), SerializeField] private Gradient _horizontalBottom = new Gradient
        {
            alphaKeys = new[] {new GradientAlphaKey(1, 1)},
            colorKeys = new[]
            {
                new GradientColorKey(Color.white, 0)
            }
        };

        [SerializeField] private Gradient _verticalLerp = new Gradient
        {
            alphaKeys = new[] {new GradientAlphaKey(1, 1)},
            colorKeys = new[]
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1)
            }
        };


        [SerializeField] private Texture2D _texture;

        private int[] _hashes = new int[4];
        public Texture2D GetTexture() => _texture;
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

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (!_texture)
            {
#if UNITY_2018
                _texture = new Texture2D(_resolution.x, _resolution.y);
#else
                _texture = new Texture2D(_resolution.x, _resolution.y, DefaultFormat.LDR, TextureCreationFlags.None);
#endif

                if (_texture.name != name) _texture.name = name;
#if UNITY_EDITOR
                if (EditorUtility.IsPersistent(this))
                {
                    AssetDatabase.AddObjectToAsset(_texture, this);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
                    _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(this));
                }
#endif
            }

            if (_texture.name != name)
            {
                _texture.name = name;
            }
            else
            {
                if (_texture.width != _resolution.x ||
                    _texture.height != _resolution.y)
                {
                    _texture.Resize(_resolution.x, _resolution.y);
                }

                _texture.alphaIsTransparency = true;


                if (_hashes[0] != _horizontalTop.GetHashCode()
                    || _hashes[1] != _horizontalBottom.GetHashCode()
                    || _hashes[2] != _verticalLerp.GetHashCode()
                    || _hashes[3] != _dithering.GetHashCode()
                   )
                {
                    _hashes[0] = _horizontalTop.GetHashCode();
                    _hashes[1] = _horizontalBottom.GetHashCode();
                    _hashes[2] = _verticalLerp.GetHashCode();
                    _hashes[3] = _dithering.GetHashCode();

                    FillColors();
                }

                SetDirtyTexture();
            }
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
            if (_texture.name == name) return;

            _texture.name = name;
            AssetDatabase.SaveAssets();
  #endif
        }
    }
}
