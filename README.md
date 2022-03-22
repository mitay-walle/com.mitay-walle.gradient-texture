# Extendable Tile
control Tile's Color, Sprite, Transform, with optional modules - TileExtensions

![alt text](https://github.com/mitay-walle/Extendable-Tile/blob/master/ExtendableTile/Documentation/readme_preview.png?raw=true)

# Like it? Buy me a candy
If you like my work, you can support me on [Patreon](https://www.patreon.com/mitaywalle)

## Navigation
- [Problems](https://github.com/mitay-walle/Extendable-Tile#problems)
- [Solution](https://github.com/mitay-walle/Extendable-Tile#solution)
- [Extendable Tile](https://github.com/mitay-walle/Extendable-Tile#extendable-tile)
- [Demo](https://github.com/mitay-walle/Extendable-Tile#demo)
- [Contents](https://github.com/mitay-walle/Extendable-Tile#contents)
- [Script types](https://github.com/mitay-walle/Extendable-Tile#script-types)
- [TileExtension list](https://github.com/mitay-walle/Extendable-Tile#tileextension-list)
- [How To](https://github.com/mitay-walle/Extendable-Tile#how-to)
- [Requriments](https://github.com/mitay-walle/Extendable-Tile#requriments)
- [Known issues](https://github.com/mitay-walle/Extendable-Tile#known-issues)
- [Planned](https://github.com/mitay-walle/Extendable-Tile#known-issues)

# Problems
[Existing tilemap tiles](https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@1.5/manual/Tiles.html) are rigid realizations with:
- lack of basic features ( color, transform, sprite manipulations ) which are allowed by TileData / AnimatedTileData
- feature set of any Tile is fixed, no customization / optional modules, that have to add repeatatively byself
- lack of Inspector Undo
- lack of Copy / Paste
- Rigid custom Inspectors, disallowing use [Odin](https://odininspector.com/), or built-in Range / Header / Space Attributes for Inspector customization

## Solution
- [SerializeReference](https://docs.unity3d.com/2019.3/Documentation/ScriptReference/SerializeReference.html) to have optional modules
- no use custom Inspector / PropertyDrawers , to allow Odin, save built-in Undo, Copy / Paste, etc

## Demo
![alt text](https://github.com/mitay-walle/Extendable-Tile/blob/master/ExtendableTile/Documentation/demo_preview.png?raw=true)

## Contents
1. Demo Scene, tiles, extensions
2. [unitypackage](https://github.com/mitay-walle/Extendable-Tile/blob/master/extendableTilePacked.unitypackage) to direct import
3. full source code on Github to explore
4. if your project contains [Odin Inspector](https://odininspector.com/) - ExpandableTile will use it, if not
5. TypeToLabelAttribute and custom ContextMenu to comfortable usage

###### Script types:
- ExtendableTile - CustomTile, that aggregates TileExtensions
- TileExtensionSO - ScriptableObject, containing TileExtension, can be referenced from ScriptableObjectEx


###### TileExtension list:
1. **AnimateSpriteEx** - analogue to [Animated Tile](https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@1.6/manual/AnimatedTile.html)
2. **WeightRandomSpriteEx** - analogue to [Weight Random Tile](https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@1.5/manual/WeightedRandomTile.html)
3. **PipelineTileEx** - analogue to [Pipeline Tile](https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@1.5/manual/PipelineTile.html)
4. **ColorOutlineEx** - evaluate Gradeint based on same-tile-neibhours-count, smooth sides
5. **PositionMapEx** - Remap tile position with split random MinMaxCurves for X and Y
6. **RandomColorEx** - Multiply tile color to MinMaxGradient.Evalute(), has Perlin module
7. **RotateMapEx** - Remap tile rotation with random MinMaxCurve for Z
8. **ScaleMapEx** - Remap tile localScale with random MinMaxCurve for XYZ
9. **ScriptableObjectEx** - refers to TileExtensionSO and execute it's TileExtension

## How To
###### Create Tile or TileExtensionSO?
- Project Window -> RMB -> Create/2D/Tiles/Extendable Tile / Extension
![alt text](https://github.com/mitay-walle/Extendable-Tile/blob/master/ExtendableTile/Documentation/Instruction_createTile_ProjectContextMenu.png?raw=true)
###### Change type of already created extension in Collection or in TileExtensionSO ?
- TileExtension property title -> RMB -> set to \[Extension]
![alt text](https://github.com/mitay-walle/Extendable-Tile/blob/master/ExtendableTile/Documentation/Instruction_setType_ContextMenu.png?raw=true)

## Requriments
- Unity 2020.2 Array became reordable at this version
 
## Known issues
- [ ] #3 some TileExtensions need Enter / Exit PlayMode to Refresh
- [ ] #4 at this moment position rerandomized after any changes in any listed ExtendableTile.Extensions
- [ ] #5 :no_entry: Error log 'Use RegisterCompleteObjectUndo' - not braking anything<br/>
- [ ] #6 :no_entry: MinMaxGradient dropdown almost not clickable - problem in Unity own realization, if you use Odin - it works fine, either you [write own CustomPropertyDrawer](https://docs.unity3d.com/ru/2019.3/Manual/editor-PropertyDrawers.html), or you can [switch Inspector to Debug mode](https://docs.unity3d.com/Manual/InspectorOptions.html), and switch gradient-type there 

## Planned
- [ ] #3 RefreshTile() Implementation
- [ ] #4 Seed-based Randomization
- [ ] #2 SiblingRuleEx (Existing [Rule Tile](https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@1.6/manual/RuleTile.html) has rigid Inspector, i've ported it to PropertyDrawer for RuleEx, but it's not ready-to-use)
- [x] #1 TerrainEx analogue to [Terrain Tile](https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@1.5/manual/TerrainTile.html)
