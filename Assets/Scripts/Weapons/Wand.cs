using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [SerializeField] List<Spell> spells;
    [SerializeField] GameObject wandTipTransform;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Cast(0);
        }
    }

    public void Cast(int spellNum)
    {
        Spell spell = Instantiate(spells[(int)spellNum], wandTipTransform.transform.position, wandTipTransform.transform.rotation);
        spell.transform.forward = wandTipTransform.transform.forward;
    }
}
