using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    private bool isMoving = false;

    private Vector3 moveDirection;
    private Vector3 mousePosition;

    private void Update()
    {
        RotatePlayerToCursor();
        Movement();
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, y, 0).normalized;
        isMoving = moveDirection == Vector3.zero ? false : true;

        if (!isMoving)
            return;

        transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.World);
    }

    private void RotatePlayerToCursor()
    {
        transform.up = GetDirectionToCursor(transform);
    }

    private Vector3 GetDirectionToCursor(Transform target)
    {

        // To make sure the object always rotates towards the mouse cursor correctly, regardless of the object's position,
        // you need to calculate the direction after converting both the mouse position and the object's position to world space.

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        Vector3 dir = (mouseWorldPos - target.position).normalized;

        float visualIndicatorDist = 5f;
        Debug.DrawRay(target.position, dir * visualIndicatorDist, Color.red);

        return dir;
    }
}