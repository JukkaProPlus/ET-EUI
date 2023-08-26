using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(DlgServer))]
    [FriendClass(typeof(ServerInfosComponent))]
    [FriendClassAttribute(typeof(ET.ServerInfo))]
    public static class DlgServerSystem
    {

        public static void RegisterUIEvent(this DlgServer self)
        {
            self.View.E_ConfirmButton.AddListenerAsync1(() => { return self.OnConfirmClickHandler(); });
            self.View.E_ServerListLoopVerticalScrollRect.AddItemRefreshListener((Transform tran, int index) => { self.OnScrollItemRefreshHandler(tran, index); });
        }
        public static void OnScrollItemRefreshHandler(this DlgServer self, Transform transform, int index)
        {
            Scroll_Item_serverTest serverTest = self.ScrollItemServerTests[index].BindTrans(transform);
            ServerInfo info = self.ZoneScene().GetComponent<ServerInfosComponent>().serverInfoList[index];
            Log.Debug($"index = {index},info.Id = {info.Id}, self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId = {self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId}");
            serverTest.EI_serverTestImage.color = info.Id == self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId ? Color.red : Color.yellow;
            serverTest.E_serverTestTipText.SetText(info.ServerName);
            serverTest.E_SelectButton.AddListener(() => { self.OnSelectServerItemHandler((int)info.Id); });
        }
        public static void OnSelectServerItemHandler(this DlgServer self, int serverId)
        {
            self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId = serverId;
            Log.Debug($"当前选择了服务器{serverId}");
            self.View.E_ServerListLoopVerticalScrollRect.RefillCells();
        }

        public static void ShowWindow(this DlgServer self, Entity contextData = null)
        {
            int count = self.ZoneScene().GetComponent<ServerInfosComponent>().serverInfoList.Count;
            Log.Debug($"count = {count}");
            self.AddUIScrollItems(ref self.ScrollItemServerTests, count);
            self.View.E_ServerListLoopVerticalScrollRect.SetVisible(true, count);
        }
        public static void HideWindow(this DlgServer self)
        {
            self.RemoveUIScrollItems(ref self.ScrollItemServerTests);
        }
        public static async ETTask OnConfirmClickHandler(this DlgServer self)
        {
            if (self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId == 0)
            {
                Log.Error("请先选择区服");
                return;
            }
            try
            {
                int errCode = await LoginHelper.GetRoles(self.ZoneScene());
                if (errCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errCode.ToString());
                    return;
                }
                self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Server);
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Roles);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
            await ETTask.CompletedTask;
        }




    }
}
