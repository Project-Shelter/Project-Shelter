using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemContainer
{
    public class ContainerInjector : MonoBehaviour
    {
        private static List<ContainerModel> _containers = new List<ContainerModel>();

        public static int countContainer = 3;
        public static void ContainerInit()
        {
            _containers.Clear();
            for (int i = 0; i < countContainer; i++)
            {
                _containers.Add(new ContainerModel(i));
            }
        }

        public static ContainerModel InjectContainer(int num)
        {
            if (num > 2) num = 2;
            return _containers[num];
        }
    }
}