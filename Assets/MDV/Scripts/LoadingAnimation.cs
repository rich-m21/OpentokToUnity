using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{

    [SerializeField] private GameObject loadingCircle;
    [SerializeField] private float step = 5f;
    private bool loading = false;
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        loading = true;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
		loading = false;
		loadingCircle.transform.rotation = Quaternion.Euler(0f,0f,0f);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (loading)
        {
            Vector3 rotation = loadingCircle.transform.rotation.eulerAngles;
            rotation += new Vector3(0f, 0f, -step * Time.deltaTime);
			loadingCircle.transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
