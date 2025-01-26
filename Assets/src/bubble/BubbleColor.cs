using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BubbleColorPalette
{
    public Color primary;
    public Color light;
    public Color dark;
}

public class BubbleColor : MonoBehaviour
{
    [System.Serializable]
    private struct ColorByHp
    {
        public BubbleColorPalette palette;
        public int hp_required;
    }

    [SerializeField]
    private List<ColorByHp> colorsByHp;

    [SerializeField]
    private BubbleHittable bubble;
    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private Animator animator;

    private bool colorOverrided = false;

    private void OnHealthChanged(float hp)
    {
        if (colorOverrided)
        {
            return;
        }
        foreach (ColorByHp colorByHp in colorsByHp)
        {
            if (hp >= colorByHp.hp_required)
            {
                SetBubbleColor(colorByHp.palette);
                break;
            }
        }
        if (hp <= 0)
        {
            animator.SetBool("Dead", true);
        }
    }

    private void SetBubbleColor(BubbleColorPalette palette)
    {
        MaterialPropertyBlock properties = new MaterialPropertyBlock();
        sprite.GetPropertyBlock(properties);
        properties.SetColor("_Primary", palette.primary);
        properties.SetColor("_Light", palette.light);
        properties.SetColor("_Dark", palette.dark);
        sprite.SetPropertyBlock(properties);
    }

    public void OverrideBubbleColors(BubbleColorPalette palette)
    {
        colorOverrided = true;
        SetBubbleColor(palette);
    }

    void Awake()
    {
        bubble.onHealthChanged.AddListener(OnHealthChanged);
        // Sort color in descending color based on hp_required
        colorsByHp.Sort((a, b) => b.hp_required.CompareTo(a.hp_required));
    }
}
