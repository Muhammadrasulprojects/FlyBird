using UnityEngine;

public class PipeMiddleScript : MonoBehaviour
{
    public LogicScript logicScript; // Reference to the LogicScript component

    void Start()
    {
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            logicScript.addScore(1);

            // Add a pipe to the end (it will not appear directly in front of the bird,
            // but will be placed at the end based on spacing)
            if (PipeSpawnScript.instance != null)
            {
                PipeSpawnScript.instance.AddPipeToEnd();
            }
        }
    }
}
