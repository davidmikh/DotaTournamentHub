﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonPlayer
    {
        public int Slot { get; set; }
        public int AccountID { get; set; }
        public int HeroID { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int LastHits { get; set; }
        public int Denies { get; set; }
        public int Gold { get; set; }
        public int Level { get; set; }
        public int GPM { get; set; }
        public int XPM { get; set; }
        public int UltimateState { get; set; }
        public int UltimateCD { get; set; }
        public int[] Items { get; set; }
        public int RespawnTimer { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public int NetWorth { get; set; }

        public JsonPlayer(JToken json)
        {
            Slot = (int)json["player_slot"];
            AccountID = (int)json["account_id"];
            HeroID = (int)json["hero_id"];
            Kills = (int)json["kills"];
            //In some parts of the API "death" is used. Other times its "deaths"
            if (json["death"] == null)
            {
                Deaths = (int)json["deaths"];
            }
            else
            {
                Deaths = (int)json["death"];
            }
            Assists = (int)json["assists"];
            LastHits = (int)json["last_hits"];
            Denies = (int)json["denies"];
            Gold = (int)json["gold"];
            Level = (int)json["level"];
            GPM = (int)json["gold_per_min"];
            XPM = (int)json["xp_per_min"];
            if (json["ultimate_state"] != null)
            {
                UltimateState = (int)json["ultimate_state"];
                UltimateCD = (int)json["ultimate_cooldown"];
            }
            Items = new int[6];
            string itemStr = "item";
            if (json["item1"] == null)
            {
                itemStr = "item_";
            }
            for (int i = 0; i < 6; i++)
            {
                Items[i] = (int)json[itemStr + i];
            }
            if (json["respawn_timer"] != null)
            {
                RespawnTimer = (int)json["respawn_timer"];
            }
            if (json["position_x"] != null)
            {
                PositionX = (double)json["position_x"];
                PositionY = (double)json["position_y"];
            }
            if (json["net_worth"] != null)
            {
                NetWorth = (int)json["net_worth"];
            }
            else
            {
                //If a match is completed net worth is no longer displayed. Current gold + spent gold however still shows net worth
                Gold += (int)json["gold_spent"];
            }
        }
    }
}
