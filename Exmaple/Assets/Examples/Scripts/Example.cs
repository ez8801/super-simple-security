using UnityEngine;
using Security;

public class Example : MonoBehaviour
{
    public Int32 Cash;
    public Int32 Gold { get; private set; }

    private Boolean _isClear;

    // Start is called before the first frame update
    void Start()
    {
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
}
