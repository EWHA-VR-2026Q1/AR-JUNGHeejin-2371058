using UnityEngine;

namespace HW23.DataSaveLoad
{
    public class SaveableTransform : MonoBehaviour
    {
        [SerializeField] private string saveId;
        [SerializeField] private bool player;

        public string SaveId => saveId;
        public bool IsPlayer => player;

        private void Reset()
        {
            saveId = gameObject.name;
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(saveId))
            {
                saveId = gameObject.name;
            }
        }
    }
}
