using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DevKitExtensions
{
    public class ObjectGroupPlaceholder : DevkitHierarchyWorldItem, IDevkitHierarchySpawnable
    {
        [Inspectable("#Uncreated::ObjectGroupID")]
        public ushort ObjectGroupID;
        public void devkitHierarchySpawn()
        {

        }
        protected override void readHierarchyItem(IFormattedFileReader reader)
        {
            base.readHierarchyItem(reader);
            this.ObjectGroupID = reader.readValue<ushort>("Object_Group_ID");
        }
        protected override void writeHierarchyItem(IFormattedFileWriter writer)
        {
            base.writeHierarchyItem(writer);
            writer.writeValue("Object_Group_ID", ObjectGroupID);
        }
        protected void OnEnable()
        {
            LevelHierarchy.addItem(this);
        }

        protected void OnDisable()
        {
            LevelHierarchy.removeItem(this);
        }
        protected void Awake()
        {
            this.name = "Object_Group_Placeholder";
            this.gameObject.layer = 8;
        }
    }
}
