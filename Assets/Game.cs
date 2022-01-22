using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Game : MonoBehaviour
{
    float speed = 2;
    public Vector3 jump;
    public float jumpForce = .001f;
    public bool isGrounded = true;
    Rigidbody rb;
    private Vector3 tilesize = new Vector3(0.1f, 0.05f, 0.1f);
    [SerializeField] public int[,] glassTiles = new int[2, 8];

    
    void createGlassTitles(int[,] glassTiles)
    {


        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(glassTiles[i, j] > 0)
                {
                    GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    tile.transform.position = new Vector3(-3 * i + 1.5f, 0, j * 2);
                    tile.transform.localScale = tilesize;

                    tile.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
        }

    }

    void setGlassTilesPositions(int[,] glassTiles)
    {
        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(i == 0)
                    glassTiles[i, j] = Random.Range(1, 3); // tile value either 1 or 2
                else
                    glassTiles[i, j] = - glassTiles[i - 1, j] + 3;  // invert 1 or 2 for the other side
            }
        }
    }

    void OnCollisionStay()
    {
        
    }

    bool checkWeakTile(int[,] glassTiles, float x, float y)
    {
        if (glassTiles[(int)(-(x - 1.5f) / 3), (int)y / 2] == 1)
            return true;

        return false;

    }

    void OnCollisionEnter(Collision col)
    {
        isGrounded = true;
        if (col.gameObject.name == "Plane")
        {
            if(checkWeakTile(glassTiles, col.gameObject.transform.position.x, col.gameObject.transform.position.y))
            {
                Destroy(col.gameObject);
            }
        }
        /*print(col.gameObject.name);*/
    }

    bool checkFallen()
    {
        if (gameObject.transform.position.y < -20)
            return true;
        
        return false;
    }

    void Start()
    {
        setGlassTilesPositions(glassTiles);
        createGlassTitles(glassTiles);

        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        Vector3 velocity = direction * speed;
        Vector3 moveAmount = velocity * Time.deltaTime;

        /*transform.position += moveAmount;*/
        transform.Translate(moveAmount);

        if(checkFallen())
        {
            gameObject.transform.position = new Vector3(0, 4, -4);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            rb.AddForce(jump * jumpForce * 0.2f, ForceMode.Impulse);
            isGrounded = false;
        }


    }
}
