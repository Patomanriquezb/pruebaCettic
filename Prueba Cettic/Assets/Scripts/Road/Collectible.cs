/*Este script controla el comportamiento de los objetos que el jugador puede recoger. En este demo sólo entregan puntaje.
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int Points { get; private set; } //Puntos que entregará al jugador.

    [SerializeField] //SerializeField expone la variable al inspector de Unity sin necesidad de hacerla pública.
    private GameObject collectibleModel; //Referencia al modelo del objeto para poder girarlo.

    // Start is called before the first frame update
    void Start()
    {
        Points = GameController.instance.Parameters.GetPointsPerItem(); //Obtiene el puntaje desde los parámetros del juego.
    }

    // Update is called once per frame
    void Update()
    {
        collectibleModel.transform.Rotate(0, 2, 0, Space.World); //Gira el objeto en cada frame.
    }

    private void OnTriggerEnter(Collider other)
    {
        //Al colisionar con el player, llama la función "Collect" del jugador y luego destruye el objeto.
        if (other.gameObject.tag == "PlayerCar")
        {
            other.GetComponent<CarController>().Collect(this);
            //play collectible sound
            Destroy(gameObject);
        }
    }
}
