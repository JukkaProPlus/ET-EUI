
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgMyTestViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button EButton_AButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_AButton == null )
     			{
		    		this.m_EButton_AButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EButton_A");
     			}
     			return this.m_EButton_AButton;
     		}
     	}

		public UnityEngine.UI.Image EButton_AImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_AImage == null )
     			{
		    		this.m_EButton_AImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EButton_A");
     			}
     			return this.m_EButton_AImage;
     		}
     	}

		public UnityEngine.UI.Text ELabel_AText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELabel_AText == null )
     			{
		    		this.m_ELabel_AText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EButton_A/ELabel_A");
     			}
     			return this.m_ELabel_AText;
     		}
     	}

		public UnityEngine.UI.Button EButton_BButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_BButton == null )
     			{
		    		this.m_EButton_BButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EButton_B");
     			}
     			return this.m_EButton_BButton;
     		}
     	}

		public UnityEngine.UI.Image EButton_BImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_BImage == null )
     			{
		    		this.m_EButton_BImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EButton_B");
     			}
     			return this.m_EButton_BImage;
     		}
     	}

		public UnityEngine.UI.Text ELabel_BText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELabel_BText == null )
     			{
		    		this.m_ELabel_BText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EButton_B/ELabel_B");
     			}
     			return this.m_ELabel_BText;
     		}
     	}

		public UnityEngine.UI.Button EButton_CButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_CButton == null )
     			{
		    		this.m_EButton_CButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EButton_C");
     			}
     			return this.m_EButton_CButton;
     		}
     	}

		public UnityEngine.UI.Image EButton_CImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_CImage == null )
     			{
		    		this.m_EButton_CImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EButton_C");
     			}
     			return this.m_EButton_CImage;
     		}
     	}

		public UnityEngine.UI.Text ELabel_CText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELabel_CText == null )
     			{
		    		this.m_ELabel_CText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EButton_C/ELabel_C");
     			}
     			return this.m_ELabel_CText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EButton_AButton = null;
			this.m_EButton_AImage = null;
			this.m_ELabel_AText = null;
			this.m_EButton_BButton = null;
			this.m_EButton_BImage = null;
			this.m_ELabel_BText = null;
			this.m_EButton_CButton = null;
			this.m_EButton_CImage = null;
			this.m_ELabel_CText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_EButton_AButton = null;
		private UnityEngine.UI.Image m_EButton_AImage = null;
		private UnityEngine.UI.Text m_ELabel_AText = null;
		private UnityEngine.UI.Button m_EButton_BButton = null;
		private UnityEngine.UI.Image m_EButton_BImage = null;
		private UnityEngine.UI.Text m_ELabel_BText = null;
		private UnityEngine.UI.Button m_EButton_CButton = null;
		private UnityEngine.UI.Image m_EButton_CImage = null;
		private UnityEngine.UI.Text m_ELabel_CText = null;
		public Transform uiTransform = null;
	}
}
