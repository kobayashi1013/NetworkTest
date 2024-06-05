using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes
{
    public class UserInfo
    {
        public static UserInfo Instance;

        public string username; //ÉÜÅ[ÉUÅ[ñº

        public UserInfo(string un)
        {
            username = un;
        }
    }
}
