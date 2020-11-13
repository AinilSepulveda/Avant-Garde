using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedDebug : MonoBehaviour, IAttackable
{
    public Scrollingtxt text;
    public Color textColor;
    public Color textColorCritical;
    //AttackedTakeDamage
  /*  private CharacterStats stats;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    } */

    public void OnAttack(GameObject attacker, Attack attack)
    {
        //info Attack
        var textstring = attack.Damage.ToString();

        var scrollingText = Instantiate(text, transform.position, Quaternion.identity);
        scrollingText.SetText(textstring);

        scrollingText.SetColor(textColor);



        if (attack.IsCritical)
        {
            scrollingText.SetColor(textColorCritical);
            Debug.Log("CRITICAAAAL");
        }

        Debug.LogFormat("{0} attacked {1} for {2} damage.", attacker.name, name, attack.Damage);

        //AttackedTakeDamageç
      /*  stats.TakeDamage(attack.Damage);

        if (stats.Getheath() <= 0)
        {
            //Destroy
        } */
    }
}
