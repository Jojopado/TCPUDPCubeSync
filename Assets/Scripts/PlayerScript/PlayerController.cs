using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CharacterController characterController;
    [SerializeField] private SkinnedMeshRenderer[] _visuals;
    [SerializeField] private Camera cam;

    [SerializeField] private Animator _animator;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;



    private bool walking { get; set; }
    private bool running { get; set; }
    private bool eating { get; set; }

    void Start()
    {
        //本地玩家
        /*if (Object.HasInputAuthority)
        {
            cam.enabled = true;
            foreach (var visual in _visuals)
            {
                visual.enabled = false;

            }
        //其他非本地玩家
        else
            {
                cam.enabled = false;
                cam.GetComponent<AudioListener>().enabled = false;
            }
        }*/
    }
    void Update()
    {
        /*if (GetInput(out NetInputCollector data))
        {
            var buttonPressed = data.Button.GetPressed(previousButton);
            previousButton = data.Button;

            var moveInput = new Vector3(data.moveInput.x, 0, data.moveInput.y);
            Vector3 movement =
            Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * new Vector3(moveInput.x * moveSpeed * Time.deltaTime, 0, moveInput.z * moveSpeed * Time.deltaTime);
            networkCharacterController.Move(movement * Time.deltaTime);

            HandlePitchYaw(data);

            transform.rotation = Quaternion.Euler(0, (float)yaw, 0);
            var cameraEuler = cam.transform.rotation.eulerAngles;
            cam.transform.rotation = Quaternion.Euler((float)pitch, cameraEuler.y, cameraEuler.z);
        }*/

        if (running)
        {
            //characterController. = 10f;
        }
        else
        {
            //characterController.maxSpeed = 2;
        }
        
    }
    void BoolControl()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            eating = true;
        }
        else
        {
            eating = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            running = true;
        }
        else
        {
            running = false;
        }
    }
    void AnimatorControl()
    {
        if (eating)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("07_Eat_Cat_Copy"))
            {

                _animator.SetBool("Eat", true);
            }
        }
        else
        {
            _animator.SetBool("Eat", false);
            if (walking)
            {
                _animator.SetBool("Walk", true);
            }
            else
            {
                _animator.SetBool("Walk", false);
            }
        }
    }
}
