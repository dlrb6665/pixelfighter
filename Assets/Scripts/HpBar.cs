using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Text hpText;
    public static int HpCurrent;
    public static int HpMax;

    private Image hpBar;

    // Start is called before the first frame update
    void Start()
    {
        hpBar = GetComponent<Image>();
        //HpCurrent = HpMax;
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = (float)HpCurrent / (float)HpMax;
        hpText.text = HpCurrent.ToString() + "/" + HpMax.ToString();
    }
}
