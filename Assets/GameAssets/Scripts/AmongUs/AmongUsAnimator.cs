using UnityEngine;

public class AmongUsAnimator : MonoBehaviour
{
    public Animator animator;
    private AmongUs player;

    void Start()
    {
        player = GetComponent<AmongUs>();
    }

    private void LateUpdate()
    {
        var state = player.states.index;
        var horizontalSpeed = player.horizontalVelocity.magnitude;
        var verticalSpeed = player.verticalVelocity.y;

        animator.SetInteger("State", state);
        animator.SetFloat("Horizontal Speed", horizontalSpeed);
        animator.SetFloat("Vertical Speed", verticalSpeed);
    }
}
