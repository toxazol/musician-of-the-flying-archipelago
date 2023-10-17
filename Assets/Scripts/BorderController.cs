using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other) {
 
        if(other.gameObject.TryGetComponent(out IFallable fallable))
        {
            fallable.OnFall(GetComponent<EdgeCollider2D>());
        }
    }
}
