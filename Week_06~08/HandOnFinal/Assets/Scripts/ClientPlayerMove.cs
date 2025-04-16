using Cinemachine;
using Unity.Netcode; 
using StarterAssets; 
using UnityEngine;
using UnityEngine.InputSystem; 
public class ClientPlayerMove: NetworkBehaviour
{
    [SerializeField]
    CharacterController m_CharacterController; 
    [SerializeField]
    ThirdPersonController m_ThirdPersonController; 
    [SerializeField]
    PlayerInput m_PlayerInput;
    [SerializeField] 
    Transform m_CameraFollow; 

    private void Awake()
    {
        m_PlayerInput.enabled = false; 
        m_ThirdPersonController.enabled = false; 
        m_CharacterController.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // enabled = IsClient; // Enable if this is a client. 
        if (!HasAuthority)
        {
            // Disable if this is not the owner 
            enabled = false; 
            m_PlayerInput.enabled = false;
            m_CharacterController.enabled = false; 
            m_ThirdPersonController.enabled = false; 
            return;
        }

        // Enable if this is an owner 
        m_PlayerInput.enabled = true; 
        m_CharacterController.enabled = true; 
        m_ThirdPersonController.enabled = true;

        var cinemachine = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        cinemachine.Follow = m_CameraFollow;

    }
}