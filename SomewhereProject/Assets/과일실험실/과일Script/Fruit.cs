using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private ParticleSystem fruitParticle;
    private void Awake()
    {
        //fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        fruitParticle = GetComponentInParent<ParticleSystem>();
        
        fruitParticle.transform.SetParent(whole.transform);
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        Object.FindFirstObjectByType<GameManagerFr>().IncreaseScore();
        whole.SetActive(false);
        sliced.SetActive(true);

        fruitCollider.enabled = false;
        fruitParticle.Play();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody slice in slices)
        {
            //slice.angularVelocity = fruitRigidbody.angularVelocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
