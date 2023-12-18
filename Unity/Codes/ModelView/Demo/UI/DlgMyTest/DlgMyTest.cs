namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgMyTest :Entity,IAwake,IUILogic
	{
		public DlgMyTestViewComponent View { get => this.Parent.GetComponent<DlgMyTestViewComponent>();} 
	}
}
