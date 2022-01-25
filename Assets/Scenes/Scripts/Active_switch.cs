using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_switch : MonoBehaviour
{
    public void ActiveSwitch()
    {
        this.gameObject.SetActive(
            !this.gameObject.activeSelf);
    }
}
