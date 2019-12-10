using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [SerializeField]
    private List<FX> fXes;
    private Dictionary<string, ParticleSystem> dictFx = new Dictionary<string, ParticleSystem>();

    private delegate void FXdelegate(string name ,Vector3 pos);
    private static FXdelegate fxDelegate;
    private delegate void StopFx();
    private static StopFx stopFx;


    [System.Serializable]
    public class FX
    {
        public string name;
        public ParticleSystem fx;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in fXes)
        {
            dictFx.Add(item.name, item.fx);
        }
        fxDelegate = StartFx;
        stopFx = StopFX;
    }

    public static void CallDelegate(string name, Vector3 pos)
    {
        if (fxDelegate != null)
            fxDelegate(name, pos);
    }

    public static void StopDelegate()
    {
        if (stopFx != null)
            stopFx();
    }
 
    private void StartFx(string name, Vector3 pos)
    {
        ParticleSystem particle = dictFx[name];
        particle.transform.position = pos;
        particle.Play();
    }

    private void StopFX()
    {
        dictFx["HealSomeone"].Stop();
        dictFx["OnHeal"].Stop();

    }

}
