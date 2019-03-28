using UnityEngine;

namespace Analytics
{
    public abstract class AbstractLastScreenTracker : MonoBehaviour
    {
        public abstract string lastScreen { get; }
    }
}
