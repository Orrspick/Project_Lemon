using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{ 
    public Transform PlayerTransform;
    private Transform m_Transform;

    // Start is called before the first frame update
    void Start()
    {
        m_Transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        m_Transform.position = PlayerTransform.position;

        m_Transform.position = PlayerTransform.position;
        m_Transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, -11); // 카메라 z축은 따라가지 않음
    }
}
