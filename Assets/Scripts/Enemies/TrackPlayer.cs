using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    Rigidbody2D rb2d;
    GameObject player;

    [Header("Options")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxDistanceTrack = 10f;
    [SerializeField] bool isBoss = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        RotateSprite();

        if (!IsGrounded() && !isBoss) return;

        float distanceX = player.transform.position.x - transform.position.x;


        Vector2 distance = new Vector2(distanceX, 0f);
        distance = distance.normalized;

        if (Mathf.Abs(distanceX) > maxDistanceTrack) return;

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), moveSpeed * Time.deltaTime);
    }

    void RotateSprite()
    {
        float distance = player.transform.position.x - transform.position.x;


        Vector3 enemyLocalScale = transform.localScale;
        if (distance < 0f)
        {
            enemyLocalScale.x = (transform.localScale.x > 0 ? -1 : 1) * transform.localScale.x;
        }
        else if (distance > 0f)
        {
            enemyLocalScale.x = (transform.localScale.x > 0 ? 1 : -1) * transform.localScale.x;
        }
        transform.localScale = enemyLocalScale;
    }

    public bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }
}
