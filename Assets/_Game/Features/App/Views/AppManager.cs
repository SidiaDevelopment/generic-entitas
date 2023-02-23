using System;
using EntityFramework;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private void Start()
    {
        var systems = new TestFeature(EntityManager.Instance);
        EntityManager.Instance.Initialize(systems);
    }

    private void Update()
    {
        EntityManager.Instance.Execute();
    }

    private void OnApplicationQuit()
    {
        EntityManager.Instance.Teardown();
    }
}