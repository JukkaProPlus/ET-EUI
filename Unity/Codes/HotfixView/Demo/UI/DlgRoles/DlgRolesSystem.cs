using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(DlgRoles))]
    [FriendClass(typeof(RoleInfosComponent))]
    [FriendClass(typeof(RoleInfo))]
    [FriendClassAttribute(typeof(ET.AccountInfoComponent))]
    [FriendClassAttribute(typeof(ET.ServerInfosComponent))]
    public static class DlgRolesSystem
    {

        public static void RegisterUIEvent(this DlgRoles self)
        {
            self.View.E_ConfirmButton.AddListenerAsync1(self.OnConfirmClickHandler);
            self.View.E_DeleteRoleButton.AddListenerAsync1(self.OnDeleteClickHandler);
            self.View.E_CreateRoleButton.AddListenerAsync1(self.OnCreateRoleClickHandler);
            self.View.E_RolesLoopVerticalScrollRect.AddItemRefreshListener((Transform transform, int index) => { self.OnRoleListRefreshHandler(transform, index); });
        }
        public static void OnRoleListRefreshHandler(this DlgRoles self, Transform transform, int index)
        {
            Scroll_Item_role scroll_Item_Role = self.ScrollItemRoles[index].BindTrans(transform);
            RoleInfo info = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos[index];
            scroll_Item_Role.E_RoleNameText.SetText(info.Name);
            scroll_Item_Role.E_RoleImage.color = info.Id == self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId ? Color.green : Color.gray;
            //info.State == (int)RoleInfoState.Normal ? Color.green : Color.red;
            scroll_Item_Role.E_RoleButton.AddListener(() => { self.OnRoleItemClickHandler(info.Id); });
        }
        public static void OnRoleItemClickHandler(this DlgRoles self, long roleId)
        {
            self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId = roleId;
            self.View.E_RolesLoopVerticalScrollRect.RefillCells();
        }

        public async static ETTask OnCreateRoleClickHandler(this DlgRoles self)
        {
            Log.Debug("创建角色按钮 ");
            string name = self.View.E_RoleNameInputField.text;
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.Error($"角色名字不能为空:{name}");
                return;
            }
            try
            {
                int errorCode = await LoginHelper.CreateRole(self.ZoneScene(), name);
                if (errorCode == ErrorCode.ERR_Success)
                {
                    self.RefreshRoleItems();
                }
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public async static ETTask OnDeleteClickHandler(this DlgRoles self)
        {
            if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
            {
                Log.Error("请选择要删除的角色");
                return;
            }
            try
            {
                int errorCode = await LoginHelper.DeleteRole(self.ZoneScene(), self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId);
                if (errorCode == ErrorCode.ERR_Success)
                {
                    self.RefreshRoleItems();
                    return;
                }
                else
                {
                    Log.Error(errorCode.ToString());
                }
            }catch(Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public async static ETTask OnConfirmClickHandler(this DlgRoles self)
        {
            Log.Debug("开始按钮");

            if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
            {
                Log.Error($"没有选择角色");
                return;
            }

            try
            {
                int code = await LoginHelper.GetRealmKey(self.ZoneScene());
                if (code != ErrorCode.ERR_Success)
                {
                    Log.Error(code.ToString());
                    return;
                }
                
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
            await ETTask.CompletedTask;
            //self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Roles);
            //self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Lobby);
        }


        public static void ShowWindow(this DlgRoles self, Entity contextData = null)
        {
            self.RefreshRoleItems();
        }
        public static void RefreshRoleItems(this DlgRoles self)
        {
            int count = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos.Count;
            self.AddUIScrollItems(ref self.ScrollItemRoles, count);
            self.View.E_RolesLoopVerticalScrollRect.SetVisible(true, count);


            //int count = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos.Count;
            //self.AddUIScrollItems(ref self.ScrollItemRoles, count);
            //self.View.E_RolesLoopVerticalScrollRect.SetVisible(true, count);
        }
        public static void HideWindow(this DlgRoles self)
        {
            self.RemoveUIScrollItems(ref self.ScrollItemRoles);
        }




    }
}
