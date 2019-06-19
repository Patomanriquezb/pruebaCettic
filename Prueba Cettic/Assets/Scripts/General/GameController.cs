/* Script que controla el flujo de la partida. Es un singleton que contiene las referencias a los distintos controladores y a los parámentros.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public GameParameters Parameters { get; private set; } //Parámetros del juego.
    public CarController CarControl { get; set; } //Controlador del auto.
    public RoadController RoadControl { get; set; } //Controlador del camino.
    public UIController UIControl { get; set; } //Controlador del UI.

    public int Score { get; private set; } = 0; //Puntaje de la partida.

    private float gameTime; //Tiempo máximo de la partida.
    public float GameTimer { get; private set; } //Cronómetro que indica cuánto tiempo le queda al jugador.

    private float startTimer = 4; //Timer que controla la cuenta regresiva para iniciar la carrera.

    public enum GameStates { Starting, Playing, Ending, Ended } //Enum con los posibles estados del juego.
    public GameStates GameState { get; private set; } //Variable que guarda el estado actual.

    void Awake()
    {
        //Patrón singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        Parameters = GetComponent<GameParameters>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameState = GameStates.Starting; //Comienza la partida, pero todavía no empieza la carrera.
        gameTime = Parameters.GetGameTime(); //Obtiene el tiempo máximo de la carrera desde los parámetros.
        GameTimer = gameTime; //Setea el cronómetro en el tiempo máximo. Luego se irá reduciendo.
    }

    // Update is called once per frame
    void Update()
    {
        //FSM que maneja el flujo de la partida.
        switch (GameState)
        {
            //Si el estado es "Starting" comienza a correr la cuenta regresiva para iniciar la carrera. Se actualiza el texto correspondiente.
            case GameStates.Starting:
                if (startTimer <= 3 && startTimer > 0)
                {
                    UIControl.SetStartText(Mathf.Ceil(startTimer).ToString());
                }
                else if (startTimer <= 0)
                {
                    UIControl.SetStartText("GO!");
                    GameState = GameStates.Playing; //Al completar la cuenta regresiva, se inicia la carrera.
                    CarControl.StartCar(); //Se cambia el estado del auto de "Stopped" a "Accelerating".
                }
                startTimer -= Time.deltaTime;
                break;
            //Si el estado es "Playing", se actualiza el cronómetro del tiempo de la carrera.
            case GameStates.Playing:
                
                if (GameTimer <= 0 || CarControl.CarState == CarController.CarStates.Destroyed)
                {
                    GameState = GameStates.Ending; //Si el cronómetro llega a 0, se termina la carrera, pero no todavía la partida.
                }
                GameTimer -= Time.deltaTime;
                break;
            //Si el estado es "Ending", se comienza a frenar el auto
            case GameStates.Ending:
                if (CarControl.CarState != CarController.CarStates.Stopping && CarControl.CarState != CarController.CarStates.Stopped)
                {
                    CarControl.StopCar(); //Si el auto se está moviendo, se aplica la fuerza de freno.
                }
                else if (CarControl.CarState == CarController.CarStates.Stopped)
                {
                    EndGame(); //Si la velocidad del auto ya llegó a 0, se termina la partida.
                    GameState = GameStates.Ended;
                }
                
                break;
            case GameStates.Ended:

                break;
        }
    }

    //Función que agrega puntos al puntaje.
    public void AddPoints(int pointstoAdd)
    {
        Score += pointstoAdd;
    }

    //Función que muestra el panel del final del juego.
    private void EndGame()
    {
        UIControl.ShowGameOver();
    }
}
