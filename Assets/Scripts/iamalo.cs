using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class iamalo : MonoBehaviour
{
    public Transform Target;
    public int rutina;
    public float cronometro;// tiempo entre rutinas
    public float grado;// variable para almacenar el grado del angulo de rotacion
    public Quaternion angulo;// variable para almacenar el angulo de rotacion

    void Start()
    {
        ;
    }
    // funcion que hace que el enemigo se mueva de manera aleatoria, todavia tengo que delimitar el area en la que se mueve, pero por ahora funciona
    public void Enemy()
    {           // distancia deteccion enemigo
        if (Vector3.Distance(transform.position, Target.transform.position) > 5)
        {
            transform.LookAt(Target.transform);
            transform.Translate(Vector3.forward * 1 * Time.deltaTime);
            //rutina aleatoria cada 4 segundos
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            switch (rutina)
            {
                case 0:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;
                case 1:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    break;
            }
        }
        // si el enemigo detecta al jugador, se mueve hacia el jugador
        else
        {
            var lookpos = Target.transform.position - transform.position;
            lookpos.y = 0;
            var rotation = Quaternion.LookRotation(lookpos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
            transform.Translate(Vector3.forward * 2 * Time.deltaTime);
        }
    }
    void Update()
    {
        Enemy();
    }
}
