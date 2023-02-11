using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPassive : MonoBehaviour
{
    public float CDSeconds;
    public float currentCD;
    public GameObject Passive;
    public GameObject SkillTimer;
    public AnimationCurve animCurve;
    public static bool InCooldown { get; private set; }

    void Start()
    {
        //put skills in CD
        InCooldown = true;
    }

    void Update()
    {
        if (InCooldown)
        {
            CDManager();
        }    
    }

    public void CDManager()
    {
        StartCoroutine(PassiveCD());
    }

    public IEnumerator PassiveCD()
    {
        StartCD();
        yield return new WaitUntil(() => currentCD >= 1);
        StopCD();
    }

    //start cooldown
    public void StartCD()
    {
        if (currentCD >= 1) {
            ResetCD();
        }
        
        currentCD += Time.deltaTime / CDSeconds;

        //update alpha
        var imageAlpha = Passive.GetComponent<Image>();
        var tempAlpha = imageAlpha.color;
        tempAlpha.a = animCurve.Evaluate(currentCD);
        imageAlpha.color = tempAlpha;

        //start cooldown
        InCooldown = true;
    }

    //stop cooldown
    public void StopCD()
    {
        currentCD = 1;

        //update alpha to zero
        var imageAlpha = Passive.GetComponent<Image>();
        var tempAlpha = imageAlpha.color;
        tempAlpha.a = 0;
        imageAlpha.color = tempAlpha;

        //stop cooldown
        InCooldown = false;
    }

    public void ResetCD()
    {
        currentCD = 0;
    }
}
