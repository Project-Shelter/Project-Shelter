using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemContainer
{
    public class ContainerInjector : MonoBehaviour
    {
        private static List<ContainerController> _containers = new List<ContainerController>();

        public static int countContainer = 3;
        public static void ContainerInit()
        {
            for (int i = 0; i < countContainer; i++)
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