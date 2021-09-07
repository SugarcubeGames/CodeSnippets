using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attributes
{
	STRENGTH,
	CONSTITUTION,
	DEXTERITY,
	INTELLIGENCE,
	WISDOM,
	CHARISMA
}

/// <summary>
/// Attribute preference, used for assigning attributes based on guild
/// </summary>
public class GuildAttributePref
{
	public List<Attributes> attributeList;
	public List<Attributes> PrimaryAttributes;
	public List<Attributes> SecondaryAttributes;
	public List<Attributes> TertiaryAttributes;

	/// <summary>
	/// Create new Attribute Preference
	/// </summary>
	/// <param name="a1">Primary Attribute 1</param>
	/// <param name="a2">Primary Attribute 2</param>
	/// <param name="a3">Secondary Attribute 1</param>
	/// <param name="a4">Secondary Attribute 2</param>
	/// <param name="a5">Tertiary Attribut 1</param>
	/// <param name="a6">TErtiary Attribute 2</param>
	public GuildAttributePref(Attributes a1, Attributes a2, Attributes a3, Attributes a4, Attributes a5, Attributes a6)
	{
		attributeList = new List<Attributes>();
		attributeList.Add(a1);
		attributeList.Add(a2);
		attributeList.Add(a3);
		attributeList.Add(a4);
		attributeList.Add(a5);
		attributeList.Add(a6);

		PrimaryAttributes = new List<Attributes>();
		PrimaryAttributes.Add(a1);
		PrimaryAttributes.Add(a2);

		SecondaryAttributes = new List<Attributes>();
		SecondaryAttributes.Add(a3);
		SecondaryAttributes.Add(a4);

		TertiaryAttributes = new List<Attributes>();
		TertiaryAttributes.Add(a5);
		TertiaryAttributes.Add(a6);
	}
}
