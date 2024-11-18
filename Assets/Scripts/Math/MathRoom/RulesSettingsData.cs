using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathRoom
{
    [Serializable]
    public class RulesSettingsData
    {
        public bool isAdditionIncluded;
        public bool isSubstractionIncluded;
        public bool isMultiplicationIncluded;
        public bool isDivisionIncluded;

        public int maxValueAdSub;
        public int maxValueMultDiv;

    }
}
