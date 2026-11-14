using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Interaction_Moneda : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private GameObject intaractText;
    [SerializeField] private TMP_Text contadorTexto;

    private static int monedas = 0;
    private static int totalMonedas = 0;

    private bool isPlayerInRange = false;

    private void Awake()
    {
        monedas = 0;
    }

    private void Start()
    {
        totalMonedas = FindObjectsByType<Interaction_Moneda>().Length;
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
            interactAction.action.performed += PerformInteraction;
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed -= PerformInteraction;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            intaractText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            intaractText.SetActive(false);
        }
    }

    private void PerformInteraction(InputAction.CallbackContext context)
    {
        if (isPlayerInRange)
        {
            monedas++;
            contadorTexto.gameObject.SetActive(true);
            contadorTexto.text = "Monedas: " + monedas;

            intaractText.SetActive(false);
            gameObject.SetActive(false);

            if (monedas >= totalMonedas)
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}