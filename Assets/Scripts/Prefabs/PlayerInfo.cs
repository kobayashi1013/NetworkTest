using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Network;
using Utils;

namespace Prefabs
{
    [RequireComponent(typeof(NetworkObjectTrackingData))]
    public class PlayerInfo : NetworkBehaviour
    {
        [Networked] public int connectionToken { get; set; }
        [Networked] public int userId { get; set; }
        [Networked] public string userName { get;  set; }

        public override void Spawned()
        {
            if (Object.HasInputAuthority && !Object.IsResume)
            {
                Rpc_PostUserInfo(
                    UserInfo.MyInfo.userId,
                    UserInfo.MyInfo.userName);
            }
        }

        //ホストへののデータ送信
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void Rpc_PostUserInfo(int uid, string name)
        {
            userId = uid;
            userName = name;
        }
    }
}
