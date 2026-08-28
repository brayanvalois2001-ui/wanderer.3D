using UnityEngine;
using UnityEngine.InputSystem; // Importante para usar el nuevo Input System

public class CamaraSigue3D : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    [SerializeField] private Transform objetivo; // Arrastra aquí a tu Jugador

    [Header("Ajustes de Control del Ratón")]
    [SerializeField] private float sensibilidadRaton = 0.1f; // Sensibilidad adaptada al nuevo sistema
    [SerializeField] private float limiteInclinacionMin = 5f;   // Límite inferior (para no ver bajo el suelo)
    [SerializeField] private float limiteInclinacionMax = 80f;  // Límite superior (mirada desde arriba)

    [Header("Ajustes de Movimiento")]
    [SerializeField] private float tiempoSuave = 0.05f;

    private float distanciaActual;
    private float rotacionX = 0f;
    private float rotacionY = 0f;
    private Vector3 velocidadActual = Vector3.zero;

    private void Start()
    {
        if (objetivo != null)
        {
            // 1. Ocultar y fijar el cursor en el centro
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 2. Medir la distancia que dejaste puesta en la escena
            distanciaActual = Vector3.Distance(transform.position, objetivo.position);

            // 3. Guardar ángulos iniciales para no dar saltos al arrancar
            Vector3 angulosIniciales = transform.eulerAngles;
            rotacionX = angulosIniciales.x;
            rotacionY = angulosIniciales.y;
        }
    }

    private void LateUpdate()
    {
        if (objetivo == null) return;

        // 4. Leer el ratón con el nuevo Input System
        if (Mouse.current != null)
        {
            Vector2 deltaRaton = Mouse.current.delta.ReadValue();

            rotacionY += deltaRaton.x * sensibilidadRaton;
            rotacionX -= deltaRaton.y * sensibilidadRaton;
        }

        // 5. Limitar inclinación vertical
        rotacionX = Mathf.Clamp(rotacionX, limiteInclinacionMin, limiteInclinacionMax);

        // 6. Convertir en rotación 3D
        Quaternion rotacionDeseada = Quaternion.Euler(rotacionX, rotacionY, 0f);

        // 7. Calcular nueva posición orbitando al personaje
        Vector3 posicionDeseada = objetivo.position - (rotacionDeseada * Vector3.forward * distanciaActual);

        // 8. Aplicar movimiento y rotación suave
        transform.position = Vector3.SmoothDamp(transform.position, posicionDeseada, ref velocidadActual, tiempoSuave);
        transform.rotation = rotacionDeseada;
    }
}