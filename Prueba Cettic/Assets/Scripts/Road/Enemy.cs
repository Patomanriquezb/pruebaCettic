/* Este script controla el comportamiento de los enemigos. Por ahora son estáticos y se destruyen al ser chocados por el jugador.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float Damage { get; private set; } //Daño que le hará al jugador al ser chocado.

    [SerializeField]
    private GameObject enemyModel; //Referencia al modelo del enemigo.

    // Start is called before the first frame update
    void Start()
    {
        Damage = GameController.instance.Parameters.GetEnemyDamage(); //Obtiene el daño definido en los parámetros del juego.

        //Además, se determinará una rotación inicial al azar para el enemigo dentro de -90 y 90 grados.
        enemyModel.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(90f, 180f), 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Al ser chocado por el jugador', se ejecuta la función "EnemyHit" y luego el enemigo se destruye.
        if (other.gameObject.tag == "PlayerCar")
        {
            other.GetComponent<CarController>().EnemyHit(this);
            //instance explosion
            Destroy(gameObject);
        }
    }
}