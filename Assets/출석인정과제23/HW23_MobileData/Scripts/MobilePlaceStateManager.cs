using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AttendanceHW23.MobileData
{
    public class MobilePlaceStateManager : MonoBehaviour
    {
        [SerializeField] private List<PlaceSaveableTransform> saveTargets = new List<PlaceSaveableTransform>();
        [SerializeField] private Transform controlledObject;
        [SerializeField] private string saveFileName = "attendance_hw23_place_state.json";
        [SerializeField] private bool loadOnStart = true;

        private string statusMessage = "Place state ready";
        private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

        private void Start()
        {
            if (loadOnStart)
            {
                LoadState();
            }
        }

        public void MoveControlledObject()
        {
            if (controlledObject == null)
            {
                statusMessage = "Controlled object is missing.";
                return;
            }

            controlledObject.position += new Vector3(0.25f, 0f, 0.15f);
            statusMessage = "Object position changed.";
        }

        public void RotateControlledObject()
        {
            if (controlledObject == null)
            {
                statusMessage = "Controlled object is missing.";
                return;
            }

            controlledObject.Rotate(Vector3.up, 30f, Space.World);
            statusMessage = "Object rotation changed.";
        }

        public void SaveState()
        {
            PlaceWorldData data = new PlaceWorldData();

            foreach (PlaceSaveableTransform target in saveTargets)
            {
                if (target == null)
                {
                    continue;
                }

                data.transforms.Add(new PlaceTransformData(
                    target.SaveId,
                    target.transform.position,
                    target.transform.rotation));
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            statusMessage = $"Saved {data.transforms.Count} object(s).";
            Debug.Log($"Attendance HW23 saved to {SavePath}");
        }

        public void LoadState()
        {
            if (!File.Exists(SavePath))
            {
                statusMessage = "No saved place state yet.";
                return;
            }

            string json = File.ReadAllText(SavePath);
            PlaceWorldData data = JsonUtility.FromJson<PlaceWorldData>(json);
            if (data == null || data.transforms == null)
            {
                statusMessage = "Saved data is invalid.";
                return;
            }

            foreach (PlaceTransformData transformData in data.transforms)
            {
                PlaceSaveableTransform target = FindTarget(transformData.id);
                if (target == null)
                {
                    continue;
                }

                target.transform.SetPositionAndRotation(
                    transformData.GetPosition(),
                    transformData.GetRotation());
            }

            statusMessage = $"Loaded {data.transforms.Count} object(s).";
            Debug.Log($"Attendance HW23 loaded from {SavePath}");
        }

        private PlaceSaveableTransform FindTarget(string id)
        {
            foreach (PlaceSaveableTransform target in saveTargets)
            {
                if (target != null && target.SaveId == id)
                {
                    return target;
                }
            }

            return null;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveState();
            }
        }

        private void OnApplicationQuit()
        {
            SaveState();
        }

        private void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                normal = { textColor = Color.white }
            };

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 24
            };

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(24, 24, 24, 24)
            };

            float panelWidth = Mathf.Min(420f, Screen.width - 48f);
            float panelHeight = 420f;
            Rect panelRect = new Rect(
                (Screen.width - panelWidth) * 0.5f,
                (Screen.height - panelHeight) * 0.5f,
                panelWidth,
                panelHeight);

            GUILayout.BeginArea(panelRect, boxStyle);
            GUILayout.Label("HW23 MobileData", labelStyle, GUILayout.Height(44f));
            GUILayout.Label(statusMessage, labelStyle, GUILayout.Height(58f));

            if (GUILayout.Button("Move Object", buttonStyle, GUILayout.Height(64f)))
            {
                MoveControlledObject();
            }

            if (GUILayout.Button("Rotate Object", buttonStyle, GUILayout.Height(64f)))
            {
                RotateControlledObject();
            }

            if (GUILayout.Button("Save", buttonStyle, GUILayout.Height(64f)))
            {
                SaveState();
            }

            if (GUILayout.Button("Load", buttonStyle, GUILayout.Height(64f)))
            {
                LoadState();
            }

            GUILayout.EndArea();
        }
    }

    [Serializable]
    public class PlaceWorldData
    {
        public List<PlaceTransformData> transforms = new List<PlaceTransformData>();
    }

    [Serializable]
    public class PlaceTransformData
    {
        public string id;
        public float positionX;
        public float positionY;
        public float positionZ;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float rotationW;

        public PlaceTransformData(string id, Vector3 position, Quaternion rotation)
        {
            this.id = id;
            positionX = position.x;
            positionY = position.y;
            positionZ = position.z;
            rotationX = rotation.x;
            rotationY = rotation.y;
            rotationZ = rotation.z;
            rotationW = rotation.w;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(positionX, positionY, positionZ);
        }

        public Quaternion GetRotation()
        {
            return new Quaternion(rotationX, rotationY, rotationZ, rotationW);
        }
    }
}
