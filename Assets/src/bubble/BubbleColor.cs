using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleColor : MonoBehaviour
{

    [System.Serializable]
    private struct ColorByHp
    {
        public Color primary;
        public Color light;
        public Color dark;
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

    private void OnHealthChanged(float hp) {
        foreach (ColorByHp colorByHp in colorsByHp) {
            if (hp >= colorByHp.hp_required) {
                MaterialPropertyBlock properties = new MaterialPropertyBlock();
                sprite.GetPropertyBlock(properties);
                properties.SetColor("_Primary", colorByHp.primary);
                properties.SetColor("_Light", colorByHp.light);
                properties.SetColor("_Dark", colorByHp.dark);
                sprite.SetPropertyBlock(properties);
                break;
            }
        }
        if (hp <= 0) {
            animator.SetBool("Dead", true);
        }
    }

    void Awake()
    {
        bubble.OnHealthChanged.AddListener(OnHealthChanged);
        // Sort color in descending color based on hp_required
        colorsByHp.Sort((a, b) => b.hp_required.CompareTo(a.hp_required));
    }
}
