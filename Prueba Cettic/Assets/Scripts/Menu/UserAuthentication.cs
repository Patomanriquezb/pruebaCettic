/*Este script controla la autentificación de los usuarios. En este demo se guardan los datos de manera local en los PlayerPrefs. Además, no se está encriptando la contraseña.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAuthentication : MonoBehaviour
{
    public enum AuthStates { WrongUser, WrongPassword, OK} //Estos estados son los que el autentificador responde en caso de intentar iniciar sesión o crear un usuario.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AuthStates TryLogin(string username, string password)
    {
        //Función que intenta iniciar sesión con los datos entregados.
        if (PlayerPrefs.HasKey("user_" + username)) //Si existe el key con el nombre de usuario ingresado en los PlayerPrefs, compara si la contraseña ingresada corresponde a la registrada.
        {
            if (PlayerPrefs.GetString("user_" + username) == password)
            {
                return AuthStates.OK;
            }
            else
            {
                return AuthStates.WrongPassword;
            }
        }
        else
        {
            return AuthStates.WrongUser;
        }

    }

    public AuthStates TryCreateUser(string username, string password)
    {
        //Función que intenta crear un usuario con los datos entregados.
        if (PlayerPrefs.HasKey("user_" + username)) //Primero revisa que no exista el usuario en los PlayerPrefs.
        {
            return AuthStates.WrongUser;
        }
        else
        {
            if (password.Length < 6) //Luego revisa que la contraseña tenga más de 6 caracteres.
            {
                return AuthStates.WrongPassword;
            }
            else
            {
                PlayerPrefs.SetString("user_" + username, password); //Si lo anterior se cumple, se registra el usuario en los PlayerPrefs.
                return AuthStates.OK;
            }
        }
    }
}
