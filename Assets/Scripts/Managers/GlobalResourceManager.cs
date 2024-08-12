using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalResourceManager : MonoBehaviour
{
    public TMP_Text GoldText;
    public int Gold = 0;

    private void Update()
    {
        GoldText.text = Gold.ToString();
    }
}
