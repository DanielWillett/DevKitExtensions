using SDG.Framework.Modules;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DevKitExtensions
{
    public class ModuleBase : IModuleNexus
    {
        public static CoroutineBase OBJ;
        public class CoroutineBase : MonoBehaviour
        {

        }

        public void initialize()
        {
            if (OBJ != null)
                UnityEngine.Object.Destroy(OBJ.gameObject);
            GameObject obj = new GameObject("DevKitExtentions");
            UnityEngine.Object.DontDestroyOnLoad(obj);
            OBJ = obj.AddComponent<CoroutineBase>();
            DevkitWindowManager.toolbarCreated += DevkitWindowManager_toolbarCreated;
        }
        private void DevkitWindowManager_toolbarCreated()
        {
            string translationFilePath = Environment.GetEnvironmentVariable("APPDATA") + @"\Uncreated\Translations\";
            string fn = "english.translation";
            FileInfo fi = new FileInfo(translationFilePath + fn);
            DirectoryInfo d = fi.Directory;
            if (!d.Exists) d.Create();
            File.WriteAllText(fi.FullName, EnglishTranslations.FullText);
            Translator.registerTranslationFile(fi.FullName);
            Translator.loadTranslation("english", "Uncreated");
            DevkitToolbarManager.registerToolbarElement("File/Mapping",                 new DevkitToolbarMapifyButton());
            DevkitToolbarManager.registerToolbarElement("File/Mapping",                 new DevkitToolbarChartifyButton());
            DevkitToolbarManager.registerToolbarElement("File/Baking",                  new DevkitToolbarBakeDetails());
            DevkitToolbarManager.registerToolbarElement("File/Baking/Splatmaps",        new DevkitToolbarBakeSplatmaps());
            DevkitToolbarManager.registerToolbarElement("File/Baking/Splatmaps",        new DevkitToolbarBakeSplatmapsLocal());
            DevkitToolbarManager.registerToolbarElement("File/Baking/Resources",        new DevkitToolbarBakeGlobalResources());
            DevkitToolbarManager.registerToolbarElement("File/Baking/Resources",        new DevkitToolbarBakeLocalResources());
            DevkitToolbarManager.registerToolbarElement("File/Baking/Resources",        new DevkitToolbarBakeSkyboxResources());
            DevkitToolbarManager.registerToolbarElement("File/U6S",                     new DevkitToolbarReplacePlaceholdersButton());
            DevkitToolbarManager.registerToolbarElement("File/U6S",                     new LoadDefaultLayoutButton());
        }
        public void shutdown()
        {
            DevkitWindowManager.toolbarCreated -= DevkitWindowManager_toolbarCreated;
        }
    }
}
