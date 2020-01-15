using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
public class QTEScript : MonoBehaviour
{
    private Carriage _carriage;
    public int point;
    public int amount;
    public int goal;
    public float timeBetweenSpawn;
    public float eyeDuration;
    private float timer;

    private bool isActive;
    private List<Eye> _eyes = new List<Eye>();

    private static bool firstEye = true;
    private bool isTimerstop = false;

    void Start()
    {
        _carriage = GetComponentInParent<Carriage>();
        _eyes = HierarchyUtils.GetComponentsInDirectChildren<Eye>(this.transform,false);
        isActive = false;
        point = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && !isTimerstop)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenSpawn)
            {
                if (amount > 0)
                {
                    SpawnEye();
                    timer = 0;
                }
                else if(timer >= timeBetweenSpawn + eyeDuration)
                    CheckEnd();

            }
        }
    }

    public void SpawnEye()
    {
        int rand = Random.Range(0, _eyes.Count);
        if (!_eyes[rand].isOpen)
        {

            if (firstEye)
            {
                _eyes[3].SpawnEye(-1);
                isTimerstop = true;
                firstEye = false;
                timer = timeBetweenSpawn;
            }
            else
            {
                _eyes[rand].SpawnEye(eyeDuration);
            }
            amount--;
        }
        else
        {
            SpawnEye();
        }
    }

    public void Launch(int amountToSpawn, int neededPoints, float timeBetweenEye, float eyeLifeDuration)
    {
        amount = amountToSpawn;
        goal = neededPoints;
        timeBetweenSpawn = timeBetweenEye;
        eyeDuration = eyeLifeDuration;
        point = 0;
        isActive = true;
    }
    public void CheckEnd()
    {
        if (!isActive) return;
        if (isTimerstop) isTimerstop = false;
        if (point >= goal)
        {
            if(isActive)
                _carriage.Victory();
            isActive = false;
            if (SoundManager.Instance.isPlaying("fight"))
            {
                SoundManager.Instance.StopSound("fight");
            }
        }
        if (amount <= 0)
        {
            _carriage.Defeat();
            isActive = false;
            if (SoundManager.Instance.isPlaying("fight"))
            {
                SoundManager.Instance.StopSound("fight");
            }
        }
    }
}
