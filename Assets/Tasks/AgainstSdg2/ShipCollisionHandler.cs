using UnityEngine;
using System.Collections;

public class ShipCollisionHandler : MonoBehaviour
{
    public float fadeDuration = 1f; // Time for the fade-out effect
    public GameObject explosionEffect; // Optional particle effect prefab

    public IEnumerator SmoothDestroy()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Transform shipTransform = transform;

        if (spriteRenderer != null)
        {
            float elapsedTime = 0f;
            Color originalColor = spriteRenderer.color;
            Vector3 originalScale = shipTransform.localScale;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / fadeDuration;

                // Fade out
                float alpha = Mathf.Lerp(1f, 0f, progress);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

                // Scale down
                shipTransform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);

                yield return null;
            }
        }

        // Trigger optional explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Destroy the ship object
        Destroy(gameObject);
    }
}
