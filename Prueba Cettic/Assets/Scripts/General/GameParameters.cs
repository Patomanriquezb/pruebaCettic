/* Parámetros modificables del juego. Estos son expuestos mediante la etiqueta [SerializeField] para no tener que hacerlos públicos.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameParameters : MonoBehaviour
{

    [SerializeField] //SerializeField expone la variable al inspector de Unity sin necesidad de hacerla pública.
    [Tooltip("Speed in m/s")]
    private float MaxCarSpeed; //Velocidad máxima del auto.

    [Tooltip("Acceleration in m/s^2")]
    [SerializeField]
    private float CarAcceleration; //Aceleración del auto.

    [Tooltip("Break force in m/s^2")]
    [SerializeField]
    private float CarBrakeForce; //Aceleración de freno del auto.

    [SerializeField]
    [Tooltip("Speed in m/s")]
    private float CarSteeringSpeed; //Velocidad horizontal del auto.

    [SerializeField]
    private float MaxCarDurability; //Durabilidad o vida máxima del auto.

    [SerializeField]
    private bool CarPowerEnabled; //Boolean que determina si el poder especial está habilitado o no.

    [Tooltip("Duration in seconds")]
    [SerializeField]
    private float CarPowerDuration; //Duración en segundos del poder especial.

    [Tooltip("Cooldown in seconds")]
    [SerializeField]
    private float CarPowerCooldown; //Tiempo en segundos que se demora en estar nuevamente disponible el poder especial.

    [Tooltip("Game time in seconds")]
    [SerializeField]
    private float GameTime; //Tiempo de cada partida.

    [SerializeField]
    private int PointsPerItem; //Puntos que da cada objeto al ser recogido.

    [SerializeField]
    private float EnemyDamage; //Daño que cada enemigo le hace al jugador al chocarlo.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Funciones que entregan los valores de los parámetros.

    public float GetMaxCarSpeed(){return MaxCarSpeed;}

    public float GetCarSteeringSpeed() { return CarSteeringSpeed; }

    public float GetMaxCarDurability(){return MaxCarDurability;}

    public float GetCarAcceleration() { return CarAcceleration; }

    public float GetCarBrakeForce() { return CarBrakeForce; }

    public bool GetCarPowerEnabled(){return CarPowerEnabled;}

    public float GetCarPowerDuration(){return CarPowerDuration;}

    public float GetCarPowerCooldown(){return CarPowerCooldown;}

    public float GetGameTime(){return GameTime;}

    public int GetPointsPerItem(){return PointsPerItem;}

    public float GetEnemyDamage(){return EnemyDamage;}

}
