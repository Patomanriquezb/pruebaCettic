/* Este script controla el menú principal del juego. Aquí se encuentran las funciones que dirigen el flujo del juego, así como funciones que manejan la UI
 * y el color del auto seleccionado por el usuario.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] //SerializeField expone la variable al inspector de Unity sin necesidad de hacerla pública.
    private GameObject carModel; //Referencia al modelo del auto usada para girarlo en cada frame.

    [SerializeField]
    private Text statusLogin; //Texto que muestra el feedback del intento de inicio de sesión.

    [SerializeField]
    private InputField userLogin; //InputField en el que se ingresa el nombre de usuario para iniciar sesión.

    [SerializeField]
    private InputField passLogin; //InputField en el que se ingresa la contraseña para iniciar sesión.

    [SerializeField]
    private GameObject buttonLogin; //Referencia al botón para iniciar sesión.

    [SerializeField]
    private Text statusSignUp; //Texto que muestra el feedback del intento de crear usuario.

    [SerializeField]
    private InputField userSignUp; //InputField en el que se ingresa el nombre de usuario para crear usuario.

    [SerializeField]
    private InputField passSignUp; //InputField en el que se ingresa la contraseña para crear usuario.

    [SerializeField]
    private GameObject buttonSignUp; //Referencia al botón para crear usuario.

    [SerializeField]
    private GameObject panelJugar; //Panel que contiene el botón para ir a la escena de juego.

    [SerializeField]
    private List<Color> carColors = new List<Color> { Color.red, Color.blue, Color.yellow, Color.black, Color.cyan, Color.grey, Color.magenta, Color.white }; //Lista de posibles colores para el auto del jugador.

    [SerializeField]
    private Material carMaterial; //Material del cuerpo del auto que puede ser modificado por el jugador según la lista de colores posibles.

    [SerializeField]
    private Material carStunMaterial; //Material del cuerpo del auto en estado de stun; toma el mismo valor que el material anterior.

    private int colorIndex = 0; //Índice para obtener el color de la lista de colores posibles.

    private UserAuthentication Authentication; //Referencia al script de autentificación.

    // Start is called before the first frame update
    void Start()
    {
        Authentication = GetComponent<UserAuthentication>(); //Obtiene el componente de autentificación del mismo objeto que contiene este script.
        carMaterial.color = carColors[0]; //Asigna el primer color de la lista al cuerpo del auto y al material de stun.
        carStunMaterial.color = carColors[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        carModel.transform.Rotate(0, 1, 0); //Gira el modelo del auto en cada frame.
    }

    public void ChangeColorUp()
    {
        //Esta función cambia el color del carro al siguiente de la lista.
        colorIndex++;
        if(colorIndex == carColors.Count)
        {
            colorIndex = 0;
        }
        carMaterial.color = carColors[colorIndex];
        carStunMaterial.color = new Color(carColors[colorIndex].r, carColors[colorIndex].g, carColors[colorIndex].b, 0.5f);
    }

    public void ChangeColorDown()
    {
        //Esta función cambia el color del carro al previo en la lista.
        colorIndex--;
        if (colorIndex == -1)
        {
            colorIndex = carColors.Count - 1;
        }
        carMaterial.color = carColors[colorIndex];
        carStunMaterial.color = new Color(carColors[colorIndex].r, carColors[colorIndex].g, carColors[colorIndex].b, 0.5f);
    }

    public void LoginButton()
    {
        //Llama la función del autentificador para intentar iniciar sesión entregándole los valores de los inputs correspondientes.
        //Además actualiza la UI del menú según la respuesta del autentificador.
        UserAuthentication.AuthStates authenticantionAnswer = Authentication.TryLogin(userLogin.text, passLogin.text);
        if (authenticantionAnswer == UserAuthentication.AuthStates.OK)
        {
            statusLogin.text = "¡Bienvenido " + userLogin.text + "!";
            userLogin.readOnly = true;
            passLogin.readOnly = true;
            buttonLogin.SetActive(false);
            panelJugar.SetActive(true);
        }
        else if (authenticantionAnswer == UserAuthentication.AuthStates.WrongPassword)
        {
            statusLogin.text = "Contraseña incorrecta.";
            passLogin.text = "";
        }
        else
        {
            statusLogin.text = "Usuario no registrado.";
            userLogin.text = "";
            passLogin.text = "";
        }
    }

    public void CreateUserButton()
    {
        //Llama la función del autentificador para intentar crear un usuario entregándole los valores de los inputs correspondientes.
        //Además actualiza la UI del menú según la respuesta del autentificador.
        UserAuthentication.AuthStates authenticantionAnswer = Authentication.TryCreateUser(userSignUp.text, passSignUp.text);
        if (authenticantionAnswer == UserAuthentication.AuthStates.OK)
        {
            statusSignUp.text = "¡Usuario creado!";
            passSignUp.readOnly = true;
            userSignUp.readOnly = true;
            buttonSignUp.SetActive(false);
            panelJugar.SetActive(true);
        }
        else if (authenticantionAnswer == UserAuthentication.AuthStates.WrongPassword)
        {
            statusSignUp.text = "Tu contraseña tiene que tener al menos 6 caracteres.";
            passSignUp.text = "";
        }
        else
        {
            statusSignUp.text = "Ya existe un usuario con ese nombre.";
            userSignUp.text = "";
            passSignUp.text = "";
        }
    }

    public void GoToGame()
    {
        //Carga la escena de juego.
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        //Cierra el juego.
        Application.Quit();
    }
}
