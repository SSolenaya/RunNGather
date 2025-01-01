using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplateNumberProvider
{
    private Settings _settings;

    private int _levelNumber = 0;//  only for Templated Levels Mode

    public int LevelNumber
    {
        get => _levelNumber;
        set
        {
            if (value >= _settings.levelTemplatesList.Count)
            {
                _levelNumber = 0;
            }
            else
            {
                _levelNumber = value;
            }
        }
    }

    public LevelTemplateNumberProvider(Settings settings)
    {
        _settings = settings;
    }
}
