using UnityEngine;
using Random = UnityEngine.Random;


public class SpawnPoint : MonoBehaviour
{
    public enum SpawnType
    {
        Obstacle,
        Wall
    }

    [SerializeField] bool tutorialMode = false;
    
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
        //NewObstacle(transform.position);
        //StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewObstacle(Vector3 pos)
    {
        int spawnHeight = Random.Range(minHeight, maxHeight);
        float xPos = transform.position.x;
        if (tutorialMode)
        {
            xPos = (FindObjectOfType<TutorialController>().GetTutorialIndex() == 1)
                ? transform.position.x + Random.Range(minHeight, maxHeight)
                : xPos;
        }
        
        Vector3 spawnPosition = new Vector3 (xPos, transform.position.y + spawnHeight, transform.position.z);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        //world.AddObstacle(newObstacle);
    }

    public void NewWall()
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
            NewObstacle(transform.position);
        }

        if (other.gameObject.tag == "Wall")
        {
            NewWall();
        }

        if (tutorialMode) 
        {
            if (FindObjectOfType<TutorialController>().GetTutorialIndex() == 1 && other.gameObject.tag == "Checkpoint")
            {
                NewObstacle(transform.position);
            }
        }
    }
}
