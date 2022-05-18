using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Script para regresar el valor de NumeroLetra a cero para que no este en 
* loop con la misma animacion.
*/

public class ReserNumLetra : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("NumeroLetra", 0);
    }
}
