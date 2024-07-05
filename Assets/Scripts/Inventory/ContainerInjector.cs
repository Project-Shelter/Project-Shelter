using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemContainer
{
    public enum Container
    {
        Inventory = 0,
        InvenBar = 1,
        Chest = 2,
    }
    public class ContainerInjector : MonoBehaviour
    {
        private static List<ContainerController> _containers = new List<ContainerController>();

        public void Start()
        {
            for (int i = 0; i < 2; i++)
            {
                _containers.Add(new ContainerController(i));
            }
        }

        public static ContainerController InjectContainer(int num)
        {
            return _containers[num];
        }
    }
}