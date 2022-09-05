using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Global
{
    public string filter;
    public int rightIndex;
    public Entery[] options = new Entery[4];
    public Entery chosenEntery;
    [JsonConstructor]
    public Global(string filter, Entery chosenEntery, Entery[] options, int rightIndex)
    {
        this.filter = filter;
        this.chosenEntery = chosenEntery;
        shit(options);
        this.rightIndex = rightIndex;
    }
    public Global(GlobalScript g)
    {
        this.filter = g.filter;
        this.chosenEntery = g.chosenEntery;
        shit(g.options);
        this.rightIndex = g.rightIndex;
    }
    public void shit(Entery[] options)
    {
        for (int i = 0; i < 4; i++)
        {
            this.options[i] = options[i];
        }

    }
}
