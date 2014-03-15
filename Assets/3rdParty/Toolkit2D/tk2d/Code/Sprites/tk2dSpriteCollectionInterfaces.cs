using UnityEngine;

namespace tk2dRuntime
{
	public interface ISpriteCollectionForceBuild
	{
		bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection);
		void ForceBuild();
	}

	public interface IRenderable
	{
		Color color { get; set;}
		float Alpha { get; set;}
		int SortingOrder {get; set;}
		Vector3 scale {get; set;}
	}
}
