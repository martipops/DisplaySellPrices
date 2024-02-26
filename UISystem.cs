
using DisplaySellPrices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

public class UISystem : ModSystem
{

    public UserInterface pricesInterface;
    internal PricesUIState pricesState;

    public void ShowMyUI()
    {
        pricesState.UpdateTextUI();
        pricesInterface?.SetState(pricesState);
    }

    public void HideMyUI()
    {
        pricesInterface?.SetState(null);
    }
    public override void Load()
    {
        if (!Main.dedServ)
        {
            pricesInterface = new UserInterface();
            pricesState = new PricesUIState();
            pricesState.Activate();
        }
    }

    public override void PostUpdateInput()
    {
        if (Main.keyState.IsKeyDown(Main.FavoriteKey) && Main.HoverItem.type != ItemID.None)
        {
            ShowMyUI();
        } else
        {
            HideMyUI();
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (pricesInterface?.CurrentState != null)
        {
            pricesInterface?.Update(gameTime);
        }
    }
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Town NPC House Banners"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "DisplaySellPrices: Prices Layer",
                delegate
                {
                    if (pricesInterface?.CurrentState != null)
                    {
                        pricesInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}