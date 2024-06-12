using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Network;

namespace Scenes.InGameMulti2.Manager
{
    public class InGameMulti2Manager : NetworkBehaviour
    {
        public override void Spawned()
        {
            Debug.Log("Start Multi2");
        }
    }
}
