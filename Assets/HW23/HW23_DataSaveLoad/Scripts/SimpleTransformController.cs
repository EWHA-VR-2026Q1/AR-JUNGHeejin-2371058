using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace HW23.DataSaveLoad
{
    public class SimpleTransformController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float rotateSpeed = 90f;

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            float horizontal = GetKeyAxis(keyboard.aKey, keyboard.dKey);
            float vertical = GetKeyAxis(keyboard.sKey, keyboard.wKey);
            Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;

            transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

            if (keyboard.qKey.isPressed)
            {
                transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime, Space.World);
            }

            if (keyboard.eKey.isPressed)
            {
                transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
            }
        }

        private float GetKeyAxis(KeyControl negativeKey, KeyControl positiveKey)
        {
            float axis = 0f;

            if (negativeKey.isPressed)
            {
                axis -= 1f;
            }

            if (positiveKey.isPressed)
            {
                axis += 1f;
            }

            return axis;
        }
    }
}
