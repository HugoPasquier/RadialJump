using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEffect : MonoBehaviour
{
    [SerializeField]
    public string label;

    [SerializeField]
    public Material mat;

    public string tagEffect;
}
