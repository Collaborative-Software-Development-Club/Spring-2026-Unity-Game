using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    
    [SerializeField] private Animator animator;
    
    [SerializeField] private int speed = 5;

    private Vector2 direction;
    private Rigidbody2D rb;

    //public Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }
    public void OnMovement(InputAction.CallbackContext ctxt)
    {
        direction = ctxt.ReadValue<Vector2>();

        if (direction.x != 0 || direction.y != 0)
        {
            animator.SetFloat(X, direction.x);
            animator.SetFloat(Y, direction.y);
            animator.SetBool(IsWalking, true);
        }
        else
        {
            animator.SetBool(IsWalking, false);
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * (speed * Time.fixedDeltaTime));
    }

}