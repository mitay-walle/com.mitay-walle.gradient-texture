# GradientTexture
Unity Gradient Texture generator.

[usage example video ( Youtube )](https://youtu.be/LmBBTqhpsbw)

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
3. switch Photoshop or rearrange windows to have both (Photshop and Unity) on screen
4. change color as is in photoshop or according to screenshot, or according to unity-view
5. save file
6. active Unity window 1-2-3 times for reimport or reimport by hand (if playmode is active?)
7. check visual changes
8. repeat all

# Solution
Texture2D generated dynamicly Editor-time, by ScriptableObject with Gradient-properties




# Like it? Buy me a candy
If you like my work, you can support me on [Patreon](https://www.patreon.com/mitaywalle)

# Summary
- package
- Create new gradient with ProjectWindow/RMB/Create/Texture/Gradient 
- Texture itself appear as GradientTexture-subasset
- realtime editing
- use HDR Gradients
- Blend 'horizontalTop' and 'horizontalBottom' Gradients with 'verticalLerp' Curve.
- 'horizontalTop' and '_horizontalBottom' use HDR and has alpha
- 'verticalLerp' is blend-t-value
- choose any resolution you want


shader in example based on [this](https://simonschreibt.de/gat/fallout-4-the-mushroom-case/)

