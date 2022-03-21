using UnityEngine;
using Random = UnityEngine.Random;


public class SpawnPoint : MonoBehaviour
{
    public enum SpawnType
    {
        Obstacle,
        Wall
    }
    
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] Transform worldTransform;
    [SerializeField] int maxHeight = 10;
    [SerializeField] int minHeight = -10;

    Player player;
    World world;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        world = FindObjectOfType<World>();

        NewWall();
        NewObstacle();
        //StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void NewObstacle()
    {
        int spawnHeight = Random.Range(minHeight, maxHeight);
        Vector3 spawnPosition = new Vector3 (transform.position.x, transform.position.y + spawnHeight, transform.position.z);
        GameObject newObstacle  = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        world.AddObstacle(newObstacle);
        //world.ListAllObjects();
    }

    void NewWall()
    {
        GameObject newWall = Instantiate(wallPrefab, transform.position, Quaternion.identity);
    }
    
    // IEnumerator SpawnTimer()
    // {
    //     //Declare a yield instruction.
    //     WaitForSeconds wait = new WaitForSeconds(3);
    //     while (!player.GetIsDead())
    //     {
    //         NewObstacle();
    //         Debug.Log("BAM");
    //         yield return wait;
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spawnable")
        {
            NewObstacle(); //TODO check type and spawn correct GameObject
        }

        if (other.gameObject.tag == "Wall")
        {
            NewWall();
        }
    }
}
