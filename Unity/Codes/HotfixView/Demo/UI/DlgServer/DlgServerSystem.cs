using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[FriendClass(typeof(DlgServer))]
	[FriendClass(typeof(ServerInfosComponent))]
	public static  class DlgServerSystem
	{

		public static void RegisterUIEvent(this DlgServer self)
		{
			self.View.E_ConfirmButton.AddListenerAsync1(() => { return self.OnConfirmClickHandler();});
		}

		public static void ShowWindow(this DlgServer self, Entity contextData = null)
		{
		}
		public static async ETTask OnConfirmClickHandler(this DlgServer self)
		{
			if(self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId == 0)
			{
				Log.Error("请先选择区服");
				return;
			}
			try
			{
				//int errCode = LoginHelper.
				//Log.Error("AA");
			}
			catch(Exception e)
			{
				Log.Error(e.ToString());
			}
			await ETTask.CompletedTask;
		}




    }
}
