using UnityEngine;

namespace AttendanceHW23.MobileData
{
    public class PlaceSaveableTransform : MonoBehaviour
    {
        [SerializeField] private string saveId;

        public string SaveId => saveId;

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
