using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class GasBarUI : MonoBehaviour
{
    #region Private fields
    private PlayerPropulsion target;

    [Tooltip("TMP Text displaying player's name.")]
    [SerializeField]
    private TextMeshProUGUI tmp;

    [Tooltip("UI Slider displaying gas remaining.")]
    [SerializeField]
    private Slider gasBar;

    #endregion

    #region MonoBehaviour Callbacks

    void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    void Start()
    {
        // Sets the gas bar's visual limits.
        if (gasBar!= null)
        {
            gasBar.maxValue = 30f;
            gasBar.minValue = 0f;
        }
        else
            Debug.LogError("<Color=Red><a>Missing</a></Color> GameBar UI Slider to initialize gas range.", this);
    }

    void Update()
    {
        // Reflects the player's gas
        if (gasBar != null)
        {
            gasBar.value = target.gas;
        }
        else 
            Debug.LogError("<Color=Red><a>Missing</a></Color> GameBar UI Slider to update gas values.", this);

        // Destroy gameObject if target is null.
        if (target == null)
        {
            //Destroy(this.gameObject);
            return;
        }
    }

    #endregion

    #region Public methods

    public void SetTarget(PlayerPropulsion _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayerPropulsion target for GasBarUI.SetTarget", this);
            return;
        }
        target = _target;
        
        if (tmp.text != null)
        {
            tmp.text = target.photonView.Owner.NickName + "'s gas";
        }

    }

    #endregion

    #region Private methods

    #endregion
}
