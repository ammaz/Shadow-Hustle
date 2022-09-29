using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateScrollBackground : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Transform _childBG1;
    [SerializeField]
    Transform _childBG2;
    [SerializeField]
    Transform _childBG3;
    float anchor = 0;
    float sizeOfRectTransform = 0;
    void Start () {
        anchor = _childBG1.transform.localPosition.x;
        sizeOfRectTransform = _childBG1.GetComponent<RectTransform>().sizeDelta.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (_childBG2.localPosition.x < anchor)
        {
            anchor = this.transform.position.x;
            _childBG1.transform.localPosition = new Vector3(_childBG1.transform.localPosition.x + 3* sizeOfRectTransform, _childBG1.transform.localPosition.y, _childBG1.transform.localPosition.z);
            Transform tempBg1 = _childBG1;
            Transform tempBg2 = _childBG2;
            Transform tempBg3 = _childBG3;
            _childBG1 = tempBg2; ;
            _childBG2 = tempBg3;
            _childBG3 = tempBg1;
        }
    }
}
