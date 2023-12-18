using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[FriendClass(typeof(DlgMyTest))]
	public static  class DlgMyTestSystem
	{

		public static void RegisterUIEvent(this DlgMyTest self)
		{
			self.View.EButton_AButton.onClick.AddListener(()=>{Log.Info("Button A clicked!");});
			self.View.EButton_BButton.onClick.AddListener(()=>{Log.Info("Button B clicked!");});
			self.View.EButton_CButton.onClick.AddListener(()=>{Log.Info("Button C clicked!");});
		}

		public static void ShowWindow(this DlgMyTest self, Entity contextData = null)
		{

		}

		 

	}
}
