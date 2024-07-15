using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Utils;

namespace Prefabs
{
    public class PlayerInfo : NetworkBehaviour
    {
        [Networked] public int token { get; set; }
        [Networked] public int hostId { get; set; }
        [Networked] public int userId { get; set; }
        [Networked] public string userName { get;  set; }

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                Rpc_PostUserInfo(UserInfo.MyInfo.userId,
                    UserInfo.MyInfo.userName);
            }
        }

        //ホストへののデータ送信
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void Rpc_PostUserInfo(int id, string name)
        {
            userId = id;
            userName = name;
        }
    }
}
