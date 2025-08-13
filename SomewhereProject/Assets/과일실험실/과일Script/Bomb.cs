using UnityEngine;

public class Bomb : MonoBehaviour
{
    private ParticleSystem bombParticle;
    private void Awake()
    {
        bombParticle = GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bombParticle.Play();
            FindAnyObjectByType<GameManagerFr>().Explode();
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
