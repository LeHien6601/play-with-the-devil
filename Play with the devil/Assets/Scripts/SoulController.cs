using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    [SerializeField] private GameObject soulPrefab;
    private List<GameObject> souls;
    static public SoulController instance;
    private int currentNumberOfSouls = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void GenerateSouls(int numberOfSouls)
    {
        souls = new List<GameObject>();
        currentNumberOfSouls = numberOfSouls;
        for (int i = 0; i < numberOfSouls; i++)
        {
            souls.Add(Instantiate(soulPrefab, transform));
        }
    }
    public int NumberOfSouls() { return currentNumberOfSouls; }
    public void LoseOneSoul()
    {
        if (currentNumberOfSouls == 0) { return; }
        currentNumberOfSouls--;
        souls[currentNumberOfSouls].GetComponent<Animator>().SetTrigger("Fade");
    }
}
