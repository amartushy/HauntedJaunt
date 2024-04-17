using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {

    public bool isAnimated = false;

    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    public float floatSpeed;
    public float floatRate;
    private float baseHeight;  // Add base height variable

    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;

    public ParticleSystem explosionEffect;

    public AudioSource GemSFX;
    public AudioSource proximitySFX;

    public Transform player; 
    public float maxHearDistance = 5f;
    public float minVolume = 0f;
    public float maxVolume = 1f;

    void Start ()
    {
        baseHeight = isFloating ? transform.position.y : 0;
        proximitySFX.volume = minVolume; 
        proximitySFX.loop = true;  // Make sure the sound loops
        proximitySFX.Play();  // Start playing the sound
    }
    
    void Update ()
    {
        if (isAnimated)
        {
            HandleAnimations();
        }

        AdjustProximitySoundVolume();
    }

    void HandleAnimations() //Written by Author of the Gem Objects, adjusted for use in CS410
    {
        if (isRotating)
        {
            transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
        }

        if (isFloating)
        {
            float newY = baseHeight + Mathf.Sin(Time.time * floatSpeed) * floatRate;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        if (isScaling)
        {
            scaleTimer += Time.deltaTime;
            if (scalingUp)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
            }
            if (scaleTimer >= scaleRate)
            {
                scalingUp = !scalingUp;
                scaleTimer = 0;
            }
        }
    }

    //Written by CS410 group
    void AdjustProximitySoundVolume()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > maxHearDistance) {
            proximitySFX.volume = minVolume; // Ensures that the sound is fully off beyond max distance
        } else {
            // This approach uses a linear interpolation from max volume at zero distance to min volume at max distance
            float volume = 1 - (distance / maxHearDistance);
            proximitySFX.volume = Mathf.Lerp(minVolume, maxVolume, volume);
        }
        Debug.Log($"Volume: {proximitySFX.volume}, Distance: {distance}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            GemSFX.Play();

            //Attempting to destroy the gem game object without destroying the particle effects:
            ParticleSystem tempEffect = Instantiate(explosionEffect, explosionEffect.transform.position, Quaternion.identity);
            tempEffect.Play();
            tempEffect.transform.SetParent(null);

            Destroy(tempEffect.gameObject, tempEffect.main.duration);

            Destroy(gameObject); 
        }
    }
}

