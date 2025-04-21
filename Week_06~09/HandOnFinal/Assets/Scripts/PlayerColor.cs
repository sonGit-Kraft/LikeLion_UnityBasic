using System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PlayerColor : NetworkBehaviour
{
    
    private readonly NetworkVariable<Color> _networkColor = new NetworkVariable<Color>(Color.red);
    public static readonly Color[] SColors = { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow };

    public override void OnNetworkSpawn()
    {
        _networkColor.OnValueChanged += OnColorChanged;
        
        if(HasAuthority)
            _networkColor.Value = SColors[(int)(NetworkObject.OwnerClientId % Convert.ToUInt64(SColors.Length)-1)];
        UpdateMaterialColor(_networkColor.Value);
    }

    public override void OnNetworkDespawn()
    {
        _networkColor.OnValueChanged -= OnColorChanged;
    }
    
    private void OnColorChanged(Color oldColor, Color newColor)
    {
        UpdateMaterialColor(newColor);
    }
    
    private void UpdateMaterialColor(Color newColor)
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material.color = newColor;
        }
    }
    
}
