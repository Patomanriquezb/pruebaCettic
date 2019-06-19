/* Este script controla cada sección del camino que será instanciado por el script RoadController.
 * Además contiene las ubicaciones para instanciar tanto a los enemigos como los objetos propios de esta sección.
 * De todas estas ubicaciones, se escoge una cantidad dependiendo de la dificultad y cantidad de objetos.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSection : MonoBehaviour
{
    public bool TouchedByPlayers { get; private set; }  = false; //Pensando a futuro, este script instanciará la siguiente sección del camino solamente al entrar el primer jugador.

    [SerializeField] //SerializeField expone la variable al inspector de Unity sin necesidad de hacerla pública.
    private List<GameObject> enemySpawners; //Lista de GameObjects que representan las posibles posiciones en que se instanciarán enemigos.

    [SerializeField]
    private GameObject enemyPrefab; //GameObject prefab de los enemigos a instanciar.

    [SerializeField]
    private List<GameObject> collectibleSpawners; //Lista de GameObjects que representan las posibles posiciones en que se instanciarán objetos.

    [SerializeField]
    private GameObject collectiblePrefab; //GameObject prefab de los objetos a instanciar.

    [SerializeField]
    private int difficulty = 1; //Parámetro que define la cantidad de enemigos a instanciar.

    [SerializeField]
    private int collectibles = 3; //Parámetro que define la cantidad de objetos a instanciar.

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Al entrar el player al collider de la sección del camino, si es el primero en entrar, se llama al RoadController para instanciar una nueva sección del camino.
        if (other.gameObject.tag == "PlayerCar")
        {
            if (!TouchedByPlayers)
            {
                GameController.instance.RoadControl.SpawnRoadSection(this);
                TouchedByPlayers = true;
            }
        }

        //Si entra el collider "RoadCleaner" se elimina esta sección del camino para ahorrar memoria. Este collider sigue a cada uno de los autos de jugador.
        //Por ahora, como sólo se puede jugar single player, no es necesario detectar cuál es el último jugador de la carrera para que vaya eliminando las secciones del camino.
        if (other.gameObject.tag == "RoadCleaner")
        {
            Destroy(gameObject);
        }
    }

    public void SpawnEnemigos()
    {
        //Esta función instancia los enemigos en la sección del camino. Es llamado por RoadController luego de instanciar cada sección de camino.
        //Instancia entre 0 y difficulty*2 (o la cantidad de GameObjects en enemySpawners) enemigos.
        int enemies = 0;
        if(difficulty*2 > enemySpawners.Count)
        {
            enemies = Random.Range(difficulty/2,enemySpawners.Count);
        }
        else
        {
            enemies = Random.Range(difficulty / 2, difficulty*2);
        }

        for(int i = 0; i < enemies; i++)
        {
            int spawnerIndex = Random.Range(1, enemySpawners.Count);
            GameObject selectedSpawner = enemySpawners[spawnerIndex];
            GameObject newEnemy = Instantiate(enemyPrefab, selectedSpawner.transform.position, Quaternion.identity);
            newEnemy.transform.parent = transform; //Asigna los enemigos como hijos de la sección del camino. De esta forma, al eliminar la sección, se eliminarán también los enemigos.
            enemySpawners.Remove(selectedSpawner); //Quita el GameObject de la lista para que no se repita en la siguiente iteración.
        }
    }

    public void SpawnCollectibles()
    {
        //Instancia exactamente la cantidad de objetos determinada en collectibles.
        for (int i = 0; i < collectibles; i++)
        {
            int spawnerIndex = Random.Range(1, collectibleSpawners.Count);
            GameObject selectedSpawner = collectibleSpawners[spawnerIndex];
            GameObject newCollectible = Instantiate(collectiblePrefab, selectedSpawner.transform.position, Quaternion.identity);
            newCollectible.transform.parent = transform; //Asigna los objetos como hijos de la sección del camino. De esta forma, al eliminar la sección, se eliminarán también los objetos.
            enemySpawners.Remove(selectedSpawner); //Quita el GameObject de la lista para que no se repita en la siguiente iteración.
        }
    }


}
