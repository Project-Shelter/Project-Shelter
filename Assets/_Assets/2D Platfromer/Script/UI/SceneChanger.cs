using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string Scane1;
    public string Scane2;
    public string Scane3;
    public string Scane4;
    public string Scane5;
    public string Scane6;
    public string Scane7;
    public string Scane8;

    public void Scene1()
    {
        SceneManager.LoadScene(Scane1);
    }
    public void Scene2()
    {
        SceneManager.LoadScene(Scane2);
    }
    public void Scene3()
    {
        SceneManager.LoadScene(Scane3);
    }
    public void Scene4()
    {
        SceneManager.LoadScene(Scane4);
    }
    public void Scene5()
    {
        SceneManager.LoadScene(Scane5);
    }
    public void Scene6()
    {
        SceneManager.LoadScene(Scane6);
    }
    public void Scene7()
    {
        SceneManager.LoadScene(Scane7);
    }
    public void Scene8()
    {
        SceneManager.LoadScene(Scane8);
    }

}
