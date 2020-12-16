using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    static public GameManager me;
    public int money = 0;
    public int mentalHealth = 100;
    public int physicalHealth = 100;
    public int filmNum = 0;
    public int pills = 0;
    public int gR = 0;
    public int foodNum = 0;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI filmText;
    public TextMeshProUGUI mH;
    public TextMeshProUGUI pH;
    public TextMeshProUGUI pillNum;
    public TextMeshProUGUI ghostRelationship;
    public TextMeshProUGUI foodAmount;

    public int flimPrice = 30;
    public int pillPrice = 20;
    public int foodPrice = 15;

    public int pHDecreaseAmount = 15; // sleep
    public int pHIncreaseAmount = 10; // food
    public int mHDecreaseAmount = 15; // pill
    public int mHIncreaseAmount = 10; // sleep
    public int grIncreaseAmount = 5; // sleep

    public int state = 0;
    public int tutorial = 0;
    public int game = 1;
    public int goCrazy = 2;
    public int die = 3;

    public float pHNaturalDescreaseInterval;
    private float pHND_timer = 0;
    public float mHNaturalIncreaseInterval;
    private float mHNI_timer = 0;

    public GameObject crazyText;
    public GameObject deadText;
    
    void Start()
    {
        me = this;
        pHND_timer = pHNaturalDescreaseInterval;
        mHNI_timer = mHNaturalIncreaseInterval;
    }

    void Update()
    {
        mentalHealth = Mathf.Min(mentalHealth, 100);
        mentalHealth = Mathf.Max(mentalHealth, 0);
        physicalHealth = Mathf.Min(physicalHealth, 100);
        physicalHealth = Mathf.Max(physicalHealth, 0);
        gR = Mathf.Min(gR, 100);
        gR = Mathf.Max(gR, 0);
        moneyText.text = money.ToString();
        filmText.text = filmNum.ToString();
        mH.text = mentalHealth.ToString();
        pH.text = physicalHealth.ToString();
        pillNum.text = pills.ToString();
        ghostRelationship.text = gR.ToString();
        foodAmount.text = foodNum.ToString();

        if (state == game)
		{
            if (pHND_timer > 0)
            {
                pHND_timer -= Time.deltaTime;
            }
            else
            {
                physicalHealth--;
                pHND_timer = pHNaturalDescreaseInterval;
            }

            //if (mHNI_timer > 0)
            //{
            //    mHNI_timer -= Time.deltaTime;
            //}
            //else
            //{
            //    mentalHealth++;
            //    mHNI_timer = mHNaturalIncreaseInterval;
            //}
            
            if (mentalHealth <= 0)
			{
                state = goCrazy;
			}
            if (physicalHealth <= 0)
			{
                state = die;
			}
        }
        else if (state == goCrazy)
		{
            crazyText.SetActive(true);
		}
        else if (state == die)
		{
            deadText.SetActive(true);
		}
    }
}
