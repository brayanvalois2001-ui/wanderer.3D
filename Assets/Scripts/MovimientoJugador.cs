using UnityEngine;
using UnityEngine.InputSystem; // Importante para el nuevo Input System

public class MovimientoJugador : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    [SerializeField] private float velocidad = 6f;
    [SerializeField] private float velocidadRotacion = 12f;

    [Header("Referencia de la Cámara")]
    [SerializeField] private Transform camaraTransform; // Si no lo asignas, se buscará automáticamente

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Si no asignaste la cámara manualmente en el Inspector, la busca por defecto
        if (camaraTransform == null && Camera.main != null)
        {
            camaraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        // 1. Leemos las teclas WASD con el nuevo Input System
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) vertical += 1f;
            if (Keyboard.current.sKey.isPressed) vertical -= 1f;
            if (Keyboard.current.dKey.isPressed) horizontal += 1f;
            if (Keyboard.current.aKey.isPressed) horizontal -= 1f;
        }

        // 2. Obtenemos la dirección HACIA DÓNDE MIRA LA CÁMARA
        Vector3 camaraAdelante = camaraTransform.forward;
        Vector3 camaraDerecha = camaraTransform.right;

        // Anulamos la inclinación vertical (Eje Y) para que el personaje no intente volar ni meterse bajo tierra al mover la cámara
        camaraAdelante.y = 0f;
        camaraDerecha.y = 0f;

        // Normalizamos para asegurarnos de que la velocidad sea constante
        camaraAdelante.Normalize();
        camaraDerecha.Normalize();

        // 3. Calculamos la dirección final multiplicando la entrada por la orientación de la cámara
        // W/S moverá en la dirección "adelante" de la cámara
        // A/D moverá en la dirección "derecha/izquierda" de la cámara
        Vector3 direccionMovimiento = (camaraAdelante * vertical + camaraDerecha * horizontal).normalized;

        // 4. Si hay movimiento, aplicamos la posición y giramos el personaje hacia la marcha
        if (direccionMovimiento != Vector3.zero)
        {
            // Mover al personaje usando el Rigidbody
            rb.MovePosition(transform.position + direccionMovimiento * velocidad * Time.deltaTime);

            // Girar suavemente al personaje hacia la dirección hacia la que avanza
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
        }
    }
}