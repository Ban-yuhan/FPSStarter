using UnityEngine;

public class ZombieAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool move = false;

    private void Update()
    {
        CheckTestInput();
    }

    void CheckTestInput()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            move = !move;
        }
        animator.SetBool("Move", move);

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("Death");
        }
    }


}
