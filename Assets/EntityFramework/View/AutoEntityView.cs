using System;
using UnityEngine;

namespace EntityFramework
{
    public class AutoEntityView : EntityView
    {
        private void Start()
        {
            Link(EntityManager.Instance, EntityManager.Instance.CreateEntity());
        }
    }
}