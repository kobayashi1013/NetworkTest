using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Utils;

namespace Prefabs
{
    public class PlayerInfo : NetworkBehaviour
    {
        [Networked] public string username { get;  set; }

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                Rpc_PostUserInfo(UserInfo.MyInfo.username);
            }
        }

        //ユーザー情報の送信
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void Rpc_PostUserInfo(string un)
        {
            username = un; //ユーザー名
        }
    }
}
