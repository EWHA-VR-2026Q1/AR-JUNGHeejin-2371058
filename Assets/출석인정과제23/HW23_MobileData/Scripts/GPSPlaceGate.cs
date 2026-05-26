using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace AttendanceHW23.MobileData
{
    public class GPSPlaceGate : MonoBehaviour
    {
        [SerializeField] private string mobileDataSceneName = "HW23_MobileData";
        [SerializeField] private double targetLatitude = 37.564102480428;
        [SerializeField] private double targetLongitude = 126.92788532245;
        [SerializeField] private float enterRadiusMeters = 100f;
        [SerializeField] private float desiredAccuracyMeters = 10f;
        [SerializeField] private float updateDistanceMeters = 5f;
        [SerializeField] private bool useEditorSimulation = true;
        [SerializeField] private bool simulateInsidePlace;

        private string message = "GPS ready";
        private double currentLatitude;
        private double currentLongitude;
        private float currentDistanceMeters = -1f;
        private bool loadingScene;

        private void Start()
        {
            StartCoroutine(StartLocationService());
        }

        private IEnumerator StartLocationService()
        {
#if UNITY_EDITOR
            if (useEditorSimulation)
            {
                message = "Editor simulation mode";
                yield break;
            }
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                message = "Requesting location permission...";
                Permission.RequestUserPermission(Permission.FineLocation);

                int permissionWait = 10;
                while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation) && permissionWait > 0)
                {
                    yield return new WaitForSeconds(1f);
                    permissionWait--;
                }

                if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                {
                    message = "Location permission was denied.";
                    yield break;
                }
            }
#endif
            if (!Input.location.isEnabledByUser)
            {
                message = "Location permission is not enabled.";
                yield break;
            }

            Input.location.Start(desiredAccuracyMeters, updateDistanceMeters);

            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                message = $"GPS initializing... {maxWait}";
                yield return new WaitForSeconds(1f);
                maxWait--;
            }

            if (maxWait <= 0)
            {
                message = "GPS initialization timed out.";
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                message = "GPS failed.";
                yield break;
            }

            message = "GPS running";
        }

        private void Update()
        {
            if (loadingScene)
            {
                return;
            }

#if UNITY_EDITOR
            if (useEditorSimulation)
            {
                currentLatitude = simulateInsidePlace ? targetLatitude : targetLatitude + 0.001;
                currentLongitude = targetLongitude;
                currentDistanceMeters = simulateInsidePlace ? 0f : DistanceMeters(currentLatitude, currentLongitude, targetLatitude, targetLongitude);
                message = simulateInsidePlace ? "Simulated inside place" : "Simulated outside place";
                CheckEnterPlace();
                return;
            }
#endif
            if (Input.location.status != LocationServiceStatus.Running)
            {
                return;
            }

            LocationInfo location = Input.location.lastData;
            currentLatitude = location.latitude;
            currentLongitude = location.longitude;
            currentDistanceMeters = DistanceMeters(currentLatitude, currentLongitude, targetLatitude, targetLongitude);
            message = $"GPS running / distance {currentDistanceMeters:F1} m";

            CheckEnterPlace();
        }

        private void CheckEnterPlace()
        {
            if (currentDistanceMeters <= enterRadiusMeters)
            {
                loadingScene = true;
                SceneManager.LoadScene(mobileDataSceneName);
            }
        }

        private float DistanceMeters(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadiusMeters = 6371000.0;
            double latRad1 = lat1 * Mathf.Deg2Rad;
            double latRad2 = lat2 * Mathf.Deg2Rad;
            double deltaLat = (lat2 - lat1) * Mathf.Deg2Rad;
            double deltaLon = (lon2 - lon1) * Mathf.Deg2Rad;

            double a = Mathf.Sin((float)(deltaLat / 2.0)) * Mathf.Sin((float)(deltaLat / 2.0)) +
                       Mathf.Cos((float)latRad1) * Mathf.Cos((float)latRad2) *
                       Mathf.Sin((float)(deltaLon / 2.0)) * Mathf.Sin((float)(deltaLon / 2.0));
            double c = 2.0 * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt((float)(1.0 - a)));

            return (float)(earthRadiusMeters * c);
        }

        private void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 23,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                normal = { textColor = Color.white }
            };

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 22
            };

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(24, 24, 24, 24)
            };

            float panelWidth = Mathf.Min(520f, Screen.width - 48f);
            float panelHeight = 390f;
            Rect panelRect = new Rect(
                (Screen.width - panelWidth) * 0.5f,
                (Screen.height - panelHeight) * 0.5f,
                panelWidth,
                panelHeight);

            GUILayout.BeginArea(panelRect, boxStyle);
            GUILayout.Label("GPS Place Check", labelStyle, GUILayout.Height(42f));
            GUILayout.Label($"Target: {targetLatitude:F6}, {targetLongitude:F6}", labelStyle, GUILayout.Height(36f));
            GUILayout.Label($"Current: {currentLatitude:F6}, {currentLongitude:F6}", labelStyle, GUILayout.Height(36f));
            GUILayout.Label($"Radius: {enterRadiusMeters:F0} m", labelStyle, GUILayout.Height(36f));
            GUILayout.Label($"Distance: {currentDistanceMeters:F1} m", labelStyle, GUILayout.Height(36f));
            GUILayout.Label(message, labelStyle, GUILayout.Height(56f));

#if UNITY_EDITOR
            if (useEditorSimulation && GUILayout.Button("Toggle Simulated Place Entry", buttonStyle, GUILayout.Height(64f)))
            {
                simulateInsidePlace = !simulateInsidePlace;
            }
#endif
            GUILayout.EndArea();
        }
    }
}
