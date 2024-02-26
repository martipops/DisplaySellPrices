using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using System;

namespace DisplaySellPrices
{
    class PriceElement : UIElement
    {
        private long price;
        private double adjustAmount = 1;
        private int[] coins;

        static readonly MyConfig config = GetInstance<MyConfig>();

        public override void OnInitialize()
        {
            SetPrice(0);
            base.OnInitialize();
        }

        internal void SetPrice(long price)
        {
            this.price = price;
            coins = Utils.CoinsSplit(price);

        }
        public void UpdatePrice(double priceAdjustment)
        {
            adjustAmount = 1/priceAdjustment;
            Item item = Main.HoverItem;
            if (item == null || item.value == 0)
            {
                SetPrice(0);
                return;
            }
            Main.LocalPlayer.GetItemExpectedPrice(item, out var calcForSelling, out _);
            double stackCalc = calcForSelling / 5.0;
            if (!Main.LocalPlayer.currentShoppingSettings.Equals(ShoppingSettings.NotInShop))
            {
                stackCalc *= Main.LocalPlayer.currentShoppingSettings.PriceAdjustment;
            }
            stackCalc /= priceAdjustment;
            long priceOfStack = (long)Math.Round(stackCalc);
            priceOfStack *= item.stack;
            SetPrice(priceOfStack);
        }

        internal void AdjustColor(ref Color c, double adjustAmount)
        {
            if (adjustAmount > 1)
            {
                c.R = (byte)(c.R / (adjustAmount));
                c.G = (byte) Math.Min(c.G * (adjustAmount), 255);
                c.B = (byte)(c.B / (adjustAmount));
            }
            else
            {
                c.R = (byte) Math.Min(c.R / (adjustAmount ), 255);
                c.G = (byte) (c.G * (adjustAmount));
                c.B = (byte) (c.B * (adjustAmount));
            }
        }

        internal void AdjustAlpha(ref Color c)
        {
            c.R = (byte)(c.R * c.A / 255.0f);
            c.G = (byte)(c.G * c.A / 255.0f);
            c.B = (byte)(c.B * c.A / 255.0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (price == 0) return;
            if (config.showBackground && config.backgroundColor.A != 0)
            {
                Color bgColor = config.backgroundColor;
                if (config.dynamicBackgroundColor)
                    AdjustColor(ref bgColor, adjustAmount);
                AdjustAlpha(ref bgColor);
                var bgRect = GetDimensions().ToRectangle();
                bgRect.Inflate(config.backgroundPadding, config.backgroundPadding);
                Utils.DrawInvBG(spriteBatch, bgRect, bgColor);
            }
            base.DrawSelf(spriteBatch);
            int xOffset = 0,
                yOffset = 0,
                maxXOffset = 0 ;
            for (int j = 3; j >=0; j--)
            {
                if (coins[j] == 0)
                {
                    continue;
                }
                var coinText = coins[j].ToString();
                Color textColor = Color.White;
                switch (j)
                {
                    case 0:
                        textColor = Colors.CoinCopper;
                        break;
                    case 1:
                        textColor = Colors.CoinSilver;
                        break;
                    case 2:
                        textColor = Colors.CoinGold;
                        break;
                    case 3:
                        textColor = Colors.CoinPlatinum;
                        break;
                }
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, coinText, new Vector2(Left.Pixels, Top.Pixels + yOffset), textColor, 0, Vector2.Zero, Vector2.One);
                var tSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, coinText, Vector2.One);
                xOffset = (int) tSize.X;
                if (config.showCoinIcon) {
                    var texture = TextureAssets.Item[71 + j];
                    xOffset += config.coinTextPadding;
                    spriteBatch.Draw(texture.Value, new Vector2( Left.Pixels + xOffset , Top.Pixels + yOffset), Color.White);
                    xOffset += texture.Width();
                }
                maxXOffset = Math.Max(xOffset, maxXOffset);
                yOffset += (int) tSize.Y - 10;
            }

            if (config.showPriceMultiplier)
            {
                Color valueColor = config.priceMultiplierColor;
                if (config.dynamicPriceMultiplierColor)
                    AdjustColor(ref valueColor, adjustAmount);
                AdjustAlpha(ref valueColor);
                string valueText = ((int)(adjustAmount * 100)) + "%";
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, valueText, new Vector2(Left.Pixels, Top.Pixels + yOffset), valueColor, 0, Vector2.Zero, Vector2.One);
                var tSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, valueText, Vector2.One);
                yOffset += (int) tSize.Y - 10;
                xOffset = (int)tSize.X;
                maxXOffset = Math.Max(xOffset, maxXOffset);
            }
            Width.Set(maxXOffset, 0f);
            Height.Set(yOffset, 0f);
        }
    }
}
