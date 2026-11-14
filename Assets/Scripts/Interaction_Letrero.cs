using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction_Letrero : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private GameObject intaractText;
    [SerializeField] private GameObject panelLetrero;

    private bool isPlayerInRange = false;

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
            panelLetrero.SetActive(false);
        }
    }

    private void PerformInteraction(InputAction.CallbackContext context)
    {
        if (isPlayerInRange)
        {
            bool estaAbierto = panelLetrero.activeSelf;
            panelLetrero.SetActive(!estaAbierto);
            intaractText.SetActive(estaAbierto);
        }
    }
}