using Microsoft.Xna.Framework;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace DisplaySellPrices
{
    internal class MyConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;


        [Header("Visual")]

        [DefaultValue(true)]
        public bool showCoinIcon;

        [DefaultValue(false)]
        public bool showPriceMultiplier;

        [DefaultValue(true)]
        public bool showBackground;


        [Header("Positioning")]

        [Range(-50,50), DefaultValue(20)]
        public int xOffset;

        [Range(-50, 50), DefaultValue(0)]
        public int yOffset;
        
        [Range(0, 20), DefaultValue(5)]
        public int backgroundPadding;

        [Range(-50,50), DefaultValue(5)]
        public int coinTextPadding;


        [Header("Colors")]

        [DefaultValue(true)]
        public bool dynamicBackgroundColor;

        [DefaultValue(true)]
        public bool dynamicPriceMultiplierColor;

        [DefaultValue(typeof(Color), "63,65,151,165")]
        public Color backgroundColor;

        [DefaultValue(typeof(Color), "200,200,200,255")]
        public Color priceMultiplierColor;

    }
}