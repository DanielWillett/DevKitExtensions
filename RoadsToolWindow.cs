using SDG.Framework.UI;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DevKitExtensions
{
    public class RoadsToolWindow : Sleek2Window
    {
        protected Sleek2ImageLabelButton bakeButton;
        protected Sleek2Inspector settingsInspector;
        protected List<Sleek2ImageLabelButton> materials;
        protected int searchLength;
        protected List<RoadMaterial> searchInfoAssets;
        protected Sleek2Field searchField;
        protected Sleek2Element foliagePanel;
        protected Sleek2Scrollview foliageView;
        public RoadsToolWindow()
        {
            this.gameObject.name = "Roads_Tool";
            this.bakeButton = new Sleek2ImageLabelButton();
            this.bakeButton.transform.anchorMin = new Vector2(0.0f, 1f);
            this.bakeButton.transform.anchorMax = new Vector2(1f, 1f);
            this.bakeButton.transform.pivot = new Vector2(0.5f, 1f);
            this.bakeButton.transform.offsetMin = new Vector2(0.0f, -Sleek2Config.bodyHeight);
            this.bakeButton.transform.offsetMax = new Vector2(0.0f, 0.0f);
            this.bakeButton.clicked += BakeButton_clicked;
            this.bakeButton.label.textComponent.text = "Bake Roads";
            this.safePanel.addElement(this.bakeButton);
            this.settingsInspector = new Sleek2Inspector();
            this.settingsInspector.transform.anchorMin = new Vector2(0.0f, 1f);
            this.settingsInspector.transform.anchorMax = new Vector2(1f, 1f);
            this.settingsInspector.transform.pivot = new Vector2(0.0f, 1f);
            this.settingsInspector.transform.offsetMin = new Vector2(0.0f, -Sleek2Config.bodyHeight * 3 - 210);
            this.settingsInspector.transform.offsetMax = new Vector2(0.0f, -Sleek2Config.bodyHeight * 3 - 10);
            this.safePanel.addElement(this.settingsInspector);
            this.settingsInspector.inspect(DevkitRoadsToolOptions.instance);
            /*
            for (int i = 0; i < LevelRoads.materials.Length; i++)
            {
                RoadMaterial mat = LevelRoads.materials[i];
                Sleek2ImageLabelButton btn = new Sleek2ImageLabelButton();
                btn.name = "matbtn_" + i.ToString();
                btn.transform.anchorMin = new Vector2(0.0f, 1f);
                btn.transform.anchorMax = new Vector2(1f, 1f);
                btn.transform.pivot = new Vector2(0.5f, 1f);
                btn.transform.offsetMin = new Vector2(0.0f, -Sleek2Config.bodyHeight);
                btn.transform.offsetMax = new Vector2(0.0f, 0.0f);
                btn.label.textComponent.text = btn.name;
                btn.imageComponent.sprite = Sprite.Create((Texture2D)mat.material.mainTexture, new Rect(0, 0, mat.material.mainTexture.width, mat.material.mainTexture.height), new Vector2(0.5f, 0.5f));
                btn.clicked += MaterialClickedHandler;
                this.safePanel.addElement(btn);
                materials.Add(btn);
            }
            */
            DevkitHotkeys.registerTool(4, this);
        }

        private void BakeButton_clicked(Sleek2ImageButton button)
        {
            if (Level.isEditor)
            {
                UnturnedLog.info("Baking roads.");
                LevelRoads.bakeRoads();
            }
        }

        private void MaterialClickedHandler(Sleek2ImageButton button)
        {
            string matstr = button.name.Substring(7);
            if (!byte.TryParse(matstr, NumberStyles.Integer, CultureInfo.InvariantCulture, out byte mat))
                return;
            if (LevelRoads.materials.Length <= mat) return;
            RoadMaterial material = LevelRoads.materials[mat];
            DevkitRoadsToolOptions.SetMaterial(material, mat);
        }
    }
}
