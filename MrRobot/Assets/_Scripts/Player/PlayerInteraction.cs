using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> _interactables = new List<Interactable>();

    private Interactable _closestIntractable;


    private void Start()
    {
        Player player = GetComponent<Player>();

        player.Controls.Character.Interaction.performed += context => InteractWithClosest();
    }
    
    

    public void UpdateClosestIntractable()
    {
        _closestIntractable?.HighlightActive(false);
        _closestIntractable = null;

        float closestDistance = float.MaxValue;

        foreach (Interactable intractable in _interactables)
        {
            float distance = Vector3.Distance(transform.position, intractable.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                _closestIntractable = intractable;
            }
        }
        
        _closestIntractable?.HighlightActive(true);
    }

    private void InteractWithClosest()
    {
        _closestIntractable?.Interaction();
        _interactables.Remove(_closestIntractable);
        
        UpdateClosestIntractable();
    }

    public List<Interactable> GetInteractables()
    {
        return _interactables;
    }
    
}
