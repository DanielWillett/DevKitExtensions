using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unturned.SystemEx;

namespace DevKitExtensions
{
    public class DevkitRoadsToolOptions : IFormattedFileReadable, IFormattedFileWritable
    {
        private static DevkitRoadsToolOptions _instance = new DevkitRoadsToolOptions();
        [Inspectable("#SDG::Devkit.Roads_Tool.Snap_Position", null)]
        public float snapPosition = 1f;
        [Inspectable("#SDG::Devkit.Roads_Tool.Normal_Mode", null)]
        public ERoadMode normalMode = ERoadMode.MIRROR;
        [Inspectable("#SDG::Devkit.Roads_Tool.Selection_Mask", null)]
        public ERayMask selectionMask = ERayMask.LARGE | ERayMask.MEDIUM | ERayMask.SMALL | ERayMask.ENVIRONMENT | ERayMask.GROUND | ERayMask.CLIP | ERayMask.TRAP;

        // per road selection
        [Inspectable("#SDG::Devkit.Roads_Tool.Offset", null)]
        public float offsetField = -1f;
        [Inspectable("#SDG::Devkit.Roads_Tool.Offset", null)]
        public bool loopToggle = false;
        [Inspectable("#SDG::Devkit.Roads_Tool.Ignore_Terrain", null)]
        public bool ignoreTerrainToggle = false;
        [Inspectable("#SDG::Devkit.Roads_Tool.Offset", null)]
        public int roadIndexBox = 0;
        public RoadMaterial material = null;

        protected static bool wasLoaded = false;
        public static DevkitRoadsToolOptions instance => _instance;
        public static void load()
        {
            wasLoaded = true;
            string path = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Roads.tool");
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            if (!File.Exists(path))
                return;
            using (StreamReader input = new StreamReader(path))
                instance.read(new KeyValueTableReader(input));
        }
        public static void save()
        {
            if (!wasLoaded)
                return;
            string path = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Roads.tool");
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            using (StreamWriter writer = new StreamWriter(path))
                new KeyValueTableWriter(writer).writeValue(instance);
        }
        public void read(IFormattedFileReader reader)
        {
            this.snapPosition = reader.readValue<float>("Snap_Position");
            this.normalMode = reader.readValue<ERoadMode>("Normal_Mode");
            this.selectionMask = reader.readValue<ERayMask>("Selection_Mask");
        }
        public void write(IFormattedFileWriter writer)
        {
            writer.writeValue("Snap_Position", this.snapPosition);
            writer.writeValue("Normal_Mode", this.normalMode);
            writer.writeValue("Selection_Mask", this.selectionMask);
        }
        public static void UpdateSelection(Road road, RoadJoint joint)
        {
            if (road == null || joint == null) return;
            instance.offsetField = joint.offset;
            instance.loopToggle = road.isLoop;
            instance.ignoreTerrainToggle = joint.ignoreTerrain;
            instance.normalMode = joint.mode;
            instance.roadIndexBox = LevelRoads.getRoadIndex(road);
            instance.material = LevelRoads.materials[road.material];
        }
        public static void SetMaterial(RoadMaterial material, byte materialid)
        {
            DevkitRoadsTool.instance.CurrentRoad.material = materialid;
            instance.material = material;
        }
        static DevkitRoadsToolOptions() => load();
    }
}
