using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HW23.DataSaveLoad
{
    public class WorldSaveLoadManager : MonoBehaviour
    {
        [SerializeField] private List<SaveableTransform> saveTargets = new List<SaveableTransform>();
        [SerializeField] private string saveFileName = "hw23_world_state.json";
        [SerializeField] private bool loadOnStart = true;
        [SerializeField] private bool saveOnApplicationPause = true;
        [SerializeField] private bool saveOnApplicationQuit = true;

        private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

        private void Start()
        {
            if (loadOnStart)
            {
                LoadWorld();
            }
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard.f5Key.wasPressedThisFrame)
            {
                SaveWorld();
            }

            if (keyboard.f9Key.wasPressedThisFrame)
            {
                LoadWorld();
            }
        }

        public void SaveWorld()
        {
            WorldData worldData = new WorldData();

            foreach (SaveableTransform saveTarget in saveTargets)
            {
                if (saveTarget == null)
                {
                    continue;
                }

                Transform targetTransform = saveTarget.transform;
                worldData.transforms.Add(new TransformData(
                    saveTarget.SaveId,
                    saveTarget.IsPlayer,
                    targetTransform.position,
                    targetTransform.rotation));
            }

            string json = JsonUtility.ToJson(worldData, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"HW23 saved {worldData.transforms.Count} transforms to {SavePath}");
        }

        public void LoadWorld()
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log($"HW23 save file does not exist yet: {SavePath}");
                return;
            }

            string json = File.ReadAllText(SavePath);
            WorldData worldData = JsonUtility.FromJson<WorldData>(json);

            if (worldData == null || worldData.transforms == null)
            {
                Debug.LogWarning("HW23 save file could not be loaded.");
                return;
            }

            foreach (TransformData transformData in worldData.transforms)
            {
                SaveableTransform saveTarget = FindTarget(transformData.id);
                if (saveTarget == null)
                {
                    Debug.LogWarning($"HW23 save target not found: {transformData.id}");
                    continue;
                }

                saveTarget.transform.SetPositionAndRotation(
                    transformData.GetPosition(),
                    transformData.GetRotation());
            }

            Debug.Log($"HW23 loaded {worldData.transforms.Count} transforms from {SavePath}");
        }

        private SaveableTransform FindTarget(string id)
        {
            foreach (SaveableTransform saveTarget in saveTargets)
            {
                if (saveTarget != null && saveTarget.SaveId == id)
                {
                    return saveTarget;
                }
            }

            return null;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && saveOnApplicationPause)
            {
                SaveWorld();
            }
        }

        private void OnApplicationQuit()
        {
            if (saveOnApplicationQuit)
            {
                SaveWorld();
            }
        }
    }

    [Serializable]
    public class WorldData
    {
        public List<TransformData> transforms = new List<TransformData>();
    }

    [Serializable]
    public class TransformData
    {
        public string id;
        public bool isPlayer;
        public float positionX;
        public float positionY;
        public float positionZ;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float rotationW;

        public TransformData(string id, bool isPlayer, Vector3 position, Quaternion rotation)
        {
            this.id = id;
            this.isPlayer = isPlayer;
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
