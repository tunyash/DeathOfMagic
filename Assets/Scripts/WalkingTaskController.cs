using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingTaskController : MonoBehaviour
{
    [SerializeField] private WalkingTask _task;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _task.PositionTo = position;
        }

        if (_task.IsFinished)
        {
            //Debug.Log("F");
        }
    }
}
