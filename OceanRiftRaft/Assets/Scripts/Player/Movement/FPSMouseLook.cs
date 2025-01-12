using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 2.0f;   // Snelheid van de muisbeweging
    public Transform playerBody;            // Verwijzing naar het object voor het draaien van de speler (meestal de speler zelf)
    
    private float xRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Zorg ervoor dat de cursor verborgen en vergrendeld is in het midden van het scherm
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Haal de muisbewegingen op
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Draai de camera horizontaal (om je heen kijken)
        transform.Rotate(Vector3.up * mouseX);

        // Draai de camera verticaal (om naar boven/beneden te kijken), met beperking om niet over de verticale as te draaien
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Beperk de verticale rotatie
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}