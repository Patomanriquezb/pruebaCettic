/* Este es el script que controla el movimiento y los estados del auto.
 * El poder especial desarrollado es un escudo que protege al auto de los enemigos.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float maxDurability; //Durabilidad o vida máxima del auto.
    private float durability; //Durabilidad actual del auto.

    public float DurabilityFill { get; private set; } = 1; //Proporcional entre la durabilidad actual y la máxima, entre 0 y 1.

    private float maxSpeed; //Velocidad máxima del auto.

    public float Speed { get; private set; } //Velocidad actual del auto.

    private float acceleration; //Aceleración del auto.

    private float brakeForce; //Aceleración negativa para frenar el auto.

    private float steeringSpeed; //Velocidad de movimiento horizontal del auto.

    private bool powerEnabled; //Bool que indica si el poder especial está habilitado o no.
    private float powerDuration; //Duración en segundos del poder especial mientras está activado.
    private float powerCooldown; //Tiempo de espera en segundos para poder usar el poder especial nuevamente.

    private bool powerReady; //Bool que indica si el poder especial está listo  o no.

    private bool powerActive; //Bool que indica si es que el poder especial está activado o no.

    private float powerTimer; //Timer del poder especial, usado para calcular el cooldown y la duración.

    public float PowerFill { get; private set; } = 1; //Proporcional entre el timer y el cooldown del poder especial, entre 0 y 1.

    public enum CarStates { Stopped, Accelerating, Running, Stopping, Stunned, Destroyed} //Posibles estados del auto.

    public CarStates CarState { get; private set; } //Estado actual del auto.

    private Rigidbody rigidBody; //Componente rigidbody del auto.

    [SerializeField]
    private int stunFactor; //Factor que determinará la velocidad máxima a la que seguirá avanzando el auto mientras está stunned.

    [SerializeField]
    private float stunDuration; //Duración en segundos del stun.

    private float stunTimer = 0; //Timer del stun.

    [SerializeField]
    private GameObject shield; //GameObject del escudo del poder especial.

    [SerializeField]
    private GameObject carBody; //Modelo del auto.

    [SerializeField]
    private Material[] carMaterialsOpaque; //Arreglo de materiales opacos del auto.

    [SerializeField]
    private Material[] carMaterialsTransparent; //Arreglo de materiales translúcidos del auto, que se usan cuando el auto está stunned.

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        if(stunFactor == 0) { stunFactor = 1; } //Este factor no puede ser 0, ya que la velocidad máxima será dividida por él.

    }


    // Start is called before the first frame update
    void Start()
    {
        //Patrón singleton.
        if (GameController.instance.CarControl == null)
        {
            GameController.instance.CarControl = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        CarState = CarStates.Stopped; //El auto empieza detenido.

        //Se obtienen valores desde los parámetros del juego.
        maxDurability = GameController.instance.Parameters.GetMaxCarDurability();
        durability = maxDurability;
        maxSpeed = GameController.instance.Parameters.GetMaxCarSpeed();
        acceleration = GameController.instance.Parameters.GetCarAcceleration();
        brakeForce = GameController.instance.Parameters.GetCarBrakeForce();
        steeringSpeed = GameController.instance.Parameters.GetCarSteeringSpeed();
        powerEnabled = GameController.instance.Parameters.GetCarPowerEnabled();
        powerDuration = GameController.instance.Parameters.GetCarPowerDuration();
        powerCooldown = GameController.instance.Parameters.GetCarPowerCooldown();
        powerReady = true; //El poder especial comienza listo para usarse.

        shield.GetComponent<Renderer>().material.renderQueue++; //El material del escudo se debe renderizar por sobre el auto cuando usa los materiales translúcidos.

    }

    // Update is called once per frame
    void Update()
    {

        UpdatePower(); //Actualización del estado del poder especial.

    }

    void FixedUpdate()
    {
        CarFSM(); //Finite State Machine del auto. Tiene que ir en FixedUpdate para que no varíe la velocidad del juego según la velocidad del procesador del equipo.
    }

    private void CarFSM()
    {
        //Finite State Machine que determina el comportamiento del auto según su estado.
        switch (CarState)
        {
            //Cuando está en estado "Stopping", se le aplica una fuerza negativa al auto. Una vez que su velocidad es 0, cambia a estado "Stopped".
            case CarStates.Stopping:
                if (rigidBody.velocity.z > 0)
                {
                    rigidBody.AddForce(new Vector3(0, 0, -brakeForce), ForceMode.Acceleration);
                }
                else
                {
                    CarState = CarStates.Stopped;
                }
                break;

            //Cuando está en estado "Stopped", se asegura que la velocidad del auto sea 0.
            case CarStates.Stopped:
                rigidBody.velocity = new Vector3(0, 0, 0);
                break;

            //Cuando el estado es "Accelerating", se aplica una aceleración positiva al auto, hasta llegar a su velocidad máxima.
            case CarStates.Accelerating:
                rigidBody.AddForce(new Vector3(0, 0, acceleration), ForceMode.Acceleration);
                Speed = rigidBody.velocity.z;
                if (rigidBody.velocity.z >= maxSpeed)
                {
                    CarState = CarStates.Running; //Cuando el auto llega a su velocidad máxima, cambia su estado a "Running".
                }
                Steer(); //Mientras está en este estado, el auto puede doblar.
                break;

            //Cuando está en estado "Running" se asegura que su velocidad sea la velocidad máxima.
            case CarStates.Running:
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, maxSpeed);
                Steer(); //En este estado el auto puede doblar.
                break;

            //Cuando está en este estado, el modelo del auto gira cada frame, se aplica una aceleración negativa hasta llegar a velocidad máxima dividida en el factor de stun.
            case CarStates.Stunned:
                carBody.transform.Rotate(new Vector3(0, 2, 0));
                if (rigidBody.velocity.z > maxSpeed / stunFactor)
                {
                    rigidBody.AddForce(new Vector3(0, 0, -brakeForce), ForceMode.Acceleration);
                }
                else
                {
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, maxSpeed / stunFactor);
                }
                if(stunTimer >= stunDuration) //Además se activa el timer de stun, que al llegar a la duración, el auto vuelve a estado "Accelerating" y vuelve a sus materiales opacos.
                {
                    carBody.GetComponent<Renderer>().materials = carMaterialsOpaque;
                    CarState = CarStates.Accelerating;
                    stunTimer = 0;
                }
                stunTimer += Time.deltaTime;
                break;

            //En este estado, el auto comienza a frenar y girar. al llegar a velocidad 0, se mantiene detenido.
            case CarStates.Destroyed:
                if (rigidBody.velocity.z > 0)
                {
                    carBody.transform.Rotate(new Vector3(0, 1, 0));
                    rigidBody.AddForce(new Vector3(0, 0, -brakeForce), ForceMode.Acceleration);

                }
                else
                {
                    rigidBody.velocity = new Vector3(0, 0, 0);
                }
                break;
        }
    }

    //Función que hace que el auto empiece a acelerar.
    public void StartCar()
    {
        CarState = CarStates.Accelerating;
    }

    //Función que mueve al auto de manera horizontal según el valor del Eje X del Input de Unity y la velocidad horizontal del auto.
    private void Steer()
    {
        if (Input.GetAxis("Horizontal") > 0 )
        {
            rigidBody.AddForce(new Vector3(steeringSpeed, 0, 0), ForceMode.VelocityChange); //Lo mueve a la derecha.
            carBody.transform.rotation = Quaternion.Euler(0, 105, 0); //Gira levemente el modelo para dar la ilusión de ir doblando.
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rigidBody.AddForce(new Vector3(-steeringSpeed, 0, 0), ForceMode.VelocityChange); //Lo mueve a la izquierda.
            carBody.transform.rotation = Quaternion.Euler(0, 75, 0); //Gira levemente el modelo para dar la ilusión de ir doblando.
        }
        else
        {
            rigidBody.velocity = new Vector3(0, 0, rigidBody.velocity.z); //Lo mantiene fijo en el eje horizontal.
            carBody.transform.rotation = Quaternion.Euler(0, 90, 0); //Lo devuelve a su rotación original, apuntando hacia adelante.
        }
    }

    //Función que detiene al auto.
    public void StopCar()
    {
        CarState = CarStates.Stopping;
        carBody.GetComponent<Renderer>().materials = carMaterialsOpaque;
    }

    //Función que reacciona al golpe de un enemigo.
    public void EnemyHit(Enemy hitEnemy)
    {
        //Si está activado el escudo, o su el auto ya está stunned o destruido, ignora el golpe del enemigo.
        if (powerActive || CarState == CarStates.Stunned || CarState == CarStates.Destroyed) { return; }
        durability -= hitEnemy.Damage; //Resta el daño que le hace el enemigo a su durabilidad o vida.
        DurabilityFill = durability / maxDurability; //Actualiza el valor proporcional de durabilidad actual / durabilidad máxima
        if (durability <= 0)
        {
            DestroyCar(); //Si su durabilidad es menor o igual a 0, el auto es destruido.
        }
        else
        {
            StunCar(); //Si su durabilidad siguie siendo mayor a 0, el auto es aturdido.
        }

    }

    //Función que activa el poder especial, en este caso, el escudo.
    private void ActivatePower()
    {
        powerActive = true; //Activa el poder especial.
        powerReady = false;
        PowerFill = 0; //Vuelve el cooldown del poder especial a 0.
        shield.SetActive(true); //Activa el modelo del escudo.
    }

    //Función que actualiza el estado del poder especial.
    private void UpdatePower()
    {
        //Si el poder no está habilitado, se ignora el resto de la función.
        if (!powerEnabled) { return; }

        //Si el poder está listo para usarse y el auto no está detenido ni destruido, al presionar el Input "PowerButton", que en este caso está configurado para ser la tecla Espacio, se activa el poder especial.
        if (powerReady)
        {
            if (CarState != CarStates.Stopped && CarState != CarStates.Destroyed)
            {
                if (Input.GetButtonDown("PowerButton"))
                {
                    ActivatePower();
                }
            }
        }
        else if(CarState != CarStates.Destroyed) //Si no está listo el poder y el auto no está destruido comienza a correr el timer.
        {
            powerTimer += Time.deltaTime;
            
            if (powerActive) //Si el poder est;a activo y el timer llega a la duración del poder, el poder se desactiva, se desactiva el modelo del escudo y el timer vuelve a 0.
            {
                if (powerTimer >= powerDuration)
                {
                    powerActive = false;
                    shield.SetActive(false);
                    powerTimer = 0;
                }
            }
            else //Si el poder no está activo, se va actualizando el proporcional del timer / tiempo de cooldown. Si el timer llega al tiempo de cooldown, el poder está listo para usarlo.
            {
                PowerFill = powerTimer / powerCooldown;
                if (powerTimer >= powerCooldown)
                {
                    powerReady = true;
                    powerTimer = 0;
                }
            }
        }
    }

    //Función que aturde al auto, haciendo que baje su velocidad y rote sobre su eje por un tiempo determinado.
    private void StunCar()
    {
        CarState = CarStates.Stunned;
        carBody.GetComponent<Renderer>().materials = carMaterialsTransparent; //Como feedback visual, al entrar en este estado, el auto se pondrá de color transparente.
    }

    //Esta función destruye el auto, terminando la carrera antes de que se acabe el tiempo. Como comentarios se agregan posibles efectos visuales de este estado.
    private void DestroyCar()
    {
        CarState = CarStates.Destroyed;
        //car explosion
        //activate fire
    }

    //Función que actualiza el puntaje del juego al pasar sobre un objeto.
    public void Collect(Collectible collected)
    {
        GameController.instance.AddPoints(collected.Points);
    }


}
