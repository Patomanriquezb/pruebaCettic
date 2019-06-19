/* Este script controla la UI de la escena de juego. Esto incluye tanto las barras de estado, el tiempo y el puntaje, así como el panel final.
 * Además controla el flujo de pantallas según los botones clickeados.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] //SerializeField expone la variable al inspector de Unity sin necesidad de hacerla pública.
    private Text statusText; //Texto que es actualizado según el estado del auto.

    [SerializeField]
    private Image statusFill; //Imagen que va cambiando su tamaño y color según el estado del auto.

    [SerializeField]
    private Text powerText; //Texto que es actualizado según el estado del poder especial.

    [SerializeField]
    private Image powerFill; //Imagen que va cambiando su tamaño y color según el estado del poder especial.

    [SerializeField]
    private Image powerBG; //Imagen de fondo de la barra del poder especial.

    [SerializeField]
    private Text startText; //Texto que muestra la cuenta regresiva para iniciar la carrera.

    [SerializeField]
    private Text gameTimer; //Texto que muestra el tiempo que le queda a la partida.

    [SerializeField]
    private Text scoreText; //Texto que muestra el puntaje del jugador.

    [SerializeField]
    private GameObject gameOver; //Panel que contiene los textos y botones que se muestran al terminar la partida.

    [SerializeField]
    private Text gameOverScore; //Texto que muestra el puntaje final del jugador.

    private float startTimer = 1; //Tiempo en segundos antes de mostrar la cuenta regresiva.

    private float barWidth; //Ancho de las barras de estado del auto y poder especial.


    // Start is called before the first frame update
    void Start()
    {
        //Patrón singleton
        if (GameController.instance.UIControl == null)
        {
            GameController.instance.UIControl = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!GameController.instance.Parameters.GetCarPowerEnabled())
        {
            //Si el poder especial no está habilitado, se modifica el panel de status, quitando la barra y el texto de estado del poder especial.
            GameObject StatusPanel = powerFill.rectTransform.parent.gameObject;
            StatusPanel.GetComponent<RectTransform>().sizeDelta = new Vector2 (StatusPanel.GetComponent<RectTransform>().rect.width, StatusPanel.GetComponent<RectTransform>().rect.height / 2);
            powerFill.gameObject.SetActive(false);
            powerText.gameObject.SetActive(false);
            powerBG.gameObject.SetActive(false);
        }

        barWidth = statusFill.rectTransform.rect.width; //Obtiene el ancho de la barra de estado del auto.
    }

    // Update is called once per frame
    void Update()
    {

        if (GameController.instance.GameState == GameController.GameStates.Playing || GameController.instance.GameState == GameController.GameStates.Ending) {
            //Si el juego se encuentra en estado "Playing" y el texto de cuenta regresiva está activo, se actualiza el su timer.
            if (startText.gameObject.activeInHierarchy)
            {
                if (startTimer <= 0)
                {
                    startText.gameObject.SetActive(false); //Si el timer llega a 0, se desactiva el texto de cuenta regresiva.
                }
                startTimer -= Time.deltaTime;
            }
            UpdateStatusBar(); //Actualiza la barra y texto de estado del auto.
            if (GameController.instance.Parameters.GetCarPowerEnabled()) { UpdatePowerBar(); } //Si el poder especial está activo, actualiza la barra y texto correspondientes.
            UpdateGameTimer(); //Actualiza el texto del tiempo de la partida.
            UpdateGameScore(); //Actualiza el texto del puntaje del jugador.
        }

    }

    private void UpdateStatusBar()
    {
        Debug.Log(barWidth);
        statusFill.rectTransform.sizeDelta = new Vector2(GameController.instance.CarControl.DurabilityFill * barWidth, statusFill.rectTransform.sizeDelta.y); //Modifica el tamaño de la imagen de relleno de la barra de estado del auto.
        if(GameController.instance.CarControl.DurabilityFill > 0.7f) //Mayor a 70% hace la imagen verde.
        {
            statusFill.color = Color.green;
            statusText.text = "Vehículo: OK";
        }
        else if(GameController.instance.CarControl.DurabilityFill > 0.2f) //Entre 20% y 70% hace la imagen amarilla.
        {
            statusFill.color = Color.yellow;
            statusText.text = "Vehículo: Precaución";

        }
        else if(GameController.instance.CarControl.DurabilityFill > 0) //menor a 20% hace la imagen roja.
        {
            statusFill.color = Color.red;
            statusText.text = "Vehículo: ¡PELIGRO!";
        }
        else
        {
            statusText.text = "Vehículo: DESTRUIDO.";
        }

    }

    private void UpdatePowerBar()
    {
        powerFill.rectTransform.sizeDelta = new Vector2(GameController.instance.CarControl.PowerFill * barWidth, powerFill.rectTransform.sizeDelta.y); //Modifica el tamaño de la imagen de relleno de la barra de estado del poder especial.
        if (GameController.instance.CarControl.PowerFill >= 1) //Si el cooldown del poder ya terminó se informa al jugador que ya está listo.
        {
            powerText.text = "Escudo: ¡Listo!";
        }
        else //Sino, le dice que está cargando.
        {
            powerText.text = "Escudo: Cargando...";
        }
    }

    private void UpdateGameTimer()
    {
        gameTimer.text = Mathf.Ceil(GameController.instance.GameTimer).ToString() + " s"; //Actualiza el texto del timer de la partida.
    }

    public void UpdateGameScore()
    {
        scoreText.text = "Puntaje: " + GameController.instance.Score; //Actualiza el texto del puntaje del jugador.
    }
    
    public void SetStartText(string text)
    {
        startText.text = text; //Setea el contenido del texto de cuenta regresiva desde el GameController.
    }

    public void ShowGameOver() //Muestra el panel del final del juego y oculta el texto del puntaje del UI principal para que no se repita al salir nuevamente en dicho panel.
    {
        gameOverScore.text = scoreText.text;
        scoreText.gameObject.SetActive(false);
        gameOver.SetActive(true);
    }

    public void ReloadScene() //Vuelve a cargar la escena de juego.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu() //Vuelve a la escena de título.
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void ExitGame() //Cierra el juego.
    {
        Application.Quit();
    }
}
