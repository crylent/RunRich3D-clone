using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public struct StatusRequirement
    {
        public PlayerStatus status;
        public int money;
        public GameObject appearance;
        public Sprite moneyBar;
        public string statusName;
        public Color statusColor;
    }
}