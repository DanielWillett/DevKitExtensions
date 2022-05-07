using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using SDG.Framework.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DevKitExtensions
{
    class DevkitRoadsTool : IDevkitTool
    {
        protected ERoadMode mode;
        public static DevkitRoadsTool instance { get; protected set; }

        protected void instantiate(Vector3 point)
        {
            if (CurrentRoad != null)
            {
                if (NormalIndex > -1)
                    Select(CurrentRoad.addVertex(VertexIndex + NormalIndex, point));
                else if (Vector3.Dot(point - CurrentJoint.vertex, CurrentJoint.getTangent(0)) > Vector3.Dot(point - CurrentJoint.vertex, CurrentJoint.getTangent(1)))
                    Select(CurrentRoad.addVertex(VertexIndex, point));
                else
                    Select(CurrentRoad.addVertex(VertexIndex + 1, point));
            }
            else
                Select(LevelRoads.addRoad(point));
        }

        protected void handleTeleportTransformed(Vector3 point, Vector3 normal)
        {
            // on pressed e with one selected
            point += normal * DevkitSelectionToolOptions.instance.surfaceOffset;
            Quaternion rotation = Quaternion.LookRotation(normal) * Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f));
            Selection.position = point;
            Selection.rotation = rotation;
        }

        public virtual void update()
        {
            if (_isPaving || DevkitNavigation.isNavigating || !DevkitInput.canEditorReceiveInput)
                return;
            Ray pointerToWorldRay = DevkitInput.pointerToWorldRay;
            bool eDown = InputEx.GetKeyDown(KeyCode.E);
            bool lClkDown = InputEx.GetKeyDown(KeyCode.Mouse0);
            RaycastHit worldHit;
            if ((eDown || lClkDown) && !Physics.Raycast(pointerToWorldRay, out RaycastHit logicHit, 8192f, RayMasks.LOGIC))
            {
                Physics.Raycast(pointerToWorldRay, out worldHit, 8192f, (int)DevkitRoadsToolOptions.instance.selectionMask);
                if (worldHit.transform != null)
                {
                    if (eDown)
                    {
                        if (DevkitSelectionManager.selection.Count > 0)
                            this.handleTeleportTransformed(worldHit.point, worldHit.normal);
                        else
                            this.instantiate(worldHit.point);
                        return;
                    }
                    else if (lClkDown)
                    {
                        this.Select(worldHit.transform);
                        return;
                    }
                }
                else worldHit = default;
            }
            else
            {
                worldHit = default;
                logicHit = default;
            }
            if ((InputEx.GetKeyDown(KeyCode.Delete) || InputEx.GetKeyDown(KeyCode.Backspace)) && (Selection != null && CurrentRoad != null))
            {
                if (InputEx.GetKey(KeyCode.LeftAlt))
                    LevelRoads.removeRoad(CurrentRoad);
                else
                    CurrentRoad.removeVertex(VertexIndex);
                Deselect();
                return;
            }
            if (InputEx.GetKeyDown(KeyCode.LeftAlt))
            {
                if (worldHit.transform != null)
                {
                    if (CurrentRoad != null)
                    {
                        if (NormalIndex > -1)
                            CurrentRoad.moveTangent(VertexIndex, NormalIndex, worldHit.point - CurrentJoint.vertex);
                        else if (VertexIndex > -1)
                            CurrentRoad.moveVertex(VertexIndex, worldHit.point);
                    }
                }
                return;
            }
            if (!InputEx.GetKeyDown(KeyCode.Mouse0))
                return;
            if (logicHit.transform != null)
            {
                if (logicHit.transform.name.IndexOf("Path") == -1)
                {
                    if (logicHit.transform.name.IndexOf("Tangent") == -1)
                        return;
                }
                Select(logicHit.transform);
            }
            else
            {
                if (worldHit.transform == null)
                    return;
                this.instantiate(worldHit.point);
            }
        }

        public virtual void equip()
        {
            instance = this;
            _isPaving = false;
            Highlighter = ((GameObject)Object.Instantiate(Resources.Load("Edit/Highlighter"))).transform;
            Highlighter.name = "Highlighter";
            Highlighter.parent = Level.editing;
            Highlighter.gameObject.SetActive(false);
            Highlighter.GetComponent<Renderer>().material.color = Color.red;
            Deselect();
        }
        public virtual void dequip()
        {
            Object.Destroy(Highlighter);
        }

        private bool _isPaving = false;
        public bool IsPaving
        {
            get => _isPaving;
            set
            {
                _isPaving = value;
                Highlighter.gameObject.SetActive(_isPaving);
                if (_isPaving)
                    return;
                Select(null);
            }
        }
        public Road CurrentRoad { get; protected set; }
        public RoadPath CurrentPath { get; protected set; }
        public RoadJoint CurrentJoint { get; protected set; }
        protected int VertexIndex;
        protected int NormalIndex;
        public Transform Selection { get; protected set; }
        public Transform Highlighter { get; protected set; }


        public void Select(Transform target)
        {
            // unhighlight current selected road vertex if one is selected
            if (CurrentRoad != null)
            {
                if (NormalIndex > -1)
                    CurrentPath.unhighlightTangent(NormalIndex);
                else if (VertexIndex > -1)
                    CurrentPath.unhighlightVertex();
            }

            // deselect if the current selection was clicked, or curser clicked off the current vertex.
            if (Selection == target || target == null)
            {
                Deselect();
                return;
            }

            // get the selected point from the transform provided.
            Selection = target;
            CurrentRoad = LevelRoads.getRoad(Selection, out VertexIndex, out NormalIndex);
            if (CurrentRoad != null)
            {
                CurrentPath = CurrentRoad.paths[VertexIndex];
                CurrentJoint = CurrentRoad.joints[VertexIndex];
                if (NormalIndex > -1)
                    CurrentPath.highlightTangent(NormalIndex);
                else if (VertexIndex > -1)
                    CurrentPath.highlightVertex();
            } else
            {
                CurrentPath = null;
                CurrentJoint = null;
            }
        }
        public void Deselect()
        {
            Selection = null;
            CurrentRoad = null;
            CurrentPath = null;
            CurrentJoint = null;
            VertexIndex = -1;
            NormalIndex = -1;
        }

        private void Start()
        {
        }
    }
}
