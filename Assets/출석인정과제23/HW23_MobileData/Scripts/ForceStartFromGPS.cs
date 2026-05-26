using UnityEngine;
using UnityEngine.SceneManagement;

namespace AttendanceHW23.MobileData
{
    public static class ForceStartFromGPS
    {
        private const string GpsSceneName = "GPS";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadGpsFirst()
        {
            if (SceneManager.GetActiveScene().name == GpsSceneName)
            {
                return;
            }

            SceneManager.LoadScene(GpsSceneName);
        }
    }
}
