using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DevKitExtensions
{
    public class DevkitToolbarBakeGlobalResources : Sleek2ImageButton
    {
        public Sleek2TranslatedLabel label { get; protected set; }
        protected override void handleButtonClick()
        {
            if (Level.isEditor)
            {
                UnturnedLog.info("Baking resources for level.");
                LevelGround.bakeGlobalResources();
            }
            base.handleButtonClick();
        }
        public DevkitToolbarBakeGlobalResources()
        {
            this.transform.sizeDelta = new Vector2(0.0f, Sleek2Config.bodyHeight);
            this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
            this.label = new Sleek2TranslatedLabel();
            this.label.transform.reset();
            this.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Toolbar.File.Bake_Resources_Global"));
            this.label.translation.format();
            this.addElement(this.label);
        }
    }
    public class DevkitToolbarBakeLocalResources : Sleek2ImageButton
    {
        public Sleek2TranslatedLabel label { get; protected set; }
        protected override void handleButtonClick()
        {
            if (Level.isEditor)
            {
                UnturnedLog.info("Baking resources for level.");
                LevelGround.bakeLocalResources();
            }
            base.handleButtonClick();
        }
        public DevkitToolbarBakeLocalResources()
        {
            this.transform.sizeDelta = new Vector2(0.0f, Sleek2Config.bodyHeight);
            this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
            this.label = new Sleek2TranslatedLabel();
            this.label.transform.reset();
            this.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Toolbar.File.Bake_Resources_Local"));
            this.label.translation.format();
            this.addElement(this.label);
        }
    }
    public class DevkitToolbarBakeSkyboxResources : Sleek2ImageButton
    {
        public Sleek2TranslatedLabel label { get; protected set; }
        protected override void handleButtonClick()
        {
            if (Level.isEditor)
            {
                UnturnedLog.info("Baking resources for level.");
                LevelGround.bakeSkyboxResources();
            }
            base.handleButtonClick();
        }
        public DevkitToolbarBakeSkyboxResources()
        {
            this.transform.sizeDelta = new Vector2(0.0f, Sleek2Config.bodyHeight);
            this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
            this.label = new Sleek2TranslatedLabel();
            this.label.transform.reset();
            this.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Toolbar.File.Bake_Resources_Skybox"));
            this.label.translation.format();
            this.addElement(this.label);
        }
    }
}
