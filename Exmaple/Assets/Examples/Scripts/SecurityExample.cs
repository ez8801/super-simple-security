using UnityEngine;
using Security;

public class SecurityExample : MonoBehaviour
{
    public Int32 Gold { get; private set; }

    public Int32 Cash;
    
    private Boolean _isClear;

    // Start is called before the first frame update
    void Start()
    {
        SecurityListener.SetOnHackDetectListener(OnHackDetected);
        SecurityListener.SetOnErrorListener(OnError);

        Gold = new Int32(8801);

        Cash = new Int32();
        Cash.Value = 8801;

        Cash.Value = Cash.Value + Gold.Value;

        _isClear = new Boolean(false);

        Debug.Log(Gold.Value);
        Debug.Log(Cash);
        Debug.Log(_isClear);
    }

    public bool IsClear()
    {
        // implicit conversions: Security.Boolean to System.Boolean (bool)
        return _isClear;
    }

    #region Listener

    public void OnHackDetected(string message)
    {
        Debug.LogError(string.Format("OnHackDetected(string) {0}", message));
        Application.Quit();
    }

    public void OnError(string error)
    {
        Debug.LogError(string.Format("OnError(string) {0}", error));
    }

    #endregion Listener
}
