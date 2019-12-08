using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public GameObject enemyPrefab;
    private GameObject enemyInstance;

    GameObject[] positions;
    int rand;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyInstance = Instantiate(enemyPrefab);
        positions = GameObject.FindGameObjectsWithTag("Hexagon");
        Warp();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        enemyInstance.SetActive(false);
    }

    public void Show()
    {
        enemyInstance.SetActive(true);
    }

    public void Warp()
    {
        rand = Random.Range(0, positions.Length - 1);
        enemyInstance.transform.position = new Vector3(positions[rand].transform.position.x, 0.23f, positions[rand].transform.position.z);
        enemyInstance.GetComponent<ParticleSystem>().Play();
    }
}
