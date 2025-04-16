using System;
using Unity.Netcode;
using UnityEngine;

public class CubeBehaviour : NetworkBehaviour
{
    private NetworkVariable<Color> _networkColor = new NetworkVariable<Color>(Color.white);
    private void OnTriggerEnter(Collider other)
    {
        var otherNetworkObject = other.GetComponent<NetworkObject>();
        if (otherNetworkObject != null && otherNetworkObject.HasAuthority)
        {
            NetworkObject.ChangeOwnership(otherNetworkObject.OwnerClientId);
            _networkColor.Value = PlayerColor.SColors[(int)(NetworkObject.OwnerClientId % Convert.ToUInt64(PlayerColor.SColors.Length)-1)];
        }
    }
    
    public override void OnNetworkSpawn()
    {
        // 색상이 변경될 때 호출될 이벤트 핸들러 등록
        _networkColor.OnValueChanged += OnColorChanged;
        UpdateMaterialColor(_networkColor.Value);
    }
    
    public override void OnNetworkDespawn()
    {
        // 이벤트 핸들러 해제
        _networkColor.OnValueChanged -= OnColorChanged;
    }

    // 색상이 변경될 때 호출되는 콜백 함수
    private void OnColorChanged(Color oldColor, Color newColor)
    {
        UpdateMaterialColor(newColor);
    }

    private void UpdateMaterialColor(Color newColor)
    {
        // 오브젝트의 색상을 변경
        var meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material.color = newColor;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (HasAuthority)
        {
            transform.Rotate(Vector3.up,Time.deltaTime * 100f); 
        }
    }
}
