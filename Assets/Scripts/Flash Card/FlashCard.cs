using UnityEngine;
using System.Collections;
using SVGImporter;

public class FlashCard : MonoBehaviour 
{

    public SVGAsset[] svgAsset;
    public SVGImage svgimage;

    private int svgIndex = 0;

    // Use this for initialization
    void Start () 
    {
        svgimage.vectorGraphics = svgAsset[svgIndex];
    }

    // Update is called once per frame
    void Update () {

    }

    public void NextFlashCard()
    {
        if (svgIndex < svgAsset.Length - 1)
        {
            svgIndex++;
            svgimage.vectorGraphics = svgAsset[svgIndex];
        }
    }

    public void PreviousFlashCard()
    {
        if (svgIndex > 0)
        {
            svgIndex--;
            svgimage.vectorGraphics = svgAsset[svgIndex];
        }
    }
}
