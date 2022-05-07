using SDG.Framework.Devkit;
using SDG.Framework.Modules;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Uncreated.U6;
using Uncreated.U6.Objects;
using SDG.Framework.Devkit.Transactions;
using System.Collections;

namespace DevKitExtensions
{
    public class DevkitToolbarReplacePlaceholdersButton : Sleek2ImageButton
    {
        public Sleek2TranslatedLabel label { get; protected set; }
        private static Coroutine c = null;
        protected override void handleButtonClick()
        {
            if (Level.isEditor)
            {
                if (c != null) return;
                UnturnedLog.info("Replacing placeholder objects for actual objects.");
                try
                {
                    c = ModuleBase.OBJ.StartCoroutine(ReplacePlaceholderObjects());
                }
                catch (Exception ex)
                {
                    UnturnedLog.exception(ex);
                }
            }
            base.handleButtonClick();
        }
        public DevkitToolbarReplacePlaceholdersButton()
        {
            this.transform.sizeDelta = new Vector2(0.0f, Sleek2Config.bodyHeight);
            this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
            this.label = new Sleek2TranslatedLabel();
            this.label.transform.reset();
            this.label.translation = new TranslatedText(new TranslationReference("Uncreated", "Buttons.ConvertPlaceholders"));
            this.label.translation.format();
            this.addElement(this.label);
        }
        public static IEnumerator ReplacePlaceholderObjects()
        {
            DevkitTransactionManager.beginTransaction(new TranslatedText(new TranslationReference("Uncreated", "Transactions.ConvertAllPlaceholders")));
            for (int i = LevelHierarchy.instance.items.Count - 1; i >= 0; i--)
            {
                IDevkitHierarchyItem item = LevelHierarchy.instance.items[i];
                if (item is ObjectGroupPlaceholder placeholder)
                {
                    if (placeholder.ObjectGroupID > 0)
                    {
                        foreach (DestructibleObjectModel mdl in DestructionManager.ObjectData)
                        {
                            if (mdl.Id == placeholder.ObjectGroupID)
                            {
                                Vector3 position = placeholder.gameObject.transform.position;
                                Quaternion rotation = placeholder.gameObject.transform.rotation;

                                for (int j = 0; j < mdl.Objects.Length; j++)
                                {
                                    if (Assets.find(mdl.Objects[j].Id) is ObjectAsset oasset)
                                    {
                                        DevkitTransactionUtility.recordInstantiation(
                                            LevelObjects.addDevkitObject(oasset.GUID, position, rotation, Vector3.one, ELevelObjectPlacementOrigin.MANUAL)
                                            .gameObject);
                                        yield return null;
                                    }
                                }
                                yield return null;
                                LevelHierarchy.removeItem(placeholder);
                            }
                        }
                    }
                    else
                    {
                        yield return null;
                        LevelHierarchy.removeItem(placeholder);
                    }
                }
            }
            DevkitTransactionManager.endTransaction();
            c = null;
            yield break;
        }
    }
}
