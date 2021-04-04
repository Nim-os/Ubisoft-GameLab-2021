using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyTextUI : MonoBehaviour
{
    private TextMeshProUGUI _TMP;
    [SerializeField]
    private GameObject _keyManager;

    void Start()
    {
        _TMP = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        string count = _keyManager.transform.childCount.ToString();
        _TMP.text = count;
    }

}
