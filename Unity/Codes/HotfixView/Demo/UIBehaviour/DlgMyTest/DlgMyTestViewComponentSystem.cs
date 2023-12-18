
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class DlgMyTestViewComponentAwakeSystem : AwakeSystem<DlgMyTestViewComponent> 
	{
		public override void Awake(DlgMyTestViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgMyTestViewComponentDestroySystem : DestroySystem<DlgMyTestViewComponent> 
	{
		public override void Destroy(DlgMyTestViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
