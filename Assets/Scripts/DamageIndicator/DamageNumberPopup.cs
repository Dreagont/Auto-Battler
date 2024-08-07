using TMPro;
using UnityEngine;

public class DamageNumberPopup : MonoBehaviour
{
    private TMP_Text damageValue;
    private Rigidbody2D rigidbodyBro;

    public float InitialYVelocity = 7f;
    public float InitialXVelocityRange = 3f;
    public float lifeTime = 0.8f;

    

    private void Start()
    {
        damageValue = GetComponent<TMP_Text>();
        rigidbodyBro = GetComponent<Rigidbody2D>();
        rigidbodyBro.velocity = new Vector2(Random.Range(-InitialXVelocityRange, InitialXVelocityRange), InitialYVelocity);
        Destroy(gameObject, lifeTime);
    }

}