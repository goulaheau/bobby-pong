using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    public Text Score1;
    public Text Score2;
    public Text Winner;
    public Text Helper;
    public TrailRenderer Trail;
    public ParticleSystem Goal1ParticleSystem;
    public ParticleSystem Goal2ParticleSystem;
    public AudioSource HitSound;

    public static bool Finished = true;

    private Rigidbody _rigidbody;
    private float _speed = 15f;

    private void Start()
    {
        Helper.enabled = true;
        InvokeRepeating("SpeedUpWhenFinished", 0f, 0.1f);
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = RandomVelocity;
    }

    private void Update()
    {
        HandleLaunchNewGame();
    }

    private void FixedUpdate()
    {
        ControlBallSpeed();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Paddle":
                PaddleCollisionAction(other);
                break;
            case "Goal":
                GoalCollisionAction(other);
                break;
        }
    }

    private void SpeedUpWhenFinished()
    {
        if (!Finished) return;

        _speed += 0.5f;
    }

    private IEnumerator LaunchBall(bool isNewGame = false)
    {
        _rigidbody.velocity = new Vector3(0, 0, 0);

        if (!isNewGame)
        {
            yield return new WaitForSeconds(2);
        }

        Trail.Clear();
        _speed = 15f;
        transform.position = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(isNewGame ? 3 : 1);

        _rigidbody.velocity = RandomVelocity;
    }

    private void HandleLaunchNewGame()
    {
        if (!Finished || !Input.GetKey(KeyCode.Space)) return;

        Finished = false;
        Helper.enabled = false;
        Winner.text = "";
        Score1.text = "0";
        Score2.text = "0";
        StartCoroutine(LaunchBall(true));
    }

    private void ControlBallSpeed()
    {
        if (_rigidbody.velocity.sqrMagnitude <= 0f) return;

        _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
    }

    private void PaddleCollisionAction(Collision other)
    {
        if (!Finished)
        {
            HitSound.Play();
        }
        
        ChangeBallSpeed();

        ChangeBallDirection(other);        
    }

    private void ChangeBallSpeed()
    {
        if (Finished) return;
        
        _speed += 2.5f;
    }

    private void ChangeBallDirection(Collision other)
    {
        var contact = other.contacts[0].point;
        var center = other.gameObject.GetComponent<Collider>().bounds.center;

        var contactOnSurface = false;
        if (Math.Abs(contact.x - 19.5f) < 0.001)
        {
            contactOnSurface = true;
            center.x += 2.5f;
        }
        else if (Math.Abs(contact.x + 19.5f) < 0.001)
        {
            contactOnSurface = true;
            center.x -= 2.5f;
        }

        if (!contactOnSurface) return;

        var direction = Vector3.Normalize(contact - center);
        _rigidbody.velocity = direction * _speed;
    }

    private void GoalCollisionAction(Collision other)
    {
        if (Finished) return;

        switch (other.gameObject.name)
        {
            case "Goal 1":
                Score2.text = (int.Parse(Score2.text) + 1).ToString();
                StartCoroutine(PlayGoalParticles(Goal1ParticleSystem));
                CheckIfFinished(Score2, 2);
                break;
            case "Goal 2":
                Score1.text = (int.Parse(Score1.text) + 1).ToString();
                StartCoroutine(PlayGoalParticles(Goal2ParticleSystem));
                CheckIfFinished(Score1, 1);
                break;
        }
    }

    private static IEnumerator PlayGoalParticles(ParticleSystem goalParticleSystem)
    {
        goalParticleSystem.Play();

        yield return new WaitForSeconds(1.5f);

        goalParticleSystem.Stop();
    }

    private void CheckIfFinished(Text score, int playerId)
    {
        if (int.Parse(score.text) >= 5)
        {
            _speed = 5f;
            Winner.text = "Winner: Player " + playerId;
            Helper.enabled = true;
            Finished = true;
        }
        else
        {
            StartCoroutine(LaunchBall());
        }
    }
    
    private Vector3 RandomVelocity
    {
        get
        {
            return new Vector3(
                       Random.Range(-1f, 1f) > 0 ? 1f : -1f,
                       Random.Range(-1f, 1f)
                   ) * _speed;
        }
    }
}