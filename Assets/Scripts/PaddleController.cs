using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public int Id = 1;
    public float Speed = 25f;

    private Rigidbody _rigidbody;
    private KeyCode _keyUp;
    private KeyCode _keyDown;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _keyUp = Id == 1 ? KeyCode.W : KeyCode.UpArrow;
        _keyDown = Id == 1 ? KeyCode.S : KeyCode.DownArrow;
    }

    private void Update()
    {
        if (BallController.Finished)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
         
        _rigidbody.velocity = !Input.GetKey(_keyUp) && !Input.GetKey(_keyDown)
            ? Vector3.zero
            : Input.GetKey(_keyUp)
                ? Vector3.up * Speed
                : Vector3.down * Speed;
    }
}