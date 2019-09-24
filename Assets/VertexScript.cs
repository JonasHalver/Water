using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexScript : MonoBehaviour
{
    float amount;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(RandomDistance());
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.up * amount * Time.deltaTime;
    }

    IEnumerator RandomDistance()
    {
        bool r = false;
        while (true)
        {
            if (!r)
            {
                amount = Random.Range(-Water.nodeDiameter/4, Water.nodeDiameter/4);
                r = true;
                yield return new WaitForSeconds(Random.Range(0.5f, 2));                
            }
            else
            {
                amount = -transform.position.y;
                r = false;
                yield return new WaitForSeconds(Random.Range(0.5f, 2f));                
            }
        }
    }
}
