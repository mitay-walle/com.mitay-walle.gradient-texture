# GradientTexture
Unity Gradient Texture generator. Texture2D-Gradient generated dynamicly Editor-time, by ScriptableObject with Gradient-properties

[usage example video ( Youtube )](https://youtu.be/LmBBTqhpsbw)
<br>shader in example based on [this](https://simonschreibt.de/gat/fallout-4-the-mushroom-case/), can be downloaded [here](https://github.com/mitay-walle/GradientTexture/issues/6)

![alt text](https://github.com/mitay-walle/GradientTexture/blob/main/Documentation/Inspector_preview.png?raw=true)

# Problem

## I. Shader Graph no Exposed Gradient
[You can't expose gradient to material inspector](https://issuetracker.unity3d.com/issues/gradient-property-cant-be-exposed-from-the-shadergraph)

You forced to use Texture2D-based gradients

[Forum last active thread](https://forum.unity.com/threads/gradients-exposed-property-is-ignored.837970/)

## II. designing VFX with gradients
While designing VFX using gradients you need to tweak colors and positions, according to vfx timings/size etc, what makes you:
1. _optional_ pause vfx
2. _optional_ make screenshot
3. switch Photoshop or rearrange windows to have both (Photoshop and Unity) visible on screen together
4. tweak Gradient as is in Photoshop or according to screenshot, or according to Unity-view
5. save file
6. switch to Unity window 1-2-3 times to reimport Texture or reimport by hand (if Playmode is active?)
7. check visual changes
8. repeat all

# Solution
Texture2D-Gradient generated dynamicly Editor-time, by ScriptableObject with Gradient-properties
<br>I. Exposed in shader graph as Texture2D
<br>II. faster iteration with no need to switch to Photoshop, rearrange windows, save file, reimport

# Summary
- [last release 1.0.6](https://github.com/mitay-walle/GradientTexture/releases/tag/1.0.6) is tested with Unity3d 2018-2022
- RGBA
- HDR
- UPM package
- Export to PNG
- create GradientTexture with ProjectWindow/RMB/Create/Texture/Gradient 
- Texture2D itself appear as GradientTexture-Subasset
- realtime editing
- Blend 'horizontalTop' and 'horizontalBottom' Gradients with 'verticalLerp' Curve.
- choose any resolution you want