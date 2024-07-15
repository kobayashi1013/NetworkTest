using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Utils;
using System;

namespace Prefabs
{
    public class PlayerInfo : NetworkBehaviour
    {
        [Networked] public int connectionToken { get; set; }
        [Networked] public int hostId { get; set; }
        [Networked] public int userId { get; set; }
        [Networked] public string userName { get;  set; }

        public override void Spawned()
        {
            if (!Object.IsResume)
            {
                if (Object.HasInputAuthority)
                {
                    Rpc_PostUserInfo(
                        connectionToken,
                        hostId,
                        UserInfo.MyInfo.userId,
                        UserInfo.MyInfo.userName);
                }
            }
        }

        //ホストへののデータ送信
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void Rpc_PostUserInfo(int token, int hid, int uid, string name)
        {
            connectionToken = token;
            hostId = hid;
            userId = uid;
            userName = name;
        }
    }
}
