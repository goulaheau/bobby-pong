using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public int Id = 1;
    public float Speed = 25f;

    private Rigidbody _rigidbody;
    private KeyCode _keyUpEn;
    private KeyCode _keyUpFr;
    private KeyCode _keyDown;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _keyUpEn = Id == 1 ? KeyCode.W : KeyCode.UpArrow;
        _keyUpFr = Id == 1 ? KeyCode.Z : KeyCode.UpArrow;
        _keyDown = Id == 1 ? KeyCode.S : KeyCode.DownArrow;
    }

    private void Update()
    {
        if (BallController.Finished)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
         
        _rigidbody.velocity = !Input.GetKey(_keyUpEn) && 
                              !Input.GetKey(_keyUpFr) && 
                              !Input.GetKey(_keyDown)
            ? Vector3.zero
            : Input.GetKey(_keyDown)
                ? Vector3.down * Speed
                : Vector3.up * Speed;
    }
}