#if UNITY_2021_2_OR_NEWER

using Packages.GradientTextureGenerator.Runtime;
using UnityEditor;
using UnityEngine;

namespace Packages.GradientTextureGenerator.Editor
{

	public static class DragAndDropUtility
	{
		static DragAndDrop.ProjectBrowserDropHandler _handlerProject;

		[InitializeOnLoadMethod]
		public static void Init()
		{
			_handlerProject = ProjectDropHandler;
			DragAndDrop.RemoveDropHandler(_handlerProject);
			DragAndDrop.AddDropHandler(_handlerProject);
		}

		private static DragAndDropVisualMode ProjectDropHandler(int dragInstanceId, string dropUponPath, bool perform)
		{
			if (!perform)
			{
				var dragged = DragAndDrop.objectReferences;
				bool found = false;
				for (var i = 0; i < dragged.Length; i++)
				{
					if (dragged[i] is GradientTexture gradient)
					{
						dragged[i] = gradient.GetTexture();
						found = true;
					}
				}
				if (found)
				{
					DragAndDrop.objectReferences = dragged;
					GUI.changed = true;
					return default;
				}

			}
			return default;
		}
	}
}

#endif