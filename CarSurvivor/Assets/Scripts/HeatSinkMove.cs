using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSinkMove : MonoBehaviour
{
    public float speed = 1f;
    public float height = 1f;
    public float bladeOffset = 1f;
    public float allBladeHeight;

    [Serializable]
    public struct Blades
    {
        public GameObject bladeObj;
        public float heightMulti;
        public float heightSingle;
    }
    public List<Blades> blades;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EachBlade();
    }

    float moveFactor(float offset)
    {
        float t = Mathf.Sin((Time.time + offset) * speed) * height;
        return t;
    }

    void SinkBladeMove(GameObject blade, float yOffset, float yIndiv, float yIndivSingle)
    {
        blade.transform.localPosition = new Vector3(blade.transform.localPosition.x, moveFactor(yOffset) * (yIndiv == 0 ? 1 : yIndiv) + yIndivSingle + allBladeHeight, blade.transform.localPosition.z); // new Vector3(0, moveFactor(yOffset), 0);
    }

    void EachBlade()
    {
        //foreach(var bladess in blades)
        //{
        //    SinkBladeMove(bladess.bladeObj, );
        //}

        for (int i = 0; i < blades.Count; i++)
        {
            SinkBladeMove(blades[i].bladeObj, i * bladeOffset, blades[i].heightMulti, blades[i].heightSingle);
        }
    }

}
