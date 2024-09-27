using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    
    protected PlayerWeaponController _weaponController;
    protected MeshRenderer mesh;
    [SerializeField] private Material highlightMaterial;
    protected Material _defaultMaterial;


    protected virtual void Start()
    {
        if (mesh == null)
        {
            mesh = GetComponentInChildren<MeshRenderer>();
        }

        _defaultMaterial = mesh.sharedMaterial;
    }

    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mesh = newMesh;
        _defaultMaterial = newMesh.sharedMaterial;
    }
    
    public void HighlightActive(bool active)
    {
        mesh.material = active ? highlightMaterial : _defaultMaterial;
    }

    public virtual void Interaction()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_weaponController == null)
            _weaponController = other.GetComponent<PlayerWeaponController>();
        
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null)
            return;
        
        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateClosestIntractable();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null)
            return;
        
        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateClosestIntractable();
    }
}
