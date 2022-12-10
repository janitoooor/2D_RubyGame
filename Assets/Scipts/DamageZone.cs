using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    //Эта функция вызывается каждый кадр, в котором Rigidbody находится внутри триггера ,
    //а не только один раз, когда он входит.
    private void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if(controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
