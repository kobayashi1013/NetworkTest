using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class UserInfo
    {
        public static UserInfo MyInfo;

        public int userId; //���[�U�[ID
        public string userName; //���[�U�[��
        public byte[] connectionToken; //�ڑ��g�[�N��

        public UserInfo(int id, string name)
        {
            userId = id;
            userName = name;
            connectionToken = Guid.NewGuid().ToByteArray();
        }
    }
}
