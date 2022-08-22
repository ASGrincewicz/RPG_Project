using System;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class Pickup : MonoBehaviour
    {
        protected abstract void OnTriggerEnter(Collider other);

    }
}