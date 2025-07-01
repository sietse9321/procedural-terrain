using Interfaces;
using UnityEngine;

namespace Components
{
    public class Movement : MonoBehaviour, IMovement
    {
        public float MoveSpeed { get; set; }

    
        /// <summary>
        /// Moves in the direction of the vector
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Vector3 direction)
        {
            if (direction.sqrMagnitude > 0)
            {
                Vector3 movement = direction.normalized * MoveSpeed * Time.deltaTime;
                transform.Translate(movement, Space.World);
            }
        }
    }
}