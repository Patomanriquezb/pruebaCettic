/* Este controlador usa el singleton pattern para que sea consistente durante el juego. Su función es manejar la instanciación de las secciones del camino.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    [SerializeField] //SerializeField expone la variable al inspector de Unity sin necesidad de hacerla pública.
    private List<GameObject> RoadSections; //Lista que contiene los GameObject prefabs de las posibles secciones del camino a instanciar.

    // Start is called before the first frame update
    void Start()
    {
        //Patrón singleton
        if (GameController.instance.RoadControl == null)
        {
            GameController.instance.RoadControl = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnRoadSection(RoadSection notifierSection)
    {
        //Esta función instancia las secciones del camino según la posición de la sección que llamó a la función.

        int randomRoad = Random.Range(0, RoadSections.Count); //Escoge una sección al azar de la lista de posibles secciones.

        GameObject newRoad = Instantiate(RoadSections[randomRoad], notifierSection.transform.position + new Vector3(0, 0, 220), Quaternion.identity);
        newRoad.transform.parent = notifierSection.transform.parent; //Luego de instanciar la sección, le asigna como parent el objeto Road que contiene todas las secciones.
        newRoad.GetComponent<RoadSection>().SpawnEnemigos(); //Instancia los enemigos de la sección creada.
        newRoad.GetComponent<RoadSection>().SpawnCollectibles(); //Instancia los objetos de la sección creada.
    }
}
