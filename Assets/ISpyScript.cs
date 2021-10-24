using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class ISpyScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;
    public ClickableIcon[] icons;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    private List<KMBombModule> AllMods = new List<KMBombModule>();
    private int[] counts;
    private int totalNumberOfIcons;
    private ClickableIcon[] usedIcons;

    void Awake () {
        moduleId = moduleIdCounter++;
        /*
        foreach (KMSelectable button in Buttons) 
            button.OnInteract += delegate () { ButtonPress(button); return false; };
        */

        //Button.OnInteract += delegate () { ButtonPress(); return false; };

    }

    void Start ()
    {
        string SN = Bomb.GetSerialNumber();
        AllMods = FindObjectsOfType<KMBombModule>().Where(module => module != this.Module).ToList()
            .Shuffle();
        if (AllMods.Count == 0)
        {
            moduleSolved = true;
            Debug.LogFormat("[I Spy #{0}] No other modules found; force-solving.", moduleId);
            Module.HandlePass();
        }
        else
        {
            totalNumberOfIcons = Rnd.Range(AllMods.Count / 6, AllMods.Count / 6 + 4);
            if (totalNumberOfIcons < 5)
                totalNumberOfIcons = 5;
            if (totalNumberOfIcons > 20)
                totalNumberOfIcons = 20;

            //counts = Partition(totalNumberOfIcons, 4);
            totalNumberOfIcons = 8;
            counts = new[] { 2, 2, 2, 2 };
            Debug.LogFormat("[I Spy #{0}] {1} icons total.", moduleId, totalNumberOfIcons);
            usedIcons = icons.Take(totalNumberOfIcons).ToArray();

            PlaceButtons();
        }

    }
    void PlaceIcon(int iconIx)
    {

    }
    void PlaceButtons()
    {
        for (int i = 0; i < totalNumberOfIcons; i++)
        {
            ClickableIcon icon = usedIcons[i];
            icon.gameObject.SetActive(true);
            icon.parentScript = this;
            icon.hostMod = AllMods[i % AllMods.Count];
            icon.SetUpHost();
            icon.PlaceSelf();
            Debug.LogFormat("[I Spy #{0}] Placed icon on {1}", moduleId, AllMods[i % AllMods.Count].ModuleDisplayName);
        }
        for (int i = totalNumberOfIcons; i < 20; i++)
            icons[i].gameObject.SetActive(false);
    }

    int[] Partition(int sum, int partitionCount)
    {
        int[] output = new int[partitionCount];
        do
            for (int i = 0; i < partitionCount; i++)
                output[i] = Rnd.Range(1, sum - 3);
        while (output.Sum() != sum);
        return output;
    }

    public void CollectIcon(int priority)
    {

    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use <!{0} foobar> to do something.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand (string command)
    {
        command = command.Trim().ToUpperInvariant();
        List<string> parameters = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve ()
    {
        yield return null;
    }
}
