using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerMovementAxis : MonoBehaviour
{
    [SerializeField] Rigidbody rb;          // Verwijzing naar de Rigidbody van de speler
    [SerializeField] Camera playerCamera;   // Verwijzing naar de camera van de speler
    [SerializeField] float speed = 5f;      // Snelheid van de speler
    [SerializeField] float swimSpeed = 3f;  // Snelheid van het zwemmen (omhoog/omlaag)
    [SerializeField] float jumpForce = 5f;  // Kracht van het springen
    [SerializeField] float crouchHeight = 0.25f;  // Hoogte van de speler bij het kruipen
    [SerializeField] float standingHeight = 1f;  // Standaard hoogte van de speler

    private bool isGrounded;      // Controle of de speler op de grond staat
    private bool isCrouching;     // Controle of de speler aan het kruipen is
    private CapsuleCollider playerCollider;

    // Voor waterdetectie
    [SerializeField] WaterSurface water;
    WaterSearchParameters Search = new WaterSearchParameters();
    WaterSearchResult SearchResult = new WaterSearchResult();

    [SerializeField] GameObject extraFloater;

    bool jump = false;

    void Start()
    {
        // Zorg ervoor dat we de CapsuleCollider krijgen
        playerCollider = GetComponent<CapsuleCollider>();
        standingHeight = playerCollider.height; // Standaard hoogte van de speler
    }

    void Update()
    {
        // Waterdetectie
        Search.startPositionWS = transform.position;
        water.ProjectPointOnWaterSurface(Search, out SearchResult);

        bool closeToWaterSurface = Mathf.Abs(transform.position.y - SearchResult.projectedPositionWS.y) < 0.75f;

        // Controleer of de speler zich onder het wateroppervlak bevindt
        if (transform.position.y < SearchResult.projectedPositionWS.y)
        {
            float vertSwim = 0;
            if (Input.GetButton("Jump") && !closeToWaterSurface)
            {
                vertSwim = 1;
            }

            if (Input.GetButtonDown("Jump") && closeToWaterSurface)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            // Kruipen
            if (Input.GetKey(KeyCode.LeftControl) && !closeToWaterSurface)
            {
                vertSwim = -1;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && closeToWaterSurface)
            {
                rb.AddForce(Vector3.up * -jumpForce * 0.5f, ForceMode.Impulse);
            }

            // De speler is onder water
            SwimMovement(vertSwim);
        }
        else
        {
            // Beweging met WASD
            MovePlayer();
            // Springen
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jump=true;
                
            }

            // Kruipen
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ToggleCrouch();
            }
        }

        if (closeToWaterSurface)
        {
            extraFloater.SetActive(true);
        }
        else
        {
            extraFloater.SetActive(false);

        }
    }

    void FixedUpdate()
    {
        if(jump){
            Jump();
            jump = false;
        }
        // Controleer of de speler de grond raakt
        CheckGroundStatus();
    }

    void MovePlayer()
    {
        // Haal de input op van de speler (W, S, A, D)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A, D voor links/rechts
        float vertical = Input.GetAxisRaw("Vertical");     // W, S voor voorwaarts/achterwaarts

        // Haal de rotatie van de camera op, maar negeer de verticale rotatie (omhoog/omlaag)
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0f; // Zorg ervoor dat we de verticale as negeren (omhoog/omlaag kijken)
        cameraForward.Normalize();

        // De camera's rechtse as wordt ook bepaald zonder de verticale rotatie
        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0f; // Verwijder de verticale component van de rechtse as
        cameraRight.Normalize();

        // Beweeg de speler in de richting van de camera
        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        // Pas de snelheid aan
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);


    }

    void Jump()
    {
        // Spring door een kracht naar boven toe te voegen
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void ToggleCrouch()
    {
        if (isCrouching)
        {
            // Sta op
            playerCollider.height = standingHeight;
            isCrouching = false;
        }
        else
        {
            // Kruip
            playerCollider.height = crouchHeight;
            isCrouching = true;
        }
    }

    void CheckGroundStatus()
    {
        // Pas de lengte van de raycast aan op basis van de hoogte van de speler
        float rayLength = isCrouching ? crouchHeight + 0.01f : standingHeight + 0.01f;

        // Controleer of de speler zich op de grond bevindt
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    bool IsOnRaft()
    {
        // Bepaal de lengte van de raycast op basis van de hoogte van de speler
        float rayLength = isCrouching ? crouchHeight + 0.1f : standingHeight + 0.1f;

        // Controleer of de speler op een object met de tag "Raft" staat
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength))
        {
            return hit.collider.CompareTag("Raft");
        }
        return false;
    }

    void SwimMovement(float pVertSwim)
    {
        // Haal de rotatie van de camera op, inclusief verticale rotatie (omhoog/omlaag kijken)
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.Normalize();

        // De camera's rechtse as wordt ook bepaald zonder de verticale rotatie
        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0f; // Verwijder de verticale component van de rechtse as
        cameraRight.Normalize();

        // Verhoog of verlaag de speler afhankelijk van waar de camera naar kijkt
        float vertical = Input.GetAxisRaw("Vertical"); // Voor/achter voorwaarts, omhoog/omlaag voor zwemmen
        float horizontal = Input.GetAxisRaw("Horizontal"); // Links/rechts beweging

        // Beweeg de speler in de richting van de camera (inclusief zwemmen omhoog/omlaag)
        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal + Vector3.up * pVertSwim).normalized;

        // Pas de snelheid aan (zwemmen)
        Vector3 swimDirection = moveDirection * swimSpeed * Time.fixedDeltaTime;

        // Pas de verticale beweging toe op basis van de camera (omhoog of omlaag)
        //swimDirection.y = (cameraForward.y * vertical + Input.GetAxis("Mouse Y")) * swimSpeed * Time.fixedDeltaTime;

        //swimDirection.y += pVertSwim * swimSpeed * Time.fixedDeltaTime;

        // Verplaats de speler
        rb.MovePosition(rb.position + swimDirection);
    }
}
