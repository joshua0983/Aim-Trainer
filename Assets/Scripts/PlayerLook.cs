using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    private float xSensitivity;
    private float ySensitivity;

    void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 0.5f);
        SetSensitivity(savedSensitivity);
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (cam != null)
        {
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    public void SetSensitivity(float sensitivity)
    {
        xSensitivity = sensitivity * 100;
        ySensitivity = sensitivity * 100;

        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }
}