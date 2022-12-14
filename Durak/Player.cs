﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak
{
    public class Player
    {
        public readonly string NickName;
        public List<Card> Hand = new();

        public Player(string nickName, int movingPriority)
        {
            NickName = nickName;
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append("Игрок" + NickName + ":\n");
            foreach (var card in Hand)
                str.Append(card.ToString() + '\n');
            return str.ToString();
        }
    }
}
