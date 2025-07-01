using UnityEngine;

namespace Interfaces
{
    public interface IDashable
    {
        bool IsDashing { get; }
        void DashDirection(Vector3 direction);
    }
}
