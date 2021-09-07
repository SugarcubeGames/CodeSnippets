using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Guilds are the equivalent of character classes.  As a particular guild
/// levels up, characters that belong to that guild will gain access to more
/// powerful abilities and bonuses.  F
/// </summary>
public class Guild
{
	public string _name;

	private GuildAttributePref _attributePrefs;

	public Guild(string name, GuildAttributePref gaPrefs)
	{
		_name = name;
		_attributePrefs = gaPrefs;
	}
}

/// <summary>
/// This will need to be replaced with a guild manager, I'm only doing this
/// for tesing purposes.
/// </summary>
public class Guilds
{
	public static List<Guild> guilds = new List<Guild>
	{
		new Guild("Warrior's Guild", new GuildAttributePref(Attributes.STRENGTH, 
										Attributes.CONSTITUTION, Attributes.DEXTERITY, Attributes.CHARISMA, 
										Attributes.INTELLIGENCE, Attributes.WISDOM)),
		new Guild("Mage's Guild", new GuildAttributePref(Attributes.INTELLIGENCE,
										Attributes.CHARISMA, Attributes.WISDOM, Attributes.DEXTERITY,
										Attributes.STRENGTH, Attributes.CONSTITUTION))
	};

	public static int Count = guilds.Count;
}
