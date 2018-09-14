using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
public class Test : MonoBehaviour
{
    [SerializeField] CustomImage _Image;
    private void Start()
    {
        var s = ResourcesManager.instance.GetSprite("Cannon"); 
        _Image.SetData(s); 
    }
}