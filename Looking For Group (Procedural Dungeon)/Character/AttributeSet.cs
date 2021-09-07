using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holder class for preselected Attribute rolls
/// </summary>
public class AttributeSet
{
    public int val1;
    public int val2;
    public int val3;
    public int val4;
    public int val5;
    public int val6;

    public AttributeSet(int v1, int v2, int v3, int v4, int v5, int v6)
    {
        val1 = v1;
        val2 = v2;
        val3 = v3;
        val4 = v4;
        val5 = v5;
        val6 = v6;
    }
}

//https://www.reddit.com/r/DnD/comments/2epkdi/5e_here_is_a_complete_list_of_valid_ability_score/
/// <summary>
/// Predefined collection of ability point distributions
/// </summary>
public class AttributeSets
{
    public static List<AttributeSet> sets = new List<AttributeSet>
    {
        new AttributeSet(15, 15, 15 ,8, 8, 8),
        new AttributeSet(15, 15, 14, 10, 8, 8),
        new AttributeSet(15, 15, 14, 9, 9, 8),
        new AttributeSet(15, 15, 13, 12, 8, 8),
        new AttributeSet(15, 15, 13, 11, 9, 8),
        new AttributeSet(15, 15, 13, 10, 10, 8),
        new AttributeSet(15, 15, 13, 10, 9, 9),
        new AttributeSet(15, 15, 12, 12, 9, 8),
        new AttributeSet(15, 15, 12, 11, 10, 8),
        new AttributeSet(15, 15, 12, 11, 9, 9),
        new AttributeSet(15, 15, 12, 10, 10, 9),
        new AttributeSet(15, 15, 11, 11, 11, 8),
        new AttributeSet(15, 15, 11, 11, 10, 9),
        new AttributeSet(15, 15, 11, 10, 10, 10),
        new AttributeSet(15, 14, 14, 12, 8, 8),
        new AttributeSet(15, 14, 14, 11, 9, 8),
        new AttributeSet(15, 14, 14, 10, 10, 8),
        new AttributeSet(15, 14, 14, 10, 9, 9),
        new AttributeSet(15, 14, 13, 13, 9, 8),
        new AttributeSet(15, 14, 13, 12, 10, 8),
        new AttributeSet(15, 14, 13, 12, 9, 9),
        new AttributeSet(15, 14, 13, 11, 11, 8),
        new AttributeSet(15, 14, 13, 11, 10, 9),
        new AttributeSet(15, 14, 13, 10, 10, 10),
        new AttributeSet(15, 14, 12, 12, 11, 8),
        new AttributeSet(15, 14, 12, 12, 10, 9),
        new AttributeSet(15, 14, 12, 11, 11, 9),
        new AttributeSet(15, 14, 12, 11, 10, 10),
        new AttributeSet(15, 14, 11, 11, 11, 10),
        new AttributeSet(15, 13, 13, 13, 11, 8),
        new AttributeSet(15, 13, 13, 13, 10, 9),
        new AttributeSet(15, 13, 13, 12, 12, 8),
        new AttributeSet(15, 13, 13, 12, 11, 9),
        new AttributeSet(15, 13, 13, 12, 10, 10),
        new AttributeSet(15, 13, 13, 11, 11, 10),
        new AttributeSet(15, 13, 12, 12, 12, 9),
        new AttributeSet(15, 13, 12, 12, 11, 10),
        new AttributeSet(15, 13, 12, 11, 11, 11),
        new AttributeSet(15, 12, 12, 12, 12, 10),
        new AttributeSet(15, 12, 12, 12, 11, 11),
        new AttributeSet(14, 14, 14, 13, 9, 8),
        new AttributeSet(14, 14, 14, 12, 10, 8),
        new AttributeSet(14, 14, 14, 12, 9, 9),
        new AttributeSet(14, 14, 14, 11, 11, 8),
        new AttributeSet(14, 14, 14, 11, 10, 9),
        new AttributeSet(14, 14, 14, 10, 10, 10),
        new AttributeSet(14, 14, 13, 13, 11, 8),
        new AttributeSet(14, 14, 13, 13, 10, 9),
        new AttributeSet(14, 14, 13, 12, 12, 8),
        new AttributeSet(14, 14, 13, 12, 11, 9),
        new AttributeSet(14, 14, 13, 12, 10, 10),
        new AttributeSet(14, 14, 13, 11, 11, 10),
        new AttributeSet(14, 14, 12, 12, 12, 9),
        new AttributeSet(14, 14, 12, 12, 11, 10),
        new AttributeSet(14, 14, 12, 11, 11, 11),
        new AttributeSet(14, 13, 13, 13, 13, 8),
        new AttributeSet(14, 13, 13, 13, 12, 9),
        new AttributeSet(14, 13, 13, 13, 11, 10),
        new AttributeSet(14, 13, 13, 12, 12, 10),
        new AttributeSet(14, 13, 13, 12, 11, 11),
        new AttributeSet(14, 13, 12, 12, 12, 11),
        new AttributeSet(14, 12, 12, 12, 12, 12),
        new AttributeSet(13, 13, 13, 13, 13, 10),
        new AttributeSet(13, 13, 13, 13, 12, 11),
        new AttributeSet(13, 13, 13, 12, 12, 12)
    };
}
