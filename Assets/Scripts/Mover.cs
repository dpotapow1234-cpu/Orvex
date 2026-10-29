using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 5f;
    public float rayDistance = 1.0f;
    public LayerMask collisionMask;    // Маска слоев, с которыми нужно сталкиваться
    public Server.TickRate serverT;

    public void Start()
    {
        serverT.OnTick += Move;

    }

    public void OnDestroy()
    {
        serverT.OnTick -= Move;
    }


    public void Move()
    {
        float moveHorizontal = Input.
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // Выпускаем луч перед объектом
        if (Physics.Raycast(transform.position, movement, out RaycastHit hit, rayDistance, collisionMask))
        {
            // Если луч во что-то уперся, останавливаемся на дистанции до точки удара (минус небольшой отступ)
            float distanceToObstacle = hit.distance;
            if (distanceToObstacle > 0.1f)
            {
                transform.position += movement * (distanceToObstacle - 0.05f);
            }
        }
        else
        {
            // Если впереди чисто — двигаемся дальше
            transform.position += movement * speed * Time.deltaTime;
        }
    }
}
