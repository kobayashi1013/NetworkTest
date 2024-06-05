using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Scenes;

namespace Prefabs
{
    public class PlayerInfo : NetworkBehaviour
    {
        [Networked] public string username { get; private set; }

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                username = UserInfo.Instance.username;
            }
        }
    }
}
