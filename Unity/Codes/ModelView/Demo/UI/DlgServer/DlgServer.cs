namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgServer :Entity,IAwake,IUILogic
	{

		public DlgServerViewComponent View { get => this.Parent.GetComponent<DlgServerViewComponent>();} 

		 

	}
}
