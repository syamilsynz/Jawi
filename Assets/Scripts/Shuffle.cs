using UnityEngine;
using System.Collections;

public class Shuffle : MonoBehaviour 
{

	/// <summary>
	/// Shuffle the specified string.
	/// </summary>
	/// <param name="str">String.</param>
	public string Shuffles(string str)
	{
		char[] array = str.ToCharArray();
		System.Random rng = new System.Random();
		int n = array.Length;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			var value = array[k];
			array[k] = array[n];
			array[n] = value;
		}
		return new string(array);
	}

}
