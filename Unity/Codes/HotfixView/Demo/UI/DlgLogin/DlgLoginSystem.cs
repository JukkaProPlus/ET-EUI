using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.ComponentModel;

namespace ET
{
	public static  class DlgLoginSystem
	{

		public static void RegisterUIEvent(this DlgLogin self)
		{
			self.View.E_LoginButton.AddListenerAsync1(() => { return self.OnLoginClickHandler();});
		}

		public static void ShowWindow(this DlgLogin self, Entity contextData = null)
		{
			
		}
		
		public static async ETTask OnLoginClickHandler(this DlgLogin self)
		{
			try
			{
				int errcode = await LoginHelper.Login(
					self.DomainScene(), 
					ConstValue.LoginAddress, 
					self.View.E_AccountInputField.GetComponent<InputField>().text, 
					self.View.E_PasswordInputField.GetComponent<InputField>().text);
				if (errcode != ErrorCode.ERR_Success)
				{
					Log.Error(errcode.ToString());
					return;
				}
				//TODO 显示登录之后的逻辑
			}
			catch(Exception e)
			{
				Log.Error(e.ToString());
			}
			
		}
		
		public static void HideWindow(this DlgLogin self)
		{

		}
		
	}
}
