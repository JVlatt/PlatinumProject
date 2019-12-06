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

    void Start()
    {
        _carriage = GetComponentInParent<Carriage>();
        _eyes = HierarchyUtils.GetComponentInDirectChildren<Eye>(this.transform,false);
        isActive = false;
        point = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
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
            _eyes[rand].Spawn(eyeDuration);
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
        if (point >= goal)
        {
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
