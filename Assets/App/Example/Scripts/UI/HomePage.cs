using UnityEngine;
using UnityEngine.UI;

namespace FrameworkDesign.Example
{
    public class HomePage : MonoBehaviour
    {      
        // Start is called before the first frame update
        void Start()
        {
            //Logger.Log("HomePage-Start");
            transform.Find("SafeArea/StartContent/InformationPanel/StartButton").GetComponent<Button>().onClick.AddListener(()=>
                {
                    Logger.Log("HomePage-onClick");
                    gameObject.SetActive(false);
                    new GameStartCommand().Excute();
                }
                
            );
        }
    }
}

