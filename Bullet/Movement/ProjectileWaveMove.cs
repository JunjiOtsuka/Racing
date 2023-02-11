using UnityEngine;

public class ProjectileWaveMove : IProjectileMovement
{
    Rigidbody rb;
    public Transform mTransform;
    public float speed;
    public float frequency;
    public float amplitude;
    public float timer;
    public GameObject leftProjectile;
    public GameObject rightProjectile;
    public string name;


    public ProjectileWaveMove(Rigidbody newRB, Transform newTransform)
    {
        this.rb = newRB;
        this.mTransform = newTransform;
    }

    public ProjectileWaveMove(Rigidbody newRB, Transform newTransform, float newSpeed, float newFrequency, float newAmplitude)
    {
        this.rb = newRB;
        this.mTransform = newTransform;
        this.speed = newSpeed;
        this.frequency = newFrequency;
        this.amplitude = newAmplitude;
        this.name = "5 parameter";
    }

    public void ProjectileMovement()
    {
        // rb.AddForce(mTransform.forward * speed, ForceMode.Force);

        timer += Time.deltaTime;
        // float x = Mathf.Sin(timer * frequency) * amplitude; 
        float x = Mathf.Cos(timer * frequency) * amplitude;
        float y = Mathf.Sin(timer * frequency) * amplitude;
        Vector3 wave = Vector3.Cross(new Vector3 (0f, -x, 0f), mTransform.forward);
        rb.GetComponent<Rigidbody>().AddForce(mTransform.forward * speed + wave * amplitude, ForceMode.Force);
        // leftProjectile.GetComponent<Rigidbody>().AddForce(mTransform.forward * speed + wave * amplitude, ForceMode.Force);
        // rb.AddForce(mTransform.forward * speed + wave * amplitude, ForceMode.Impulse);
    }
}

public class ProjectileWaveMove_Left : IProjectileMovement
{
    Rigidbody rb;
    public Transform mTransform;
    public float speed;
    public float frequency;
    public float amplitude;
    public float timer;
    public GameObject leftProjectile;
    public GameObject rightProjectile;
    public string name;


    public ProjectileWaveMove_Left(Rigidbody newRB, Transform newTransform)
    {
        this.rb = newRB;
        this.mTransform = newTransform;
    }

    public ProjectileWaveMove_Left(Rigidbody newRB, Transform newTransform, float newSpeed, float newFrequency, float newAmplitude)
    {
        this.rb = newRB;
        this.mTransform = newTransform;
        this.speed = newSpeed;
        this.frequency = newFrequency;
        this.amplitude = newAmplitude;
        this.name = "5 parameter";
    }

    public void ProjectileMovement()
    {
        timer += Time.deltaTime;
        // float x = Mathf.Sin(timer * frequency) * amplitude; 
        float x = Mathf.Cos(timer * frequency) * amplitude;
        float y = Mathf.Sin(timer * frequency) * amplitude;
        Vector3 wave = Vector3.Cross(new Vector3 (0f, -x, 0f), mTransform.forward);
        rb.GetComponent<Rigidbody>().AddForce(mTransform.forward * speed + wave * amplitude, ForceMode.Force);
        // leftProjectile.GetComponent<Rigidbody>().AddForce(mTransform.forward * speed + wave * amplitude, ForceMode.Force);
        // rb.AddForce(mTransform.forward * speed + wave * amplitude, ForceMode.Impulse);
    }
}

public class ProjectileWaveMove_Right : IProjectileMovement
{
    Rigidbody rb;
    public Transform mTransform;
    public float speed;
    public float frequency;
    public float amplitude;
    public float timer;
    public GameObject leftProjectile;
    public GameObject rightProjectile;
    public string name;


    public ProjectileWaveMove_Right(Rigidbody newRB, Transform newTransform)
    {
        this.rb = newRB;
        this.mTransform = newTransform;
    }

    public ProjectileWaveMove_Right(Rigidbody newRB, Transform newTransform, float newSpeed, float newFrequency, float newAmplitude)
    {
        this.rb = newRB;
        this.mTransform = newTransform;
        this.speed = newSpeed;
        this.frequency = newFrequency;
        this.amplitude = newAmplitude;
        this.name = "5 parameter";
    }

    public void ProjectileMovement()
    {
        Debug.Log("check right");
        timer += Time.deltaTime;
        // float x = Mathf.Sin(timer * frequency) * amplitude; 
        float x = Mathf.Cos(timer * frequency) * amplitude;
        float y = Mathf.Sin(timer * frequency) * amplitude;
        Vector3 wave = Vector3.Cross(new Vector3 (0f, x, 0f), mTransform.forward);
        rightProjectile.GetComponent<Rigidbody>().AddForce(mTransform.forward * speed + wave * amplitude, ForceMode.Force);
    }
}