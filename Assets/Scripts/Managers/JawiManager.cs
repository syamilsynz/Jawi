using UnityEngine;
using System.Collections;
using System;
using SVGImporter;

public class JawiManager : MonoBehaviour 
{
    public string[] jawiStr;

    public SVGAsset[] jawiCharacterSVG;

    SVGAsset[] jawiSVG;

    void Awake()
    {
        //SetupJawi();

        jawiSVG = Resources.LoadAll <SVGAsset> ("Svg/Letter/Jawi White Alpha/");  
        jawiCharacterSVG = new SVGAsset[jawiSVG.Length];
//        jawiSVG = Resources.LoadAll <SVGAsset> ("Jawi Letter");  
        SetupJawiSVG();
    }

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void SetupJawi()
    {
        jawiStr = new string[37];

        jawiStr[0] = "alif";
        jawiStr[1] = "ba";
        jawiStr[2] = "ta";
        jawiStr[3] = "ta marbutah";
        jawiStr[4] = "sa";
        jawiStr[5] = "jim";
        jawiStr[6] = "ca";
        jawiStr[7] = "hah";
        jawiStr[8] = "kha";
        jawiStr[9] = "dal";
        jawiStr[10] = "zal";
        jawiStr[11] = "ra";
        jawiStr[12] = "zai";
        jawiStr[13] = "sin";
        jawiStr[14] = "syin";
        jawiStr[15] = "sod";
        jawiStr[16] = "dhod";
        jawiStr[17] = "tho";
        jawiStr[18] = "dzho";
        jawiStr[19] = "ain";
        jawiStr[20] = "ghain";
        jawiStr[21] = "nga";
        jawiStr[22] = "fa";
        jawiStr[23] = "pa";
        jawiStr[24] = "qaf";
        jawiStr[25] = "kaf";
        jawiStr[26] = "ga";
        jawiStr[27] = "lam";
        jawiStr[28] = "mim";
        jawiStr[29] = "nun";
        jawiStr[30] = "wau";
        jawiStr[31] = "va";
        jawiStr[32] = "ha";
        jawiStr[33] = "hamzah";
        jawiStr[34] = "ya";
        jawiStr[35] = "ye";
        jawiStr[36] = "nya";
//        jawiStr[37] = "kosong";
//        jawiStr[38] = "satu";
//        jawiStr[39] = "dua";
//        jawiStr[40] = "tiga";
//        jawiStr[41] = "empat";
//        jawiStr[42] = "lima";
//        jawiStr[43] = "enam";
//        jawiStr[44] = "tujuh";
//        jawiStr[45] = "lapan";
//        jawiStr[46] = "sembilan";
    }
        
    public void SetupJawiSVG()
    {
        for (int i = 0; i < jawiSVG.Length; i++)
        {
            for (int j = 0; j < jawiStr.Length; j++)
            {
                if (jawiSVG[i].name == jawiStr[j])
                    jawiCharacterSVG[j] = jawiSVG[i];
            }
        }
    }


    public void JawiConversion(int index)
    {
        string letter = jawiStr[index];               
    }

    public void JawiConversion(string str)
    {
        int jawiIndex = Array.IndexOf(jawiStr, str);
    }

    public int GetJawiIndex(string str)
    {
        // sumber : http://www.dotnetperls.com/array-indexof
        int jawiIndex = Array.IndexOf(jawiStr, str);

        return jawiIndex;
       
    }
        
}
