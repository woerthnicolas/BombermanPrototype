using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoPickableItem", menuName = "Assets/Item", order = 1)]
public class SoPickableItem : ScriptableObject, IPickableItem
{
    public Sprite icon;
    public ItemPickup.ItemType itemType;

    private delegate void Effect(PlayerController playerController);

    private Effect _effect;

    public void Activate(PlayerController playerController)
    {
        switch (itemType)
        {
            case ItemPickup.ItemType.ExtraBomb:
                _effect = controller => controller.AddBomb();
                break;
            case ItemPickup.ItemType.BlastRadius:
                _effect = controller => controller.AddExplosionRadius();
                break;
            case ItemPickup.ItemType.SpeedIncrease:
                _effect = controller => controller.AddSpeed();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        _effect?.Invoke(playerController);
    }
}