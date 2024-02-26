using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using tModPorter;

namespace DisplaySellPrices
{
    internal class PricesUIState : UIState
    {
        public static Dictionary<string, PriceElement> npcUIPairs = new();

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateTextUI();
        }

        public void UpdateTextUI()
        {
            foreach (NPC npc in Main.npc.Where(n => n.isLikeATownNPC && n.friendly && NPCShopDatabase.AllShops.Any(s => s.NpcType == n.type)))
            {
                ShoppingSettings tempSettings = Main.ShopHelper.GetShoppingSettings(Main.LocalPlayer, npc);
                if (!npcUIPairs.ContainsKey(npc.FullName))
                {
                    PriceElement npcPriceText = new();
                    npcUIPairs.Add(npc.FullName, npcPriceText);
                    Append(npcUIPairs[npc.FullName]);
                }
                npcUIPairs[npc.FullName].UpdatePrice(tempSettings.PriceAdjustment);
                npcUIPairs[npc.FullName].Left.Set(npc.position.ToScreenPosition().X + ModContent.GetInstance<MyConfig>().xOffset, 0f);
                npcUIPairs[npc.FullName].Top.Set(npc.position.ToScreenPosition().Y + ModContent.GetInstance<MyConfig>().yOffset, 0f);
            }
        }
        

        
    }
}
